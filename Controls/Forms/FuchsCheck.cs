#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using FuchsControls.Theme;

namespace FuchsControls.Controls;

public sealed class FuchsCheck : FuchsBoolControl
{
	private readonly CheckBox _checkBox;
	private bool _updating;

	public static readonly BindableProperty IsCheckedProperty =
		BindableProperty.Create(nameof(IsChecked), typeof(bool?), typeof(FuchsCheck), false, BindingMode.TwoWay, propertyChanged: OnCheckedChanged);

	public static readonly BindableProperty IsThreeStateProperty =
		BindableProperty.Create(nameof(IsThreeState), typeof(bool), typeof(FuchsCheck), false, propertyChanged: OnVisualPropertyChanged);


	public bool? IsChecked
	{
		get => (bool?)GetValue(IsCheckedProperty);
		set => SetValue(IsCheckedProperty, value);
	}

	public bool IsThreeState
	{
		get => (bool)GetValue(IsThreeStateProperty);
		set => SetValue(IsThreeStateProperty, value);
	}

	public FuchsCheck()
	{
		_checkBox = new CheckBox();
		InitializeBoolControl(_checkBox);
		_checkBox.CheckedChanged += OnCheckedChanged;
		ControlBorder.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(CycleState) });
		ApplyTheme();
	}

	protected override void ApplyInputTheme(FuchsTheme theme)
	{
		_checkBox.IsChecked = IsChecked == true;
		_checkBox.IsEnabled = !IsDisabled && !IsThreeState;
		_checkBox.Color = State == FuchsInputState.Valid ? theme.Success : theme.Primary;
	}

	private void CycleState()
	{
		if (IsDisabled)
			return;

		if (IsThreeState)
			IsChecked = IsChecked switch { false => null, null => true, _ => false };
		else
			IsChecked = IsChecked != true;
	}

	private void OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
	{
		if (!_updating && !IsThreeState)
			IsChecked = e.Value;
	}


	private static void OnCheckedChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsCheck control && !control._updating)
		{
			control._updating = true;
			try
			{
				control._checkBox.IsChecked = (bool?)newValue == true;
			}
			finally
			{
				control._updating = false;
			}

			control.ApplyTheme();
		}
	}

	private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsCheck control)
			control.ApplyTheme();
	}
}