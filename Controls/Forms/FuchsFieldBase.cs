using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls;

public abstract class FuchsFieldBase : ContentView
{
	private readonly FuchsTypo _label;
	private readonly FuchsTypo _helpText;
	private readonly Border _inputBorder;
	private View? _input;
	private bool _isInputFocused;

	public static readonly BindableProperty LabelProperty = BindableProperty.Create(
		nameof(Label), typeof(string), typeof(FuchsFieldBase), string.Empty, propertyChanged: OnPresentationChanged);

	public static readonly BindableProperty HelpTextProperty = BindableProperty.Create(
		nameof(HelpText), typeof(string), typeof(FuchsFieldBase), string.Empty, propertyChanged: OnPresentationChanged);

	public static readonly BindableProperty InputStateProperty = BindableProperty.Create(
		nameof(InputState), typeof(FuchsInputState), typeof(FuchsFieldBase), FuchsInputState.Normal, propertyChanged: OnPresentationChanged);

	public static readonly BindableProperty IsRequiredProperty = BindableProperty.Create(
		nameof(IsRequired), typeof(bool), typeof(FuchsFieldBase), false, propertyChanged: OnPresentationChanged);

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

	protected void SetInput(View input)
	{
		_input = input;
		InputContainer.Children.Clear();
		InputContainer.Children.Add(input);
		input.Behaviors.Add(new FocusHighlightBehavior(this));
		ToolTipProperties.SetText(input, HelpText);
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

		UpdateBorder();
	}

	private void UpdateBorder()
	{
		var key = InputState switch
				  {
					  FuchsInputState.Valid => "FuchsFieldValidColor", FuchsInputState.Warning => "FuchsFieldWarningColor"
					  , FuchsInputState.Invalid => "FuchsFieldInvalidColor", _ when _isInputFocused => "FuchsFieldFocusColor", _ => "FuchsFieldBorderColor"
				  };
		_inputBorder.Stroke = Application.Current?.Resources.TryGetValue(key, out var value) == true && value is Brush brush
			? brush
			: new SolidColorBrush(InputState == FuchsInputState.Invalid ? Colors.IndianRed : _isInputFocused ? Colors.DodgerBlue : Colors.SlateGray);
	}
}