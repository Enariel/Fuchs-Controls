using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls;

public sealed class FuchsIcon : ContentView
{
	private const double DefaultSize = 24;

	public static readonly BindableProperty PathDataProperty = BindableProperty.Create(
		nameof(PathData), typeof(string), typeof(FuchsIcon), string.Empty, propertyChanged: OnIconPropertyChanged);

	public static readonly BindableProperty ColorProperty = BindableProperty.Create(
		nameof(Color), typeof(FuchsThemeColor), typeof(FuchsIcon), FuchsThemeColor.Default, BindingMode.TwoWay, propertyChanged: OnIconPropertyChanged);

	public static readonly BindableProperty SizeProperty = BindableProperty.Create(
		nameof(Size), typeof(double), typeof(FuchsIcon), DefaultSize, BindingMode.TwoWay, propertyChanged: OnSizeChanged,
		validateValue: static (_, value) => value is double size && size >= 0);

	private readonly Microsoft.Maui.Controls.Shapes.Path path = new();
	private bool isThemeChangeSubscribed;

	public FuchsIcon()
	{
		InputTransparent = true;
		path.Aspect = Stretch.Uniform;
		path.HorizontalOptions = LayoutOptions.Fill;
		path.VerticalOptions = LayoutOptions.Fill;
		Content = path;
		UpdateSize();
		UpdateIcon();
	}

	public string PathData
	{
		get => (string)GetValue(PathDataProperty);
		set => SetValue(PathDataProperty, value);
	}

	public FuchsThemeColor Color
	{
		get => (FuchsThemeColor)GetValue(ColorProperty);
		set => SetValue(ColorProperty, value);
	}

	public double Size
	{
		get => (double)GetValue(SizeProperty);
		set => SetValue(SizeProperty, value);
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
		if (Handler is null)
		{
			UnsubscribeFromThemeChanges();
			return;
		}

		SubscribeToThemeChanges();
		UpdateIcon();
	}

	private static void OnIconPropertyChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsIcon)bindable).UpdateIcon();

	private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsIcon)bindable).UpdateSize();

	private void SubscribeToThemeChanges()
	{
		if (isThemeChangeSubscribed)
		{
			return;
		}

		FuchsThemeManager.ThemeChanged += OnThemeChanged;
		isThemeChangeSubscribed = true;
	}

	private void UnsubscribeFromThemeChanges()
	{
		if (!isThemeChangeSubscribed)
		{
			return;
		}

		FuchsThemeManager.ThemeChanged -= OnThemeChanged;
		isThemeChangeSubscribed = false;
	}

	private void OnThemeChanged(object? sender, EventArgs e) => UpdateIcon();

	private void UpdateSize()
	{
		WidthRequest = Size;
		HeightRequest = Size;
	}

	private void UpdateIcon()
	{
		path.Fill = new SolidColorBrush(FuchsThemeResourceLookup.GetColor(Color));
		path.Data = string.IsNullOrWhiteSpace(PathData)
			? null
			: new PathGeometryConverter().ConvertFromInvariantString(PathData) as Geometry;
	}
}