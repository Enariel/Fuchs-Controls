using System.Windows.Input;
using FuchsControls.Theme;
using ThemeColor = FuchsControls.Theme.FuchsColor;

namespace FuchsControls.Controls;

public class FuchsLink : FuchsComponent
{
	private readonly Label _label;

	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(FuchsLink), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty UrlProperty =
		BindableProperty.Create(nameof(Url), typeof(string), typeof(FuchsLink));

	public static readonly BindableProperty CommandProperty =
		BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(FuchsLink));

	public static readonly BindableProperty CommandParameterProperty =
		BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(FuchsLink));

	public static readonly BindableProperty UnderlineProperty =
		BindableProperty.Create(nameof(Underline), typeof(bool), typeof(FuchsLink), false, propertyChanged: OnVisualPropertyChanged);

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public string? Url
	{
		get => (string?)GetValue(UrlProperty);
		set => SetValue(UrlProperty, value);
	}

	public ICommand? Command
	{
		get => (ICommand?)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public object? CommandParameter
	{
		get => GetValue(CommandParameterProperty);
		set => SetValue(CommandParameterProperty, value);
	}

	public bool Underline
	{
		get => (bool)GetValue(UnderlineProperty);
		set => SetValue(UnderlineProperty, value);
	}

	public FuchsLink()
	{
		_label = new Label
		{
			LineBreakMode = LineBreakMode.WordWrap
		};

		Content = _label;

		GestureRecognizers.Add(new TapGestureRecognizer
		{
			Command = new Command(async () =>
			{
				if (IsDisabled)
					return;

				if (Command?.CanExecute(CommandParameter) == true)
				{
					Command.Execute(CommandParameter);
					return;
				}

				if (!string.IsNullOrWhiteSpace(Url))
					await Launcher.OpenAsync(Url);
			})
		});

		ApplyTheme();
	}

	protected override void ApplyTheme()
	{
		FuchsTheme theme = FuchsThemeProvider.Current;

		_label.Text = Text;
		_label.TextColor = Color == ThemeColor.Default ? theme.Primary : theme.GetBorderColor(Color);
		_label.FontSize = ResolveFontSize();
		_label.TextDecorations = Underline ? TextDecorations.Underline : TextDecorations.None;
		_label.Opacity = IsDisabled ? 0.55 : 1;
	}

	private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsLink link)
			link.ApplyTheme();
	}
}