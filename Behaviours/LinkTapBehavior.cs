namespace FuchsControls.Behaviours;

public sealed class LinkTapBehavior : Behavior<Label>
{
	public static readonly BindableProperty CommandProperty =
		BindableProperty.Create(nameof(Command), typeof(Command), typeof(LinkTapBehavior));

	public static readonly BindableProperty CommandParameterProperty =
		BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(LinkTapBehavior));

	public static readonly BindableProperty UrlProperty =
		BindableProperty.Create(nameof(Url), typeof(string), typeof(LinkTapBehavior));

	public Command? Command
	{
		get => (Command?)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public object? CommandParameter
	{
		get => GetValue(CommandParameterProperty);
		set => SetValue(CommandParameterProperty, value);
	}

	public string? Url
	{
		get => (string?)GetValue(UrlProperty);
		set => SetValue(UrlProperty, value);
	}

	protected override void OnAttachedTo(Label bindable)
	{
		base.OnAttachedTo(bindable);

		TapGestureRecognizer tap = new();
		tap.Tapped += OnTapped;
		bindable.GestureRecognizers.Add(tap);
	}

	private async void OnTapped(object? sender, TappedEventArgs e)
	{
		if (Command?.CanExecute(CommandParameter) == true)
		{
			Command.Execute(CommandParameter);
			return;
		}

		if (!string.IsNullOrWhiteSpace(Url))
			await Launcher.OpenAsync(Url);
	}
}
