using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Microsoft.Maui.Controls.Xaml;

namespace FuchsControls;

[ContentProperty(nameof(Items))]
public sealed class FuchsRadioButtonGroup : FuchsFieldBase
{
	private readonly VerticalStackLayout _itemsLayout = new() { Spacing = 4 };
	private readonly string _groupName = Guid.NewGuid().ToString("N");
	private bool _isUpdating;

	public static readonly BindableProperty SelectedValueProperty = BindableProperty.Create(
		nameof(SelectedValue), typeof(object), typeof(FuchsRadioButtonGroup), null, BindingMode.TwoWay, propertyChanged: OnSelectedValueChanged);

	public ObservableCollection<FuchsRadioButton> Items { get; } = [];

	public FuchsRadioButtonGroup()
	{
		Items.CollectionChanged += OnItemsChanged;
		SetInput(_itemsLayout);
	}

	public object? SelectedValue
	{
		get => GetValue(SelectedValueProperty);
		set => SetValue(SelectedValueProperty, value);
	}

	private static void OnSelectedValueChanged(BindableObject bindable, object oldValue, object newValue) =>
		((FuchsRadioButtonGroup)bindable).UpdateSelectedValue();

	private void OnItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		_itemsLayout.Children.Clear();
		foreach (var item in Items)
		{
			item.GroupName = _groupName;
			item.PropertyChanged += OnItemPropertyChanged;
			_itemsLayout.Children.Add(item);
		}

		UpdateSelectedValue();
	}

	private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (!_isUpdating && e.PropertyName == nameof(FuchsRadioButton.IsSelected) && sender is FuchsRadioButton { IsSelected: true } item)
		{
			SetValue(SelectedValueProperty, item.Value);
		}
	}

	private void UpdateSelectedValue()
	{
		_isUpdating = true;
		foreach (var item in Items)
		{
			item.IsSelected = Equals(item.Value, SelectedValue);
		}

		_isUpdating = false;
	}
}