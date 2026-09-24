using System.Windows.Input;
using FuchsControls.Behaviours;
using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls.Controls;

public class FuchsButton : FuchsComponent
{
	private readonly Border _root;
	private readonly Label _label;
	private readonly ActivityIndicator _loader;

	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(FuchsButton), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty CommandProperty =
		BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(FuchsButton));

	public static readonly BindableProperty CommandParameterProperty =
		BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(FuchsButton));

	public static readonly BindableProperty IsLoadingProperty =
		BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(FuchsButton), false, propertyChanged: OnVisualPropertyChanged);

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
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

	public bool IsLoading
	{
		get => (bool)GetValue(IsLoadingProperty);
		set => SetValue(IsLoadingProperty, value);
	}

	public FuchsButton()
	{
		_label = new Label
		{
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center,
			FontAttributes = FontAttributes.Bold
		};

		_loader = new ActivityIndicator
		{
			IsVisible = false,
			IsRunning = false,
			WidthRequest = 18,
			HeightRequest = 18
		};

		_root = new Border
		{
			Content = new HorizontalStackLayout
			{
				Spacing = 8,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Children =
				{
					_loader,
					_label
				}
			}
		};

		Content = _root;

		GestureRecognizers.Add(new TapGestureRecognizer
		{
			Command = new Command(() =>
			{
				if (IsDisabled || IsLoading)
					return;

				if (Command?.CanExecute(CommandParameter) == true)
					Command.Execute(CommandParameter);
			})
		});

		Behaviors.Add(new PressedScaleBehavior { PressedScale = 0.98, PressedTranslationY = 3 });

		ApplyTheme();
	}

	protected override void ApplyTheme()
	{
		FuchsTheme theme = FuchsThemeProvider.Current;

		Color background = theme.GetMainColor(Color);
		Color border = theme.GetBorderColor(Color);
		Color text = theme.GetTextColor(Color, Variant);

		_root.Padding = ResolvePadding();
		_root.StrokeThickness = Variant == FuchsVariant.Text ? 0 : theme.BorderWidth;
		_root.StrokeShape = new RoundRectangle { CornerRadius = theme.CornerRadius };
		_root.Opacity = IsDisabled ? 0.65 : 1;
		_root.Shadow = (Variant == FuchsVariant.Filled && !IsDisabled
			? new Shadow { Brush = new SolidColorBrush(border), Offset = new Point(0, 3), Radius = 0, Opacity = 1 }
			: null)!;

		_root.BackgroundColor = Variant switch
		{
			FuchsVariant.Text or FuchsVariant.Outlined => Colors.Transparent,
			_ => background
		};

		_root.Stroke = Variant switch
		{
			FuchsVariant.Outlined => new SolidColorBrush(border),
			_ => Brush.Transparent
		};

		_label.Text = Text;
		_label.TextColor = text;
		_label.FontSize = ResolveFontSize();

		_loader.Color = text;
		_loader.IsVisible = IsLoading;
		_loader.IsRunning = IsLoading;

		IsEnabled = !IsDisabled;
	}

	private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsButton button)
			button.ApplyTheme();
	}
}
