#region Meta

// FuchsControls
// Created: 03/02/2026
// Modified: 03/02/2026

#endregion

using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls.Containers;

/// <summary>
/// A lightweight card component designed for custom content and custom actions.
/// Optimized for use in large collections.
/// </summary>
[ContentProperty(nameof(CardContent))]
public class FuchsContentCard : ContentView
{
	public static readonly BindableProperty HeaderContentProperty = BindableProperty.Create(
		nameof(HeaderContent),
		typeof(View),
		typeof(FuchsContentCard),
		null,
		propertyChanged: (b, o, n) => ((FuchsContentCard)b)._headerContainer.Content = (View)n);

	public static readonly BindableProperty CardContentProperty = BindableProperty.Create(
		nameof(CardContent),
		typeof(View),
		typeof(FuchsContentCard),
		null,
		propertyChanged: (b, o, n) => ((FuchsContentCard)b)._bodyContainer.Content = (View)n);

	public static readonly BindableProperty ActionContentProperty = BindableProperty.Create(
		nameof(ActionContent),
		typeof(View),
		typeof(FuchsContentCard),
		null,
		propertyChanged: (b, o, n) => ((FuchsContentCard)b)._actionContainer.Content = (View)n);

	public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
		nameof(CornerRadius),
		typeof(CornerRadius),
		typeof(FuchsContentCard),
		new CornerRadius(8));

	public static readonly BindableProperty SpacingProperty = BindableProperty.Create(
		nameof(Spacing),
		typeof(double),
		typeof(FuchsContentCard),
		10.0);

	public static readonly BindableProperty AccentColorProperty = BindableProperty.Create(
		nameof(AccentColor),
		typeof(Color),
		typeof(FuchsContentCard),
		null);

	public View? HeaderContent
	{
		get => (View?)GetValue(HeaderContentProperty);
		set => SetValue(HeaderContentProperty, value);
	}

	public View? CardContent
	{
		get => (View?)GetValue(CardContentProperty);
		set => SetValue(CardContentProperty, value);
	}

	public View? ActionContent
	{
		get => (View?)GetValue(ActionContentProperty);
		set => SetValue(ActionContentProperty, value);
	}

	public CornerRadius CornerRadius
	{
		get => (CornerRadius)GetValue(CornerRadiusProperty);
		set => SetValue(CornerRadiusProperty, value);
	}

	public double Spacing
	{
		get => (double)GetValue(SpacingProperty);
		set => SetValue(SpacingProperty, value);
	}

	public Color? AccentColor
	{
		get => (Color?)GetValue(AccentColorProperty);
		set => SetValue(AccentColorProperty, value);
	}

	private readonly Border _border;
	private readonly VerticalStackLayout _mainLayout;
	private readonly ContentView _headerContainer;
	private readonly ContentView _bodyContainer;
	private readonly ContentView _actionContainer;

	public FuchsContentCard()
	{
		_headerContainer = new ContentView { IsVisible = false };
		_headerContainer.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(ContentView.Content))
				_headerContainer.IsVisible = _headerContainer.Content != null;
		};

		_bodyContainer = new ContentView();
		_bodyContainer.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(ContentView.Content))
				_bodyContainer.IsVisible = _bodyContainer.Content != null;
		};

		_actionContainer = new ContentView { IsVisible = false };
		_actionContainer.PropertyChanged += (s, e) =>
		{
			if (e.PropertyName == nameof(ContentView.Content))
				_actionContainer.IsVisible = _actionContainer.Content != null;
		};

		_mainLayout = new VerticalStackLayout
		{
			Children = { _headerContainer, _bodyContainer, _actionContainer }
		};
		_mainLayout.SetBinding(StackBase.SpacingProperty, new Binding(nameof(Spacing), source: this));

		_border = new Border
		{
			Content = _mainLayout
		};

		var roundRect = new RoundRectangle();
		roundRect.SetBinding(RoundRectangle.CornerRadiusProperty, new Binding(nameof(CornerRadius), source: this));
		_border.StrokeShape = roundRect;

		// Bind background and padding properties to Root
		_border.SetBinding(Border.PaddingProperty, new Binding(nameof(Padding), source: this));
		_border.SetBinding(Border.BackgroundColorProperty, new Binding(nameof(BackgroundColor), source: this));
		_border.SetBinding(Border.BackgroundProperty, new Binding(nameof(Background), source: this));

		// Set default styles/resources
		this.SetDynamicResource(CornerRadiusProperty, "FuchsCornerRadius");
		this.SetDynamicResource(SpacingProperty, "FuchsSpacing");
		this.SetDynamicResource(PaddingProperty, "FuchsCardPadding");
		this.SetDynamicResource(AccentColorProperty, "FuchsAccentColor");

		_border.SetDynamicResource(Border.StrokeProperty, "FuchsBgColor3");
		_border.SetDynamicResource(Border.StrokeThicknessProperty, "FuchsBorderWidth");
		_border.SetDynamicResource(VisualElement.BackgroundColorProperty, "FuchsBgColor");

		Content = _border;

		// Accessibility
		AutomationProperties.SetIsInAccessibleTree(this, true);
	}
}
