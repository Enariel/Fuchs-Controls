#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls.Controls;

public sealed class FuchsCheck : FuchsComponent
{
	private readonly Border _root;
	private readonly CheckBox _checkBox;
	private readonly Label _label;
	private bool _updating;

	public static readonly BindableProperty IsCheckedProperty =
		BindableProperty.Create(nameof(IsChecked), typeof(bool?), typeof(FuchsCheck), false, BindingMode.TwoWay, propertyChanged: OnCheckedChanged);

	public static readonly BindableProperty IsThreeStateProperty =
		BindableProperty.Create(nameof(IsThreeState), typeof(bool), typeof(FuchsCheck), false, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty LabelProperty =
		BindableProperty.Create(nameof(Label), typeof(string), typeof(FuchsCheck), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty IsFocusedProperty =
		BindableProperty.Create(nameof(IsFocused), typeof(bool), typeof(FuchsCheck), false, BindingMode.OneWayToSource);

	public bool? IsChecked
	{
		get => (bool?)GetValue(IsCheckedProperty);
		set => SetValue(IsCheckedProperty, value);
	}

	public bool IsThreeState
	{
		get => (bool)GetValue(IsThreeStateProperty);
		set => SetValue(IsThreeStateProperty, value);
	}

	public string Label
	{
		get => (string)GetValue(LabelProperty);
		set => SetValue(LabelProperty, value);
	}

	public bool IsFocused
	{
		get => (bool)GetValue(IsFocusedProperty);
		private set => SetValue(IsFocusedProperty, value);
	}

	public FuchsCheck()
	{
		_checkBox = new CheckBox();
		_label = new Label { VerticalTextAlignment = TextAlignment.Center };
		_checkBox.CheckedChanged += OnCheckedChanged;
		_checkBox.Focused += OnFocused;
		_checkBox.Unfocused += OnUnfocused;
		HorizontalStackLayout content = new() { Spacing = 10, Children = { _checkBox, _label } };
		_root = new Border { Content = content };
		_root.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(CycleState) });
		Content = _root;
		ApplyTheme();
	}

	protected override void ApplyTheme()
	{
		FuchsTheme theme = FuchsThemeProvider.Current;
		_label.Text = Label;
		_label.TextColor = theme.Text;
		_label.FontSize = ResolveFontSize();
		_checkBox.IsChecked = IsChecked == true;
		_checkBox.IsEnabled = !IsDisabled && !IsThreeState;
		_root.BackgroundColor = Variant == FuchsVariant.Text ? Colors.Transparent : theme.BackgroundDark;
		_root.Stroke = IsFocused ? new SolidColorBrush(theme.Primary) : new SolidColorBrush(theme.BackgroundDarker);
		_root.StrokeThickness = IsFocused || Variant != FuchsVariant.Filled ? theme.BorderWidth : 0;
		_root.StrokeShape = new RoundRectangle { CornerRadius = theme.CornerRadius };
		_root.Padding = ResolvePadding();
		_root.Opacity = IsDisabled ? 0.65 : 1;
	}

	private void CycleState()
	{
		if (IsDisabled)
			return;

		if (IsThreeState)
			IsChecked = IsChecked switch { false => null, null => true, _ => false };
		else
			IsChecked = IsChecked != true;
	}

	private void OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
	{
		if (!_updating && !IsThreeState)
			IsChecked = e.Value;
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

	private static void OnCheckedChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsCheck control && !control._updating)
		{
			control._updating = true;
			try
			{
				control._checkBox.IsChecked = (bool?)newValue == true;
			}
			finally
			{
				control._updating = false;
			}

			control.ApplyTheme();
		}
	}

	private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsCheck control)
			control.ApplyTheme();
	}
}