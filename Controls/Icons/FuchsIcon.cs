using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls;

public sealed class FuchsIcon : ContentView
{
	private const double DefaultSize = 24;

	public static readonly BindableProperty PathDataProperty = BindableProperty.Create(
		nameof(PathData), typeof(string), typeof(FuchsIcon), string.Empty, propertyChanged: OnIconPropertyChanged);

	public static readonly BindableProperty IconProperty = BindableProperty.Create(
		nameof(Icon), typeof(string), typeof(FuchsIcon), FuchsIcons.QuestionMark, propertyChanged: OnIconPropertyChanged);

	public static readonly BindableProperty ThemeColorProperty = BindableProperty.Create(
		nameof(Color), typeof(FuchsThemeColor), typeof(FuchsIcon), FuchsThemeColor.Text, BindingMode.TwoWay, propertyChanged: OnIconPropertyChanged);

	public static readonly BindableProperty SizeProperty = BindableProperty.Create(
		nameof(Size), typeof(double), typeof(FuchsIcon), DefaultSize, BindingMode.TwoWay, propertyChanged: OnSizeChanged,
		validateValue: static (_, value) => value is double size && size >= 0);

	private readonly Microsoft.Maui.Controls.Shapes.Path path = new();
	private bool isThemeChangeSubscribed;

	public FuchsIcon()
	{
		this.ApplyFuchsStyle("FuchsIconStyle");
		path.ApplyFuchsStyle("FuchsIconPathStyle");
		Content = path;
		UpdateSize();
		UpdateIcon();
	}

	public string PathData
	{
		get => (string)GetValue(PathDataProperty);
		set => SetValue(PathDataProperty, value);
	}

	public string Icon
	{
		get => (string)GetValue(IconProperty);
		set => SetValue(IconProperty, value);
	}

	public FuchsThemeColor Color
	{
		get => (FuchsThemeColor)GetValue(ThemeColorProperty);
		set => SetValue(ThemeColorProperty, value);
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
		var pathData = string.IsNullOrWhiteSpace(PathData) ? Icon : PathData;
		path.Data = string.IsNullOrWhiteSpace(pathData)
			? null
			: new PathGeometryConverter().ConvertFromInvariantString(pathData) as Geometry;
	}
}