#region Meta

// FuchsControls
// Created: 04/02/2026
// Modified: 04/02/2026

#endregion

using System.Windows.Input;

namespace FuchsControls;

/// <summary>
/// Base button component providing common behavior (Command, busy state, visuals).
/// Derived classes should only supply the inner content via <see cref="SetInnerContent"/>.
/// </summary>
public class FuchsButtonBase : ContentView
{
	public static readonly BindableProperty CommandProperty = BindableProperty.Create(
		nameof(Command), typeof(ICommand), typeof(FuchsButtonBase));

	public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
		nameof(CommandParameter), typeof(object), typeof(FuchsButtonBase));

	public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(
		nameof(IsBusy), typeof(bool), typeof(FuchsButtonBase), false,
		propertyChanged: (b, _, _) => ((FuchsButtonBase)b).UpdateBusyState());

	public static readonly BindableProperty AccentColorProperty = BindableProperty.Create(
		nameof(AccentColor), typeof(Color), typeof(FuchsButtonBase), null,
		propertyChanged: (b, _, _) => ((FuchsButtonBase)b).UpdateColors());

	public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
		nameof(CornerRadius), typeof(CornerRadius), typeof(FuchsButtonBase), new CornerRadius(8),
		propertyChanged: (b, _, _) => ((FuchsButtonBase)b).UpdateCornerRadius());

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

	public bool IsBusy
	{
		get => (bool)GetValue(IsBusyProperty);
		set => SetValue(IsBusyProperty, value);
	}

	public Color? AccentColor
	{
		get => (Color?)GetValue(AccentColorProperty);
		set => SetValue(AccentColorProperty, value);
	}

	public CornerRadius CornerRadius
	{
		get => (CornerRadius)GetValue(CornerRadiusProperty);
		set => SetValue(CornerRadiusProperty, value);
	}

	public event EventHandler? Clicked;

	private readonly Grid _root;
	private readonly Border _border;
	private readonly Grid _contentHost;
	private readonly ActivityIndicator _spinner;

	public FuchsButtonBase()
	{
		// Root grid allows us to overlay the spinner
		_root = new Grid();

		_contentHost = new Grid
		{
			InputTransparent = false
		};

		_spinner = new ActivityIndicator
		{
			IsVisible = false,
			IsRunning = false,
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center
		};

		_border = new Border
		{
			Content = _contentHost,
			StrokeThickness = 1,
			Padding = new Thickness(12, 10),
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center
		};

		// Dynamic resources (align with tokens/colors)
		this.SetDynamicResource(VisualElement.BackgroundColorProperty, "FuchsBgColor4");
		this.SetDynamicResource(CornerRadiusProperty, "FuchsCornerRadius");
		this.SetDynamicResource(AccentColorProperty, "FuchsAccentColor");
		_border.SetDynamicResource(Border.StrokeProperty, "FuchsBgColor3");
		_border.SetDynamicResource(Border.StrokeThicknessProperty, "FuchsBorderWidth");
		_border.SetBinding(VisualElement.BackgroundColorProperty, new Binding(nameof(BackgroundColor), source: this));

		// Apply corner radius shape
		UpdateCornerRadius();

		_root.Children.Add(_border);
		_root.Children.Add(_spinner);
		Content = _root;

		// Accessibility
		AutomationProperties.SetIsInAccessibleTree(this, true);

		// Interaction
		var tap = new TapGestureRecognizer();
		tap.Tapped += OnTapped;
		GestureRecognizers.Add(tap);

		// Visual states inspired by SCSS hover/pressed effects
		VisualStateManager.SetVisualStateGroups(this, new VisualStateGroupList
		{
			new VisualStateGroup
			{
				Name = "CommonStates",
				States =
				{
					new VisualState{ Name = "Normal" },
					new VisualState
					{
						Name = "PointerOver",
						Setters =
						{
							new Setter{ TargetName = null, Property = OpacityProperty, Value = 0.95 }
						}
					},
					new VisualState
					{
						Name = "Pressed",
						Setters =
						{
							new Setter{ TargetName = null, Property = OpacityProperty, Value = 0.9 },
							new Setter{ TargetName = null, Property = TranslationYProperty, Value = 2 }
						}
					}
				}
			}
		});
	}

	protected void SetInnerContent(View content)
	{
		_contentHost.Children.Clear();
		_contentHost.Children.Add(content);
	}

	private void OnTapped(object? sender, TappedEventArgs e)
	{
		if (IsBusy) return;
		Clicked?.Invoke(this, EventArgs.Empty);
		if (Command?.CanExecute(CommandParameter) == true)
			Command.Execute(CommandParameter);
	}

	private void UpdateBusyState()
	{
		_spinner.IsVisible = IsBusy;
		_spinner.IsRunning = IsBusy;
		_contentHost.IsVisible = !IsBusy;
		_spinner.SetDynamicResource(ActivityIndicator.ColorProperty, "FuchsAccentColor");
	}

	private void UpdateColors()
	{
		// If AccentColor is provided, prefer it for text/icon in derived components.
		// Background and stroke are already dynamic via resources; no-op here.
	}

	private void UpdateCornerRadius()
	{
		var shape = new Microsoft.Maui.Controls.Shapes.RoundRectangle();
		shape.SetBinding(Microsoft.Maui.Controls.Shapes.RoundRectangle.CornerRadiusProperty,
			new Binding(nameof(CornerRadius), source: this));
		_border.StrokeShape = shape;
	}
}
