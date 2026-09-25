using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Layouts;

namespace FuchsControls;

[ContentProperty(nameof(Tabs))]
public sealed partial class FuchsTabs : ContentView
{
	public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
		nameof(SelectedIndex), typeof(int), typeof(FuchsTabs), 0, propertyChanged: OnSelectionChanged);

	public static readonly BindableProperty IsScrollableProperty = BindableProperty.Create(
		nameof(IsScrollable), typeof(bool), typeof(FuchsTabs), false, propertyChanged: OnLayoutChanged);

	public static readonly BindableProperty IsBorderedProperty = BindableProperty.Create(
		nameof(IsBordered), typeof(bool), typeof(FuchsTabs), true, propertyChanged: OnLayoutChanged);

	public static readonly BindableProperty LineAtTopProperty = BindableProperty.Create(
		nameof(LineAtTop), typeof(bool), typeof(FuchsTabs), false, propertyChanged: OnHeadersChanged);

	public static readonly BindableProperty TabAnimationProperty = BindableProperty.Create(
		nameof(TabAnimation), typeof(FuchsTabAnimation), typeof(FuchsTabs), FuchsTabAnimation.Fade);

	private readonly ObservableCollection<FuchsTab> tabs = new();
	private readonly Border tabsBorder = new();
	private readonly Border tabsHeader = new();
	private readonly Grid tabsHeaderGrid = new();
	private readonly FlexLayout tabHeaders = new();
	private readonly ScrollView tabHeadersScroll = new();
	private readonly ContentView tabHeadersHost = new();
	private readonly Border tabsContent = new();
	private readonly Grid tabPanels = new();
	private readonly BoxView tabHeaderDivider = new();
	private readonly List<PanelEntry> panelEntries = new();
	private readonly ICommand selectTabCommand;
	private int selectionVersion;
	private int previousSelectedIndex = -1;
	private FuchsTab? lastSelectedTab;

	public FuchsTabs()
	{
		tabs.CollectionChanged += OnTabsChanged;
		selectTabCommand = new Command<int>(index => SelectedIndex = index);

		SetDynamicResource(StyleProperty, "FuchsTabsStyle");
		tabsBorder.SetDynamicResource(StyleProperty, "FuchsTabsBorderStyle");
		tabsHeader.SetDynamicResource(StyleProperty, "FuchsTabsHeaderStyle");
		tabHeaders.SetDynamicResource(StyleProperty, "FuchsTabsHeaderItemsStyle");
		tabHeaderDivider.SetDynamicResource(StyleProperty, "FuchsTabsHeaderDividerStyle");
		tabsContent.SetDynamicResource(StyleProperty, "FuchsTabsContentStyle");
		tabPanels.SetDynamicResource(StyleProperty, "FuchsTabPanelsStyle");

		tabHeadersScroll.Orientation = ScrollOrientation.Horizontal;
		tabHeadersScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Never;
		tabHeadersScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Never;
		tabHeadersHost.Content = tabHeaders;

		tabsHeaderGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
		tabsHeaderGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
		tabsHeaderGrid.Add(tabHeadersHost);
		tabsHeaderGrid.Add(tabHeaderDivider);
		Grid.SetRow(tabHeaderDivider, 1);
		tabsHeader.Content = tabsHeaderGrid;

		tabsContent.Content = tabPanels;

		var tabsLayout = new Grid();
		tabsLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
		tabsLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
		tabsLayout.Add(tabsHeader);
		tabsLayout.Add(tabsContent);
		Grid.SetRow(tabsContent, 1);

		tabsBorder.Content = tabsLayout;
		Content = tabsBorder;
		ApplyHeaderLayout();
	}

	public ObservableCollection<FuchsTab> Tabs => tabs;

	public int SelectedIndex
	{
		get => (int)GetValue(SelectedIndexProperty);
		set => SetValue(SelectedIndexProperty, value);
	}

	public FuchsTab? SelectedTab => SelectedIndex >= 0 && SelectedIndex < tabs.Count ? tabs[SelectedIndex] : null;

	public bool IsScrollable
	{
		get => (bool)GetValue(IsScrollableProperty);
		set => SetValue(IsScrollableProperty, value);
	}

	public bool IsBordered
	{
		get => (bool)GetValue(IsBorderedProperty);
		set => SetValue(IsBorderedProperty, value);
	}

	public bool LineAtTop
	{
		get => (bool)GetValue(LineAtTopProperty);
		set => SetValue(LineAtTopProperty, value);
	}

	public FuchsTabAnimation TabAnimation
	{
		get => (FuchsTabAnimation)GetValue(TabAnimationProperty);
		set => SetValue(TabAnimationProperty, value);
	}

	public ICommand SelectTabCommand => selectTabCommand;

	public event EventHandler? SelectedTabChanged;

	private static void OnSelectionChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (FuchsTabs)bindable;
		var requestedIndex = (int)newValue;
		if (control.tabs.Count == 0)
		{
			if (requestedIndex != -1)
			{
				control.SetValue(SelectedIndexProperty, -1);
			}

			return;
		}

		var index = control.FindSelectableIndex(requestedIndex, (int)oldValue);
		if (index != requestedIndex)
		{
			control.SetValue(SelectedIndexProperty, index);
			return;
		}

		_ = control.ApplySelectionAsync(index);
	}

	private static void OnLayoutChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var tabs = (FuchsTabs)bindable;
		tabs.ApplyHeaderLayout();
		tabs.ApplyBorderStyle();
	}

	private static void OnHeadersChanged(BindableObject bindable, object oldValue, object newValue)
	{
		((FuchsTabs)bindable).RebuildHeaders();
	}

	private void OnTabsChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		var selectedTab = lastSelectedTab ?? SelectedTab;
		if (e.OldItems is not null)
		{
			foreach (FuchsTab tab in e.OldItems)
			{
				if (!tabs.Contains(tab))
				{
					tab.PropertyChanged -= OnTabPropertyChanged;
				}
			}
		}

		if (e.NewItems is not null)
		{
			foreach (FuchsTab tab in e.NewItems)
			{
				if (tabs.Count(item => ReferenceEquals(item, tab)) == 1)
				{
					tab.PropertyChanged += OnTabPropertyChanged;
				}
			}
		}

		RebuildHeaders();
		RebuildPanels();

		if (tabs.Count == 0)
		{
			SetValue(SelectedIndexProperty, -1);
			previousSelectedIndex = -1;
			lastSelectedTab = null;
		}
		else if (selectedTab is not null && tabs.Contains(selectedTab))
		{
			var selectedIndex = tabs.IndexOf(selectedTab);
			if (selectedIndex != SelectedIndex)
			{
				SetValue(SelectedIndexProperty, selectedIndex);
			}
			else
			{
				_ = ApplySelectionAsync(selectedIndex);
			}
		}
		else
		{
			SetValue(SelectedIndexProperty, FindSelectableIndex(SelectedIndex, 0));
		}
	}

	private void OnTabPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (sender is not FuchsTab tab)
		{
			return;
		}

		if (e.PropertyName is nameof(FuchsTab.Header) or nameof(FuchsTab.HeaderFormattedText) or nameof(FuchsTab.HeaderView))
		{
			RebuildHeaders();
		}
		else if (e.PropertyName == nameof(FuchsTab.IsEnabled))
		{
			RebuildHeaders();
			if (!tab.IsEnabled && ReferenceEquals(tab, SelectedTab))
			{
				SelectedIndex = FindSelectableIndex(SelectedIndex, -1);
			}
		}
		else if (e.PropertyName == nameof(FuchsTab.Content))
		{
			var panel = panelEntries.FirstOrDefault(entry => ReferenceEquals(entry.Tab, tab));
			if (panel is not null)
			{
				panel.Host.Content = tab.Content;
			}
		}
	}

	private void RebuildHeaders()
	{
		tabHeaders.Children.Clear();
		for (var index = 0; index < tabs.Count; index++)
		{
			var tab = tabs[index];
			tabHeaders.Add(CreateTabHeader(tab, index));
		}
	}

	private Border CreateTabHeader(FuchsTab tab, int index)
	{
		var header = new Border();
		header.SetDynamicResource(StyleProperty, tab.IsEnabled ? "FuchsTabHeaderStyle" : "FuchsTabDisabledHeaderStyle");
		header.GestureRecognizers.Add(new TapGestureRecognizer
		{
			Command = new Command(() =>
			{
				if (tab.IsEnabled)
				{
					SelectedIndex = index;
				}
			})
		});
		header.AutomationId = $"FuchsTabHeader{index}";
		SemanticProperties.SetDescription(header, tab.Header);
		SemanticProperties.SetHint(header, tab.IsEnabled ? "Select tab" : "Disabled tab");

		var headerLayout = new Grid();
		headerLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
		headerLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

		var headerContent = CreateHeaderContent(tab);
		var indicator = new BoxView();
		indicator.SetDynamicResource(
			StyleProperty,
			tab.IsSelected ? "FuchsTabActiveIndicatorStyle" : tab.IsEnabled ? "FuchsTabIndicatorStyle" : "FuchsTabDisabledIndicatorStyle");
		Grid.SetRow(headerContent, LineAtTop ? 1 : 0);
		Grid.SetRow(indicator, LineAtTop ? 0 : 1);
		headerLayout.Add(headerContent);
		headerLayout.Add(indicator);
		header.Content = headerLayout;
		return header;
	}

	private static View CreateHeaderContent(FuchsTab tab)
	{
		if (tab.HeaderView is not null)
		{
			return tab.HeaderView;
		}

		var header = new FuchsTypo
		{
			Text = tab.Header,
			Type = FuchsTypoType.Body,
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center
		};
		if (tab.HeaderFormattedText is not null)
		{
			header.FormattedText = tab.HeaderFormattedText;
		}

		header.SetDynamicResource(StyleProperty, tab.IsSelected ? "FuchsTabActiveTextStyle" : "FuchsTabTextStyle");
		return header;
	}

	private void ApplyHeaderLayout()
	{
		tabHeaders.Wrap = IsScrollable ? FlexWrap.NoWrap : FlexWrap.Wrap;
		if (IsScrollable)
		{
			tabHeadersScroll.Content = tabHeaders;
			tabHeadersHost.Content = tabHeadersScroll;
		}
		else
		{
			tabHeadersScroll.Content = null;
			tabHeadersHost.Content = tabHeaders;
		}
	}

	private void ApplyBorderStyle() => tabsBorder.SetDynamicResource(
		StyleProperty, IsBordered ? "FuchsTabsBorderStyle" : "FuchsTabsPlainStyle");

	private void RebuildPanels()
	{
		var oldEntries = panelEntries.ToList();
		var newEntries = new List<PanelEntry>(tabs.Count);
		var usedEntries = new HashSet<PanelEntry>();

		for (var index = 0; index < tabs.Count; index++)
		{
			var tab = tabs[index];
			var entry = oldEntries.FirstOrDefault(item => !usedEntries.Contains(item) && ReferenceEquals(item.Tab, tab));
			if (entry is null)
			{
				var host = new ContentView();
				host.SetDynamicResource(StyleProperty, "FuchsTabPanelStyle");
				entry = new PanelEntry(tab, host);
			}

			usedEntries.Add(entry);
			entry.Host.Content = tab.Content;
			entry.Host.IsVisible = index == SelectedIndex;
			entry.Host.Opacity = 1;
			entry.Host.TranslationX = 0;
			newEntries.Add(entry);
		}

		tabPanels.Children.Clear();
		foreach (var entry in newEntries)
		{
			tabPanels.Add(entry.Host);
		}

		panelEntries.Clear();
		panelEntries.AddRange(newEntries);
	}

	private int FindSelectableIndex(int requestedIndex, int fallbackIndex)
	{
		if (tabs.Count == 0)
		{
			return -1;
		}

		if (requestedIndex >= 0 && requestedIndex < tabs.Count && tabs[requestedIndex].IsEnabled)
		{
			return requestedIndex;
		}

		if (fallbackIndex >= 0 && fallbackIndex < tabs.Count && tabs[fallbackIndex].IsEnabled)
		{
			return fallbackIndex;
		}

		for (var index = 0; index < tabs.Count; index++)
		{
			if (tabs[index].IsEnabled)
			{
				return index;
			}
		}

		return -1;
	}

	private async Task ApplySelectionAsync(int index)
	{
		if (index < 0 || index >= tabs.Count || !tabs[index].IsEnabled)
		{
			return;
		}

		var transition = ++selectionVersion;
		var outgoing = lastSelectedTab is null
			? null
			: panelEntries.FirstOrDefault(entry => ReferenceEquals(entry.Tab, lastSelectedTab))?.Host;
		var incoming = panelEntries.FirstOrDefault(entry => ReferenceEquals(entry.Tab, tabs[index]))?.Host;
		var outgoingIndex = lastSelectedTab is null ? previousSelectedIndex : tabs.IndexOf(lastSelectedTab);
		previousSelectedIndex = index;

		for (var tabIndex = 0; tabIndex < tabs.Count; tabIndex++)
		{
			tabs[tabIndex].IsSelected = tabIndex == index;
		}

		RebuildHeaders();
		if (incoming is null)
		{
			RebuildPanels();
			incoming = panelEntries.FirstOrDefault(entry => ReferenceEquals(entry.Tab, tabs[index]))?.Host;
		}

		if (incoming is null)
		{
			return;
		}

		if (!ReferenceEquals(lastSelectedTab, tabs[index]))
		{
			lastSelectedTab = tabs[index];
			SelectedTabChanged?.Invoke(this, EventArgs.Empty);
		}

		try
		{
			if (outgoing is not null && !ReferenceEquals(outgoing, incoming))
			{
				if (TabAnimation != FuchsTabAnimation.None)
				{
					await outgoing.FadeToAsync(0, 200, Easing.CubicIn);
				}

				outgoing.IsVisible = false;
				outgoing.Opacity = 1;
			}

			if (transition != selectionVersion)
			{
				return;
			}

			incoming.IsVisible = true;
			if (TabAnimation == FuchsTabAnimation.None)
			{
				return;
			}

			incoming.Opacity = 0;
			if (TabAnimation == FuchsTabAnimation.Slide)
			{
				incoming.TranslationX = index >= outgoingIndex ? 24 : -24;
				await Task.WhenAll(
					incoming.FadeToAsync(1, 300, Easing.CubicOut),
					incoming.TranslateToAsync(0, 0, 300, Easing.CubicOut));
			}
			else
			{
				await incoming.FadeToAsync(1, 300, Easing.CubicOut);
			}
		}
		catch (Exception exception)
		{
			System.Diagnostics.Trace.WriteLine($"FuchsTabs selection animation failed: {exception}");
			if (transition == selectionVersion)
			{
				incoming.IsVisible = true;
				incoming.Opacity = 1;
			}
		}
	}

	private sealed class PanelEntry(FuchsTab tab, ContentView host)
	{
		public FuchsTab Tab { get; } = tab;
		public ContentView Host { get; } = host;
	}
}