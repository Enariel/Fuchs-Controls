using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls.Controls;

public abstract class FuchsFieldControl : FuchsComponent
{
	private readonly Border _root;
	private readonly InputView _input;
	private readonly Label _label;
	private readonly Label _helperText;

	public static readonly BindableProperty LabelProperty =
		BindableProperty.Create(nameof(Label), typeof(string), typeof(FuchsFieldControl), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(FuchsFieldControl), string.Empty, BindingMode.TwoWay
			, propertyChanged: OnTextPropertyChanged);

	public static readonly BindableProperty PlaceholderProperty =
		BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(FuchsFieldControl), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty HelperTextProperty =
		BindableProperty.Create(nameof(HelperText), typeof(string), typeof(FuchsFieldControl), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty StateProperty =
		BindableProperty.Create(nameof(State), typeof(FuchsInputState), typeof(FuchsFieldControl), FuchsInputState.Normal
			, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty IsPasswordProperty =
		BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(FuchsFieldControl), false, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty KeyboardProperty =
		BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(FuchsFieldControl), Keyboard.Default, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty MaxLengthProperty =
		BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(FuchsFieldControl), -1, propertyChanged: OnVisualPropertyChanged);

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

	protected FuchsFieldControl()
	{
		_label = new Label();
		_input = CreateInput();
		_input.SetBinding(InputView.TextProperty, new Binding(nameof(Text), source: this, mode: BindingMode.TwoWay));

		_helperText = new Label();

		_root = new Border
		{
			Content = _input
		};

		Grid layout = new()
		{
			ColumnDefinitions =
			{
				new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto)
			}
			, RowDefinitions =
			{
				new RowDefinition(GridLength.Auto), new RowDefinition(GridLength.Auto)
			}
			, ColumnSpacing = 12, RowSpacing = 4
		};
		layout.Add(_label, 0, 0);
		layout.Add(_root, 0, 1);
		if (DeviceInfo.Current.Idiom == DeviceIdiom.Phone || DeviceInfo.Current.Idiom == DeviceIdiom.Tablet)
		{
			layout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			layout.Add(_helperText, 0, 2);
			Grid.SetColumnSpan(_helperText, 2);
		}
		else
			layout.Add(_helperText, 1, 1);

		Content = new VerticalStackLayout
		{
			Children = { layout }
		};

		ApplyTheme();
	}

	protected override void ApplyTheme()
	{
		FuchsTheme theme = FuchsThemeProvider.Current;

		Color border = State switch
		{
			FuchsInputState.Valid => theme.Success, FuchsInputState.Warning => theme.Warning, FuchsInputState.Invalid => theme.Danger
			, _ => theme.BackgroundDarker
		};

		_label.Text = Label;
		_label.IsVisible = !string.IsNullOrWhiteSpace(Label);
		_label.TextColor = theme.Text;
		_label.FontSize = theme.FontSizeSm;

		_input.Placeholder = Placeholder;
		_input.TextColor = theme.Text;
		_input.PlaceholderColor = theme.TextLight;
		_input.FontSize = ResolveFontSize();
		if (_input is Entry entry)
			entry.IsPassword = IsPassword;
		_input.Keyboard = Keyboard;
		_input.MaxLength = MaxLength;
		_input.IsEnabled = !IsDisabled;

		_root.BackgroundColor = Variant == FuchsVariant.Text ? Colors.Transparent : theme.BackgroundDark;
		_root.Stroke = Variant == FuchsVariant.Filled ? new SolidColorBrush(Colors.Transparent) : new SolidColorBrush(border);
		_root.StrokeThickness = Variant == FuchsVariant.Filled ? 0 : theme.BorderWidth;
		_root.StrokeShape = new RoundRectangle { CornerRadius = theme.CornerRadius };
		_root.Padding = ResolvePadding();
		_root.Opacity = IsDisabled ? 0.65 : 1;

		_helperText.Text = HelperText;
		_helperText.IsVisible = !string.IsNullOrWhiteSpace(HelperText);
		_helperText.FontSize = theme.FontSizeSm;
		_helperText.TextColor = State switch
		{
			FuchsInputState.Valid => theme.Success, FuchsInputState.Warning => theme.Warning, FuchsInputState.Invalid => theme.Danger, _ => theme.TextLight
		};
	}

	protected abstract InputView CreateInput();

	protected virtual void OnTextChanged(string text) { }

	private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsFieldControl field)
			field.ApplyTheme();
	}

	private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsFieldControl field)
		{
			field.ApplyTheme();
			field.OnTextChanged(field.Text);
		}
	}
}