using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System.Threading;

namespace FuchsControls.Containers.Responsive;

#region Meta

// FuchsControls
// Created: 29/01/2026
// Modified: 29/01/2026

#endregion

/// <summary>
/// Base class for responsive container controls. It listens to size changes and
/// allows derived classes to decide which content to render based on conditions
/// like screen size or device idiom.
/// </summary>
public abstract class FuchsResponsiveView : ContentView
{
	private DataTemplate? _lastTemplate;
	private Window? _attachedWindow;
	private bool _updatePending;
	private readonly TimeSpan _updateThrottle = TimeSpan.FromMilliseconds(50);
	private DateTime _lastUpdateAt;

	protected void SetContentFromTemplate(DataTemplate? template)
	{
		if (ReferenceEquals(template, _lastTemplate))
			return;

		_lastTemplate = template;

		if (template == null)
		{
			Content = null;
			return;
		}

		View? resolvedView = null;

		try
		{
			var created = template.CreateContent();

			if (created is View v1)
				resolvedView = v1;
			else if (created is Element e && e is View v2)
				resolvedView = v2;
			else if (created is IContentView cv && cv.Content is View v3)
				resolvedView = v3;
			else
			{
				var contentProp = created?.GetType().GetProperty("Content");
				if (contentProp?.GetValue(created) is View v4)
					resolvedView = v4;
			}
		}
		catch
		{
			Content = null;
			return;
		}

		Content = resolvedView;
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		AttachWindowSizeChanged();
		ThrottledUpdateContent();
	}

        protected void RequestUpdate() => ThrottledUpdateContent();

	protected override void OnParentChanged()
	{
		base.OnParentChanged();
		AttachWindowSizeChanged();
		ThrottledUpdateContent();
	}

	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();
		ThrottledUpdateContent();
	}

	private void AttachWindowSizeChanged()
	{
		if (_attachedWindow != null)
		{
			_attachedWindow.SizeChanged -= OnWindowSizeChanged;
			_attachedWindow = null;
		}

		if (Window == null)
			return;

		_attachedWindow = Window;
		_attachedWindow.SizeChanged -= OnWindowSizeChanged;
		_attachedWindow.SizeChanged += OnWindowSizeChanged;
	}

	private void OnWindowSizeChanged(object? sender, EventArgs e)
	{
		ThrottledUpdateContent();
	}

	private void ThrottledUpdateContent()
	{
		var now = DateTime.UtcNow;
		if (_updatePending || (now - _lastUpdateAt) < _updateThrottle)
		{
			if (_updatePending) return;
			_updatePending = true;
			_ = Dispatcher.DispatchDelayed(_updateThrottle, () =>
			{
				_updatePending = false;
				_lastUpdateAt = DateTime.UtcNow;
				SafeUpdateContent();
			});
			return;
		}

		_lastUpdateAt = now;
		SafeUpdateContent();
	}

	private void SafeUpdateContent()
	{
		try
		{
			UpdateContent();
		}
		catch
		{
			// Swallow to keep UI stable; render nothing on failure.
			Content = null;
			_lastTemplate = null;
		}
	}

	protected override void OnHandlerChanging(HandlerChangingEventArgs args)
	{
		base.OnHandlerChanging(args);
		if (_attachedWindow != null)
		{
			_attachedWindow.SizeChanged -= OnWindowSizeChanged;
			_attachedWindow = null;
		}
	}

	/// <summary>
	/// Returns the best available width to drive responsive decisions.
	/// Falls back from element width to window width when needed.
	/// </summary>
	protected double GetCurrentWidth()
	{
		if (!double.IsNaN(Width) && Width > 0)
			return Width;
		if (Window != null && Window.Width > 0)
			return Window.Width;
		return Shell.Current?.Width ?? 0;
	}

	/// <summary>
	/// Derived classes must implement to decide which content to display.
	/// </summary>
	protected abstract void UpdateContent();
}