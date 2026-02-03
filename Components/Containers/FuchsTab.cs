#region Meta

// FuchsControls
// Created: 03/02/2026
// Modified: 03/02/2026

#endregion

namespace FuchsControls.Containers;

public class FuchsTab : BindableObject
{
	public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(FuchsTab), string.Empty);
	public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(FuchsTab), null);

	public static readonly BindableProperty ContentTemplateProperty =
		BindableProperty.Create(nameof(ContentTemplate), typeof(DataTemplate), typeof(FuchsTab), null);

	public string Title
	{
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}

	public View? Content
	{
		get => (View?)GetValue(ContentProperty);
		set => SetValue(ContentProperty, value);
	}

	public DataTemplate? ContentTemplate
	{
		get => (DataTemplate?)GetValue(ContentTemplateProperty);
		set => SetValue(ContentTemplateProperty, value);
	}

	private View? _cachedContent;

	public View? GetContent()
	{
		if (Content != null) return Content;
		if (_cachedContent != null) return _cachedContent;

		if (ContentTemplate != null)
		{
			var created = ContentTemplate.CreateContent();
			if (created is View v)
				_cachedContent = v;
			else if (created is Element e && e is View v2)
				_cachedContent = v2;
		}

		return _cachedContent;
	}
}