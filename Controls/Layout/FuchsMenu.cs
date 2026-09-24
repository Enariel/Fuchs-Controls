#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using ThemeColor = FuchsControls.Theme.FuchsColor;

namespace FuchsControls.Controls;

public sealed class FuchsMenu : FuchsComponent
{
	private readonly Border _root;
	private readonly FlexLayout _itemsLayout;
	private readonly List<FuchsMenuItem> _observedItems = [];
	private INotifyCollectionChanged? _observedCollection;

	public static readonly BindableProperty ItemsProperty =
		BindableProperty.Create(nameof(Items), typeof(IList<FuchsMenuItem>), typeof(FuchsMenu),
			defaultValueCreator: _ => new ObservableCollection<FuchsMenuItem>(), propertyChanged: OnItemsChanged);

	public static readonly BindableProperty IsHorizontalProperty =
		BindableProperty.Create(nameof(IsHorizontal), typeof(bool), typeof(FuchsMenu), false, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty IsBorderedProperty =
		BindableProperty.Create(nameof(IsBordered), typeof(bool), typeof(FuchsMenu), true, propertyChanged: OnVisualPropertyChanged);

	public IList<FuchsMenuItem> Items
	{
		get => (IList<FuchsMenuItem>?)GetValue(ItemsProperty) ?? Array.Empty<FuchsMenuItem>();
		set => SetValue(ItemsProperty, value);
	}

	public bool IsHorizontal
	{
		get => (bool)GetValue(IsHorizontalProperty);
		set => SetValue(IsHorizontalProperty, value);
	}

	public bool IsBordered
	{
		get => (bool)GetValue(IsBorderedProperty);
		set => SetValue(IsBorderedProperty, value);
	}

	public event EventHandler? ItemClicked;

	public FuchsMenu()
	{
		_itemsLayout = new FlexLayout { Direction = FlexDirection.Column, AlignItems = FlexAlignItems.Stretch, Padding = new Thickness(8) };
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
		Color background = Color == ThemeColor.Default ? theme.BackgroundDark : theme.GetMainColor(Color);
		Color border = Color == ThemeColor.Default ? theme.BackgroundDarker : theme.GetBorderColor(Color);
		Color text = theme.GetTextColor(Color, Variant);

		_root.BackgroundColor = background;
		_root.Stroke = IsBordered ? new SolidColorBrush(border) : Brush.Transparent;
		_root.StrokeThickness = IsBordered ? theme.BorderWidth : 0;
		_root.StrokeShape = new RoundRectangle { CornerRadius = theme.CornerRadius };
		_root.Opacity = IsDisabled ? 0.65 : 1;
		_itemsLayout.Direction = IsHorizontal ? FlexDirection.Row : FlexDirection.Column;
		_itemsLayout.Wrap = IsHorizontal ? FlexWrap.NoWrap : FlexWrap.NoWrap;

		foreach (Label label in _itemsLayout.Children.OfType<Label>())
		{
			if (label.BindingContext is not FuchsMenuItem item)
				continue;

			label.TextColor = item.IsActive ? text : text;
			label.BackgroundColor = item.IsActive ? border : background;
			label.FontSize = ResolveFontSize();
			label.FontAttributes = item.IsActive ? FontAttributes.Bold : FontAttributes.None;
			label.Opacity = item.IsEnabled && !IsDisabled ? 1 : 0.5;
		}
	}

	private void ObserveItems(IList<FuchsMenuItem> items)
	{
		StopObservingItems();
		_observedCollection = items as INotifyCollectionChanged;
		if (_observedCollection is not null)
			_observedCollection.CollectionChanged += OnItemsCollectionChanged;

		_observedItems.AddRange(items);
		foreach (FuchsMenuItem item in _observedItems)
			item.PropertyChanged += OnItemPropertyChanged;

		RebuildItems();
	}

	private void StopObservingItems()
	{
		if (_observedCollection is not null)
			_observedCollection.CollectionChanged -= OnItemsCollectionChanged;

		_observedCollection = null;
		foreach (FuchsMenuItem item in _observedItems)
			item.PropertyChanged -= OnItemPropertyChanged;
		_observedItems.Clear();
	}

	private void RebuildItems()
	{
		_itemsLayout.Children.Clear();
		foreach (FuchsMenuItem item in Items.Distinct())
		{
			Label label = new()
			{
				BindingContext = item,
				Text = item.Text,
				Padding = ResolvePadding(),
				VerticalTextAlignment = TextAlignment.Center,
				LineBreakMode = LineBreakMode.NoWrap
			};
			TapGestureRecognizer tap = new();
			tap.Tapped += (_, _) => ExecuteItem(item);
			label.GestureRecognizers.Add(tap);
			AutomationProperties.SetName(label, item.Text);
			AutomationProperties.SetHelpText(label, item.IsActive ? "Selected menu item" : "Select menu item");
			_itemsLayout.Children.Add(label);
		}

		ApplyTheme();
	}

	private void ExecuteItem(FuchsMenuItem item)
	{
		if (IsDisabled || !item.IsEnabled)
			return;

		if (item.Command?.CanExecute(item.CommandParameter) == true)
			item.Command.Execute(item.CommandParameter);

		ItemClicked?.Invoke(this, EventArgs.Empty);
	}

	private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
	{
		foreach (FuchsMenuItem item in _observedItems)
			item.PropertyChanged -= OnItemPropertyChanged;
		_observedItems.Clear();
		_observedItems.AddRange(Items);
		foreach (FuchsMenuItem item in _observedItems)
			item.PropertyChanged += OnItemPropertyChanged;
		RebuildItems();
	}

	private void OnItemPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs args) => RebuildItems();

	private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsMenu menu)
			menu.ObserveItems(newValue as IList<FuchsMenuItem> ?? new ObservableCollection<FuchsMenuItem>());
	}

	private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsMenu menu)
			menu.ApplyTheme();
	}
}
