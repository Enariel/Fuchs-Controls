#region Meta

// FuchsControls
// Created: 04/02/2026
// Modified: 04/02/2026

#endregion

using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls;

public class FuchsButton : FuchsButtonBase
{
	public static readonly BindableProperty TextProperty = BindableProperty.Create(
		nameof(Text), typeof(string), typeof(FuchsButton), string.Empty,
		propertyChanged: (b, _, _) => ((FuchsButton)b).UpdateContent());

	public static readonly BindableProperty IconPathDataProperty = BindableProperty.Create(
		nameof(IconPathData), typeof(string), typeof(FuchsButton), string.Empty,
		propertyChanged: (b, _, _) => ((FuchsButton)b).UpdateContent());

	public static readonly BindableProperty IconSizeProperty = BindableProperty.Create(
		nameof(IconSize), typeof(double), typeof(FuchsButton), 18.0,
		propertyChanged: (b, _, _) => ((FuchsButton)b).UpdateContent());

	public static readonly BindableProperty IconColorProperty = BindableProperty.Create(
		nameof(IconColor), typeof(Color), typeof(FuchsButton), null,
		propertyChanged: (b, _, _) => ((FuchsButton)b).UpdateContent());

	public static readonly BindableProperty IconPositionProperty = BindableProperty.Create(
		nameof(IconPosition), typeof(IconPosition), typeof(FuchsButton), IconPosition.Left,
		propertyChanged: (b, _, _) => ((FuchsButton)b).UpdateContent());

	public static readonly BindableProperty SpacingProperty = BindableProperty.Create(
		nameof(Spacing), typeof(double), typeof(FuchsButton), 8.0,
		propertyChanged: (b, _, _) => ((FuchsButton)b).UpdateContent());

	public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
		nameof(FontSize), typeof(double), typeof(FuchsButton), 14.0);

	public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
		nameof(TextColor), typeof(Color), typeof(FuchsButton), null);

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public string IconPathData
	{
		get => (string)GetValue(IconPathDataProperty);
		set => SetValue(IconPathDataProperty, value);
	}

	public double IconSize
	{
		get => (double)GetValue(IconSizeProperty);
		set => SetValue(IconSizeProperty, value);
	}

	public Color? IconColor
	{
		get => (Color?)GetValue(IconColorProperty);
		set => SetValue(IconColorProperty, value);
	}

	public IconPosition IconPosition
	{
		get => (IconPosition)GetValue(IconPositionProperty);
		set => SetValue(IconPositionProperty, value);
	}

	public double Spacing
	{
		get => (double)GetValue(SpacingProperty);
		set => SetValue(SpacingProperty, value);
	}

	public double FontSize
	{
		get => (double)GetValue(FontSizeProperty);
		set => SetValue(FontSizeProperty, value);
	}

	public Color? TextColor
	{
		get => (Color?)GetValue(TextColorProperty);
		set => SetValue(TextColorProperty, value);
	}

	private readonly StackLayout _layout;
	private readonly Label _label;
	private readonly FuchsIcon _icon;

	public FuchsButton()
	{
		_layout = new StackLayout
		{
			Orientation = StackOrientation.Horizontal,
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center
		};

		_label = new Label
		{
			VerticalOptions = LayoutOptions.Center,
			HorizontalOptions = LayoutOptions.Center
		};
		_label.SetBinding(Label.TextProperty, new Binding(nameof(Text), source: this));
		_label.SetBinding(Label.FontSizeProperty, new Binding(nameof(FontSize), source: this));
		_label.SetBinding(Label.TextColorProperty, new Binding(nameof(TextColor), source: this) { FallbackValue = Colors.White });
		_label.SetDynamicResource(VisualElement.StyleProperty, "typo-button");

		_icon = new FuchsIcon();
		_icon.SetBinding(FuchsIcon.PathDataProperty, new Binding(nameof(IconPathData), source: this));
		_icon.SetBinding(FuchsIcon.IconSizeProperty, new Binding(nameof(IconSize), source: this));
		_icon.SetBinding(FuchsIcon.FillColorProperty, new Binding(nameof(IconColor), source: this) { FallbackValue = Colors.White });

		UpdateContent();
		SetInnerContent(_layout);
		
		SemanticProperties.SetDescription(this, Text);
	}

	private void UpdateContent()
	{
		_layout.Children.Clear();
		_layout.Spacing = Spacing;

		bool hasIcon = !string.IsNullOrEmpty(IconPathData);
		bool hasText = !string.IsNullOrEmpty(Text);

		if (hasIcon && hasText)
		{
			switch (IconPosition)
			{
				case IconPosition.Left:
					_layout.Orientation = StackOrientation.Horizontal;
					_layout.Children.Add(_icon);
					_layout.Children.Add(_label);
					break;
				case IconPosition.Right:
					_layout.Orientation = StackOrientation.Horizontal;
					_layout.Children.Add(_label);
					_layout.Children.Add(_icon);
					break;
				case IconPosition.Top:
					_layout.Orientation = StackOrientation.Vertical;
					_layout.Children.Add(_icon);
					_layout.Children.Add(_label);
					break;
				case IconPosition.Bottom:
					_layout.Orientation = StackOrientation.Vertical;
					_layout.Children.Add(_label);
					_layout.Children.Add(_icon);
					break;
			}
		}
		else if (hasIcon)
		{
			_layout.Children.Add(_icon);
		}
		else if (hasText)
		{
			_layout.Children.Add(_label);
		}
		
		if (hasText)
		{
			SemanticProperties.SetDescription(this, Text);
		}
	}
}
