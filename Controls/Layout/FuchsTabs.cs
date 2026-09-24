#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using ThemeColor = FuchsControls.Theme.FuchsColor;

namespace FuchsControls.Controls;

public sealed class FuchsTabs : FuchsComponent
{
	private sealed record HeaderVisual(Grid Root, Label Label, BoxView Indicator);

	private readonly Border _root;
	private readonly Border _headerBorder;
	private readonly Border _contentBorder;
	private readonly Grid _layout;
	private readonly FlexLayout _headerLayout;
	private readonly ScrollView _headerScroller;
	private readonly Grid _contentHost;
	private readonly List<HeaderVisual> _headerVisuals = [];
	private readonly List<FuchsTab> _renderedTabs = [];
	private INotifyCollectionChanged? _observedCollection;
	private CancellationTokenSource? _transitionCancellation;
	private bool _isSynchronizing;

	public static readonly BindableProperty TabsProperty =
		BindableProperty.Create(
			nameof(Tabs),
			typeof(IList<FuchsTab>),
			typeof(FuchsTabs),
			defaultValueCreator: _ => new ObservableCollection<FuchsTab>(),
			propertyChanged: OnTabsChanged);

	public static readonly BindableProperty SelectedIndexProperty =
		BindableProperty.Create(
			nameof(SelectedIndex),
			typeof(int),
			typeof(FuchsTabs),
			-1,
			coerceValue: CoerceSelectedIndex,
			propertyChanged: OnSelectedIndexChanged);

