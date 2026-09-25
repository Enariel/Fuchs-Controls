using CommunityToolkit.Maui.Markup;

namespace FuchsControls;

public static class FuchsControlExtensions
{
	public static FuchsTypo ApplyFuchsFieldLabelStyle(this FuchsTypo label)
	{
		label.Type = FuchsTypoType.Caption;
		label.FontAttributes = FontAttributes.Bold;
		label.Margin = new Thickness(6, 4, 6, 0);
		label.TextColor(FuchsThemeManager.Current.TextColor);
		label.SetDynamicResource(Label.TextColorProperty, FuchsThemeResourceKeys.TextColor);
		label.SetDynamicResource(Label.FontSizeProperty, FuchsThemeResourceKeys.CaptionFontSize);
		return label;
	}

	public static FuchsTypo ApplyFuchsFieldHelpTextStyle(this FuchsTypo label)
	{
		label.Type = FuchsTypoType.Caption;
		label.Margin = new Thickness(6, 0, 6, 4);
		label.TextColor(FuchsThemeManager.Current.MutedTextColor);
		label.SetDynamicResource(Label.TextColorProperty, FuchsThemeResourceKeys.MutedTextColor);
		label.SetDynamicResource(Label.FontSizeProperty, FuchsThemeResourceKeys.CaptionFontSize);
		return label;
	}

	public static T ApplyFuchsInputStyle<T>(this T input) where T : InputView
	{
		input.BackgroundColor = Colors.Transparent;
		input.TextColor = FuchsThemeManager.Current.TextColor;
		input.Margin = new Thickness(10, 0);
		input.HeightRequest = 42;
		input.SetDynamicResource(InputView.TextColorProperty, FuchsThemeResourceKeys.TextColor);
		return input;
	}

	public static Entry ApplyFuchsEntryStyle(this Entry entry)
	{
		entry.ApplyFuchsInputStyle();
		entry.PlaceholderColor = FuchsThemeManager.Current.MutedTextColor;
		entry.SetDynamicResource(Entry.PlaceholderColorProperty, FuchsThemeResourceKeys.MutedTextColor);
		return entry;
	}

	public static Editor ApplyFuchsEditorStyle(this Editor editor)
	{
		editor.ApplyFuchsInputStyle();
		editor.PlaceholderColor = FuchsThemeManager.Current.MutedTextColor;
		editor.AutoSize = EditorAutoSizeOption.TextChanges;
		editor.MinimumHeightRequest = 88;
		editor.SetDynamicResource(Editor.PlaceholderColorProperty, FuchsThemeResourceKeys.MutedTextColor);
		return editor;
	}

	public static Picker ApplyFuchsPickerStyle(this Picker picker)
	{
		picker.BackgroundColor = Colors.Transparent;
		picker.TextColor = FuchsThemeManager.Current.TextColor;
		picker.Margin = new Thickness(10, 0);
		picker.HeightRequest = 42;
		picker.SetDynamicResource(Picker.TextColorProperty, FuchsThemeResourceKeys.TextColor);
		return picker;
	}

	public static DatePicker ApplyFuchsDatePickerStyle(this DatePicker picker)
	{
		picker.BackgroundColor = Colors.Transparent;
		picker.TextColor = FuchsThemeManager.Current.TextColor;
		picker.Margin = new Thickness(10, 0);
		picker.HeightRequest = 42;
		picker.SetDynamicResource(DatePicker.TextColorProperty, FuchsThemeResourceKeys.TextColor);
		return picker;
	}

	public static TimePicker ApplyFuchsTimePickerStyle(this TimePicker picker)
	{
		picker.BackgroundColor = Colors.Transparent;
		picker.TextColor = FuchsThemeManager.Current.TextColor;
		picker.Margin = new Thickness(10, 0);
		picker.HeightRequest = 42;
		picker.SetDynamicResource(TimePicker.TextColorProperty, FuchsThemeResourceKeys.TextColor);
		return picker;
	}

	public static Switch ApplyFuchsSwitchStyle(this Switch control)
	{
		control.OnColor = FuchsThemeManager.Current.PrimaryColor;
		control.ThumbColor = FuchsThemeManager.Current.LightColor;
		control.SetDynamicResource(Switch.OnColorProperty, FuchsThemeResourceKeys.PrimaryColor);
		control.SetDynamicResource(Switch.ThumbColorProperty, FuchsThemeResourceKeys.LightColor);
		return control;
	}

	public static Button ApplyFuchsCheckboxStyle(this Button button)
	{
		button.BackgroundColor = FuchsThemeManager.Current.FieldBackgroundColor;
		button.BorderColor = FuchsThemeManager.Current.FieldBorderColor;
		button.BorderWidth = 1;
		button.CornerRadius = 3;
		button.TextColor = FuchsThemeManager.Current.SuccessColor;
		button.SetDynamicResource(Button.BackgroundColorProperty, FuchsThemeResourceKeys.FieldBackgroundColor);
		button.SetDynamicResource(Button.BorderColorProperty, FuchsThemeResourceKeys.FieldBorderColor);
		button.SetDynamicResource(Button.TextColorProperty, FuchsThemeResourceKeys.SuccessColor);
		return button;
	}

	public static RadioButton ApplyFuchsRadioButtonStyle(this RadioButton button)
	{
		button.TextColor = FuchsThemeManager.Current.PrimaryColor;
		button.SetDynamicResource(RadioButton.TextColorProperty, FuchsThemeResourceKeys.PrimaryColor);
		return button;
	}

	public static Slider ApplyFuchsRangeStyle(this Slider slider)
	{
		slider.MinimumTrackColor = FuchsThemeManager.Current.PrimaryColor;
		slider.MaximumTrackColor = FuchsThemeManager.Current.FieldBorderColor;
		slider.ThumbColor = FuchsThemeManager.Current.FieldBackgroundColor;
		slider.Margin = new Thickness(10, 0);
		slider.SetDynamicResource(Slider.MinimumTrackColorProperty, FuchsThemeResourceKeys.PrimaryColor);
		slider.SetDynamicResource(Slider.MaximumTrackColorProperty, FuchsThemeResourceKeys.FieldBorderColor);
		slider.SetDynamicResource(Slider.ThumbColorProperty, FuchsThemeResourceKeys.FieldBackgroundColor);
		return slider;
	}

	public static FuchsTypo ApplyFuchsTabTextStyle(this FuchsTypo text, bool active)
	{
		text.Type = FuchsTypoType.Body;
		text.TextColor = active ? FuchsThemeManager.Current.PrimaryColor : FuchsThemeManager.Current.TextColor;
		text.Opacity = active ? 1 : 0.6;
		text.FontAttributes = active ? FontAttributes.Bold : FontAttributes.None;
		text.SetDynamicResource(Label.TextColorProperty, active ? FuchsThemeResourceKeys.PrimaryColor : FuchsThemeResourceKeys.TextColor);
		return text;
	}
}