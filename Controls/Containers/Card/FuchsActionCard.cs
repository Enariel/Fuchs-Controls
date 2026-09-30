using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace FuchsControls;

public sealed class FuchsActionCard : FuchsCard
{
	public static readonly BindableProperty TitleProperty = BindableProperty.Create(
		nameof(Title), typeof(string), typeof(FuchsActionCard), string.Empty, propertyChanged: OnTitleChanged);

	public static readonly BindableProperty BodyProperty = BindableProperty.Create(
		nameof(Body), typeof(string), typeof(FuchsActionCard), string.Empty, propertyChanged: OnBodyChanged);

	public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
		nameof(ImageSource), typeof(ImageSource), typeof(FuchsActionCard), propertyChanged: OnImageSourceChanged);

	public static readonly BindableProperty PathProperty = BindableProperty.Create(
		nameof(Path), typeof(string), typeof(FuchsActionCard), propertyChanged: OnImageSourceChanged);

	public static readonly BindableProperty ImageAlternativeTextProperty = BindableProperty.Create(
		nameof(ImageAlternativeText), typeof(string), typeof(FuchsActionCard), string.Empty, propertyChanged: OnImageAlternativeTextChanged);

	private readonly ObservableCollection<FuchsBadge> badges = new();
	private readonly VerticalStackLayout layout = new();
	private readonly FlexLayout badgeLayout = new();
	private readonly FuchsImage image = new();
	private readonly FuchsIcon icon = new() { Icon = string.Empty, Size = 180 };
	private readonly FuchsTypo title = new() { Typo = FuchsTypoType.H5 };
	private readonly FuchsTypo body = new();
	private readonly HorizontalStackLayout footerLayout = new();

	public FuchsActionCard()
	{
		this.ApplyFuchsStyle("FuchsCardStyle");
		layout.ApplyFuchsStyle("FuchsActionCardLayoutStyle");
		badgeLayout.ApplyFuchsStyle("FuchsActionCardBadgesStyle");
		image.ApplyFuchsStyle("FuchsActionCardImageStyle");
		icon.ApplyFuchsStyle("FuchsCardIconStyle");
		footerLayout.ApplyFuchsStyle("FuchsActionCardFooterStyle");

		body.Typo = FuchsTypoType.Body;
		badgeLayout.IsVisible = false;
		image.IsVisible = false;
		icon.IsVisible = false;
		title.IsVisible = false;
		body.IsVisible = false;

		footerLayout.IsVisible = false;
		layout.Children.Add(badgeLayout);
		layout.Children.Add(image);
		layout.Children.Add(icon);
		layout.Children.Add(title);
		layout.Children.Add(body);
		layout.Children.Add(footerLayout);
		Content = layout;

		badges.CollectionChanged += OnBadgesChanged;
		footerLayout.ChildAdded += OnFooterContentChanged;
		footerLayout.ChildRemoved += OnFooterContentChanged;
		UpdateTitle();
		UpdateBody();
		UpdateImage();
		UpdateImageAlternativeText();
	}

	public string? Title
	{
		get => (string?)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}

	public string? Body
	{
		get => (string?)GetValue(BodyProperty);
		set => SetValue(BodyProperty, value);
	}

	public ImageSource? ImageSource
	{
		get => (ImageSource?)GetValue(ImageSourceProperty);
		set => SetValue(ImageSourceProperty, value);
	}

	public string? Path
	{
		get => (string?)GetValue(PathProperty);
		set => SetValue(PathProperty, value);
	}

	public string? ImageAlternativeText
	{
		get => (string?)GetValue(ImageAlternativeTextProperty);
		set => SetValue(ImageAlternativeTextProperty, value);
	}

	public ObservableCollection<FuchsBadge> Badges => badges;

	public HorizontalStackLayout FooterContent => footerLayout;

	private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsActionCard)bindable).UpdateTitle();

	private static void OnBodyChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsActionCard)bindable).UpdateBody();

	private static void OnImageSourceChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsActionCard)bindable).UpdateImage();

	private static void OnImageAlternativeTextChanged(BindableObject bindable, object oldValue, object newValue) =>
		((FuchsActionCard)bindable).UpdateImageAlternativeText();

	private void UpdateTitle()
	{
		title.Text = Title ?? string.Empty;
		title.IsVisible = !string.IsNullOrWhiteSpace(Title);
	}

	private void UpdateBody()
	{
		body.Text = Body ?? string.Empty;
		body.IsVisible = !string.IsNullOrWhiteSpace(Body);
	}

	private void UpdateImage()
	{
		image.Source = ImageSource;
		var hasPath = Path is not null;
		icon.PathData = Path ?? string.Empty;
		icon.IsVisible = hasPath;
		image.IsVisible = !hasPath && ImageSource is not null;
		UpdateImageAlternativeText();
	}

	private void UpdateImageAlternativeText()
	{
		image.AlternativeText = ImageAlternativeText ?? string.Empty;
		SemanticProperties.SetDescription(icon, ImageAlternativeText ?? string.Empty);
	}

	private void OnBadgesChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		badgeLayout.Children.Clear();
		foreach (var badge in badges)
		{
			badgeLayout.Children.Add(badge);
		}

		badgeLayout.IsVisible = badges.Count > 0;
	}

	private void OnFooterContentChanged(object? sender, ElementEventArgs e) => footerLayout.IsVisible = footerLayout.Children.Count > 0;
}