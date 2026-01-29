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

#if IOS
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
#elif IOS
                handler.PlatformView.BackgroundColor = UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.BorderStyle = UITextBorderStyle.None;
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
#elif IOS
                handler.PlatformView.BackgroundColor = UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.BorderStyle = UITextBorderStyle.None;
#elif WINDOWS
                var combobox = handler.PlatformView;
                combobox.BorderBrush = null;
                combobox.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);

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
#elif IOS
                handler.PlatformView.BackgroundColor = UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.BorderStyle = UITextBorderStyle.None;
#elif WINDOWS
                var timePicker = handler.PlatformView;
                timePicker.BorderBrush = null;
                timePicker.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
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
#elif IOS
                handler.PlatformView.BackgroundColor = UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.BorderStyle = UITextBorderStyle.None;
#elif WINDOWS
                var timePicker = handler.PlatformView;
                timePicker.BorderBrush = null;
                timePicker.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            }
        );
    }
}