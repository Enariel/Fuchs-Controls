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

		var label = new Label { FontSize = 16 };
		label.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(Label), source: this, mode: BindingMode.OneWay));

		_picker = new Picker();
		_picker.SetBinding(Picker.ItemsSourceProperty, new Binding(nameof(ItemsSource), source: this, mode: BindingMode.OneWay));
		_picker.SetBinding(Picker.SelectedItemProperty, new Binding(nameof(SelectedValue), source: this, mode: BindingMode.TwoWay));
		_picker.SetBinding(Picker.TitleProperty, new Binding(nameof(Placeholder), source: this, mode: BindingMode.OneWay));

		var stack = new StackLayout { Spacing = 5 };
		stack.SetBinding(StackLayout.OrientationProperty, new Binding(nameof(Orientation), source: this, mode: BindingMode.OneWay));
		stack.Children.Add(label);
		stack.Children.Add(_picker);

#if WINDOWS
		ToolTipProperties.SetText(
			stack,
			new Binding(
				nameof(HelpText),
				source: this,
				converter: new FuchsControls.EmptyStringToNullConverter()));
#else
		var helpText = new Label { FontSize = 12 };
		helpText.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(HelpText), source: this));
		helpText.SetBinding(VisualElement.IsVisibleProperty,
			new Binding(nameof(HelpText), source: this, converter: new FuchsControls.StringIsNotNullOrEmptyConverter()));
		stack.Children.Add(helpText);
#endif

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