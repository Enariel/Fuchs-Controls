using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

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
	protected FuchsResponsiveView()
	{
		SizeChanged += OnSelfSizeChanged;
	}

	protected void SetContentFromTemplate(DataTemplate? template)
	{
		if (template == null)
		{
			Content = null;
			return;
		}

		var created = template.CreateContent();
		Content = created as View ?? throw new InvalidOperationException("Template must return a View");
	}

	private void OnSelfSizeChanged(object? sender, EventArgs e)
	{
		UpdateContent();
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		HookWindowEvents(true);
		UpdateContent();
	}

	protected override void OnParentChanged()
	{
		base.OnParentChanged();
		HookWindowEvents(true);
	}

	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();
		UpdateContent();
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