using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls.Controls;

public class FuchsField : FuchsComponent
{
	private readonly Border _root;
	private readonly Entry _entry;
	private readonly Label _label;
	private readonly Label _helperText;

	public static readonly BindableProperty LabelProperty =
		BindableProperty.Create(nameof(Label), typeof(string), typeof(FuchsField), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(FuchsField), string.Empty, BindingMode.TwoWay, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty PlaceholderProperty =
		BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(FuchsField), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty HelperTextProperty =
		BindableProperty.Create(nameof(HelperText), typeof(string), typeof(FuchsField), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty StateProperty =
		BindableProperty.Create(nameof(State), typeof(FuchsInputState), typeof(FuchsField), FuchsInputState.Normal, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty IsPasswordProperty =
		BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(FuchsField), false, propertyChanged: OnVisualPropertyChanged);

	public string Label
	{
		get => (string)GetValue(LabelProperty);
		set => SetValue(LabelProperty, value);
	}

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
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

	public bool IsPassword
	{
		get => (bool)GetValue(IsPasswordProperty);
		set => SetValue(IsPasswordProperty, value);
	}

	public FuchsField()
	{
		_label = new Label();

		_entry = new Entry
		{
			BackgroundColor = Colors.Transparent,
			ClearButtonVisibility = ClearButtonVisibility.WhileEditing
		};

		_entry.SetBinding(Entry.TextProperty, new Binding(nameof(Text), source: this, mode: BindingMode.TwoWay));

		_helperText = new Label();

		_root = new Border
		{
			Content = _entry
		};

		Content = new VerticalStackLayout
		{
			Spacing = 4,
			Children =
			{
				_label,
				_root,
				_helperText
			}
		};

		ApplyTheme();
	}

	protected override void ApplyTheme()
	{
		FuchsTheme theme = FuchsThemeProvider.Current;

		Color border = State switch
		{
			FuchsInputState.Valid => theme.Success,
			FuchsInputState.Warning => theme.Warning,
			FuchsInputState.Invalid => theme.Danger,
			_ => theme.BackgroundDarker
		};

		_label.Text = Label;
		_label.IsVisible = !string.IsNullOrWhiteSpace(Label);
		_label.TextColor = theme.Text;
		_label.FontSize = theme.FontSizeSm;

		_entry.Placeholder = Placeholder;
		_entry.TextColor = theme.Text;
		_entry.PlaceholderColor = theme.TextLight;
		_entry.FontSize = ResolveFontSize();
		_entry.IsPassword = IsPassword;
		_entry.IsEnabled = !IsDisabled;

		_root.BackgroundColor = theme.BackgroundDark;
		_root.Stroke = new SolidColorBrush(border);
		_root.StrokeThickness = theme.BorderWidth;
		_root.StrokeShape = new RoundRectangle { CornerRadius = theme.CornerRadius };
		_root.Padding = theme.ControlPadding;
		_root.Opacity = IsDisabled ? 0.65 : 1;

		_helperText.Text = HelperText;
		_helperText.IsVisible = !string.IsNullOrWhiteSpace(HelperText);
		_helperText.FontSize = theme.FontSizeSm;
		_helperText.TextColor = State switch
		{
			FuchsInputState.Valid => theme.Success,
			FuchsInputState.Warning => theme.Warning,
			FuchsInputState.Invalid => theme.Danger,
			_ => theme.TextLight
		};
	}

	private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsField field)
			field.ApplyTheme();
	}
}
