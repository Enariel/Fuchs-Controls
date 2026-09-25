using Microsoft.Maui.Controls.Xaml;

namespace FuchsControls;

[ContentProperty(nameof(Content))]
public sealed partial class FuchsTab : BindableObject
{
	public static readonly BindableProperty HeaderProperty = BindableProperty.Create(
		nameof(Header), typeof(string), typeof(FuchsTab), string.Empty);

	public static readonly BindableProperty HeaderFormattedTextProperty = BindableProperty.Create(
		nameof(HeaderFormattedText), typeof(FormattedString), typeof(FuchsTab));

	public static readonly BindableProperty HeaderViewProperty = BindableProperty.Create(
		nameof(HeaderView), typeof(View), typeof(FuchsTab));

	public static readonly BindableProperty ContentProperty = BindableProperty.Create(
		nameof(Content), typeof(View), typeof(FuchsTab));

	public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
		nameof(IsSelected), typeof(bool), typeof(FuchsTab), false);

	public static readonly BindableProperty IsEnabledProperty = BindableProperty.Create(
		nameof(IsEnabled), typeof(bool), typeof(FuchsTab), true);

	public string Header
	{
		get => (string)GetValue(HeaderProperty);
		set => SetValue(HeaderProperty, value);
	}

	/// <summary>
	/// Gets or sets optional formatted tab text. Use <see cref="FuchsSpan"/> instances for inline typography.
	/// </summary>
	public FormattedString? HeaderFormattedText
	{
		get => (FormattedString?)GetValue(HeaderFormattedTextProperty);
		set => SetValue(HeaderFormattedTextProperty, value);
	}

	/// <summary>
	/// Gets or sets an optional custom header view. When set, it replaces the default <see cref="FuchsTypo"/> header.
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

	public bool IsEnabled
	{
		get => (bool)GetValue(IsEnabledProperty);
		set => SetValue(IsEnabledProperty, value);
	}

	public bool IsSelected
	{
		get => (bool)GetValue(IsSelectedProperty);
		internal set => SetValue(IsSelectedProperty, value);
	}
}