namespace FuchsControls;

public sealed class FuchsColorPicker : FuchsFieldBase
{
	private readonly Entry _entry = new Entry { Placeholder = "#RRGGBB" }.ApplyFuchsEntryStyle();
	private readonly BoxView _preview = new() { WidthRequest = 28, HeightRequest = 28, CornerRadius = 3, VerticalOptions = LayoutOptions.Center };
	private bool _isUpdating;

	public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(Color), typeof(FuchsColorPicker), Colors.Black
		, BindingMode.TwoWay, propertyChanged: OnColorChanged);

	public FuchsColorPicker()
	{
		_entry.TextChanged += OnTextChanged;
		SetInput(new HorizontalStackLayout { Spacing = 8, Children = { _preview, _entry } });
		UpdateColor();
	}

	public Color Value
	{
		get => (Color)GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}

	private static void OnColorChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsColorPicker)bindable).UpdateColor();

	private void OnTextChanged(object? sender, TextChangedEventArgs e)
	{
		if (_isUpdating)
		{
			return;
		}

		var color = Color.FromArgb(e.NewTextValue);
		if (color is null)
		{
			InputState = FuchsInputState.Invalid;
			return;
		}

		InputState = FuchsInputState.Normal;
		SetValue(ValueProperty, color);
	}

	private void UpdateColor()
	{
		_isUpdating = true;
		_preview.Color = Value;
		_entry.Text = Value.ToArgbHex();
		_isUpdating = false;
	}
}