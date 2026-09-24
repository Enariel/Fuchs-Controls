#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

using FuchsControls.Theme;
using Microsoft.Maui.Controls.Shapes;

namespace FuchsControls.Controls;

public sealed class FuchsAccordionItem : FuchsComponent
{
	private readonly Border _root;
	private readonly Border _headerBorder;
	private readonly Grid _bodyHost;
	private readonly Label _headerLabel;
	private readonly Label _toggleLabel;
	private CancellationTokenSource? _animationCancellation;
	private bool _isApplyingExpansion;

	public static readonly BindableProperty HeaderProperty =
		BindableProperty.Create(nameof(Header), typeof(string), typeof(FuchsAccordionItem), string.Empty, propertyChanged: OnVisualPropertyChanged);

	public static readonly BindableProperty BodyProperty =
		BindableProperty.Create(nameof(Body), typeof(View), typeof(FuchsAccordionItem), null, propertyChanged: OnBodyChanged);

	public static readonly BindableProperty IsExpandedProperty =
		BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(FuchsAccordionItem), false, BindingMode.TwoWay, propertyChanged: OnExpandedChanged);

	public string Header
	{
		get => (string)GetValue(HeaderProperty);
		set => SetValue(HeaderProperty, value);
	}

	public View? Body
	{
		get => (View?)GetValue(BodyProperty);
		set => SetValue(BodyProperty, value);
	}

	public bool IsExpanded
	{
		get => (bool)GetValue(IsExpandedProperty);
		set => SetValue(IsExpandedProperty, value);
	}

	public event EventHandler? ExpandedChanged;

	public FuchsAccordionItem()
	{
		_headerLabel = new Label { VerticalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold };
		_toggleLabel = new Label { Text = "+", FontSize = 22, HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center };
		_headerBorder = new Border();
		_bodyHost = new Grid { IsVisible = false, Opacity = 0, ScaleY = 0.96, TranslationY = -8 };
		Grid header = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
		header.Add(_headerLabel, 0, 0);
		header.Add(_toggleLabel, 1, 0);
		_headerBorder.Content = header;
		_headerBorder.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(Toggle) });
		VerticalStackLayout content = new() { Spacing = 0, Children = { _headerBorder, _bodyHost } };
		_root = new Border { Content = content };
		Content = _root;
		ApplyTheme();
	}

	protected override void OnHandlerChanging(HandlerChangingEventArgs args)
	{
		if (args.NewHandler is null)
			CancelAnimation();
		base.OnHandlerChanging(args);
	}

	protected override void ApplyTheme()
	{
		FuchsTheme theme = FuchsThemeProvider.Current;
		_headerLabel.Text = Header;
		_headerLabel.TextColor = theme.Text;
		_headerLabel.FontSize = ResolveFontSize();
		_toggleLabel.Text = IsExpanded ? "−" : "+";
		_toggleLabel.TextColor = theme.Primary;
		_root.BackgroundColor = theme.Background;
		_root.Stroke = new SolidColorBrush(theme.BackgroundDarker);
		_root.StrokeThickness = theme.BorderWidth;
		_root.StrokeShape = new RoundRectangle { CornerRadius = theme.CornerRadiusSmall };
		_headerBorder.BackgroundColor = theme.Background;
		_headerBorder.Padding = theme.PanelPadding;
		_headerBorder.Stroke = IsExpanded ? new SolidColorBrush(theme.BackgroundDarker) : Brush.Transparent;
		_headerBorder.StrokeThickness = IsExpanded ? theme.BorderWidth : 0;
		_bodyHost.Padding = theme.PanelPadding;
		_root.Opacity = IsDisabled ? 0.65 : 1;
	}

	internal void SetExpandedFromParent(bool expanded)
	{
		if (IsExpanded != expanded)
			IsExpanded = expanded;
	}

	internal void RefreshTheme() => ApplyTheme();

	private void Toggle()
	{
		if (!IsDisabled)
			IsExpanded = !IsExpanded;
	}

	private async Task AnimateExpansionAsync(bool expanded)
	{
		CancelAnimation();
		CancellationTokenSource animation = new();
		_animationCancellation = animation;
		CancellationToken token = animation.Token;
		if (expanded)
		{
			_bodyHost.IsVisible = true;
			_bodyHost.Opacity = 0;
			_bodyHost.ScaleY = 0.96;
			_bodyHost.TranslationY = -8;
		}

		try
		{
			Task opacity = _bodyHost.FadeToAsync(expanded ? 1 : 0, 180, Easing.CubicInOut);
			Task scale = _bodyHost.ScaleToAsync(expanded ? 1 : 0.96, 180, Easing.CubicInOut);
			Task translation = _bodyHost.TranslateToAsync(0, expanded ? 0 : -8, 180, Easing.CubicInOut);
			await Task.WhenAll(opacity, scale, translation).WaitAsync(token);
			if (!expanded)
				_bodyHost.IsVisible = false;
		}
		catch (OperationCanceledException) { }
		finally
		{
			if (ReferenceEquals(_animationCancellation, animation))
			{
				animation.Dispose();
				_animationCancellation = null;
			}
		}
	}

	private void CancelAnimation()
	{
		_animationCancellation?.Cancel();
		_animationCancellation?.Dispose();
		_animationCancellation = null;
	}

	private static void OnBodyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsAccordionItem item)
		{
			item._bodyHost.Children.Clear();
			if (newValue is View view)
				item._bodyHost.Children.Add(view);
		}
	}

	private static void OnExpandedChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsAccordionItem item && !item._isApplyingExpansion)
		{
			item._isApplyingExpansion = true;
			try
			{
				item.ApplyTheme();
				item.ExpandedChanged?.Invoke(item, EventArgs.Empty);
				_ = item.AnimateExpansionAsync((bool)newValue);
			}
			finally
			{
				item._isApplyingExpansion = false;
			}
		}
	}

	private static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is FuchsAccordionItem item)
			item.ApplyTheme();
	}
}