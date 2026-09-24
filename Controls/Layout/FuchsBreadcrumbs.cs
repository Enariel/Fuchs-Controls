#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;
using ThemeColor = FuchsControls.Theme.FuchsColor;

namespace FuchsControls.Controls;

public sealed class FuchsBreadcrumbs : FuchsComponent
{
	private readonly Border _root;
	private readonly HorizontalStackLayout _itemsLayout;
	private readonly ScrollView _scroller;
	private readonly List<FuchsBreadcrumbItem> _observedItems = [];
	private INotifyCollectionChanged? _observedCollection;

	public static readonly BindableProperty ItemsProperty =
		BindableProperty.Create(nameof(Items), typeof(IList<FuchsBreadcrumbItem>), typeof(FuchsBreadcrumbs),
			defaultValueCreator: _ => new ObservableCollection<FuchsBreadcrumbItem>(), propertyChanged: OnItemsChanged);

	public static readonly BindableProperty IsBorderedProperty =
		BindableProperty.Create(nameof(IsBordered), typeof(bool), typeof(FuchsBreadcrumbs), true, propertyChanged: OnVisualPropertyChanged);

	public IList<FuchsBreadcrumbItem> Items
	{
		get => (IList<FuchsBreadcrumbItem>?)GetValue(ItemsProperty) ?? Array.Empty<FuchsBreadcrumbItem>();
		set => SetValue(ItemsProperty, value);
	}

	public bool IsBordered
	{
		get => (bool)GetValue(IsBorderedProperty);
		set => SetValue(IsBorderedProperty, value);
	}

	public event EventHandler? ItemClicked;

	public FuchsBreadcrumbs()
	{
		_itemsLayout = new HorizontalStackLayout { Spacing = 0 };
		_scroller = new ScrollView
		{
			Orientation = ScrollOrientation.Horizontal,
			HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
			Content = _itemsLayout
		};
		_root = new Border { Content = _scroller };
		Content = _root;
		ObserveItems(Items);
		ApplyTheme();
	}

	protected override void OnHandlerChanging(HandlerChangingEventArgs args)
	{
		if (args.NewHandler is null)
			StopObservingItems();
		else if (args.OldHandler is null)
			ObserveItems(Items);

		base.OnHandlerChanging(args);
	}

	protected override void ApplyTheme()
	{
		FuchsTheme theme = FuchsThemeProvider.Current;
		Color background = Color == ThemeColor.Default ? theme.Background : theme.GetMainColor(Color);
		Color border = Color == ThemeColor.Default ? theme.BackgroundDarker : theme.GetBorderColor(Color);
		Color text = theme.GetTextColor(Color, Variant);

		_root.BackgroundColor = background;
		_root.Stroke = IsBordered ? new SolidColorBrush(border) : Brush.Transparent;
		_root.StrokeThickness = IsBordered ? theme.BorderWidth : 0;
		_root.StrokeShape = new RoundRectangle { CornerRadius = theme.CornerRadius };
		_root.Opacity = IsDisabled ? 0.65 : 1;

		for (int index = 0; index < _itemsLayout.Children.Count; index++)
		{
			if (_itemsLayout.Children[index] is Label label && label.BindingContext is FuchsBreadcrumbItem item)
			{
				label.TextColor = item.IsCurrent ? theme.Primary : text;
				label.FontSize = ResolveFontSize();
				label.FontAttributes = item.IsCurrent ? FontAttributes.Bold : FontAttributes.None;
				label.Opacity = item.IsEnabled && !IsDisabled ? 1 : 0.5;
			}
			else if (_itemsLayout.Children[index] is Label separator)
			{
				separator.TextColor = border;
				separator.FontSize = ResolveFontSize();
				separator.Opacity = 0.75;
			}
		}
	}

	private void ObserveItems(IList<FuchsBreadcrumbItem> items)
	{
		StopObservingItems();
		_observedCollection = items as INotifyCollectionChanged;
		if (_observedCollection is not null)
			_observedCollection.CollectionChanged += OnItemsCollectionChanged;

		_observedItems.AddRange(items);
		foreach (FuchsBreadcrumbItem item in _observedItems)
			item.PropertyChanged += OnItemPropertyChanged;

		RebuildItems();
	}

	private void StopObservingItems()
	{
		if (_observedCollection is not null)
			_observedCollection.CollectionChanged -= OnItemsCollectionChanged;

		_observedCollection = null;
		foreach (FuchsBreadcrumbItem item in _observedItems)
			item.PropertyChanged -= OnItemPropertyChanged;
		_observedItems.Clear();
	}

	private void RebuildItems()
	{
		_itemsLayout.Children.Clear();
		for (int index = 0; index < Items.Count; index++)
		{
			if (index > 0)
				_itemsLayout.Children.Add(new Label { Text = "›", VerticalTextAlignment = TextAlignment.Center, Padding = new Thickness(8, 0) });

			FuchsBreadcrumbItem item = Items[index];
			Label label = new()
			{
				BindingContext = item,
				VerticalTextAlignment = TextAlignment.Center,
				Padding = new Thickness(12, 10),
				LineBreakMode = LineBreakMode.NoWrap
			};
			TapGestureRecognizer tap = new();
			tap.Tapped += (_, _) => ExecuteItem(item);
			label.GestureRecognizers.Add(tap);
			AutomationProperties.SetName(label, item.Text);
			AutomationProperties.SetHelpText(label, item.IsCurrent ? "Current page" : "Navigate");
			_itemsLayout.Children.Add(label);
		}

		ApplyTheme();
	}

	private void ExecuteItem(FuchsBreadcrumbItem item)
	{
		if (IsDisabled || !item.IsEnabled || item.IsCurrent)
			return;

		if (item.Command?.CanExecute(item.CommandParameter) == true)
			item.Command.Execute(item.CommandParameter);

		ItemClicked?.Invoke(this, EventArgs.Empty);
	}

	private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
	{
		foreach (FuchsBreadcrumbItem item in _observedItems)
			item.PropertyChanged -= OnItemPropertyChanged;
		_observedItems.Clear();
		_observedItems.AddRange(Items);
		foreach (FuchsBreadcrumbItem item in _observedItems)
			item.PropertyChanged += OnItemPropertyChanged;
		RebuildItems();
	}

	private void OnItemPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs args) => RebuildItems();

	private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsBreadcrumbs breadcrumbs)
			breadcrumbs.ObserveItems(newValue as IList<FuchsBreadcrumbItem> ?? new ObservableCollection<FuchsBreadcrumbItem>());
	}

	private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsBreadcrumbs breadcrumbs)
			breadcrumbs.ApplyTheme();
	}
}
