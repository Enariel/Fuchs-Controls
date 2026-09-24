#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls.Controls;

public sealed class FuchsSwitch : FuchsComponent
{
	private readonly Border _root;
	private readonly Switch _switch;
	private readonly Label _label;

	public static readonly BindableProperty IsOnProperty =
		BindableProperty.Create(nameof(IsOn), typeof(bool), typeof(FuchsSwitch), false, BindingMode.TwoWay, propertyChanged: OnIsOnChanged);

	public static readonly BindableProperty LabelProperty =
		BindableProperty.Create(nameof(Label), typeof(string), typeof(FuchsSwitch), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty IsFocusedProperty =
		BindableProperty.Create(nameof(IsFocused), typeof(bool), typeof(FuchsSwitch), false, BindingMode.OneWayToSource);

	public bool IsOn
	{
		get => (bool)GetValue(IsOnProperty);
		set => SetValue(IsOnProperty, value);
	}

	public string Label
	{
		get => (string)GetValue(LabelProperty);
		set => SetValue(LabelProperty, value);
	}

	public bool IsFocused
	{
		get => (bool)GetValue(IsFocusedProperty);
		private set => SetValue(IsFocusedProperty, value);
	}

	public FuchsSwitch()
	{
		_switch = new Switch();
		_label = new Label { VerticalTextAlignment = TextAlignment.Center };
		_switch.Toggled += OnToggled;
		_switch.Focused += OnFocused;
		_switch.Unfocused += OnUnfocused;
		HorizontalStackLayout content = new() { Spacing = 10, Children = { _switch, _label } };
		_root = new Border { Content = content };
		Content = _root;
		ApplyTheme();
	}

	protected override void ApplyTheme()
	{
		FuchsTheme theme = FuchsThemeProvider.Current;
		_label.Text = Label;
		_label.TextColor = theme.Text;
		_label.FontSize = ResolveFontSize();
		_switch.IsToggled = IsOn;
		_switch.OnColor = theme.Primary;
		_switch.ThumbColor = theme.TextInverted;
		_switch.IsEnabled = !IsDisabled;
		_root.BackgroundColor = Variant == FuchsVariant.Text ? Colors.Transparent : theme.BackgroundDark;
		_root.Stroke = IsFocused ? new SolidColorBrush(theme.Primary) : new SolidColorBrush(theme.BackgroundDarker);
		_root.StrokeThickness = IsFocused || Variant != FuchsVariant.Filled ? theme.BorderWidth : 0;
		_root.StrokeShape = new RoundRectangle { CornerRadius = theme.CornerRadius };
		_root.Padding = ResolvePadding();
		_root.Opacity = IsDisabled ? 0.65 : 1;
	}

	private void OnToggled(object? sender, ToggledEventArgs e)
	{
		if (IsOn != e.Value)
			IsOn = e.Value;
	}

	private void OnFocused(object? sender, FocusEventArgs e)
	{
		IsFocused = true;
		ApplyTheme();
	}

	private void OnUnfocused(object? sender, FocusEventArgs e)
	{
		IsFocused = false;
		ApplyTheme();
	}

	private static void OnIsOnChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsSwitch control)
		{
			control._switch.IsToggled = (bool)newValue;
			control.ApplyTheme();
		}
	}

	private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsSwitch control)
			control.ApplyTheme();
	}
}