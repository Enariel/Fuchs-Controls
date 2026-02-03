#region Meta

// FuchsControls
// Created: 03/02/2026
// Modified: 03/02/2026

#endregion

using System.Collections.ObjectModel;

namespace FuchsControls.Containers;

[ContentProperty(nameof(Tabs))]
public class FuchsTabs : ContentView
{
	public static readonly BindableProperty TabsProperty = BindableProperty.Create(
		nameof(Tabs),
		typeof(ObservableCollection<FuchsTab>),
		typeof(FuchsTabs),
		defaultValueCreator: _ => new ObservableCollection<FuchsTab>());

	public static readonly BindableProperty SelectedTabProperty = BindableProperty.Create(
		nameof(SelectedTab),
		typeof(FuchsTab),
		typeof(FuchsTabs),
		null,
		BindingMode.TwoWay,
		propertyChanged: OnSelectedTabChanged);

	public static readonly BindableProperty TabHeaderBackgroundColorProperty = BindableProperty.Create(
		nameof(TabHeaderBackgroundColor),
		typeof(Color),
		typeof(FuchsTabs),
		Colors.Transparent);

	public static readonly BindableProperty TabIndicatorColorProperty = BindableProperty.Create(
		nameof(TabIndicatorColor),
		typeof(Color),
		typeof(FuchsTabs),
		null);

	public ObservableCollection<FuchsTab> Tabs
	{
		get => (ObservableCollection<FuchsTab>)GetValue(TabsProperty);
		set => SetValue(TabsProperty, value);
	}

	public FuchsTab? SelectedTab
	{
		get => (FuchsTab?)GetValue(SelectedTabProperty);
		set => SetValue(SelectedTabProperty, value);
	}

	public Color TabHeaderBackgroundColor
	{
		get => (Color)GetValue(TabHeaderBackgroundColorProperty);
		set => SetValue(TabHeaderBackgroundColorProperty, value);
	}

	public Color? TabIndicatorColor
	{
		get => (Color?)GetValue(TabIndicatorColorProperty);
		set => SetValue(TabIndicatorColorProperty, value);
	}

	private readonly StackLayout _headerContainer;
	private readonly Grid _contentContainer;
	private readonly BoxView _indicator;
	private readonly Grid _headerGrid;

	public FuchsTabs()
	{
		Tabs.CollectionChanged += (_, _) => RebuildHeaders();

		this.SetDynamicResource(TabIndicatorColorProperty, "FuchsAccentColor");

		_headerContainer = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 10 };

		_indicator = new BoxView
		{
			HeightRequest = 3,
			HorizontalOptions = LayoutOptions.Start,
			VerticalOptions = LayoutOptions.End,
			Color = TabIndicatorColor
		};
		_indicator.SetBinding(BoxView.ColorProperty, new Binding(nameof(TabIndicatorColor), source: this));

		_headerGrid = new Grid();
		_headerGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
		_headerGrid.Children.Add(_headerContainer);
		_headerGrid.Children.Add(_indicator);

		_contentContainer = new Grid();

		var mainStack = new VerticalStackLayout();
		mainStack.Children.Add(_headerGrid);
		mainStack.Children.Add(_contentContainer);

		Content = mainStack;
	}

	private void RebuildHeaders()
	{
		_headerContainer.Children.Clear();
		foreach (var tab in Tabs)
		{
			var btn = new Button
			{
				Text = tab.Title,
				BackgroundColor = Colors.Transparent,
				BorderWidth = 0,
				Padding = new Thickness(10, 5)
			};
			btn.SetDynamicResource(Button.TextColorProperty, "FuchsTextColorLight");
			btn.Clicked += (s, e) => SelectedTab = tab;
			_headerContainer.Children.Add(btn);
		}

		if (SelectedTab == null && Tabs.Count > 0)
		{
			SelectedTab = Tabs[0];
		}
		else
		{
			UpdateSelectionUI();
		}
	}

	private static void OnSelectedTabChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsTabs tabControl)
		{
			tabControl.UpdateSelectionUI();
		}
	}

	private async void UpdateSelectionUI()
	{
		if (SelectedTab == null) return;

		bool reduceMotion = false;
		if (Application.Current?.Resources.TryGetValue("FuchsReduceMotion", out var val) == true && val is bool b)
			reduceMotion = b;

		uint duration = reduceMotion ? 0u : 250u;

		// Update Header appearance
		for (int i = 0; i < Tabs.Count; i++)
		{
			if (_headerContainer.Children[i] is Button btn)
			{
				if (Tabs[i] == SelectedTab)
					btn.SetDynamicResource(Button.TextColorProperty, "FuchsTextColor");
				else
					btn.SetDynamicResource(Button.TextColorProperty, "FuchsTextColorLight");

				if (Tabs[i] == SelectedTab)
				{
					if (reduceMotion)
						MoveIndicatorToInstant(btn);
					else
						await MoveIndicatorTo(btn).ConfigureAwait(true);
				}
			}
		}

		// Update Content
		View? content = SelectedTab.GetContent();
		if (content != null)
		{
			// Animation transition
			var oldContent = _contentContainer.Children.FirstOrDefault() as View;
			if (oldContent != null && oldContent != content)
			{
				if (reduceMotion)
				{
					_contentContainer.Children.Add(content);
					_contentContainer.Children.Remove(oldContent);
				}
				else
				{
					content.Opacity = 0;
					_contentContainer.Children.Add(content);

					await Task.WhenAll(
						oldContent.FadeTo(0, duration),
						content.FadeTo(1, duration)
					).ConfigureAwait(true);

					_contentContainer.Children.Remove(oldContent);
				}
			}
			else if (oldContent == null)
			{
				_contentContainer.Children.Add(content);
			}
		}
	}

	private void MoveIndicatorToInstant(Button btn)
	{
		_indicator.TranslationX = btn.X;
		_indicator.WidthRequest = btn.Width;
	}

	private async Task MoveIndicatorTo(Button btn)
	{
		// Need to wait for layout if it's first time
		if (btn.Width <= 0)
			await Task.Delay(50).ConfigureAwait(true);

		double targetX = btn.X;
		double targetWidth = btn.Width;

		var indicatorAnimation = new Animation();
		indicatorAnimation.WithConcurrent(
			(f) => _indicator.TranslationX = targetX * f,
			0,
			1,
			Easing.CubicOut
		);
		indicatorAnimation.WithConcurrent(
			(f) => { _indicator.WidthRequest = targetWidth * f; },
			0,
			1,
			Easing.CubicOut
		);
		indicatorAnimation.Commit(_indicator, "IndicatorAnimation", 16, 250);
		await Task.Delay(250).ConfigureAwait(true);

		_indicator.WidthRequest = targetWidth;
	}
}