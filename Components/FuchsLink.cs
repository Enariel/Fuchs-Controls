#region Meta

// FuchsControls
// Created: 22/01/2026
// Modified: 28/01/2026

#endregion

using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FuchsControls;

/// <summary>
/// Represents a clickable hyperlink label with customizable appearance and behavior.
/// </summary>
public partial class FuchsLink : Label
{
	// Reuse shared bindable properties from the behavior
	public static readonly BindableProperty UrlProperty = FuchsLinkBehavior.UrlProperty;
	public static readonly BindableProperty CommandProperty = FuchsLinkBehavior.CommandProperty;
	public static readonly BindableProperty CommandParameterProperty = FuchsLinkBehavior.CommandParameterProperty;
	public static readonly BindableProperty UnderlineProperty = FuchsLinkBehavior.UnderlineProperty;
	public static readonly BindableProperty LinkColorProperty = FuchsLinkBehavior.LinkColorProperty;

	private readonly FuchsLinkBehavior _behavior;

	public FuchsLink()
	{
		this.SetDynamicResource(LinkColorProperty, "FuchsAccentColor");

		_behavior = new FuchsLinkBehavior(
			this,
			c => TextColor = c,
			d => TextDecorations = d);
		_behavior.Attach();

		GestureRecognizers.Add(new TapGestureRecognizer
		{
			Command = new Command(async () => await _behavior.OnTappedAsync())
		});

#if WINDOWS || MACCATALYST
		var pointer = new PointerGestureRecognizer();
		pointer.PointerEntered += (_, _) => _behavior.SetHovered(true);
		pointer.PointerExited += (_, _) => _behavior.SetHovered(false);
		GestureRecognizers.Add(pointer);
#endif
	}

	/// <summary>
	/// Optional URL to open when tapped (used if Command is null or can't execute).
	/// Example: "https://example.com"
	/// </summary>
	public string? Url
	{
		get => (string?)GetValue(UrlProperty);
		set => SetValue(UrlProperty, value);
	}

	public ICommand Command
	{
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public object CommandParameter
	{
		get => GetValue(CommandParameterProperty);
		set => SetValue(CommandParameterProperty, value);
	}

	/// <summary>
	/// When true, the link will underline on hover (desktop platforms).
	/// </summary>
	public bool Underline
	{
		get => (bool)GetValue(UnderlineProperty);
		set => SetValue(UnderlineProperty, value);
	}

	public Color LinkColor
	{
		get => (Color)GetValue(LinkColorProperty);
		set => SetValue(LinkColorProperty, value);
	}
}