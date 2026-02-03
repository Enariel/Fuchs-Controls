#region Meta

// FuchsControls
// Created: 23/01/2026
// Modified: 23/01/2026

#endregion

namespace FuchsControls.Fields;

public abstract class FieldBase : ContentView, IFieldBase
{
	public static readonly BindableProperty LabelProperty = BindableProperty.Create(
		nameof(Label),
		typeof(string),
		typeof(FieldBase),
		defaultValue: string.Empty);

	public static readonly BindableProperty HelpTextProperty = BindableProperty.Create(
		nameof(HelpText),
		typeof(string),
		typeof(FieldBase),
		defaultValue: string.Empty);

	public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
		nameof(Placeholder),
		typeof(string),
		typeof(FieldBase),
		defaultValue: string.Empty);

	public static readonly BindableProperty OrientationProperty = BindableProperty.Create(
		nameof(Orientation),
		typeof(StackOrientation),
		typeof(FieldBase),
		defaultValue: StackOrientation.Vertical);

	public static readonly BindableProperty IsHelpVisibleProperty = BindableProperty.Create(
		nameof(IsHelpVisible),
		typeof(bool),
		typeof(FieldBase),
		defaultValue: false);

	public string Label
	{
		get => (string)GetValue(LabelProperty);
		set => SetValue(LabelProperty, value);
	}

	public string HelpText
	{
		get => (string)GetValue(HelpTextProperty);
		set => SetValue(HelpTextProperty, value);
	}

	public string Placeholder
	{
		get => (string)GetValue(PlaceholderProperty);
		set => SetValue(PlaceholderProperty, value);
	}

	public StackOrientation Orientation
	{
		get => (StackOrientation)GetValue(OrientationProperty);
		set => SetValue(OrientationProperty, value);
	}

	public bool IsHelpVisible
	{
		get => (bool)GetValue(IsHelpVisibleProperty);
		set => SetValue(IsHelpVisibleProperty, value);
	}

	protected Label CreateLabel()
	{
		var label = new Label { FontSize = 16 };
		label.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(Label), source: this, mode: BindingMode.OneWay));
		return label;
	}

	protected View CreateLabelWithHelpToggle()
	{
		var label = CreateLabel();

#if WINDOWS
		return label;
#else
		var helpButton = new Button
		{
			Text = "?",
			FontSize = 12,
			Padding = new Thickness(4, 0),
			BackgroundColor = Colors.Transparent
		};

		helpButton.SetBinding(IsVisibleProperty,
			new Binding(nameof(HelpText), source: this, converter: new FuchsControls.StringIsNotNullOrEmptyConverter()));
		helpButton.Clicked += (_, _) => IsHelpVisible = !IsHelpVisible;
		AutomationProperties.SetName(helpButton, "Help");

		return new HorizontalStackLayout
		{
			Spacing = 4,
			Children = { label, helpButton }
		};
#endif
	}

	protected View CreateHelpText()
	{
#if WINDOWS
		var invisibleHelpLabel = new Label { IsVisible = false };
		invisibleHelpLabel.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(HelpText), source: this));
		return invisibleHelpLabel;
#else
		var helpLabel = new Label { FontSize = 12 };
		helpLabel.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding(nameof(HelpText), source: this));
		helpLabel.SetBinding(VisualElement.IsVisibleProperty,
			new MultiBinding
			{
				Bindings =
				{
					new Binding(nameof(IsHelpVisible), source: this),
					new Binding(nameof(HelpText), source: this)
				},
				Converter = new FuchsControls.HelpTextVisibilityConverter()
			});
		return helpLabel;
#endif
	}

	protected void ApplyToolTip(View target)
	{
#if WINDOWS
		ToolTipProperties.SetText(
			target,
			new Binding(
				nameof(HelpText),
				source: this,
				converter: new FuchsControls.EmptyStringToNullConverter()));
#endif
	}

	protected void ApplyAccessibility(View target)
	{
		target.SetBinding(SemanticProperties.HintProperty, new Binding(nameof(HelpText), source: this));
		target.SetBinding(SemanticProperties.DescriptionProperty, new Binding(nameof(HelpText), source: this));
	}
}