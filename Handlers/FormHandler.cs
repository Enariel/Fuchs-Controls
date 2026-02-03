#region Meta

// FuchsControls
// Created: 29/01/2026
// Modified: 29/01/2026

#endregion

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
#if ANDROID
using Color = Android.Graphics.Color;
using Android.Content.Res;
#endif

#if IOS || MACCATALYST
using UIKit;
#endif

namespace FuchsControls.Handlers;

public static class FormHandler
{
	public static void RemoveBorders()
	{
		EntryHandler.Mapper.AppendToMapping
		(
			"Borderless", (handler, view) =>
			{
#if ANDROID
				handler.PlatformView.Background = null;
				handler.PlatformView.SetBackgroundColor(Color.Transparent);
				handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
#elif IOS || MACCATALYST
				handler.PlatformView.BackgroundColor = UIColor.Clear;
				handler.PlatformView.Layer.BorderWidth = 0;
				handler.PlatformView.BorderStyle = UITextBorderStyle.None;
#elif WINDOWS
				handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Background = Colors.Transparent.ToPlatform();
				handler.PlatformView.Resources["TextControlBorderThickness"] = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Resources["TextControlBorderThicknessPointerOver"] = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Resources["TextControlBorderThicknessFocused"] = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Resources["TextControlBorderThicknessDisabled"] = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Resources["TextControlBorderBrush"] = Colors.Transparent.ToPlatform();
				handler.PlatformView.Resources["TextControlBorderBrushPointerOver"] = Colors.Transparent.ToPlatform();
				handler.PlatformView.Resources["TextControlBorderBrushFocused"] = Colors.Transparent.ToPlatform();
				handler.PlatformView.Resources["TextControlBorderBrushDisabled"] = Colors.Transparent.ToPlatform();
#endif
			}
		);

		EditorHandler.Mapper.AppendToMapping
		(
			"Borderless", (handler, view) =>
			{
#if ANDROID
				handler.PlatformView.Background = null;
				handler.PlatformView.SetBackgroundColor(Color.Transparent);
				handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
#elif IOS || MACCATALYST
				handler.PlatformView.BackgroundColor = UIColor.Clear;
				handler.PlatformView.Layer.BorderWidth = 0;
#elif WINDOWS
				handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Background = Colors.Transparent.ToPlatform();
				handler.PlatformView.Resources["TextControlBorderThickness"] = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Resources["TextControlBorderThicknessPointerOver"] = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Resources["TextControlBorderThicknessFocused"] = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Resources["TextControlBorderThicknessDisabled"] = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Resources["TextControlBorderBrush"] = Colors.Transparent.ToPlatform();
				handler.PlatformView.Resources["TextControlBorderBrushPointerOver"] = Colors.Transparent.ToPlatform();
				handler.PlatformView.Resources["TextControlBorderBrushFocused"] = Colors.Transparent.ToPlatform();
				handler.PlatformView.Resources["TextControlBorderBrushDisabled"] = Colors.Transparent.ToPlatform();
#endif
			}
		);

		PickerHandler.Mapper.AppendToMapping
		(
			"Borderless", (handler, view) =>
			{
#if ANDROID
				handler.PlatformView.Background = null;
				handler.PlatformView.SetBackgroundColor(Color.Transparent);
				handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
#elif IOS || MACCATALYST
				handler.PlatformView.BackgroundColor = UIColor.Clear;
				handler.PlatformView.Layer.BorderWidth = 0;
				if (handler.PlatformView is UITextField textField)
					textField.BorderStyle = UITextBorderStyle.None;
#elif WINDOWS
				handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Background = Colors.Transparent.ToPlatform();
				handler.PlatformView.Resources["ComboBoxBorderThickness"] = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Resources["ComboBoxBorderThicknessPointerOver"] = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Resources["ComboBoxBorderThicknessFocused"] = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Resources["ComboBoxBorderThicknessPressed"] = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Resources["ComboBoxBorderThicknessDisabled"] = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Resources["ComboBoxBorderBrush"] = Colors.Transparent.ToPlatform();
				handler.PlatformView.Resources["ComboBoxBorderBrushPointerOver"] = Colors.Transparent.ToPlatform();
				handler.PlatformView.Resources["ComboBoxBorderBrushFocused"] = Colors.Transparent.ToPlatform();
				handler.PlatformView.Resources["ComboBoxBorderBrushPressed"] = Colors.Transparent.ToPlatform();
				handler.PlatformView.Resources["ComboBoxBorderBrushDisabled"] = Colors.Transparent.ToPlatform();
#endif
			}
		);

		TimePickerHandler.Mapper.AppendToMapping
		(
			"Borderless", (handler, view) =>
			{
#if ANDROID
				handler.PlatformView.Background = null;
				handler.PlatformView.SetBackgroundColor(Color.Transparent);
				handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
#elif IOS || MACCATALYST
				handler.PlatformView.BackgroundColor = UIColor.Clear;
				handler.PlatformView.Layer.BorderWidth = 0;
#elif WINDOWS
				handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Background = Colors.Transparent.ToPlatform();
				handler.PlatformView.Resources["TimePickerBorderThickness"] = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Resources["TimePickerBorderBrush"] = Colors.Transparent.ToPlatform();
#endif
			}
		);

		DatePickerHandler.Mapper.AppendToMapping
		(
			"Borderless", (handler, view) =>
			{
#if ANDROID
				handler.PlatformView.Background = null;
				handler.PlatformView.SetBackgroundColor(Color.Transparent);
				handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
#elif IOS || MACCATALYST
				handler.PlatformView.BackgroundColor = UIColor.Clear;
				handler.PlatformView.Layer.BorderWidth = 0;
#elif WINDOWS
				handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Background = Colors.Transparent.ToPlatform();
				handler.PlatformView.Resources["DatePickerBorderThickness"] = new Microsoft.UI.Xaml.Thickness(0);
				handler.PlatformView.Resources["DatePickerBorderBrush"] = Colors.Transparent.ToPlatform();
#endif
			}
		);
	}
}