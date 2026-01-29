#region Meta

// FuchsControls
// Created: 28/01/2026
// Modified: 28/01/2026

#endregion

using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FuchsControls;

/// <summary>
/// Shared link behavior used by label-like and span-like link components.
/// Encapsulates bindable properties, tap handling, URL launching, and decoration updates.
/// </summary>
internal sealed class FuchsLinkBehavior
{
	// Shared bindable properties (owner type is this behavior class to reuse across controls)
	public static readonly BindableProperty UrlProperty =
		BindableProperty.Create(
			"Url",
			typeof(string),
			typeof(FuchsLinkBehavior),
			defaultValue: null);

	public static readonly BindableProperty CommandProperty =
		BindableProperty.Create(
			"Command",
			typeof(ICommand),
			typeof(FuchsLinkBehavior),
			defaultValue: null);

	public static readonly BindableProperty CommandParameterProperty =
		BindableProperty.Create(
			"CommandParameter",
			typeof(object),
			typeof(FuchsLinkBehavior),
			defaultValue: null);

	public static readonly BindableProperty UnderlineProperty =
		BindableProperty.Create(
			"Underline",
			typeof(bool),
			typeof(FuchsLinkBehavior),
			defaultValue: true,
			propertyChanged: (b, _, __) => GetBehavior(b)?.UpdateDecoration());

	public static readonly BindableProperty LinkColorProperty =
		BindableProperty.Create(
			"LinkColor",
			typeof(Color),
			typeof(FuchsLinkBehavior),
			defaultValue: Colors.Blue,
			propertyChanged: (b, _, __) => GetBehavior(b)?.UpdateDecoration());

	// Hidden attached property to find the behavior instance from property-changed callbacks
	internal static readonly BindableProperty BehaviorProperty =
		BindableProperty.CreateAttached(
			"Behavior",
			typeof(FuchsLinkBehavior),
			typeof(FuchsLinkBehavior),
			defaultValue: null);

	private static FuchsLinkBehavior? GetBehavior(BindableObject bindable)
		=> (FuchsLinkBehavior?)bindable.GetValue(BehaviorProperty);

	private readonly BindableObject _owner;
	private readonly Action<Color> _setTextColor;
	private readonly Action<TextDecorations> _setTextDecorations;
	private bool _isHovered;

	public FuchsLinkBehavior(BindableObject owner, Action<Color> setTextColor, Action<TextDecorations> setTextDecorations)
	{
		_owner = owner;
		_setTextColor = setTextColor;
		_setTextDecorations = setTextDecorations;

		// initial decoration
		UpdateDecoration();
	}

	public void Attach()
	{
		_owner.SetValue(BehaviorProperty, this);
		UpdateDecoration();
	}

	public void SetHovered(bool isHovered)
	{
		_isHovered = isHovered;
		UpdateDecoration();
	}

	public void UpdateDecoration()
	{
		var color = (Color)_owner.GetValue(LinkColorProperty);
		_setTextColor(color);

		var underline = (bool)_owner.GetValue(UnderlineProperty);
		_setTextDecorations(underline && _isHovered ? TextDecorations.Underline : TextDecorations.None);
	}

	public async Task OnTappedAsync()
	{
		var parameter = _owner.GetValue(CommandParameterProperty);
		var command = (ICommand?)_owner.GetValue(CommandProperty);

		if (command?.CanExecute(parameter) == true)
		{
			command.Execute(parameter);
			return;
		}

		var url = (string?)_owner.GetValue(UrlProperty);
		if (string.IsNullOrWhiteSpace(url))
			return;

		if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
			await Launcher.Default.OpenAsync(uri);
	}
}