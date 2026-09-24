#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using System.Windows.Input;

namespace FuchsControls.Controls;

public sealed class FuchsMenuItem : BindableObject
{
	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(FuchsMenuItem), string.Empty);

	public static readonly BindableProperty IsActiveProperty =
		BindableProperty.Create(nameof(IsActive), typeof(bool), typeof(FuchsMenuItem), false);

	public static readonly BindableProperty IsEnabledProperty =
		BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(FuchsMenuItem), true);

	public static readonly BindableProperty CommandProperty =
		BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(FuchsMenuItem), null);

	public static readonly BindableProperty CommandParameterProperty =
		BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(FuchsMenuItem), null);

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public bool IsActive
	{
		get => (bool)GetValue(IsActiveProperty);
		set => SetValue(IsActiveProperty, value);
	}

	public bool IsEnabled
	{
		get => (bool)GetValue(IsEnabledProperty);
		set => SetValue(IsEnabledProperty, value);
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
}
