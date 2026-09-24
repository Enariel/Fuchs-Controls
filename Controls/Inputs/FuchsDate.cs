#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls.Controls;

public sealed class FuchsDate : FormFieldControl
{
	private readonly DatePicker _picker;
	private readonly Entry _entry;
	private bool _updating;

	public static readonly BindableProperty DateProperty =
		BindableProperty.Create(nameof(Date), typeof(DateTime?), typeof(FuchsDate), DateTime.Today, BindingMode.TwoWay, propertyChanged: OnDateChanged);

	public static readonly BindableProperty MinimumDateProperty =
		BindableProperty.Create(nameof(MinimumDate), typeof(DateTime), typeof(FuchsDate), DateTime.MinValue, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty MaximumDateProperty =
		BindableProperty.Create(nameof(MaximumDate), typeof(DateTime), typeof(FuchsDate), DateTime.MaxValue, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty FormatProperty =
		BindableProperty.Create(nameof(Format), typeof(string), typeof(FuchsDate), "d", propertyChanged: OnVisualPropertyChanged);


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

	public FuchsDate()
	{
		_picker = new DatePicker();
		_entry = new Entry { ClearButtonVisibility = ClearButtonVisibility.WhileEditing };
		Grid input = new() { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
		input.Add(_entry, 0, 0);
		input.Add(_picker, 1, 0);
		InitializeField(input);
		AttachFocusTarget(_picker);
		AttachFocusTarget(_entry);
		_picker.DateSelected += OnPickerDateSelected;
		_entry.TextChanged += OnEntryTextChanged;

		ApplyTheme();
	}

	protected override void ApplyTheme()
	{
		base.ApplyTheme();
		FuchsTheme theme = FuchsThemeProvider.Current;
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