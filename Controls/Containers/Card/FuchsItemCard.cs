using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace FuchsControls;

public sealed class FuchsItemCard : FuchsCard
{
	private const double CompactLayoutBreakpoint = 480;

	public static readonly BindableProperty TitleProperty = BindableProperty.Create(
		nameof(Title), typeof(string), typeof(FuchsItemCard), string.Empty, propertyChanged: OnTitleChanged);

	public static readonly BindableProperty BodyProperty = BindableProperty.Create(
		nameof(Body), typeof(string), typeof(FuchsItemCard), string.Empty, propertyChanged: OnBodyChanged);

	public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
		nameof(ImageSource), typeof(ImageSource), typeof(FuchsItemCard), propertyChanged: OnImageSourceChanged);

	public static readonly BindableProperty PathProperty = BindableProperty.Create(
		nameof(Path), typeof(string), typeof(FuchsItemCard), propertyChanged: OnImageSourceChanged);

	public static readonly BindableProperty ImageAlternativeTextProperty = BindableProperty.Create(
		nameof(ImageAlternativeText), typeof(string), typeof(FuchsItemCard), string.Empty, propertyChanged: OnImageAlternativeTextChanged);

	private readonly ObservableCollection<FuchsBadge> badges = new();
	private readonly Grid layout = new();
	private readonly VerticalStackLayout imageArea = new();
	private readonly FlexLayout badgeLayout = new();
	private readonly FuchsImage image = new();
	private readonly FuchsIcon icon = new() { Icon = string.Empty };
	private readonly VerticalStackLayout contentLayout = new();
	private readonly FuchsTypo title = new() { Typo = FuchsTypoType.H5 };
	private readonly FuchsTypo body = new();
	private readonly HorizontalStackLayout footerLayout = new();
	private bool isCompactLayout;

	public FuchsItemCard()
	{
		this.ApplyFuchsStyle("FuchsCardStyle");
		layout.ApplyFuchsStyle("FuchsItemCardLayoutStyle");
		imageArea.ApplyFuchsStyle("FuchsItemCardImageAreaStyle");
		badgeLayout.ApplyFuchsStyle("FuchsActionCardBadgesStyle");
		image.ApplyFuchsStyle("FuchsImageStyle");
		icon.ApplyFuchsStyle("FuchsCardIconStyle");
		contentLayout.ApplyFuchsStyle("FuchsItemCardContentStyle");
		footerLayout.ApplyFuchsStyle("FuchsActionCardFooterStyle");

		body.Typo = FuchsTypoType.Body;
		badgeLayout.IsVisible = false;
		image.IsVisible = false;
		icon.IsVisible = false;
		imageArea.IsVisible = false;
		title.IsVisible = false;
		body.IsVisible = false;

		imageArea.Children.Add(badgeLayout);
		imageArea.Children.Add(icon);
		imageArea.Children.Add(image);
		contentLayout.Children.Add(title);
		contentLayout.Children.Add(body);
		footerLayout.IsVisible = false;
		contentLayout.Children.Add(footerLayout);
		layout.Children.Add(imageArea);
		layout.Children.Add(contentLayout);
		Content = layout;

		badges.CollectionChanged += OnBadgesChanged;
		footerLayout.ChildAdded += OnFooterContentChanged;
		footerLayout.ChildRemoved += OnFooterContentChanged;
		UpdateTitle();
		UpdateBody();
		UpdateImage();
		UpdateImageAlternativeText();
		ApplyResponsiveLayout();
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

	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);
		if (width <= 0)
		{
			return;
		}

		var compact = width < CompactLayoutBreakpoint;
		if (compact == isCompactLayout)
		{
			return;
		}

		isCompactLayout = compact;
		ApplyResponsiveLayout();
	}

	private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsItemCard)bindable).UpdateTitle();

	private static void OnBodyChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsItemCard)bindable).UpdateBody();

	private static void OnImageSourceChanged(BindableObject bindable, object oldValue, object newValue) => ((FuchsItemCard)bindable).UpdateImage();

	private static void OnImageAlternativeTextChanged(BindableObject bindable, object oldValue, object newValue) =>
		((FuchsItemCard)bindable).UpdateImageAlternativeText();

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
		UpdateImageAreaVisibility();
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
		UpdateImageAreaVisibility();
	}

	private void UpdateImageAreaVisibility()
	{
		imageArea.IsVisible = Path is not null || ImageSource is not null || badges.Count > 0;
		ApplyResponsiveLayout();
	}

	private void OnFooterContentChanged(object? sender, ElementEventArgs e) => footerLayout.IsVisible = footerLayout.Children.Count > 0;

	private void ApplyResponsiveLayout()
	{
		layout.RowDefinitions.Clear();
		layout.ColumnDefinitions.Clear();

		if (!imageArea.IsVisible)
		{
			layout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			layout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
			Grid.SetRow(contentLayout, 0);
			Grid.SetColumn(contentLayout, 0);
			image.WidthRequest = -1;
			image.HeightRequest = 128;
			icon.Size = 128;
			return;
		}

		if (isCompactLayout)
		{
			layout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			layout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			layout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
			Grid.SetRow(imageArea, 0);
			Grid.SetColumn(imageArea, 0);
			Grid.SetRow(contentLayout, 1);
			Grid.SetColumn(contentLayout, 0);
			image.WidthRequest = -1;
			image.HeightRequest = 128;
			icon.Size = 128;
			return;
		}

		layout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
		layout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
		layout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
		Grid.SetRow(imageArea, 0);
		Grid.SetColumn(imageArea, 0);
		Grid.SetRow(contentLayout, 0);
		Grid.SetColumn(contentLayout, 1);
		image.WidthRequest = 148;
		image.HeightRequest = 148;
		icon.Size = 148;
	}
}