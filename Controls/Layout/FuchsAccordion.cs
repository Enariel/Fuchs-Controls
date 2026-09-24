#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls.Controls;

public sealed class FuchsAccordion : FuchsComponent
{
	private readonly Border _root;
	private readonly VerticalStackLayout _itemsLayout;
	private INotifyCollectionChanged? _observedCollection;
	private readonly List<FuchsAccordionItem> _observedItems = [];

	public static readonly BindableProperty ItemsProperty =
		BindableProperty.Create(nameof(Items), typeof(IList<FuchsAccordionItem>), typeof(FuchsAccordion), null,
			defaultValueCreator: _ => new ObservableCollection<FuchsAccordionItem>(), propertyChanged: OnItemsChanged);

	public static readonly BindableProperty AllowMultipleExpandedProperty =
		BindableProperty.Create(nameof(AllowMultipleExpanded), typeof(bool), typeof(FuchsAccordion), false, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty IsBorderedProperty =
		BindableProperty.Create(nameof(IsBordered), typeof(bool), typeof(FuchsAccordion), true, propertyChanged: OnVisualPropertyChanged);

	public IList<FuchsAccordionItem> Items
	{
		get => (IList<FuchsAccordionItem>?)GetValue(ItemsProperty) ?? Array.Empty<FuchsAccordionItem>();
		set => SetValue(ItemsProperty, value);
	}

	public bool AllowMultipleExpanded
	{
		get => (bool)GetValue(AllowMultipleExpandedProperty);
		set => SetValue(AllowMultipleExpandedProperty, value);
	}

	public bool IsBordered
	{
		get => (bool)GetValue(IsBorderedProperty);
		set => SetValue(IsBorderedProperty, value);
	}

	public FuchsAccordion()
	{
		_itemsLayout = new VerticalStackLayout { Spacing = 0 };
		_root = new Border { Content = _itemsLayout };
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
		_root.BackgroundColor = Color == FuchsControls.Theme.FuchsColor.Default ? theme.Background : theme.GetMainColor(Color);
		_root.Stroke = IsBordered ? new SolidColorBrush(theme.GetBorderColor(Color)) : Brush.Transparent;
		_root.StrokeThickness = IsBordered ? theme.BorderWidth : 0;
		_root.StrokeShape = new RoundRectangle { CornerRadius = theme.CornerRadius };
		_root.Padding = IsBordered ? 0 : theme.PanelPadding;
		_root.Opacity = IsDisabled ? 0.65 : 1;
		foreach (FuchsAccordionItem item in _observedItems)
			item.RefreshTheme();
	}

	private void ObserveItems(IList<FuchsAccordionItem> items)
	{
		StopObservingItems();
		_observedCollection = items as INotifyCollectionChanged;
		if (_observedCollection is not null)
			_observedCollection.CollectionChanged += OnItemsCollectionChanged;
		RebuildItems();
	}

	private void StopObservingItems()
	{
		if (_observedCollection is not null)
			_observedCollection.CollectionChanged -= OnItemsCollectionChanged;
		_observedCollection = null;
		foreach (FuchsAccordionItem item in _observedItems)
			item.ExpandedChanged -= OnItemExpandedChanged;
		_observedItems.Clear();
	}

	private void RebuildItems()
	{
		foreach (FuchsAccordionItem item in _observedItems)
			item.ExpandedChanged -= OnItemExpandedChanged;
		_observedItems.Clear();
		_itemsLayout.Children.Clear();
		foreach (FuchsAccordionItem item in Items.Distinct())
		{
			_observedItems.Add(item);
			item.ExpandedChanged += OnItemExpandedChanged;
			_itemsLayout.Children.Add(item);
		}

		ApplyTheme();
	}

	private void OnItemExpandedChanged(object? sender, EventArgs e)
	{
		if (!AllowMultipleExpanded && sender is FuchsAccordionItem expanded && expanded.IsExpanded)
		{
			foreach (FuchsAccordionItem item in _observedItems.Where(item => item != expanded && item.IsExpanded))
				item.SetExpandedFromParent(false);
		}
	}

	private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => RebuildItems();

	private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsAccordion accordion)
			accordion.ObserveItems(newValue as IList<FuchsAccordionItem> ?? new ObservableCollection<FuchsAccordionItem>());
	}

	private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsAccordion accordion)
			accordion.ApplyTheme();
	}
}