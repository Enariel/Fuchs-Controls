#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using FuchsControls.Theme;

namespace FuchsControls.Controls;

public class FuchsIconButton : FuchsButton
{
	private readonly FuchsIcon _icon;

	public static readonly BindableProperty IconProperty =
		BindableProperty.Create(nameof(Icon), typeof(string), typeof(FuchsIconButton), string.Empty, propertyChanged: OnIconPropertyChanged);

	public string Icon
	{
		get => (string)GetValue(IconProperty);
		set => SetValue(IconProperty, value);
	}

	public string Data
	{
		get => Icon;
		set => Icon = value;
	}

	public FuchsIconButton()
	{
		_icon = new FuchsIcon
		{
			HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center
		};

		ButtonLayout.Children.Insert(1, _icon);
		Text = string.Empty;
		ApplyTheme();
	}

	protected override void OnButtonThemeApplied()
	{
		if (_icon is null)
			return;

		FuchsTheme theme = FuchsThemeProvider.Current;
		double fontSize = ResolveFontSize();

		_icon.Icon = Icon;
		_icon.Color = Color;
		_icon.Size = Size;
		_icon.Variant = Variant;
		_icon.IsDisabled = IsDisabled || IsLoading;
		_icon.IsVisible = !IsLoading && !string.IsNullOrWhiteSpace(Icon);

		ButtonRoot.WidthRequest = fontSize * 2.5;
		ButtonRoot.HeightRequest = fontSize * 2.5;
		ButtonRoot.Padding = new Thickness(fontSize * 0.5);
		ButtonRoot.StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = theme.CornerRadius };
	}

	private static void OnIconPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsIconButton button)
			button.ApplyTheme();
	}
}