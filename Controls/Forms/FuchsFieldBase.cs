using FuchsControls.Behaviours;
using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls;

public abstract class FuchsFieldBase : ContentView
{
	private readonly FuchsTypo _label;
	private readonly FuchsTypo _helpText;
	private readonly Border _inputBorder;
	private View? _input;
	private VisualElement? _focusTarget;
	private bool _isInputFocused;
	private bool _isThemeChangeSubscribed;

	public static readonly BindableProperty LabelProperty = BindableProperty.Create(
		nameof(Label), typeof(string), typeof(FuchsFieldBase), string.Empty, propertyChanged: OnPresentationChanged);

	public static readonly BindableProperty HelpTextProperty = BindableProperty.Create(
		nameof(HelpText), typeof(string), typeof(FuchsFieldBase), string.Empty, propertyChanged: OnPresentationChanged);

	public static readonly BindableProperty InputStateProperty = BindableProperty.Create(
		nameof(InputState), typeof(FuchsInputState), typeof(FuchsFieldBase), FuchsInputState.Normal, propertyChanged: OnPresentationChanged);

	public static readonly BindableProperty IsRequiredProperty = BindableProperty.Create(
		nameof(IsRequired), typeof(bool), typeof(FuchsFieldBase), false, propertyChanged: OnPresentationChanged);

	public static readonly BindableProperty AccessibilityLabelProperty = BindableProperty.Create(
		nameof(AccessibilityLabel), typeof(string), typeof(FuchsFieldBase), string.Empty, propertyChanged: OnPresentationChanged);

	public static readonly BindableProperty AccessibilityHintProperty = BindableProperty.Create(
		nameof(AccessibilityHint), typeof(string), typeof(FuchsFieldBase), string.Empty, propertyChanged: OnPresentationChanged);

	protected Grid InputContainer { get; } = new();

	protected FuchsFieldBase()
	{
		_label = new FuchsTypo().ApplyFuchsFieldLabelStyle();
		_helpText = new FuchsTypo().ApplyFuchsFieldHelpTextStyle();
		_inputBorder = new Border
					   {
						   Padding = 0, StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(4) }, Content = InputContainer
					   };
		_inputBorder.SetDynamicResource(Border.StrokeProperty, "FuchsFieldBorderColor");
		_inputBorder.SetDynamicResource(BackgroundColorProperty, "FuchsFieldBackgroundColor");
		InputContainer.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(FocusInput) });

		Content = new VerticalStackLayout { Spacing = 4, Children = { _label, _inputBorder, _helpText } };
		UpdatePresentation();
	}

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

	public FuchsInputState InputState
	{
		get => (FuchsInputState)GetValue(InputStateProperty);
		set => SetValue(InputStateProperty, value);
	}

	public bool IsRequired
	{
		get => (bool)GetValue(IsRequiredProperty);
		set => SetValue(IsRequiredProperty, value);
	}

	public string AccessibilityLabel
	{
		get => (string)GetValue(AccessibilityLabelProperty);
		set => SetValue(AccessibilityLabelProperty, value);
	}

	public string AccessibilityHint
	{
		get => (string)GetValue(AccessibilityHintProperty);
		set => SetValue(AccessibilityHintProperty, value);
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		if (Handler is null)
		{
			UnsubscribeFromThemeChanges();
			return;
		}

		SubscribeToThemeChanges();
		UpdateBorder();
	}

	protected void SetInput(View input, VisualElement? focusTarget = null)
	{
		_input = input;
		_focusTarget = focusTarget ?? input as VisualElement;
		InputContainer.Children.Clear();
		InputContainer.Children.Add(input);
		if (_focusTarget is not null)
		{
			_focusTarget.Behaviors.Add(new FocusHighlightBehavior(this));
		}

		ToolTipProperties.SetText(input, HelpText);
		UpdateAccessibility();
	}

	internal void SetInputFocus(bool isFocused)
	{
		_isInputFocused = isFocused;
		UpdateBorder();
	}

	private static void OnPresentationChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsFieldBase)bindable).UpdatePresentation();

	private void UpdatePresentation()
	{
		_label.Text = IsRequired && !string.IsNullOrWhiteSpace(Label) ? $"{Label} *" : Label;
		_label.IsVisible = !string.IsNullOrWhiteSpace(Label);
		_helpText.Text = HelpText;
		_helpText.IsVisible = !string.IsNullOrWhiteSpace(HelpText);
		if (_input is not null)
		{
			ToolTipProperties.SetText(_input, HelpText);
		}

		UpdateAccessibility();

		UpdateBorder();
	}

	private void FocusInput()
	{
		if (_focusTarget is { IsEnabled: true })
		{
			_focusTarget.Focus();
		}
	}

	private void UpdateAccessibility()
	{
		if (_focusTarget is null)
		{
			return;
		}

		SemanticProperties.SetDescription(_focusTarget, string.IsNullOrWhiteSpace(AccessibilityLabel) ? Label : AccessibilityLabel);
		SemanticProperties.SetHint(_focusTarget, string.IsNullOrWhiteSpace(AccessibilityHint) ? HelpText : AccessibilityHint);
	}

	private void SubscribeToThemeChanges()
	{
		if (_isThemeChangeSubscribed)
		{
			return;
		}

		FuchsThemeManager.ThemeChanged += OnThemeChanged;
		_isThemeChangeSubscribed = true;
	}

	private void UnsubscribeFromThemeChanges()
	{
		if (!_isThemeChangeSubscribed)
		{
			return;
		}

		FuchsThemeManager.ThemeChanged -= OnThemeChanged;
		_isThemeChangeSubscribed = false;
	}

	private void OnThemeChanged(object? sender, EventArgs e) => UpdateBorder();

	private void UpdateBorder()
	{
		var key = InputState switch
				  {
					  FuchsInputState.Valid => FuchsThemeResourceKeys.FieldValidColor, FuchsInputState.Warning => FuchsThemeResourceKeys.FieldWarningColor
					  , FuchsInputState.Invalid => FuchsThemeResourceKeys.FieldInvalidColor, _ when _isInputFocused => FuchsThemeResourceKeys.FieldFocusColor
					  , _ => FuchsThemeResourceKeys.FieldBorderColor
				  };
		_inputBorder.Stroke = new SolidColorBrush(GetThemeColor(key));
	}

	private static Color GetThemeColor(string key) =>
		Application.Current?.Resources.TryGetValue(key, out var value) == true && value is Color color
			? color
			: key switch
			  {
				  FuchsThemeResourceKeys.FieldValidColor => FuchsThemeManager.Current.FieldValidColor
				  , FuchsThemeResourceKeys.FieldWarningColor => FuchsThemeManager.Current.FieldWarningColor
				  , FuchsThemeResourceKeys.FieldInvalidColor => FuchsThemeManager.Current.FieldInvalidColor
				  , FuchsThemeResourceKeys.FieldFocusColor => FuchsThemeManager.Current.FieldFocusColor
				  , _ => FuchsThemeManager.Current.FieldBorderColor
			  };
}