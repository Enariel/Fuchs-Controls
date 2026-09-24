using FuchsControls.Behaviours;
using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls.Controls;

public abstract class FuchsBoolBase : FormField
{
	private readonly List<FocusHighlightBehavior> _focusBehaviors = [];
	private View? _input;
	private Border _root = null!;
	private Label _label = null!;
	private Label _helpText = null!;

	public new static readonly BindableProperty LabelProperty =
		BindableProperty.Create(nameof(Label), typeof(string), typeof(FuchsBoolBase), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public new static readonly BindableProperty HelpTextProperty =
		BindableProperty.Create(nameof(HelpText), typeof(string), typeof(FuchsBoolBase), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public new static readonly BindableProperty StateProperty =
		BindableProperty.Create(nameof(State), typeof(FuchsInputState), typeof(FuchsBoolBase), FuchsInputState.Normal
			, propertyChanged: OnVisualPropertyChanged);

	public new static readonly BindableProperty IsFocusedProperty =
		BindableProperty.Create(nameof(IsFocused), typeof(bool), typeof(FuchsBoolBase), false, BindingMode.OneWayToSource);

	public new string Label
	{
		get => (string)GetValue(LabelProperty);
		set => SetValue(LabelProperty, value);
	}

	public new string HelpText
	{
		get => (string)GetValue(HelpTextProperty);
		set => SetValue(HelpTextProperty, value);
	}

	public new FuchsInputState State
	{
		get => (FuchsInputState)GetValue(StateProperty);
		set => SetValue(StateProperty, value);
	}

	public new bool IsFocused
	{
		get => (bool)GetValue(IsFocusedProperty);
		private set => SetValue(IsFocusedProperty, value);
	}

	protected new View Input => _input ?? throw new InvalidOperationException("The boolean control has not been initialized.");

	protected Border ControlBorder => _root;

	protected void InitializeBoolControl(View input)
	{
		_input = input;
		_label = new Label { VerticalTextAlignment = TextAlignment.Center };
		_helpText = new Label();
		_root = new Border();

		HorizontalStackLayout row = new() { Spacing = 10 };
		row.Children.Add(input);
		row.Children.Add(_label);
		_root.Content = row;

		VerticalStackLayout layout = new() { Spacing = 4 };
		layout.Children.Add(_root);
		layout.Children.Add(_helpText);
		Content = layout;

		FocusHighlightBehavior behavior = new() { Target = _root };
		input.Behaviors.Add(behavior);
		input.Focused += OnInputFocused;
		input.Unfocused += OnInputUnfocused;
		_focusBehaviors.Add(behavior);
	}

	protected override void ApplyTheme()
	{
		if (_input is null)
			return;

		FuchsTheme theme = FuchsThemeProvider.Current;
		Color borderColor = State switch
		{
			FuchsInputState.Valid => theme.Success, FuchsInputState.Warning => theme.Warning, FuchsInputState.Invalid => theme.Danger
			, _ => theme.BackgroundDarker
		};

		_label.Text = Label;
		_label.TextColor = theme.Text;
		_label.FontSize = ResolveFontSize();
		_helpText.Text = HelpText;
		_helpText.IsVisible = !string.IsNullOrWhiteSpace(HelpText);
		_helpText.TextColor = State switch
		{
			FuchsInputState.Valid => theme.Success, FuchsInputState.Warning => theme.Warning, FuchsInputState.Invalid => theme.Danger, _ => theme.TextLight
		};
		_helpText.FontSize = theme.FontSizeSm;

		_root.BackgroundColor = Variant == FuchsVariant.Text ? Colors.Transparent : theme.BackgroundDark;
		_root.StrokeShape = new RoundRectangle { CornerRadius = theme.CornerRadius };
		_root.Padding = ResolvePadding();
		_root.Opacity = IsDisabled ? 0.65 : 1;
		_root.Stroke = IsFocused ? new SolidColorBrush(theme.Primary) : new SolidColorBrush(borderColor);
		_root.StrokeThickness = IsFocused || Variant != FuchsVariant.Filled ? theme.BorderWidth : 0;

		foreach (FocusHighlightBehavior behavior in _focusBehaviors)
		{
			behavior.FocusColor = theme.Primary;
			behavior.FocusStrokeThickness = theme.BorderWidth;
			behavior.NormalStroke = new SolidColorBrush(borderColor);
			behavior.NormalStrokeThickness = Variant == FuchsVariant.Filled ? 0 : theme.BorderWidth;
			behavior.Refresh();
		}

		ApplyInputTheme(theme);
	}

	protected abstract void ApplyInputTheme(FuchsTheme theme);

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
		if (bindable is FuchsBoolBase control)
			control.ApplyTheme();
	}
}