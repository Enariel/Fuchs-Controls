#region Meta

// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026

#endregion

namespace FuchsControls.Fields;

public class FuchsSelect<TValue> : SelectFieldBase<TValue>
{
	public static readonly BindableProperty ItemsSourceProperty =
		BindableProperty.Create(
			nameof(ItemsSource),
			typeof(IList<TValue>),
			typeof(FuchsSelect<TValue>),
			defaultValueCreator: _ => new List<TValue>());

	public IList<TValue> ItemsSource
	{
		get => (IList<TValue>)GetValue(ItemsSourceProperty);
		set => SetValue(ItemsSourceProperty, value);
	}

	public static readonly BindableProperty DisplayPathProperty =
		BindableProperty.Create(
			nameof(DisplayPath),
			typeof(string),
			typeof(FuchsSelect<TValue>),
			null,
			propertyChanged: OnDisplayPathChanged);

	public string DisplayPath
	{
		get => (string)GetValue(DisplayPathProperty);
		set => SetValue(DisplayPathProperty, value);
	}

	private readonly Picker _picker;

	public FuchsSelect()
	{
		Margin = new Thickness(2, 5, 2, 5);

		_picker = new Picker();
		_picker.SetBinding(Picker.ItemsSourceProperty, new Binding(nameof(ItemsSource), source: this));
		_picker.SetBinding(Picker.SelectedItemProperty, new Binding(nameof(SelectedValue), source: this, mode: BindingMode.TwoWay));
		_picker.SetBinding(Picker.TitleProperty, new Binding(nameof(Placeholder), source: this));

		var labelView = CreateLabelWithHelpToggle();
		var helpView = CreateHelpText();

		var stack = new StackLayout { Spacing = 5 };
		stack.SetBinding(StackLayout.OrientationProperty, new Binding(nameof(Orientation), source: this));

		stack.Children.Add(labelView);
		stack.Children.Add(_picker);
		stack.Children.Add(helpView);

		ApplyToolTip(stack);
		ApplyAccessibility(_picker);

		Content = stack;
	}

	private static void OnDisplayPathChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is not FuchsSelect<TValue> select)
			return;

		var path = newValue as string;
		select._picker.ItemDisplayBinding = string.IsNullOrWhiteSpace(path) ? null : new Binding(path);
	}
}