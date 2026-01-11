#region Meta
// FuchsControls
// Created: 11/01/2026
// Modified: 11/01/2026
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuchsControls.Containers;

public partial class FuchsAccordion : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title), typeof(string), typeof(FuchsAccordion), string.Empty);

    public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(
        nameof(IsExpanded), typeof(bool), typeof(FuchsAccordion), false, propertyChanged: OnIsExpandedChanged);

    public static readonly BindableProperty AccordionContentProperty = BindableProperty.Create(
        nameof(AccordionContent), typeof(View), typeof(FuchsAccordion));

    public static readonly BindableProperty HeaderBackgroundColorProperty = BindableProperty.Create(
        nameof(HeaderBackgroundColor), typeof(Color), typeof(FuchsAccordion), Colors.Transparent);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    public View AccordionContent
    {
        get => (View)GetValue(AccordionContentProperty);
        set => SetValue(AccordionContentProperty, value);
    }

    public Color HeaderBackgroundColor
    {
        get => (Color)GetValue(HeaderBackgroundColorProperty);
        set => SetValue(HeaderBackgroundColorProperty, value);
    }

    public FuchsAccordion()
    {
        InitializeComponent();
    }

    private async void OnHeaderTapped(object sender, EventArgs e)
    {
        IsExpanded = !IsExpanded;
    }

    private static async void OnIsExpandedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is FuchsAccordion accordion)
        {
            await accordion.AnimateExpansion((bool)newValue);
        }
    }

    private async Task AnimateExpansion(bool expanded)
    {
        if (expanded)
        {
            ContentContainer.Opacity = 0;
            ContentContainer.TranslationY = -10;
            await Task.WhenAll(
                ContentContainer.FadeToAsync(1, 250, Easing.CubicOut),
                ContentContainer.TranslateToAsync(0, 0, 250, Easing.CubicOut),
                ChevronLabel.RotateToAsync(180, 250, Easing.CubicInOut)
            );
        }
        else
        {
            await Task.WhenAll(
                ContentContainer.FadeToAsync(0, 200, Easing.CubicIn),
                ChevronLabel.RotateToAsync(0, 250, Easing.CubicInOut)
            );
        }
    }
}