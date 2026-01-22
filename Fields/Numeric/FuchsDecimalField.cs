#region Meta

// FuchsControls
// Created: 11/01/2026
// Modified: 11/01/2026

#endregion

using CommunityToolkit.Maui.Converters;

namespace FuchsControls.Fields;

public partial class FuchsDecimal : NumericFieldBase<decimal>
{
	public FuchsDecimal()
	{
		Margin = new Thickness(0, 5, 0, 5);

		BuildLayout();
	}

	private void BuildLayout()
	{
		MainEditor = new Entry
		{
			Keyboard = Keyboard.Numeric,
		};
		MainEditor.SetBinding(Entry.IsReadOnlyProperty, new Binding(nameof(IsReadOnly), source: this, mode: BindingMode.TwoWay));
		MainEditor.SetBinding(Entry.TextProperty, new Binding(nameof(Text), source: this, mode: BindingMode.TwoWay));
		MainEditor.TextChanged += OnTextChanged!;

		MainStepper = new Stepper
		{
			IsVisible = DeviceInfo.Current.Platform == DevicePlatform.WinUI
		};
		MainStepper.SetBinding(Stepper.IsEnabledProperty,
			new Binding(nameof(IsReadOnly), source: this, converter: new InvertedBoolConverter()));
		MainStepper.SetBinding(Stepper.MinimumProperty,
			new Binding(nameof(Minimum), source: this, mode: BindingMode.TwoWay, converter: new DoubleToDecimalConverter()));
		MainStepper.SetBinding(Stepper.MaximumProperty,
			new Binding(nameof(Maximum), source: this, mode: BindingMode.TwoWay, converter: new DoubleToDecimalConverter()));
		MainStepper.SetBinding(Stepper.ValueProperty,
			new Binding(nameof(NumericValue), source: this, mode: BindingMode.TwoWay, converter: new DoubleToDecimalConverter()));
		MainStepper.ValueChanged += OnNumberChanged;

		// 3. Border & Grid
		var grid = new Grid
		{
			RowDefinitions = { new RowDefinition { Height = GridLength.Auto } },
			ColumnDefinitions =
			{
				new ColumnDefinition { Width = GridLength.Star },
				new ColumnDefinition { Width = GridLength.Auto }
			}
		};
		grid.Add(MainEditor, 0, 0);
		grid.Add(MainStepper, 1, 0);

#if WINDOWS
		if (!string.IsNullOrEmpty(HelpText))
			ToolTipProperties.SetText(MainStepper, new Binding(nameof(HelpText), source: this));
#endif
		
		var editorBorder = new Border
		{
			Content = grid
		};

		// 4. Mobile Stepper
		MobileStepper = new Stepper
		{
			Margin = new Thickness(0, 6, 0, 0),
			IsVisible = DeviceInfo.Current.Platform != DevicePlatform.WinUI
		};
		MobileStepper.SetBinding(Stepper.IsEnabledProperty, new Binding(nameof(IsReadOnly), source: this, converter: new InvertedBoolConverter()));
		MobileStepper.SetBinding(Stepper.MinimumProperty,
			new Binding(nameof(Minimum), source: this, mode: BindingMode.TwoWay, converter: new DoubleToDecimalConverter()));
		MobileStepper.SetBinding(Stepper.MaximumProperty,
			new Binding(nameof(Maximum), source: this, mode: BindingMode.TwoWay, converter: new DoubleToDecimalConverter()));
		MobileStepper.SetBinding(Stepper.ValueProperty,
			new Binding(nameof(NumericValue), source: this, mode: BindingMode.TwoWay, converter: new DoubleToDecimalConverter()));
		MobileStepper.ValueChanged += OnNumberChanged;

		// 5. Labels
		var label = new Label { };
		label.SetBinding(TextProperty, new Binding(nameof(Label), source: this));

		var helpLabel = new Label
		{
			FontSize = 12,
			IsVisible = DeviceInfo.Current.Platform != DevicePlatform.WinUI
		};
		helpLabel.SetBinding(TextProperty, new Binding(nameof(HelpText), source: this));

		// 6. Assemble StackLayout
		Content = new StackLayout
		{
			Orientation = StackOrientation.Horizontal,
			Spacing = 5,
			Children =
			{
				label, editorBorder, MobileStepper, helpLabel
			}
		};
	}

	private Stepper MobileStepper { get; set; } = null!;

	private Stepper MainStepper { get; set; } = null!;

	private Entry MainEditor { get; set; } = null!;

	/// <inheritdoc />
	protected override void OnNumberChanged(object? sender, ValueChangedEventArgs e)
	{
		NumericValue = (decimal)e.NewValue;
		Text = NumericValue.ToString();
	}

	/// <inheritdoc />
	protected override void OnTextChanged(object sender, TextChangedEventArgs e)
	{
		if (string.IsNullOrWhiteSpace(e.NewTextValue)) return;

		if (decimal.TryParse(e.NewTextValue, out var value))
			NumericValue = value;
		else
			Text = NumericValue.ToString();
	}
}