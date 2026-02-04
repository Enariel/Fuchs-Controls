#region Meta

// FuchsControls
// Created: 29/01/2026
// Modified: 03/02/2026

#endregion

using System.Windows.Input;

namespace FuchsControls;

public class FuchsIconButton : FuchsButtonBase
{
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

 private readonly Grid _container;
 private readonly FuchsIcon _fuchsIcon;

	public FuchsIconButton()
	{
		_container = new Grid
		{
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center
		};

		_fuchsIcon = new FuchsIcon();
		_fuchsIcon.SetBinding(FuchsIcon.PathDataProperty, new Binding(nameof(IconPathData), source: this));
		_fuchsIcon.SetBinding(FuchsIcon.IconSizeProperty, new Binding(nameof(IconSize), source: this));

		_fuchsIcon.SetBinding(FuchsIcon.FillColorProperty, new Binding(nameof(IconColor), source: this) { FallbackValue = Colors.Grey });
		_container.Children.Add(_fuchsIcon);
		SetInnerContent(_container);

		// Set default Accessibility
		AutomationProperties.SetIsInAccessibleTree(this, true);
		SemanticProperties.SetDescription(this, "Icon button");

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
						Setters =
						{
							new Setter { Property = OpacityProperty, Value = 0.6 },
							new Setter { Property = ScaleProperty, Value = 0.85 }
						}
					}
				}
			}
		});
	}
}