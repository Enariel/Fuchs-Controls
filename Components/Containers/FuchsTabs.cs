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

	public static readonly BindableProperty SpacingProperty = BindableProperty.Create(
		nameof(Spacing), typeof(double), typeof(FuchsTabs), 10.0);

	public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
		nameof(CornerRadius), typeof(CornerRadius), typeof(FuchsTabs), new CornerRadius(0));

	public static readonly BindableProperty AccentColorProperty = BindableProperty.Create(
		nameof(AccentColor), typeof(Color), typeof(FuchsTabs), null, propertyChanged: (b, o, n) => ((FuchsTabs)b).TabIndicatorColor = (Color?)n);

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

	public double Spacing
	{
		get => (double)GetValue(SpacingProperty);
		set => SetValue(SpacingProperty, value);
	}

	public CornerRadius CornerRadius
	{
		get => (CornerRadius)GetValue(CornerRadiusProperty);
		set => SetValue(CornerRadiusProperty, value);
	}

	public Color? AccentColor
	{
		get => (Color?)GetValue(AccentColorProperty);
		set => SetValue(AccentColorProperty, value);
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

		this.SetDynamicResource(AccentColorProperty, "FuchsAccentColor");
		this.SetDynamicResource(SpacingProperty, "FuchsSpacing");

		_headerContainer = new StackLayout { Orientation = StackOrientation.Horizontal };
		_headerContainer.SetBinding(StackLayout.SpacingProperty, new Binding(nameof(Spacing), source: this));

		_indicator = new BoxView
		{
			HeightRequest = 3,
			HorizontalOptions = LayoutOptions.Start,
			VerticalOptions = LayoutOptions.End
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
			var label = new Label
			{
				Text = tab.Title,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				Padding = new Thickness(10, 5),
				StyleClass = new[] { "typo-button" }
			};
			label.SetDynamicResource(Label.TextColorProperty, "FuchsTextColorLight");

			var tap = new TapGestureRecognizer();
			tap.Tapped += (s, e) => SelectedTab = tab;
			label.GestureRecognizers.Add(tap);

			_headerContainer.Children.Add(label);
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
			if (_headerContainer.Children[i] is Label lbl)
			{
				if (Tabs[i] == SelectedTab)
					lbl.SetDynamicResource(Label.TextColorProperty, "FuchsTextColor");
				else
					lbl.SetDynamicResource(Label.TextColorProperty, "FuchsTextColorLight");

				if (Tabs[i] == SelectedTab)
				{
					if (reduceMotion)
						MoveIndicatorToInstant(lbl);
					else
						await MoveIndicatorTo(lbl).ConfigureAwait(true);
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

	private void MoveIndicatorToInstant(View view)
	{
		_indicator.TranslationX = view.X;
		_indicator.WidthRequest = view.Width;
	}

	private async Task MoveIndicatorTo(View view)
	{
		// Need to wait for layout if it's first time
		if (view.Width <= 0)
			await Task.Delay(50).ConfigureAwait(true);

		double targetX = view.X;
		double targetWidth = view.Width;

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