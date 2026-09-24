#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls.Controls;

public sealed class FuchsColor : FormField
{
	private readonly Entry _entry;
	private readonly BoxView _preview;
	private bool _updating;

	public static readonly BindableProperty SelectedColorProperty =
		BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(FuchsColor), Colors.Transparent, BindingMode.TwoWay
			, propertyChanged: OnColorChanged);

	public new static readonly BindableProperty PlaceholderProperty =
		BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(FuchsColor), "#RRGGBB", propertyChanged: OnVisualPropertyChanged);

	public Color SelectedColor
	{
		get => (Color)GetValue(SelectedColorProperty);
		set => SetValue(SelectedColorProperty, value);
	}


	public new string Placeholder
	{
		get => (string)GetValue(PlaceholderProperty);
		set => SetValue(PlaceholderProperty, value);
	}

	public FuchsColor()
	{
		_entry = new Entry { ClearButtonVisibility = ClearButtonVisibility.WhileEditing };
		_preview = new BoxView { WidthRequest = 30, HeightRequest = 30, CornerRadius = 8, VerticalOptions = LayoutOptions.Center };
		Grid input = new() { ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) }, ColumnSpacing = 8 };
		input.Add(_preview, 0, 0);
		input.Add(_entry, 1, 0);
		InitializeField(input);
		AttachFocusTarget(_entry);
		_entry.TextChanged += OnTextChanged;
		ApplyTheme();
	}

	protected override void ApplyTheme()
	{
		base.ApplyTheme();
		FuchsTheme theme = FuchsThemeProvider.Current;
		_entry.Placeholder = Placeholder;
		_entry.TextColor = theme.Text;
		_entry.PlaceholderColor = theme.TextLight;
		_entry.FontSize = ResolveFontSize();
		_entry.IsEnabled = !IsDisabled;
		_preview.Color = SelectedColor;
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