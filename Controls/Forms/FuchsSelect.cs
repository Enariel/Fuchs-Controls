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

public sealed class FuchsSelect : FormField
{
	private readonly Picker _picker;
	private readonly CollectionView _multiPicker;
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

	public FuchsSelect()
	{
		_picker = new Picker();
		_multiPicker = new CollectionView { SelectionMode = SelectionMode.Multiple, HeightRequest = 160 };
		Grid input = new();
		input.Add(_picker);
		InitializeField(input);
		AttachFocusTarget(_picker);
		AttachFocusTarget(_multiPicker);
		_picker.SelectedIndexChanged += OnPickerSelectionChanged;
		_multiPicker.SelectionChanged += OnMultiSelectionChanged;
		ApplyTheme();
	}

	protected override void ApplyTheme()
	{
		base.ApplyTheme();
		FuchsTheme theme = FuchsThemeProvider.Current;
		_picker.Title = Placeholder;
		_picker.TextColor = theme.Text;
		_picker.TitleColor = theme.TextLight;
		_picker.FontSize = ResolveFontSize();
		_picker.IsEnabled = !IsDisabled;
		_multiPicker.IsEnabled = !IsDisabled;
		_multiPicker.ItemTemplate = CreateItemTemplate(theme);
	}

	private DataTemplate CreateItemTemplate(FuchsTheme theme) => new(() =>
	{
		Label label = new Label { TextColor = theme.Text, FontSize = ResolveFontSize(), Padding = new Thickness(4, 8) };
		label.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, string.IsNullOrWhiteSpace(DisplayMemberPath) ? "." : DisplayMemberPath);
		return label;
	});

	private void RebuildSelectionView()
	{
		FieldBorder.Content = IsMultiSelect ? _multiPicker : _picker;
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