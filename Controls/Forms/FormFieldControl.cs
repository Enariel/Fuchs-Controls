using FuchsControls.Behaviours;
using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls.Controls;

public abstract class FormFieldControl : FuchsComponent
{
	private readonly List<FocusHighlightBehavior> _focusBehaviors = [];
	private View? _input;
	private Border _root = null!;
	private Label _label = null!;
	private Label _helpText = null!;

	public static readonly BindableProperty LabelProperty =
		BindableProperty.Create(nameof(Label), typeof(string), typeof(FormFieldControl), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty HelpTextProperty =
		BindableProperty.Create(nameof(HelpText), typeof(string), typeof(FormFieldControl), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(FormFieldControl), string.Empty, BindingMode.TwoWay
			, propertyChanged: OnTextPropertyChanged);

	public static readonly BindableProperty PlaceholderProperty =
		BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(FormFieldControl), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty StateProperty =
		BindableProperty.Create(nameof(State), typeof(FuchsInputState), typeof(FormFieldControl), FuchsInputState.Normal
			, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty IsPasswordProperty =
		BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(FormFieldControl), false, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty KeyboardProperty =
		BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(FormFieldControl), Keyboard.Default, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty MaxLengthProperty =
		BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(FormFieldControl), -1, propertyChanged: OnVisualPropertyChanged);

	public new static readonly BindableProperty IsFocusedProperty =
		BindableProperty.Create(nameof(IsFocused), typeof(bool), typeof(FormFieldControl), false, BindingMode.OneWayToSource);

	public static readonly BindableProperty HelperTextProperty = HelpTextProperty;

	public string Label
	{
		get => (string)GetValue(LabelProperty);
		set => SetValue(LabelProperty, value);
	}

	public string HelpText
	{
		get => (string)GetValue(HelpTextProperty);
		set => SetValue(HelpTextProperty, value);
	}

	public string HelperText
	{
		get => HelpText;
		set => HelpText = value;
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

	public Keyboard Keyboard
	{
		get => (Keyboard)GetValue(KeyboardProperty);
		set => SetValue(KeyboardProperty, value);
	}

	public int MaxLength
	{
		get => (int)GetValue(MaxLengthProperty);
		set => SetValue(MaxLengthProperty, value);
	}

	public new bool IsFocused
	{
		get => (bool)GetValue(IsFocusedProperty);
		private set => SetValue(IsFocusedProperty, value);
	}

	protected View Input => _input ?? throw new InvalidOperationException("The form field has not been initialized.");

	protected Border FieldBorder => _root;

	protected void InitializeField(View input)
	{
		if (_input is not null)
			throw new InvalidOperationException("A form field can only be initialized once.");

		_input = input;
		_label = new Label();
		_helpText = new Label();
		_root = new Border { Content = input };

		VerticalStackLayout layout = new() { Spacing = 4 };
		layout.Children.Add(_label);
		layout.Children.Add(_root);
		layout.Children.Add(_helpText);
		Content = layout;

		if (input is VisualElement visual)
			AttachFocusTarget(visual);

		ApplyTheme();
	}

	protected void AttachFocusTarget(VisualElement target)
	{
		FocusHighlightBehavior behavior = new()
		{
			Target = _root, FocusColor = FuchsThemeProvider.Current.Primary, FocusStrokeThickness = FuchsThemeProvider.Current.BorderWidth
		};
		target.Behaviors.Add(behavior);
		target.Focused += OnInputFocused;
		target.Unfocused += OnInputUnfocused;
		_focusBehaviors.Add(behavior);
	}

	protected override void ApplyTheme()
	{
		if (_input is null)
			return;

		FuchsTheme theme = FuchsThemeProvider.Current;
		Color borderColor = ResolveStateColor(theme);

		_label.Text = Label;
		_label.IsVisible = !string.IsNullOrWhiteSpace(Label);
		_label.TextColor = theme.Text;
		_label.FontSize = theme.FontSizeSm;
		_helpText.Text = HelpText;
		_helpText.IsVisible = !string.IsNullOrWhiteSpace(HelpText);
		_helpText.TextColor = ResolveStateColor(theme, theme.TextLight);
		_helpText.FontSize = theme.FontSizeSm;

		if (_input is InputView input)
		{
			input.SetBinding(InputView.TextProperty, new Binding(nameof(Text), source: this, mode: BindingMode.TwoWay));
			input.Placeholder = Placeholder;
			input.TextColor = theme.Text;
			input.PlaceholderColor = theme.TextLight;
			input.FontSize = ResolveFontSize();
			input.Keyboard = Keyboard;
			input.MaxLength = MaxLength;
			if (input is Entry entry)
				entry.IsPassword = IsPassword;
			SemanticProperties.SetHint(input, HelpText);
		}

		_root.BackgroundColor = Variant == FuchsVariant.Text ? Colors.Transparent : theme.BackgroundDark;
		_root.StrokeShape = new RoundRectangle { CornerRadius = theme.CornerRadius };
		_root.Padding = ResolvePadding();
		_root.Opacity = IsDisabled ? 0.65 : 1;

		Brush normalStroke = Variant == FuchsVariant.Filled && !IsFocused
			? Brush.Transparent
			: new SolidColorBrush(borderColor);
		_root.Stroke = IsFocused ? new SolidColorBrush(theme.Primary) : normalStroke;
		_root.StrokeThickness = Variant == FuchsVariant.Filled && !IsFocused ? 0 : theme.BorderWidth;
		foreach (FocusHighlightBehavior behavior in _focusBehaviors)
		{
			behavior.FocusColor = theme.Primary;
			behavior.FocusStrokeThickness = theme.BorderWidth;
			behavior.NormalStroke = normalStroke;
			behavior.NormalStrokeThickness = Variant == FuchsVariant.Filled ? 0 : theme.BorderWidth;
			behavior.Refresh();
		}
	}

	protected virtual void OnTextChanged(string text) { }

	protected Color ResolveStateColor(FuchsTheme theme, Color? normal = null) => State switch
	{
		FuchsInputState.Valid => theme.Success, FuchsInputState.Warning => theme.Warning, FuchsInputState.Invalid => theme.Danger
		, _ => normal ?? theme.BackgroundDarker
	};

	private void OnInputFocused(object? sender, FocusEventArgs e)
	{
		IsFocused = true;
		ApplyTheme();
	}

	private void OnInputUnfocused(object? sender, FocusEventArgs e)
	{
		IsFocused = false;
		ApplyTheme();
	}

	private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FormFieldControl field)
			field.ApplyTheme();
	}

	private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FormFieldControl field)
		{
			field.ApplyTheme();
			field.OnTextChanged(field.Text);
		}
	}
}