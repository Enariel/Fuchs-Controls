using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;
using ThemeColor = FuchsControls.Theme.FuchsColor;

namespace FuchsControls.Controls;

public class FuchsCard : FuchsComponent
{
	private readonly Border _root;
	private readonly Grid _layout;
	private readonly ContentPresenter _headerPresenter;
	private readonly ContentPresenter _bodyPresenter;
	private readonly ContentPresenter _footerPresenter;

	public static readonly BindableProperty HeaderProperty =
		BindableProperty.Create(nameof(Header), typeof(View), typeof(FuchsCard), propertyChanged: OnContentPropertyChanged);

	public static readonly BindableProperty BodyProperty =
		BindableProperty.Create(nameof(Body), typeof(View), typeof(FuchsCard), propertyChanged: OnContentPropertyChanged);

	public static readonly BindableProperty FooterProperty =
		BindableProperty.Create(nameof(Footer), typeof(View), typeof(FuchsCard), propertyChanged: OnContentPropertyChanged);

	public static readonly BindableProperty IsHorizontalProperty =
		BindableProperty.Create(nameof(IsHorizontal), typeof(bool), typeof(FuchsCard), false, propertyChanged: OnContentPropertyChanged);

	public View? Header
	{
		get => (View?)GetValue(HeaderProperty);
		set => SetValue(HeaderProperty, value);
	}

	public View? Body
	{
		get => (View?)GetValue(BodyProperty);
		set => SetValue(BodyProperty, value);
	}

	public View? Footer
	{
		get => (View?)GetValue(FooterProperty);
		set => SetValue(FooterProperty, value);
	}

	public bool IsHorizontal
	{
		get => (bool)GetValue(IsHorizontalProperty);
		set => SetValue(IsHorizontalProperty, value);
	}

	public FuchsCard()
	{
		_headerPresenter = new ContentPresenter();
		_bodyPresenter = new ContentPresenter();
		_footerPresenter = new ContentPresenter();

		_layout = new Grid
		{
			RowSpacing = 10, ColumnSpacing = 16
		};

		_root = new Border
		{
			Content = _layout
		};

		Content = _root;
		BuildLayout();
		ApplyTheme();
	}

	protected override void ApplyTheme()
	{
		FuchsTheme theme = FuchsThemeProvider.Current;

		Color background = Color == ThemeColor.Default ? theme.Background : theme.GetMainColor(Color);
		Color border = Color == ThemeColor.Default ? theme.BackgroundDarker : theme.GetBorderColor(Color);
		Color text = theme.GetTextColor(Color, Variant);

		_root.BackgroundColor = Variant == FuchsVariant.Text ? Colors.Transparent : background;
		_root.Stroke = Variant == FuchsVariant.Text ? Brush.Transparent : new SolidColorBrush(border);
		_root.StrokeThickness = Variant == FuchsVariant.Text ? 0 : theme.BorderWidth;
		_root.StrokeShape = new RoundRectangle { CornerRadius = theme.CornerRadius };
		_root.Padding = theme.CardPadding;
		_root.Shadow = (Variant == FuchsVariant.Filled
			? new Shadow { Brush = new SolidColorBrush(border), Offset = new Point(0, 2), Radius = 0, Opacity = 1 }
			: null)!;

		SetInheritedTextColor(_layout, text);
	}

	private void BuildLayout()
	{
		_layout.Children.Clear();
		_layout.RowDefinitions.Clear();
		_layout.ColumnDefinitions.Clear();

		_headerPresenter.Content = Header;
		_bodyPresenter.Content = Body;
		_footerPresenter.Content = Footer;

		if (IsHorizontal)
		{
			_layout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
			_layout.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

			VerticalStackLayout right = new()
			{
				Spacing = 10, Children =
				{
					_bodyPresenter, _footerPresenter
				}
			};

			_layout.Add(_headerPresenter, 0, 0);
			_layout.Add(right, 1, 0);
		}
		else
		{
			_layout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			_layout.RowDefinitions.Add(new RowDefinition(GridLength.Star));
			_layout.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

			_layout.Add(_headerPresenter, 0, 0);
			_layout.Add(_bodyPresenter, 0, 1);
			_layout.Add(_footerPresenter, 0, 2);
		}
	}

	private static void SetInheritedTextColor(Element element, Color color)
	{
		if (element is Label label)
			label.TextColor = color;

		if (element is Layout layout)
		{
			foreach (IView child in layout.Children)
			{
				if (child is Element childElement)
					SetInheritedTextColor(childElement, color);
			}
		}

		if (element is ContentView contentView && contentView.Content is Element contentElement)
			SetInheritedTextColor(contentElement, color);
	}

	private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsCard card)
		{
			card.BuildLayout();
			card.ApplyTheme();
		}
	}
}