using System;

namespace FuchsControls.Containers.Responsive;

#region Meta

// FuchsControls
// Created: 29/01/2026
// Modified: 29/01/2026

#endregion

/// <summary>
/// Thrown when a responsive view cannot find any template to display.
/// </summary>
public class NullTemplateException : Exception
{
	public NullTemplateException()
	{
	}

	public NullTemplateException(string? message) : base(message)
	{
	}

	public NullTemplateException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}