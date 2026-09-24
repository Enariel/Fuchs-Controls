#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using FuchsControls.Theme;

namespace FuchsControls.Controls;

public sealed class FuchsSwitch : FuchsBoolBase
{
	private readonly Switch _switch;

	public static readonly BindableProperty IsOnProperty =
		BindableProperty.Create(nameof(IsOn), typeof(bool), typeof(FuchsSwitch), false, BindingMode.TwoWay, propertyChanged: OnIsOnChanged);


	public bool IsOn
	{
		get => (bool)GetValue(IsOnProperty);
		set => SetValue(IsOnProperty, value);
	}

	public FuchsSwitch()
	{
		_switch = new Switch();
		InitializeBoolControl(_switch);
		_switch.Toggled += OnToggled;
		ApplyTheme();
	}

	protected override void ApplyInputTheme(FuchsTheme theme)
	{
		_switch.IsToggled = IsOn;
		_switch.WidthRequest = theme.FontSizeMd * 3;
		_switch.HeightRequest = theme.FontSizeMd * 1.5;
		_switch.OnColor = theme.Primary;
		_switch.ThumbColor = IsOn ? theme.TextInverted : theme.BackgroundDarker;
		_switch.IsEnabled = !IsDisabled;
	}

	private void OnToggled(object? sender, ToggledEventArgs e)
	{
		if (IsOn != e.Value)
			IsOn = e.Value;
	}


	private static void OnIsOnChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsSwitch control)
		{
			control._switch.IsToggled = (bool)newValue;
			control.ApplyTheme();
		}
	}
}