#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;
using ThemeColor = FuchsControls.Theme.FuchsColor;

namespace FuchsControls.Controls;

public sealed class FuchsTable : FuchsComponent
{
	private readonly Border _tableBorder;
	private readonly Grid _tableGrid;
	private readonly ScrollView _horizontalScroll;
	private readonly CollectionView _rows;
	private Grid? _header;
	private INotifyCollectionChanged? _observedColumns;

	public static readonly BindableProperty ItemsSourceProperty =
		BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(FuchsTable), null, propertyChanged: OnItemsSourceChanged);

	public static readonly BindableProperty ColumnsProperty =
		BindableProperty.Create(nameof(Columns), typeof(IList<FuchsTableColumn>), typeof(FuchsTable),
			defaultValueCreator: _ => new ObservableCollection<FuchsTableColumn>(), propertyChanged: OnColumnsChanged);

	public static readonly BindableProperty IsBorderedProperty =
		BindableProperty.Create(nameof(IsBordered), typeof(bool), typeof(FuchsTable), true, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty IsStripedProperty =
		BindableProperty.Create(nameof(IsStriped), typeof(bool), typeof(FuchsTable), false, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty IsHorizontalProperty =
		BindableProperty.Create(nameof(IsHorizontal), typeof(bool), typeof(FuchsTable), true, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty IsDenseProperty =
		BindableProperty.Create(nameof(IsDense), typeof(bool), typeof(FuchsTable), false, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty ShowHeaderProperty =
		BindableProperty.Create(nameof(ShowHeader), typeof(bool), typeof(FuchsTable), true, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty TableBackgroundColorProperty =
		BindableProperty.Create(nameof(TableBackgroundColor), typeof(Color), typeof(FuchsTable), null, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty HeaderBackgroundColorProperty =
		BindableProperty.Create(nameof(HeaderBackgroundColor), typeof(Color), typeof(FuchsTable), null, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty RowBackgroundColorProperty =
		BindableProperty.Create(nameof(RowBackgroundColor), typeof(Color), typeof(FuchsTable), null, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty AlternateRowBackgroundColorProperty =
		BindableProperty.Create(nameof(AlternateRowBackgroundColor), typeof(Color), typeof(FuchsTable), null, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty BorderColorProperty =
		BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(FuchsTable), null, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty TextColorProperty =
		BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(FuchsTable), null, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty HeaderTextColorProperty =
		BindableProperty.Create(nameof(HeaderTextColor), typeof(Color), typeof(FuchsTable), null, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty CellPaddingProperty =
		BindableProperty.Create(nameof(CellPadding), typeof(Thickness), typeof(FuchsTable), new Thickness(16, 12), propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty RowHeightProperty =
		BindableProperty.Create(nameof(RowHeight), typeof(double), typeof(FuchsTable), -1d, propertyChanged: OnVisualPropertyChanged);

	public IEnumerable? ItemsSource
	{
		get => (IEnumerable?)GetValue(ItemsSourceProperty);
		set => SetValue(ItemsSourceProperty, value);
	}

	public IList<FuchsTableColumn> Columns
	{
		get => (IList<FuchsTableColumn>?)GetValue(ColumnsProperty) ?? new ObservableCollection<FuchsTableColumn>();
		set => SetValue(ColumnsProperty, value);
	}

	public bool IsBordered
	{
		get => (bool)GetValue(IsBorderedProperty);
		set => SetValue(IsBorderedProperty, value);
	}

	public bool IsStriped
	{
		get => (bool)GetValue(IsStripedProperty);
		set => SetValue(IsStripedProperty, value);
	}

	public bool IsHorizontal
	{
		get => (bool)GetValue(IsHorizontalProperty);
		set => SetValue(IsHorizontalProperty, value);
	}

	public bool IsDense
	{
		get => (bool)GetValue(IsDenseProperty);
		set => SetValue(IsDenseProperty, value);
	}

	public bool ShowHeader
	{
		get => (bool)GetValue(ShowHeaderProperty);
		set => SetValue(ShowHeaderProperty, value);
	}

	public Color? TableBackgroundColor
	{
		get => (Color?)GetValue(TableBackgroundColorProperty);
		set => SetValue(TableBackgroundColorProperty, value);
	}

	public Color? HeaderBackgroundColor
	{
		get => (Color?)GetValue(HeaderBackgroundColorProperty);
		set => SetValue(HeaderBackgroundColorProperty, value);
	}

	public Color? RowBackgroundColor
	{
		get => (Color?)GetValue(RowBackgroundColorProperty);
		set => SetValue(RowBackgroundColorProperty, value);
	}

	public Color? AlternateRowBackgroundColor
	{
		get => (Color?)GetValue(AlternateRowBackgroundColorProperty);
		set => SetValue(AlternateRowBackgroundColorProperty, value);
	}

	public Color? BorderColor
	{
		get => (Color?)GetValue(BorderColorProperty);
		set => SetValue(BorderColorProperty, value);
	}

	public Color? TextColor
	{
		get => (Color?)GetValue(TextColorProperty);
		set => SetValue(TextColorProperty, value);
	}

	public Color? HeaderTextColor
	{
		get => (Color?)GetValue(HeaderTextColorProperty);
		set => SetValue(HeaderTextColorProperty, value);
	}

	public Thickness CellPadding
	{
		get => (Thickness)GetValue(CellPaddingProperty);
		set => SetValue(CellPaddingProperty, value);
	}

	public double RowHeight
	{
		get => (double)GetValue(RowHeightProperty);
		set => SetValue(RowHeightProperty, value);
	}

	public FuchsTable()
	{
		_tableGrid = new Grid();
		_rows = new CollectionView
		{
			SelectionMode = SelectionMode.None,
			ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical) { ItemSpacing = 0 },
			ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem,
			BackgroundColor = Colors.Transparent
		};
		_horizontalScroll = new ScrollView { Orientation = ScrollOrientation.Horizontal, Content = _tableGrid };
		_tableBorder = new Border { Content = _horizontalScroll };
		Content = _tableBorder;

		ObserveColumns(Columns);
		_rows.ItemsSource = ItemsSource;
		ApplyTheme();
	}

	protected override void ApplyTheme()
	{
		FuchsTheme theme = FuchsThemeProvider.Current;
		Color borderColor = BorderColor ?? (Color == ThemeColor.Default ? theme.BackgroundDarker : theme.GetBorderColor(Color));
		Color tableBackground = TableBackgroundColor ?? theme.Background;
		Color rowBackground = RowBackgroundColor ?? tableBackground;
		Color alternateRowBackground = AlternateRowBackgroundColor ?? theme.BackgroundDark;
		Color headerBackground = HeaderBackgroundColor ?? (Variant == FuchsVariant.Filled && Color != ThemeColor.Default
			? theme.GetMainColor(Color)
			: theme.BackgroundDark);
		Color textColor = TextColor ?? theme.Text;
		Color headerTextColor = HeaderTextColor ?? (Variant == FuchsVariant.Filled && Color != ThemeColor.Default
			? theme.GetTextColor(Color, Variant)
			: theme.Text);

		_tableBorder.BackgroundColor = tableBackground;
		_tableBorder.Stroke = new SolidColorBrush(borderColor);
		_tableBorder.StrokeThickness = IsBordered ? theme.BorderWidth : 0;
		_tableBorder.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(theme.CornerRadius) };
		_tableGrid.BackgroundColor = tableBackground;
		_horizontalScroll.HorizontalScrollBarVisibility = IsHorizontal ? ScrollBarVisibility.Default : ScrollBarVisibility.Never;
		_rows.ItemSizingStrategy = RowHeight > 0 ? ItemSizingStrategy.MeasureAllItems : ItemSizingStrategy.MeasureFirstItem;
		_rows.BackgroundColor = rowBackground;

		RebuildTable(theme, borderColor, rowBackground, alternateRowBackground, headerBackground, textColor, headerTextColor);
	}

	private void RebuildTable(
		FuchsTheme theme,
		Color borderColor,
		Color rowBackground,
		Color alternateRowBackground,
		Color headerBackground,
		Color textColor,
		Color headerTextColor)
	{
		_tableGrid.RowDefinitions.Clear();
		_tableGrid.Children.Clear();

		int row = 0;
		if (ShowHeader && Columns.Count > 0)
		{
			_header = CreateHeader(theme, borderColor, headerBackground, headerTextColor);
			_tableGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
			_tableGrid.Add(_header, 0, row++);
		}
		else
		{
			_header = null;
		}

		_tableGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
		Grid.SetRow(_rows, row);
		_tableGrid.Add(_rows);
		_rows.ItemTemplate = CreateRowTemplate(theme, borderColor, rowBackground, alternateRowBackground, textColor);
		_rows.ItemsSource = ItemsSource;
	}

	private Grid CreateHeader(FuchsTheme theme, Color borderColor, Color backgroundColor, Color textColor)
	{
		Grid header = CreateColumnGrid();
		for (int index = 0; index < Columns.Count; index++)
		{
			FuchsTableColumn column = Columns[index];
			View content = column.HeaderTemplate is null
				? CreateDefaultHeader(column, theme, textColor)
				: CreateTemplateView(column.HeaderTemplate);
			content.BindingContext = column;
			Border cell = CreateCell(content, backgroundColor, borderColor, IsBordered, theme.BorderWidth);
			header.Add(cell, index, 0);
		}

		return header;
	}

	private DataTemplate CreateRowTemplate(FuchsTheme theme, Color borderColor, Color rowBackground, Color alternateRowBackground, Color textColor) => new(() =>
	{
		Grid row = CreateColumnGrid();
		row.BindingContextChanged += (_, _) => UpdateRowBackground(row, rowBackground, alternateRowBackground);

		for (int index = 0; index < Columns.Count; index++)
		{
			FuchsTableColumn column = Columns[index];
			View content = column.CellTemplate is null
				? CreateDefaultCell(column, theme, textColor)
				: CreateTemplateView(column.CellTemplate);
			Border cell = CreateCell(content, rowBackground, borderColor, IsBordered, theme.BorderWidth);
			row.Add(cell, index, 0);
		}

		if (RowHeight > 0)
			row.HeightRequest = RowHeight;

		return row;
	});

	private Grid CreateColumnGrid()
	{
		Grid grid = new() { ColumnSpacing = 0, RowSpacing = 0 };
		foreach (FuchsTableColumn column in Columns)
			grid.ColumnDefinitions.Add(new ColumnDefinition(column.Width));
		return grid;
	}

	private Label CreateDefaultHeader(FuchsTableColumn column, FuchsTheme theme, Color textColor)
	{
		return new Label
		{
			Text = column.Header,
			TextColor = textColor,
			FontSize = ResolveFontSize(),
			FontAttributes = FontAttributes.Bold,
			HorizontalTextAlignment = column.HorizontalTextAlignment,
			VerticalTextAlignment = column.VerticalTextAlignment,
			LineBreakMode = LineBreakMode.WordWrap
		};
	}

	private Label CreateDefaultCell(FuchsTableColumn column, FuchsTheme theme, Color textColor)
	{
		Label label = new()
		{
			TextColor = textColor,
			FontSize = ResolveFontSize(),
			HorizontalTextAlignment = column.HorizontalTextAlignment,
			VerticalTextAlignment = column.VerticalTextAlignment,
			LineBreakMode = LineBreakMode.TailTruncation
		};
		label.SetBinding(Label.TextProperty, new Binding
		{
			Path = string.IsNullOrWhiteSpace(column.BindingPath) ? "." : column.BindingPath,
			StringFormat = column.StringFormat,
			Mode = BindingMode.OneWay
		});
		return label;
	}

	private Border CreateCell(View content, Color backgroundColor, Color borderColor, bool bordered, double borderWidth)
	{
		Thickness padding = IsDense ? new Thickness(12, 8) : CellPadding;
		return new Border
		{
			Content = content,
			BackgroundColor = backgroundColor,
			Padding = padding,
			Stroke = new SolidColorBrush(borderColor),
			StrokeThickness = bordered ? borderWidth : 0
		};
	}

	private void UpdateRowBackground(Grid row, Color defaultBackground, Color alternateBackground)
	{
		Color background = defaultBackground;
		if (IsStriped)
		{
			int index = GetItemIndex(row.BindingContext);
			if (index >= 0 && index % 2 == 0)
				background = alternateBackground;
		}

		foreach (var child in row.Children)
		{
			if (child is Border cell)
				cell.BackgroundColor = background;
		}
	}

	private int GetItemIndex(object? item)
	{
		if (item is null || ItemsSource is null)
			return -1;

		if (ItemsSource is IList list)
			return list.IndexOf(item);

		int index = 0;
		foreach (object? candidate in ItemsSource)
		{
			if (Equals(candidate, item))
				return index;
			index++;
		}

		return -1;
	}

	private static View CreateTemplateView(DataTemplate template)
	{
		object content = template.CreateContent();
		return content switch
		{
			View view => view,
			_ => new Label()
		};
	}

	private void ObserveColumns(IList<FuchsTableColumn> columns)
	{
		if (_observedColumns is not null)
			_observedColumns.CollectionChanged -= OnColumnsCollectionChanged;

		foreach (FuchsTableColumn column in columns)
			column.PropertyChanged -= OnColumnPropertyChanged;

		_observedColumns = columns as INotifyCollectionChanged;
		if (_observedColumns is not null)
			_observedColumns.CollectionChanged += OnColumnsCollectionChanged;

		foreach (FuchsTableColumn column in columns)
			column.PropertyChanged += OnColumnPropertyChanged;
	}

	private void OnColumnsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		ObserveColumns(Columns);
		ApplyTheme();
	}

	private void OnColumnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) => ApplyTheme();

	private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsTable table)
			table._rows.ItemsSource = newValue as IEnumerable;
	}

	private static void OnColumnsChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsTable table)
		{
			table.ObserveColumns(table.Columns);
			table.ApplyTheme();
		}
	}

	private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsTable table)
			table.ApplyTheme();
	}
}