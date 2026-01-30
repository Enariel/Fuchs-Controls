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
	// Throttle updates to avoid storms from Handler/Size changes
	int _pendingUpdate; // 0 = none, 1 = scheduled

	protected FuchsResponsiveView()
	{
		SizeChanged += OnSelfSizeChanged;
	}

	protected void SetContentFromTemplate(DataTemplate? template)
	{
		if (template == null)
			return; // do nothing; keep current content to avoid crashes during XAML load

		View? resolvedView = null;

		try
		{
			var created = template.CreateContent();

			// 1) Direct View
			if (created is View v1)
				resolvedView = v1;
			// 2) Element that is also View
			else if (created is Element e && e is View v2)
				resolvedView = v2;
			// 3) IContentView Content unwrap
			else if (created is IContentView cv && cv.Content is View v3)
				resolvedView = v3;
			// 4) Reflective Content property
			else
			{
				var contentProp = created?.GetType().GetProperty("Content");
				if (contentProp?.GetValue(created) is View v4)
					resolvedView = v4;
			}
		}
		catch
		{
			// Swallow any template creation issues; keep existing Content intact.
			return;
		}

		// Only swap if we actually obtained a view; otherwise leave Content as-is
		if (resolvedView is not null && !ReferenceEquals(Content, resolvedView))
			Content = resolvedView;
	}

	private void OnSelfSizeChanged(object? sender, EventArgs e) => RequestUpdate();

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		HookWindowEvents(true);
		RequestUpdate();
	}

	protected override void OnParentChanged()
	{
		base.OnParentChanged();
		HookWindowEvents(true);
	}

	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();
		RequestUpdate();
	}

	private void HookWindowEvents(bool subscribe)
	{
		if (Window == null) return;
		if (subscribe)
		{
			Window.SizeChanged -= OnWindowSizeChanged;
			Window.SizeChanged += OnWindowSizeChanged;
		}
		else
		{
			Window.SizeChanged -= OnWindowSizeChanged;
		}
	}

	private void OnWindowSizeChanged(object? sender, EventArgs e) => RequestUpdate();

	// Coalesces multiple triggers into a single UpdateContent on the UI thread
	protected void RequestUpdate()
	{
		if (Interlocked.Exchange(ref _pendingUpdate, 1) == 1)
			return; // already scheduled

		Dispatcher.Dispatch(() =>
		{
			try
			{
				UpdateContent();
			}
			finally
			{
				Interlocked.Exchange(ref _pendingUpdate, 0);
			}
		});
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