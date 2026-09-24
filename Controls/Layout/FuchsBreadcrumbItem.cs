#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using System.Windows.Input;

namespace FuchsControls.Controls;

public sealed class FuchsBreadcrumbItem : BindableObject
{
	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(FuchsBreadcrumbItem), string.Empty);

	public static readonly BindableProperty IsCurrentProperty =
		BindableProperty.Create(nameof(IsCurrent), typeof(bool), typeof(FuchsBreadcrumbItem), false);

	public static readonly BindableProperty IsEnabledProperty =
		BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(FuchsBreadcrumbItem), true);

	public static readonly BindableProperty CommandProperty =
		BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(FuchsBreadcrumbItem), null);

	public static readonly BindableProperty CommandParameterProperty =
		BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(FuchsBreadcrumbItem), null);

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public bool IsCurrent
	{
		get => (bool)GetValue(IsCurrentProperty);
		set => SetValue(IsCurrentProperty, value);
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
