#region Meta
// FuchsControls
// Created: 11/01/2026
// Modified: 11/01/2026
#endregion

using System.Windows.Input;

namespace FuchsControls.Fields;

public abstract class BooleanFieldBase : FieldBase, IBooleanField
{
	public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
		nameof(IsChecked),
		typeof(bool?),
		typeof(BooleanFieldBase),
		defaultValue: null,
		defaultBindingMode: BindingMode.TwoWay);

	public static readonly BindableProperty CommandProperty = BindableProperty.Create(
		nameof(Command),
		typeof(ICommand),
		typeof(BooleanFieldBase),
		defaultValue: null);

	public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
		nameof(CommandParameter),
		typeof(object),
		typeof(BooleanFieldBase),
		defaultValue: null);

	public bool? IsChecked
	{
		get => (bool?)GetValue(IsCheckedProperty);
		set => SetValue(IsCheckedProperty, value);
	}

	public ICommand? Command
	{
		get => (ICommand?)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public object? CommandParameter
	{
		get => (object?)GetValue(CommandParameterProperty);
		set => SetValue(CommandParameterProperty, value);
	}

	protected virtual void ExecuteCommand()
	{
		if (Command?.CanExecute(CommandParameter) == true)
		{
			Command.Execute(CommandParameter);
		}
	}
}