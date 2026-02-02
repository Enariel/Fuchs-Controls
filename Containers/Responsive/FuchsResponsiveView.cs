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
	protected void SetContentFromTemplate(DataTemplate? template)
	{
		if (template == null)
		{
			// No fallbacks: render nothing if no template is provided for the current case.
			Content = null;
			return;
		}

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
			// No fallbacks: if template creation fails, render nothing.
			Content = null;
			return;
		}

		// Only swap if we actually obtained a view; otherwise render nothing
		Content = resolvedView;
	}


	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		AttachWindowSizeChanged();
		UpdateContent();
	}

	protected override void OnParentChanged()
	{
		base.OnParentChanged();
		AttachWindowSizeChanged();
		UpdateContent();
	}

	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();
		UpdateContent();
	}

	private void AttachWindowSizeChanged()
	{
		if (Window == null)
			return;

		Window.SizeChanged -= OnWindowSizeChanged;
		Window.SizeChanged += OnWindowSizeChanged;
	}

	private void OnWindowSizeChanged(object? sender, EventArgs e)
	{
		UpdateContent();
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