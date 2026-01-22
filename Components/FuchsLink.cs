#region Meta
// FuchsControls
// Created: 22/01/2026
// Modified: 22/01/2026
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FuchsControls;

/// <summary>
/// Represents a clickable hyperlink label with customizable appearance and behavior.
/// </summary>
public partial class FuchsLink : Label
{
	public static readonly BindableProperty UrlProperty =
		BindableProperty.Create(
			nameof(Url),
			typeof(string),
			typeof(FuchsLink),
			defaultValue: null);

	public static readonly BindableProperty CommandProperty =
		BindableProperty.Create(
			nameof(Command),
			typeof(ICommand),
			typeof(FuchsLink),
			defaultValue: null);

	public static readonly BindableProperty CommandParameterProperty =
		BindableProperty.Create(
			nameof(CommandParameter),
			typeof(object),
			typeof(FuchsLink),
			defaultValue: null);

	public static readonly BindableProperty UnderlineProperty =
		BindableProperty.Create(
			nameof(Underline),
			typeof(bool),
			typeof(FuchsLink),
			defaultValue: true,
			propertyChanged: (b, _, __) => ((FuchsLink)b).UpdateDecoration());

	public static readonly BindableProperty LinkColorProperty =
		BindableProperty.Create(
			nameof(LinkColor),
			typeof(Color),
			typeof(FuchsLink),
			defaultValue: Colors.Blue,
			propertyChanged: (b, _, __) => ((FuchsLink)b).UpdateDecoration());

	private bool _isHovered;

	public FuchsLink()
	{
		TextColor = LinkColor;
		UpdateDecoration();

		GestureRecognizers.Add(new TapGestureRecognizer
		{
			Command = new Command(async () => await OnTappedAsync())
		});

#if WINDOWS || MACCATALYST
		var pointer = new PointerGestureRecognizer();
		pointer.PointerEntered += (_, _) =>
		{
			_isHovered = true;
			UpdateDecoration();
		};
		pointer.PointerExited += (_, _) =>
		{
			_isHovered = false;
			UpdateDecoration();
		};
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

	private void UpdateDecoration()
	{
		TextColor = LinkColor;

		// Only underline while hovered (and only on platforms where hover exists).
		TextDecorations = (Underline && _isHovered) ? TextDecorations.Underline : TextDecorations.None;
	}

	private async Task OnTappedAsync()
	{
		var parameter = CommandParameter;

		if (Command?.CanExecute(parameter) == true)
		{
			Command.Execute(parameter);
			return;
		}

		if (string.IsNullOrWhiteSpace(Url))
			return;

		if (Uri.TryCreate(Url, UriKind.Absolute, out var uri))
			await Launcher.Default.OpenAsync(uri);
	}
}