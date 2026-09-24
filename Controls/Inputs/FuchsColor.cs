#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls.Controls;

public sealed class FuchsColor : FuchsComponent
{
	private readonly Border _root;
	private readonly Entry _entry;
	private readonly BoxView _preview;
	private readonly Label _label;
	private bool _updating;

	public static readonly BindableProperty SelectedColorProperty =
		BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(FuchsColor), Colors.Transparent, BindingMode.TwoWay
			, propertyChanged: OnColorChanged);

	public static readonly BindableProperty LabelProperty =
		BindableProperty.Create(nameof(Label), typeof(string), typeof(FuchsColor), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty PlaceholderProperty =
		BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(FuchsColor), "#RRGGBB", propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty IsFocusedProperty =
		BindableProperty.Create(nameof(IsFocused), typeof(bool), typeof(FuchsColor), false, BindingMode.OneWayToSource);

	public Color SelectedColor
	{
		get => (Color)GetValue(SelectedColorProperty);
		set => SetValue(SelectedColorProperty, value);
	}

	public string Label
	{
		get => (string)GetValue(LabelProperty);
		set => SetValue(LabelProperty, value);
	}

	public string Placeholder
	{
		get => (string)GetValue(PlaceholderProperty);
		set => SetValue(PlaceholderProperty, value);
	}

	public bool IsFocused
	{
		get => (bool)GetValue(IsFocusedProperty);
		private set => SetValue(IsFocusedProperty, value);
	}

	public FuchsColor()
	{
		_label = new Label();
		_entry = new Entry { ClearButtonVisibility = ClearButtonVisibility.WhileEditing };
		_preview = new BoxView { WidthRequest = 30, HeightRequest = 30, CornerRadius = 8, VerticalOptions = LayoutOptions.Center };
		_entry.TextChanged += OnTextChanged;
		_entry.Focused += OnFocused;
		_entry.Unfocused += OnUnfocused;
		HorizontalStackLayout input = new() { Spacing = 8, Children = { _preview, _entry } };
		_root = new Border { Content = input };
		VerticalStackLayout layout = new() { Spacing = 4, Children = { _label, _root } };
		Content = layout;
		ApplyTheme();
	}

	protected override void ApplyTheme()
	{
		FuchsTheme theme = FuchsThemeProvider.Current;
		_label.Text = Label;
		_label.IsVisible = !string.IsNullOrWhiteSpace(Label);
		_label.TextColor = theme.Text;
		_label.FontSize = theme.FontSizeSm;
		_entry.Placeholder = Placeholder;
		_entry.TextColor = theme.Text;
		_entry.PlaceholderColor = theme.TextLight;
		_entry.FontSize = ResolveFontSize();
		_entry.IsEnabled = !IsDisabled;
		_preview.Color = SelectedColor;
		_root.BackgroundColor = theme.BackgroundDark;
		_root.Stroke = IsFocused ? new SolidColorBrush(theme.Primary) : new SolidColorBrush(theme.BackgroundDarker);
		_root.StrokeThickness = theme.BorderWidth;
		_root.StrokeShape = new RoundRectangle { CornerRadius = theme.CornerRadius };
		_root.Padding = ResolvePadding();
		_root.Opacity = IsDisabled ? 0.65 : 1;
	}

	private void OnTextChanged(object? sender, TextChangedEventArgs e)
	{
		if (_updating || string.IsNullOrWhiteSpace(e.NewTextValue))
			return;

		try
		{
			Color color = Microsoft.Maui.Graphics.Color.Parse(e.NewTextValue);
			SelectedColor = color;
		}
		catch (ArgumentException) { }
	}

	private void OnFocused(object? sender, FocusEventArgs e)
	{
		IsFocused = true;
		ApplyTheme();
	}

	private void OnUnfocused(object? sender, FocusEventArgs e)
	{
		IsFocused = false;
		ApplyTheme();
	}

	private static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is not FuchsColor color || color._updating || newValue is not Color value)
			return;

		color._updating = true;
		try
		{
			color._preview.Color = value;
			color._entry.Text = value.ToArgbHex();
		}
		finally
		{
			color._updating = false;
		}
	}

	private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsColor color)
			color.ApplyTheme();
	}
}