using System.Collections;

namespace FuchsControls;

public sealed partial class FuchsEnumSelect : FuchsSelectBase
{
	public static readonly BindableProperty EnumTypeProperty = BindableProperty.Create(
		nameof(EnumType), typeof(Type), typeof(FuchsEnumSelect), null, propertyChanged: OnEnumTypeChanged);

	public Type? EnumType
	{
		get => (Type?)GetValue(EnumTypeProperty);
		set => SetValue(EnumTypeProperty, value);
	}

	private static void OnEnumTypeChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (FuchsEnumSelect)bindable;
		control.ItemsSource = control.EnumType?.IsEnum == true ? new ArrayList(Enum.GetValues(control.EnumType)) : null;
	}
}