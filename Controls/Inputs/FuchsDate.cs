#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls.Controls;

public sealed class FuchsDate : FuchsComponent
{
	private readonly Border _root;
	private readonly DatePicker _picker;
	private readonly Entry _entry;
	private readonly Label _label;
	private readonly Label _helperText;
	private bool _updating;

	public static readonly BindableProperty DateProperty =
		BindableProperty.Create(nameof(Date), typeof(DateTime?), typeof(FuchsDate), DateTime.Today, BindingMode.TwoWay, propertyChanged: OnDateChanged);

	public static readonly BindableProperty MinimumDateProperty =
		BindableProperty.Create(nameof(MinimumDate), typeof(DateTime), typeof(FuchsDate), DateTime.MinValue, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty MaximumDateProperty =
		BindableProperty.Create(nameof(MaximumDate), typeof(DateTime), typeof(FuchsDate), DateTime.MaxValue, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty FormatProperty =
		BindableProperty.Create(nameof(Format), typeof(string), typeof(FuchsDate), "d", propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty LabelProperty =
		BindableProperty.Create(nameof(Label), typeof(string), typeof(FuchsDate), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty HelperTextProperty =
		BindableProperty.Create(nameof(HelperText), typeof(string), typeof(FuchsDate), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty IsFocusedProperty =
		BindableProperty.Create(nameof(IsFocused), typeof(bool), typeof(FuchsDate), false, BindingMode.OneWayToSource);

	public DateTime? Date
	{
		get => (DateTime?)GetValue(DateProperty);
		set => SetValue(DateProperty, value);
	}

	public DateTime MinimumDate
	{
		get => (DateTime)GetValue(MinimumDateProperty);
		set => SetValue(MinimumDateProperty, value);
	}

	public DateTime MaximumDate
	{
		get => (DateTime)GetValue(MaximumDateProperty);
		set => SetValue(MaximumDateProperty, value);
	}

	public string Format
	{
		get => (string)GetValue(FormatProperty);
		set => SetValue(FormatProperty, value);
	}

	public string Label
	{
		get => (string)GetValue(LabelProperty);
		set => SetValue(LabelProperty, value);
	}

	public string HelperText
	{
		get => (string)GetValue(HelperTextProperty);
		set => SetValue(HelperTextProperty, value);
	}

	public bool IsFocused
	{
		get => (bool)GetValue(IsFocusedProperty);
		private set => SetValue(IsFocusedProperty, value);
	}

	public FuchsDate()
	{
		_label = new Label();
		_helperText = new Label();
		_picker = new DatePicker();
		_entry = new Entry { ClearButtonVisibility = ClearButtonVisibility.WhileEditing };
		_picker.DateSelected += OnPickerDateSelected;
		_entry.TextChanged += OnEntryTextChanged;
		_picker.Focused += OnFocused;
		_picker.Unfocused += OnUnfocused;
		_entry.Focused += OnFocused;
		_entry.Unfocused += OnUnfocused;

		Grid input = new() { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
		input.Add(_entry, 0, 0);
		input.Add(_picker, 1, 0);
		_root = new Border { Content = input };
		Grid layout = new()
		{
			RowDefinitions = { new RowDefinition(GridLength.Auto), new RowDefinition(GridLength.Auto), new RowDefinition(GridLength.Auto) }, RowSpacing = 4
		};
		layout.Add(_label, 0, 0);
		layout.Add(_root, 0, 1);
		layout.Add(_helperText, 0, 2);
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
		_helperText.Text = HelperText;
		_helperText.IsVisible = !string.IsNullOrWhiteSpace(HelperText);
		_helperText.TextColor = theme.TextLight;
		_helperText.FontSize = theme.FontSizeSm;
		_picker.Date = Date ?? DateTime.Today;
		_picker.MinimumDate = MinimumDate;
		_picker.MaximumDate = MaximumDate;
		_picker.Format = Format;
		_picker.TextColor = theme.Text;
		_entry.TextColor = theme.Text;
		_entry.PlaceholderColor = theme.TextLight;
		_entry.FontSize = ResolveFontSize();
		_picker.FontSize = ResolveFontSize();
		_entry.IsEnabled = !IsDisabled;
		_picker.IsEnabled = !IsDisabled;
		_root.BackgroundColor = theme.BackgroundDark;
		_root.Stroke = IsFocused ? new SolidColorBrush(theme.Primary) : new SolidColorBrush(theme.BackgroundDarker);
		_root.StrokeThickness = theme.BorderWidth;
		_root.StrokeShape = new RoundRectangle { CornerRadius = theme.CornerRadius };
		_root.Padding = ResolvePadding();
		_root.Opacity = IsDisabled ? 0.65 : 1;
	}

	private void OnPickerDateSelected(object? sender, DateChangedEventArgs e)
	{
		if (!_updating)
			Date = e.NewDate;
	}

	private void OnEntryTextChanged(object? sender, TextChangedEventArgs e)
	{
		if (_updating || !DateTime.TryParse(e.NewTextValue, out DateTime date))
			return;

		Date = date;
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

	private static void OnDateChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is not FuchsDate date || date._updating || newValue is not DateTime value)
			return;

		date._updating = true;
		try
		{
			date._picker.Date = value;
			date._entry.Text = value.ToString(date.Format);
		}
		finally
		{
			date._updating = false;
		}
	}

	private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsDate date)
			date.ApplyTheme();
	}
}