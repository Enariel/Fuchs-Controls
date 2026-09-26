using System.Collections;

namespace FuchsControls;

public sealed class FuchsSelect : FuchsFieldBase
{
	private readonly Picker _picker = new Picker().ApplyFuchsPickerStyle();
	private bool _isUpdating;

	public static readonly BindableProperty ItemsSourceProperty =
		BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(FuchsSelect), null, propertyChanged: OnSelectChanged);

	public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(FuchsSelect), null
		, BindingMode.TwoWay, propertyChanged: OnSelectChanged);

	public static readonly BindableProperty TitleProperty =
		BindableProperty.Create(nameof(Title), typeof(string), typeof(FuchsSelect), string.Empty, propertyChanged: OnSelectChanged);

	public FuchsSelect()
	{
		_picker.SelectedIndexChanged += (_, _) =>
		{
			if (!_isUpdating) SetValue(SelectedItemProperty, _picker.SelectedItem);
		};
		SetInput(_picker);
		UpdatePicker();
	}

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

	public string Title
	{
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}

	private static void OnSelectChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsSelect)bindable).UpdatePicker();

	private void UpdatePicker()
	{
		_isUpdating = true;
		_picker.ItemsSource = ItemsSource as IList ?? ItemsSource?.Cast<object>().ToList();
		_picker.SelectedItem = SelectedItem;
		_picker.Title = Title;
		_isUpdating = false;
	}
}