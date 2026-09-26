using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls.Xaml;

namespace FuchsControls;

[ContentProperty(nameof(Items))]
public sealed class FuchsAccordion : ContentView
{
	public static readonly BindableProperty IsMultipleExpansionEnabledProperty = BindableProperty.Create(
		nameof(IsMultipleExpansionEnabled), typeof(bool), typeof(FuchsAccordion), false, propertyChanged: OnMultipleExpansionChanged);

	private readonly ObservableCollection<FuchsAccordionItem> items = new();
	private readonly VerticalStackLayout itemLayout = new() { Spacing = 8 };
	private readonly Dictionary<FuchsAccordionItem, AccordionEntry> entries = new();
	private bool isSynchronizing;
	private bool isThemeChangeSubscribed;

	public FuchsAccordion()
	{
		items.CollectionChanged += OnItemsChanged;
		Content = itemLayout;
		ApplyTheme();
	}

	public ObservableCollection<FuchsAccordionItem> Items => items;

	/// <summary>
	/// Gets or sets whether more than one accordion item can remain expanded at once.
	/// </summary>
	public bool IsMultipleExpansionEnabled
	{
		get => (bool)GetValue(IsMultipleExpansionEnabledProperty);
		set => SetValue(IsMultipleExpansionEnabledProperty, value);
	}

	public event EventHandler? ExpandedItemsChanged;

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		if (Handler is null)
		{
			UnsubscribeFromThemeChanges();
			return;
		}

