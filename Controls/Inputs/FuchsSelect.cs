#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using System.Collections;
using System.Collections.ObjectModel;
using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls.Controls;

public sealed class FuchsSelect : FuchsComponent
{
	private readonly Border _root;
	private readonly Picker _picker;
	private readonly CollectionView _multiPicker;
	private readonly Label _label;
	private readonly Label _helperText;
	private readonly Grid _layout;
	private bool _updatingSelection;

	public static readonly BindableProperty ItemsSourceProperty =
		BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(FuchsSelect), null, propertyChanged: OnItemsSourceChanged);

	public static readonly BindableProperty SelectedItemProperty =
		BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(FuchsSelect), null, BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);

	public static readonly BindableProperty SelectedItemsProperty =
		BindableProperty.Create(nameof(SelectedItems), typeof(IList), typeof(FuchsSelect), null, BindingMode.TwoWay,
			defaultValueCreator: _ => new ObservableCollection<object>(), propertyChanged: OnSelectedItemsChanged);

	public static readonly BindableProperty DisplayMemberPathProperty =
		BindableProperty.Create(nameof(DisplayMemberPath), typeof(string), typeof(FuchsSelect), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty IsMultiSelectProperty =
		BindableProperty.Create(nameof(IsMultiSelect), typeof(bool), typeof(FuchsSelect), false, propertyChanged: OnMultiSelectChanged);

	public static readonly BindableProperty LabelProperty =
		BindableProperty.Create(nameof(Label), typeof(string), typeof(FuchsSelect), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty PlaceholderProperty =
		BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(FuchsSelect), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty HelperTextProperty =
		BindableProperty.Create(nameof(HelperText), typeof(string), typeof(FuchsSelect), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty StateProperty =
		BindableProperty.Create(nameof(State), typeof(FuchsInputState), typeof(FuchsSelect), FuchsInputState.Normal, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty IsFocusedProperty =
		BindableProperty.Create(nameof(IsFocused), typeof(bool), typeof(FuchsSelect), false, BindingMode.OneWayToSource);

	public IEnumerable? ItemsSource
	{
		get => (IEnumerable?)GetValue(ItemsSourceProperty);
		set => SetValue(ItemsSourceProperty, value);
	}

	public object? SelectedItem
	{
		get => GetValue(SelectedItemProperty);
		set => SetValue(SelectedItemProperty, value);
	}

	public IList SelectedItems
	{
		get => (IList?)GetValue(SelectedItemsProperty) ?? new ObservableCollection<object>();
		set => SetValue(SelectedItemsProperty, value);
	}

	public string DisplayMemberPath
	{
		get => (string)GetValue(DisplayMemberPathProperty);
		set => SetValue(DisplayMemberPathProperty, value);
	}

	public bool IsMultiSelect
	{
		get => (bool)GetValue(IsMultiSelectProperty);
		set => SetValue(IsMultiSelectProperty, value);
	}

	public string Label
	{
		get => (string)GetValue(LabelProperty);
		set => SetValue(LabelProperty, value);
	}

	public string Placeholder
	{
		get => (string)GetValue(PlaceholderProperty);
		set => SetValue(PlaceholderProperty, value);
	}

	public string HelperText
	{
		get => (string)GetValue(HelperTextProperty);
		set => SetValue(HelperTextProperty, value);
	}

	public FuchsInputState State
	{
		get => (FuchsInputState)GetValue(StateProperty);
		set => SetValue(StateProperty, value);
	}

	public bool IsFocused
	{
		get => (bool)GetValue(IsFocusedProperty);
		private set => SetValue(IsFocusedProperty, value);
	}

	public FuchsSelect()
	{
		_label = new Label();
		_helperText = new Label();
		_picker = new Picker();
		_multiPicker = new CollectionView { SelectionMode = SelectionMode.Multiple, HeightRequest = 160 };
		_picker.SelectedIndexChanged += OnPickerSelectionChanged;
		_multiPicker.SelectionChanged += OnMultiSelectionChanged;
		_picker.Focused += OnFocused;
		_picker.Unfocused += OnUnfocused;
		_multiPicker.Focused += OnFocused;
		_multiPicker.Unfocused += OnUnfocused;

		_root = new Border { Content = _picker };
		_layout = new Grid
		{
			RowDefinitions = { new RowDefinition(GridLength.Auto), new RowDefinition(GridLength.Auto), new RowDefinition(GridLength.Auto) }, RowSpacing = 4
		};
		_layout.Add(_label, 0, 0);
		_layout.Add(_root, 0, 1);
		_layout.Add(_helperText, 0, 2);
		Content = _layout;
		ApplyTheme();
	}

	protected override void ApplyTheme()
	{
		FuchsTheme theme = FuchsThemeProvider.Current;
		Color stateColor = State switch
		{
			FuchsInputState.Valid => theme.Success, FuchsInputState.Warning => theme.Warning, FuchsInputState.Invalid => theme.Danger
			, _ => theme.BackgroundDarker
		};
		Color borderColor = IsFocused ? theme.Primary : stateColor;

		_label.Text = Label;
		_label.IsVisible = !string.IsNullOrWhiteSpace(Label);
		_label.TextColor = theme.Text;
		_label.FontSize = theme.FontSizeSm;
		_helperText.Text = HelperText;
		_helperText.IsVisible = !string.IsNullOrWhiteSpace(HelperText);
		_helperText.TextColor = State switch
		{
			FuchsInputState.Valid => theme.Success, FuchsInputState.Warning => theme.Warning, FuchsInputState.Invalid => theme.Danger, _ => theme.TextLight
		};
		_helperText.FontSize = theme.FontSizeSm;

		_picker.Title = Placeholder;
		_picker.TextColor = theme.Text;
		_picker.TitleColor = theme.TextLight;
		_picker.FontSize = ResolveFontSize();
		_picker.IsEnabled = !IsDisabled;
		_multiPicker.IsEnabled = !IsDisabled;
		_multiPicker.ItemTemplate = CreateItemTemplate(theme);
		_root.BackgroundColor = Variant == FuchsVariant.Text ? Colors.Transparent : theme.BackgroundDark;
		_root.Stroke = Variant == FuchsVariant.Filled && !IsFocused ? Brush.Transparent : new SolidColorBrush(borderColor);
		_root.StrokeThickness = Variant == FuchsVariant.Filled && !IsFocused ? 0 : theme.BorderWidth;
		_root.StrokeShape = new RoundRectangle { CornerRadius = theme.CornerRadius };
		_root.Padding = ResolvePadding();
		_root.Opacity = IsDisabled ? 0.65 : 1;
	}

	private DataTemplate CreateItemTemplate(FuchsTheme theme) => new(() =>
	{
		Label label = new() { TextColor = theme.Text, FontSize = ResolveFontSize(), Padding = new Thickness(4, 8) };
		label.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, string.IsNullOrWhiteSpace(DisplayMemberPath) ? "." : DisplayMemberPath);
		return label;
	});

	private void RebuildSelectionView()
	{
		_root.Content = IsMultiSelect ? _multiPicker : _picker;
		ApplyTheme();
		if (IsMultiSelect)
			_multiPicker.SelectedItems = SelectedItems.Cast<object>().ToList();
		else
			_picker.SelectedItem = SelectedItem;
	}

	private void OnPickerSelectionChanged(object? sender, EventArgs e)
	{
		if (!_updatingSelection)
			SelectedItem = _picker.SelectedItem;
	}

	private void OnMultiSelectionChanged(object? sender, SelectionChangedEventArgs e)
	{
		if (_updatingSelection)
			return;

		_updatingSelection = true;
		try
		{
			SelectedItems = new ObservableCollection<object>(e.CurrentSelection.Cast<object>());
		}
		finally
		{
			_updatingSelection = false;
		}
	}

	private void OnFocused(object? sender, FocusEventArgs e)
	{
		IsFocused = true;
		ApplyTheme();
	}

	private void OnUnfocused(object? sender, FocusEventArgs e)
	{
		IsFocused = false;
		ApplyTheme();
	}

	private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsSelect select)
		{
			select._picker.ItemsSource = newValue as IList;
			select._multiPicker.ItemsSource = newValue as IEnumerable;
		}
	}

	private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsSelect select && !select._updatingSelection)
			select._picker.SelectedItem = newValue;
	}

	private static void OnSelectedItemsChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsSelect select && !select._updatingSelection && newValue is IList items)
			select._multiPicker.SelectedItems = items.Cast<object>().ToList();
	}

	private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsSelect select)
			select.ApplyTheme();
	}

	private static void OnMultiSelectChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsSelect select)
			select.RebuildSelectionView();
	}
}