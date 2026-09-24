using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

#if ANDROID
using Android.Content.Res;
using Color = Android.Graphics.Color;
#endif

#if IOS || MACCATALYST
using UIKit;
#endif

namespace FuchsControls.Handlers;

public static class FuchsEntryHandler
{
	public static void Register()
	{
		EntryHandler.Mapper.AppendToMapping("FuchsEntryChrome", (handler, view) =>
		{
#if ANDROID
			handler.PlatformView.Background = null;
			handler.PlatformView.SetBackgroundColor(Color.Transparent);
			handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
			handler.PlatformView.SetPadding(0, 0, 0, 0);
#elif IOS || MACCATALYST
			handler.PlatformView.BackgroundColor = UIColor.Clear;
			handler.PlatformView.BorderStyle = UITextBorderStyle.None;
#elif WINDOWS
			handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
			handler.PlatformView.Background = Colors.Transparent.ToPlatform();
			handler.PlatformView.Resources["TextControlBorderThickness"] = new Microsoft.UI.Xaml.Thickness(0);
			handler.PlatformView.Resources["TextControlBorderThicknessPointerOver"] = new Microsoft.UI.Xaml.Thickness(0);
			handler.PlatformView.Resources["TextControlBorderThicknessFocused"] = new Microsoft.UI.Xaml.Thickness(0);
			handler.PlatformView.Resources["TextControlBorderBrush"] = Colors.Transparent.ToPlatform();
			handler.PlatformView.Resources["TextControlBorderBrushPointerOver"] = Colors.Transparent.ToPlatform();
			handler.PlatformView.Resources["TextControlBorderBrushFocused"] = Colors.Transparent.ToPlatform();
#endif
		});
	}
}