		SubscribeToThemeChanges();
		ApplyTheme();
		RebuildItems();
	}

	private static void OnMultipleExpansionChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var accordion = (FuchsAccordion)bindable;
		accordion.NormalizeExpandedItems();
		accordion.RefreshEntries(false);
	}

	private void OnItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if (e.OldItems is not null)
		{
			foreach (FuchsAccordionItem item in e.OldItems)
			{
				item.PropertyChanged -= OnItemPropertyChanged;
			}
		}

		if (e.NewItems is not null)
		{
			foreach (FuchsAccordionItem item in e.NewItems)
			{
				item.PropertyChanged += OnItemPropertyChanged;
			}
		}

		NormalizeExpandedItems();
		RebuildItems();
	}

	private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (sender is not FuchsAccordionItem item || isSynchronizing)
		{
			return;
		}

		if (e.PropertyName == nameof(FuchsAccordionItem.IsExpanded))
		{
			if (item.IsExpanded && !IsMultipleExpansionEnabled)
			{
				CollapseOtherItems(item);
			}

			RefreshEntries(true);
			ExpandedItemsChanged?.Invoke(this, EventArgs.Empty);
			return;
		}

		if (e.PropertyName is nameof(FuchsAccordionItem.Header) or nameof(FuchsAccordionItem.HeaderFormattedText) or nameof(FuchsAccordionItem.HeaderView)
			or nameof(FuchsAccordionItem.Content) or nameof(FuchsAccordionItem.IsEnabled))
		{
			RebuildItems();
		}
	}

	private void ToggleItem(FuchsAccordionItem item)
	{
		if (item.IsEnabled)
		{
			item.IsExpanded = !item.IsExpanded;
		}
	}

	private void NormalizeExpandedItems()
	{
		if (IsMultipleExpansionEnabled)
		{
			return;
		}

		var expandedItem = items.FirstOrDefault(item => item.IsExpanded);
		if (expandedItem is not null)
		{
			CollapseOtherItems(expandedItem);
		}
	}

	private void CollapseOtherItems(FuchsAccordionItem expandedItem)
	{
		isSynchronizing = true;
		foreach (var item in items.Where(item => !ReferenceEquals(item, expandedItem) && item.IsExpanded))
		{
			item.IsExpanded = false;
		}

		isSynchronizing = false;
	}

	private void RebuildItems()
	{
		itemLayout.Children.Clear();
		entries.Clear();
		foreach (var item in items)
		{
			var entry = CreateItemEntry(item);
			entries.Add(item, entry);
			itemLayout.Add(entry.Host);
		}
	}

	private AccordionEntry CreateItemEntry(FuchsAccordionItem item)
	{
		var header = new Border
					 {
						 Padding = new Thickness(16, 12)
						 , StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(FuchsThemeManager.Current.CornerRadius) }
						 , StrokeThickness = FuchsThemeManager.Current.BorderWidth, Opacity = item.IsEnabled ? 1 : 0.45
					 };
		header.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => ToggleItem(item)) });
		SemanticProperties.SetDescription(header, item.Header);
		SemanticProperties.SetHint(header, item.IsEnabled ? (item.IsExpanded ? "Collapse section" : "Expand section") : "Disabled section");

		var headerLayout = new Grid { ColumnSpacing = 12 };
		headerLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
		headerLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
		headerLayout.Add(CreateHeaderContent(item), 0);
		var indicator = new Label
						{
							Text = "⌄", FontSize = FuchsThemeManager.Current.BodyFontSize, VerticalTextAlignment = TextAlignment.Center
							, HorizontalTextAlignment = TextAlignment.Center, Rotation = item.IsExpanded ? 180 : 0
						};
		headerLayout.Add(indicator, 1);
		header.Content = headerLayout;

		var panel = new ContentView
					{
						Content = item.Content, Padding = new Thickness(16), IsVisible = item.IsExpanded, Opacity = 1
					};
		var host = new VerticalStackLayout { Spacing = 0, Children = { header, panel } };
		var entry = new AccordionEntry(item, host, header, panel, indicator);
		ApplyEntryTheme(entry);
		return entry;
	}

	private static View CreateHeaderContent(FuchsAccordionItem item)
	{
		if (item.HeaderView is not null)
		{
			return item.HeaderView;
		}

		var header = new FuchsTypo { Text = item.Header, Type = FuchsTypoType.Body, VerticalTextAlignment = TextAlignment.Center };
		if (item.HeaderFormattedText is not null)
		{
			header.FormattedText = item.HeaderFormattedText;
		}

		return header;
	}

	private void RefreshEntries(bool animate)
	{
		foreach (var entry in entries.Values)
		{
			ApplyEntryTheme(entry);
			_ = SetPanelVisibilityAsync(entry, animate);
		}
	}

	private static async Task SetPanelVisibilityAsync(AccordionEntry entry, bool animate)
	{
		var transition = ++entry.TransitionVersion;
		if (entry.Item.IsExpanded)
		{
			entry.Panel.IsVisible = true;
			entry.Panel.Opacity = animate ? 0 : 1;
			if (animate)
			{
				try
				{
					await entry.Panel.FadeToAsync(1, 200, Easing.CubicOut);
				}
				catch (Exception exception)
				{
					System.Diagnostics.Trace.WriteLine($"FuchsAccordion expand animation failed: {exception}");
				}
			}

			return;
		}

		if (animate && entry.Panel.IsVisible)
		{
			try
			{
				await entry.Panel.FadeToAsync(0, 200, Easing.CubicIn);
			}
			catch (Exception exception)
			{
				System.Diagnostics.Trace.WriteLine($"FuchsAccordion collapse animation failed: {exception}");
			}

			if (transition == entry.TransitionVersion && !entry.Item.IsExpanded)
			{
				entry.Panel.IsVisible = false;
				entry.Panel.Opacity = 1;
			}
		}
		else
		{
			entry.Panel.IsVisible = false;
			entry.Panel.Opacity = 1;
		}
	}

	private void ApplyTheme()
	{
		Margin = new Thickness(0, 10);
		BackgroundColor = FuchsThemeManager.Current.BackgroundColor;
		RefreshEntries(false);
	}

	private static void ApplyEntryTheme(AccordionEntry entry)
	{
		var theme = FuchsThemeManager.Current;
		entry.Header.Background = new SolidColorBrush(entry.Item.IsExpanded ? theme.PrimaryLight : theme.FieldBackgroundColor);
		entry.Header.Stroke = new SolidColorBrush(entry.Item.IsExpanded ? theme.PrimaryColor : theme.FieldBorderColor);
		entry.Header.StrokeThickness = theme.BorderWidth;
		entry.Header.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(theme.CornerRadius) };
		entry.Indicator.TextColor = entry.Item.IsExpanded ? theme.PrimaryColor : theme.TextColor;
		entry.Panel.BackgroundColor = theme.BackgroundColor;
		SemanticProperties.SetHint(entry.Header, entry.Item.IsEnabled ? (entry.Item.IsExpanded ? "Collapse section" : "Expand section") : "Disabled section");
	}

	private void SubscribeToThemeChanges()
	{
		if (isThemeChangeSubscribed)
		{
			return;
		}

		FuchsThemeManager.ThemeChanged += OnThemeChanged;
		isThemeChangeSubscribed = true;
	}

	private void UnsubscribeFromThemeChanges()
	{
		if (!isThemeChangeSubscribed)
		{
			return;
		}

		FuchsThemeManager.ThemeChanged -= OnThemeChanged;
		isThemeChangeSubscribed = false;
	}

	private void OnThemeChanged(object? sender, EventArgs e)
	{
		ApplyTheme();
		RebuildItems();
	}

	private sealed class AccordionEntry(FuchsAccordionItem item, VerticalStackLayout host, Border header, ContentView panel, Label indicator)
	{
		public FuchsAccordionItem Item { get; } = item;
		public VerticalStackLayout Host { get; } = host;
		public Border Header { get; } = header;
		public ContentView Panel { get; } = panel;
		public Label Indicator { get; } = indicator;
		public int TransitionVersion { get; set; }
	}
}