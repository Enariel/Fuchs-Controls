using Microsoft.Maui.Controls.Xaml;

namespace FuchsControls;

[ContentProperty(nameof(Content))]
public sealed class FuchsAccordionItem : BindableObject
{
	public static readonly BindableProperty HeaderProperty = BindableProperty.Create(
		nameof(Header), typeof(string), typeof(FuchsAccordionItem), string.Empty);

	public static readonly BindableProperty HeaderFormattedTextProperty = BindableProperty.Create(
		nameof(HeaderFormattedText), typeof(FormattedString), typeof(FuchsAccordionItem));

	public static readonly BindableProperty HeaderViewProperty = BindableProperty.Create(
		nameof(HeaderView), typeof(View), typeof(FuchsAccordionItem));

	public static readonly BindableProperty ContentProperty = BindableProperty.Create(
		nameof(Content), typeof(View), typeof(FuchsAccordionItem));

	public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(
		nameof(IsExpanded), typeof(bool), typeof(FuchsAccordionItem), false, BindingMode.TwoWay);

	public static readonly BindableProperty IsEnabledProperty = BindableProperty.Create(
		nameof(IsEnabled), typeof(bool), typeof(FuchsAccordionItem), true);

	public string Header
	{
		get => (string)GetValue(HeaderProperty);
		set => SetValue(HeaderProperty, value);
	}

	/// <summary>
	/// Gets or sets optional formatted header text. Use <see cref="FuchsSpan"/> instances for inline typography.
	/// </summary>
	public FormattedString? HeaderFormattedText
	{
		get => (FormattedString?)GetValue(HeaderFormattedTextProperty);
		set => SetValue(HeaderFormattedTextProperty, value);
	}

	/// <summary>
	/// Gets or sets an optional custom header view. When set, it replaces the default header text.
	/// </summary>
	public View? HeaderView
	{
		get => (View?)GetValue(HeaderViewProperty);
		set => SetValue(HeaderViewProperty, value);
	}

	public View? Content
	{
		get => (View?)GetValue(ContentProperty);
		set => SetValue(ContentProperty, value);
	}

	public bool IsExpanded
	{
		get => (bool)GetValue(IsExpandedProperty);
		set => SetValue(IsExpandedProperty, value);
	}

	public bool IsEnabled
	{
		get => (bool)GetValue(IsEnabledProperty);
		set => SetValue(IsEnabledProperty, value);
	}
}