	public static readonly BindableProperty IsBorderedProperty =
		BindableProperty.Create(nameof(IsBordered), typeof(bool), typeof(FuchsTabs), true, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty IsScrollableProperty =
		BindableProperty.Create(nameof(IsScrollable), typeof(bool), typeof(FuchsTabs), false, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty LineAtTopProperty =
		BindableProperty.Create(nameof(LineAtTop), typeof(bool), typeof(FuchsTabs), false, propertyChanged: OnVisualPropertyChanged);

	public IList<FuchsTab> Tabs
	{
		get => (IList<FuchsTab>?)GetValue(TabsProperty) ?? Array.Empty<FuchsTab>();
		set => SetValue(TabsProperty, value);
	}

	public int SelectedIndex
	{
		get => (int)GetValue(SelectedIndexProperty);
		set => SetValue(SelectedIndexProperty, value);
	}

	public FuchsTab? SelectedTab => SelectedIndex >= 0 && SelectedIndex < _renderedTabs.Count
		? _renderedTabs[SelectedIndex]
		: null;

	public bool IsBordered
	{
		get => (bool)GetValue(IsBorderedProperty);
		set => SetValue(IsBorderedProperty, value);
	}

	public bool IsScrollable
	{
		get => (bool)GetValue(IsScrollableProperty);
		set => SetValue(IsScrollableProperty, value);
	}

	public bool LineAtTop
	{
		get => (bool)GetValue(LineAtTopProperty);
		set => SetValue(LineAtTopProperty, value);
	}

	public event EventHandler<FuchsTabChangedEventArgs>? SelectedTabChanged;

	public FuchsTabs()
	{
		_headerLayout = new FlexLayout
		{
			AlignItems = FlexAlignItems.Center, Direction = FlexDirection.Row, Wrap = FlexWrap.Wrap, Padding = new Thickness(8, 0)
		};
		_headerScroller = new ScrollView
		{
			Orientation = ScrollOrientation.Horizontal, Content = _headerLayout
		};

		_contentHost = new Grid();
		_contentBorder = new Border { Content = _contentHost };
		_headerBorder = new Border { Content = _headerScroller };

		_layout = new Grid
		{
			RowSpacing = 0, RowDefinitions =
			{
				new RowDefinition(GridLength.Auto), new RowDefinition(GridLength.Star)
			}
			, Children =
			{
				_headerBorder, _contentBorder
			}
		};
		Grid.SetRow(_contentBorder, 1);

		_root = new Border { Content = _layout };
		Content = _root;

		ObserveTabs(Tabs);
		ApplyTheme();
	}

	protected override void OnHandlerChanging(HandlerChangingEventArgs args)
	{
		if (args.NewHandler is null)
		{
			CancelTransition();
			StopObservingTabs();
		}
		else if (args.OldHandler is null)
			ObserveTabs(Tabs);

		base.OnHandlerChanging(args);
	}

	protected override void ApplyTheme()
	{
		FuchsTheme theme = FuchsThemeProvider.Current;
		Color background = Color == ThemeColor.Default ? theme.Background : theme.GetMainColor(Color);
		Color border = Color == ThemeColor.Default ? theme.BackgroundDarker : theme.GetBorderColor(Color);
		Color text = theme.GetTextColor(Color, Variant);

		_root.BackgroundColor = background;
		_root.Stroke = IsBordered ? new SolidColorBrush(border) : Brush.Transparent;
		_root.StrokeThickness = IsBordered ? theme.BorderWidth : 0;
		_root.StrokeShape = new RoundRectangle { CornerRadius = theme.CornerRadius };
		_root.Shadow = IsBordered
			? new Shadow { Brush = new SolidColorBrush(border), Offset = new Point(0, 2), Radius = 0, Opacity = 1 }
			: null!;

		_headerBorder.BackgroundColor = background;
		_headerBorder.Stroke = new SolidColorBrush(border);
		_headerBorder.StrokeThickness = theme.BorderWidth;
		_headerBorder.Padding = new Thickness(0, LineAtTop ? theme.BorderWidth : 0, 0, LineAtTop ? 0 : theme.BorderWidth);
		_contentBorder.BackgroundColor = background;
		_contentBorder.Stroke = Brush.Transparent;
		_contentBorder.Padding = theme.PanelPadding;
		_headerLayout.Padding = new Thickness(8, LineAtTop ? theme.BorderWidth : 0, 8, LineAtTop ? 0 : theme.BorderWidth);
		_headerLayout.Wrap = IsScrollable ? FlexWrap.NoWrap : FlexWrap.Wrap;
		_headerScroller.HorizontalScrollBarVisibility = IsScrollable ? ScrollBarVisibility.Default : ScrollBarVisibility.Never;
		_headerScroller.VerticalScrollBarVisibility = ScrollBarVisibility.Never;

		for (int index = 0; index < _headerVisuals.Count; index++)
			ApplyHeaderTheme(index, text, border, theme);

		foreach (FuchsTab tab in _renderedTabs)
			tab.RefreshTheme();
	}

	private void ObserveTabs(IList<FuchsTab> tabs)
	{
		StopObservingTabs();
		_observedCollection = tabs as INotifyCollectionChanged;
		if (_observedCollection is not null)
			_observedCollection.CollectionChanged += OnTabsCollectionChanged;

		RebuildVisualTree();
	}

	private void StopObservingTabs()
	{
		if (_observedCollection is not null)
			_observedCollection.CollectionChanged -= OnTabsCollectionChanged;

		_observedCollection = null;
		foreach (FuchsTab tab in _renderedTabs)
			tab.PropertyChanged -= OnTabPropertyChanged;
	}

	private void RebuildVisualTree()
	{
		CancelTransition();
		foreach (FuchsTab tab in _renderedTabs)
			tab.PropertyChanged -= OnTabPropertyChanged;

		_renderedTabs.Clear();
		_headerVisuals.Clear();
		_headerLayout.Children.Clear();
		_contentHost.Children.Clear();

		HashSet<FuchsTab> seenTabs = [];
		foreach (FuchsTab tab in Tabs)
		{
			if (!seenTabs.Add(tab))
				continue;

			_renderedTabs.Add(tab);
			tab.PropertyChanged += OnTabPropertyChanged;
			_contentHost.Children.Add(tab);
			_headerVisuals.Add(CreateHeaderVisual(tab, _renderedTabs.Count - 1));
		}

		foreach (HeaderVisual visual in _headerVisuals)
			_headerLayout.Children.Add(visual.Root);

		SelectValidTab(false);
		ApplyTheme();
	}

	private HeaderVisual CreateHeaderVisual(FuchsTab tab, int index)
	{
		Label label = new()
		{
			Text = tab.Header, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center
			, FontAttributes = FontAttributes.Bold, FontSize = ResolveFontSize()
		};

		BoxView indicator = new()
		{
			HeightRequest = 0, HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LineAtTop ? LayoutOptions.Start : LayoutOptions.End, CornerRadius = 2
		};

		Grid header = new()
		{
			Padding = ResolvePadding(), RowSpacing = 0, RowDefinitions =
			{
				new RowDefinition(GridLength.Star), new RowDefinition(GridLength.Auto)
			}
			, Children = { label, indicator }
		};
		Grid.SetRow(indicator, 1);

		TapGestureRecognizer tap = new();
		tap.Tapped += (_, _) => SelectTab(index);
		header.GestureRecognizers.Add(tap);
		AutomationProperties.SetName(header, tab.Header);
		AutomationProperties.SetHelpText(header, "Select tab");

		return new HeaderVisual(header, label, indicator);
	}

	private void SelectTab(int index)
	{
		if (IsDisabled || index < 0 || index >= _renderedTabs.Count || _renderedTabs[index].IsDisabled)
			return;

		SelectIndex(index, true);
	}

	private void SelectValidTab(bool notify)
	{
		int current = SelectedIndex;
		int valid = FindSelectableIndex(current);
		if (valid < 0)
		{
			if (current != -1)
				SetSelectedIndex(-1, notify);
			else
				UpdateSelectionVisuals();
			return;
		}

		if (current != valid)
			SetSelectedIndex(valid, notify);
		else
			UpdateSelectionVisuals();
	}

	private void SelectIndex(int index, bool notify)
	{
		int valid = FindSelectableIndex(index);
		if (valid < 0)
			return;

		SetSelectedIndex(valid, notify);
	}

	private void SetSelectedIndex(int index, bool notify)
	{
		if (_isSynchronizing)
			return;

		FuchsTab? oldTab = SelectedTab;
		int oldIndex = SelectedIndex;
		_isSynchronizing = true;
		try
		{
			SetValue(SelectedIndexProperty, index);
			UpdateSelectionVisuals();
		}
		finally
		{
			_isSynchronizing = false;
		}

		if (notify && oldIndex != SelectedIndex)
		{
			OnPropertyChanged(nameof(SelectedTab));
			SelectedTabChanged?.Invoke(this, new FuchsTabChangedEventArgs(oldIndex, oldTab, SelectedIndex, SelectedTab));
			_ = AnimateSelectionAsync(oldTab, SelectedTab, oldIndex, SelectedIndex);
		}
	}

	private async Task AnimateSelectionAsync(FuchsTab? oldTab, FuchsTab? newTab, int oldIndex, int newIndex)
	{
		if (newTab is null || oldIndex < 0 || newIndex < 0 || oldIndex == newIndex)
			return;

		CancelTransition();
		CancellationTokenSource transition = new CancellationTokenSource();
		_transitionCancellation = transition;
		CancellationToken token = transition.Token;
		newTab.Opacity = 0;
		newTab.TranslationX = newIndex > oldIndex ? 24 : -24;

		try
		{
			await Task.WhenAll(
				newTab.FadeToAsync(1, 180, Easing.CubicOut),
				newTab.TranslateToAsync(0, 0, 180, Easing.CubicOut)).WaitAsync(token);
		}
		catch (OperationCanceledException) { }
		finally
		{
			if (ReferenceEquals(_transitionCancellation, transition))
			{
				transition.Dispose();
				_transitionCancellation = null;
			}
		}
	}

	private void CancelTransition()
	{
		_transitionCancellation?.Cancel();
		_transitionCancellation?.Dispose();
		_transitionCancellation = null;
	}

	private void UpdateSelectionVisuals()
	{
		for (int index = 0; index < _renderedTabs.Count; index++)
		{
			bool isSelected = index == SelectedIndex;
			_renderedTabs[index].SetSelectedState(isSelected);
			if (index < _headerVisuals.Count)
				_headerVisuals[index].Root.IsEnabled = !IsDisabled && !_renderedTabs[index].IsDisabled;
		}

		ApplyTheme();
	}

	private int FindSelectableIndex(int requestedIndex)
	{
		if (_renderedTabs.Count == 0)
			return -1;

		if (requestedIndex >= 0 && requestedIndex < _renderedTabs.Count && !_renderedTabs[requestedIndex].IsDisabled)
			return requestedIndex;

		for (int index = 0; index < _renderedTabs.Count; index++)
			if (!_renderedTabs[index].IsDisabled)
				return index;

		return -1;
	}

	private void ApplyHeaderTheme(int index, Color text, Color border, FuchsTheme theme)
	{
		if (index >= _headerVisuals.Count || index >= _renderedTabs.Count)
			return;

		HeaderVisual visual = _headerVisuals[index];
		FuchsTab tab = _renderedTabs[index];
		bool active = index == SelectedIndex;
		Color activeText = active ? theme.Primary : text;
		visual.Label.Text = tab.Header;
		visual.Label.TextColor = activeText;
		visual.Label.FontSize = ResolveFontSize();
		visual.Label.Opacity = tab.IsDisabled ? 0.4 : active ? 1 : 0.6;
		visual.Root.Padding = ResolvePadding();
		visual.Root.Opacity = IsDisabled || tab.IsDisabled ? 0.65 : 1;
		visual.Indicator.Color = active ? theme.Primary : border;
		visual.Indicator.HeightRequest = active ? theme.BorderWidth * 1.5 : 0;
		visual.Indicator.Opacity = active ? 1 : 0.6;
		visual.Indicator.ScaleX = active ? 1 : 0.7;
		visual.Indicator.VerticalOptions = LineAtTop ? LayoutOptions.Start : LayoutOptions.End;
		Grid.SetRow(visual.Label, LineAtTop ? 1 : 0);
		Grid.SetRow(visual.Indicator, LineAtTop ? 0 : 1);
		AutomationProperties.SetIsInAccessibleTree(visual.Root, true);
		AutomationProperties.SetName(visual.Root, tab.Header);
		AutomationProperties.SetHelpText(visual.Root, active ? "Selected tab" : "Select tab");
	}

	private static object CoerceSelectedIndex(BindableObject bindable, object value)
	{
		if (bindable is not FuchsTabs tabs || tabs._isSynchronizing)
			return value;

		return tabs.FindSelectableIndex((int)value);
	}

	private static void OnTabsChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsTabs tabs)
			tabs.ObserveTabs(newValue as IList<FuchsTab> ?? new ObservableCollection<FuchsTab>());
	}

	private static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsTabs tabs && !tabs._isSynchronizing)
			tabs.NotifySelectionChanged((int)oldValue, (int)newValue);
	}

	private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsTabs tabs)
		{
			tabs.ApplyTheme();
		}
	}

	private void NotifySelectionChanged(int oldIndex, int newIndex)
	{
		FuchsTab? oldTab = GetTab(oldIndex);
		UpdateSelectionVisuals();
		OnPropertyChanged(nameof(SelectedTab));
		SelectedTabChanged?.Invoke(this, new FuchsTabChangedEventArgs(oldIndex, oldTab, newIndex, SelectedTab));
	}

	private FuchsTab? GetTab(int index) => index >= 0 && index < _renderedTabs.Count ? _renderedTabs[index] : null;

	private void OnTabsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args) => RebuildVisualTree();

	private void OnTabPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs args)
	{
		if (sender is FuchsTab tab && args.PropertyName == nameof(FuchsTab.IsDisabled) && tab.IsDisabled && tab.IsSelected)
			SelectValidTab(true);
		else
			ApplyTheme();
	}
}