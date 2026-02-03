#region Meta

// FuchsControls
// Created: 29/01/2026
// Modified: 03/02/2026

#endregion

using System.Windows.Input;

namespace FuchsControls;

public enum IconPosition
{
	Top,
	Bottom,
	Left,
	Right
}

public class FuchsIconButton : ContentView
{
	public static readonly BindableProperty TextProperty = BindableProperty.Create(
		nameof(Text),
		typeof(string),
		typeof(FuchsIconButton),
		string.Empty,
		propertyChanged: (bindable, _, _) => ((FuchsIconButton)bindable).UpdateLayout());

	public static readonly BindableProperty IconPathDataProperty = BindableProperty.Create(
		nameof(IconPathData),
		typeof(string),
		typeof(FuchsIconButton),
		string.Empty);

	public static readonly BindableProperty IconSizeProperty = BindableProperty.Create(
		nameof(IconSize),
		typeof(double),
		typeof(FuchsIconButton),
		24.0);

	public static readonly BindableProperty IconColorProperty = BindableProperty.Create(
		nameof(IconColor),
		typeof(Color),
		typeof(FuchsIconButton),
		null);

	public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(
		nameof(IsBusy),
		typeof(bool),
		typeof(FuchsIconButton),
		false,
		propertyChanged: (bindable, _, _) => ((FuchsIconButton)bindable).UpdateBusyState());

	public static readonly BindableProperty IconPositionProperty = BindableProperty.Create(
		nameof(IconPosition),
		typeof(IconPosition),
		typeof(FuchsIconButton),
		IconPosition.Left,
		propertyChanged: (bindable, _, _) => ((FuchsIconButton)bindable).UpdateLayout());

	public static readonly BindableProperty CommandProperty = BindableProperty.Create(
		nameof(Command),
		typeof(ICommand),
		typeof(FuchsIconButton));

	public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
		nameof(CommandParameter),
		typeof(object),
		typeof(FuchsIconButton));

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public string IconPathData
	{
		get => (string)GetValue(IconPathDataProperty);
		set => SetValue(IconPathDataProperty, value);
	}

	public double IconSize
	{
		get => (double)GetValue(IconSizeProperty);
		set => SetValue(IconSizeProperty, value);
	}

	public Color? IconColor
	{
		get => (Color?)GetValue(IconColorProperty);
		set => SetValue(IconColorProperty, value);
	}

	public bool IsBusy
	{
		get => (bool)GetValue(IsBusyProperty);
		set => SetValue(IsBusyProperty, value);
	}

	public IconPosition IconPosition
	{
		get => (IconPosition)GetValue(IconPositionProperty);
		set => SetValue(IconPositionProperty, value);
	}

	public ICommand Command
	{
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public object CommandParameter
	{
		get => GetValue(CommandParameterProperty);
		set => SetValue(CommandParameterProperty, value);
	}

	public event EventHandler? Clicked;

	private readonly Grid _container;
	private readonly FuchsIcon _fuchsIcon;
	private readonly Label _label;
	private readonly ActivityIndicator _activityIndicator;

	public FuchsIconButton()
	{
		_container = new Grid();

		_fuchsIcon = new FuchsIcon();
		_fuchsIcon.SetBinding(FuchsIcon.PathDataProperty, new Binding(nameof(IconPathData), source: this));
		_fuchsIcon.SetBinding(FuchsIcon.IconSizeProperty, new Binding(nameof(IconSize), source: this));

		_fuchsIcon.SetBinding(FuchsIcon.FillColorProperty, new Binding(nameof(IconColor), source: this) { FallbackValue = Colors.Grey });

		_label = new Label
		{
			VerticalOptions = LayoutOptions.Center,
			HorizontalOptions = LayoutOptions.Center,
			StyleClass = new[] { "typo-button" }
		};
		_label.SetBinding(Label.TextProperty, new Binding(nameof(Text), source: this));

		_activityIndicator = new ActivityIndicator
		{
			IsRunning = false,
			IsVisible = false,
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center
		};
		_activityIndicator.SetBinding(ActivityIndicator.ColorProperty, new Binding(nameof(IconColor), source: this) { FallbackValue = Colors.Grey });

		UpdateLayout();

		var tapGesture = new TapGestureRecognizer();
		tapGesture.Tapped += OnTapped;
		GestureRecognizers.Add(tapGesture);

		Content = _container;

		// Set default Accessibility
		AutomationProperties.SetIsInAccessibleTree(this, true);
		this.SetBinding(SemanticProperties.DescriptionProperty, new Binding(nameof(Text), source: this));

		VisualStateManager.SetVisualStateGroups(this, new VisualStateGroupList
		{
			new VisualStateGroup
			{
				Name = "CommonStates",
				States =
				{
					new VisualState { Name = "Normal" },
					new VisualState
					{
						Name = "PointerOver",
						Setters = { new Setter { Property = OpacityProperty, Value = 0.8 } }
					},
					new VisualState
					{
						Name = "Pressed",
						Setters = { new Setter { Property = OpacityProperty, Value = 0.6 } }
					}
				}
			}
		});
	}

	private void UpdateLayout()
	{
		_container.Children.Clear();
		_container.RowDefinitions.Clear();
		_container.ColumnDefinitions.Clear();

		switch (IconPosition)
		{
			case IconPosition.Top:
				_container.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
				_container.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
				_container.Add(_fuchsIcon, 0, 0);
				_container.Add(_label, 0, 1);
				break;
			case IconPosition.Bottom:
				_container.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
				_container.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
				_container.Add(_label, 0, 0);
				_container.Add(_fuchsIcon, 0, 1);
				break;
			case IconPosition.Left:
				_container.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
				_container.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
				_container.Add(_fuchsIcon, 0, 0);
				_container.Add(_label, 1, 0);
				break;
			case IconPosition.Right:
				_container.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
				_container.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
				_container.Add(_label, 0, 0);
				_container.Add(_fuchsIcon, 1, 0);
				break;
		}
	}

	private void UpdateBusyState()
	{
		if (IsBusy)
		{
			_fuchsIcon.IsVisible = false;
			_label.IsVisible = false;
			_activityIndicator.IsVisible = true;
			_activityIndicator.IsRunning = true;

			if (!_container.Children.Contains(_activityIndicator))
				_container.Add(_activityIndicator);
		}
		else
		{
			_fuchsIcon.IsVisible = true;
			_label.IsVisible = true;
			_activityIndicator.IsVisible = false;
			_activityIndicator.IsRunning = false;

			if (_container.Children.Contains(_activityIndicator))
				_container.Remove(_activityIndicator);
		}
	}

	private void OnTapped(object? sender, TappedEventArgs e)
	{
		if (IsBusy) return;

		Clicked?.Invoke(this, EventArgs.Empty);

		if (Command?.CanExecute(CommandParameter) == true)
		{
			Command.Execute(CommandParameter);
		}
	}
}