#region Meta
// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026
#endregion

using System.Diagnostics;

namespace FuchsControls.Fields;

public abstract class TextFieldBase : ContentView, ITextField
{
   public static readonly BindableProperty LabelProperty = BindableProperty.Create
    (
        nameof(Label),
        typeof(string),
        typeof(TextFieldBase),
        defaultValue: string.Empty
    );

    public static readonly BindableProperty HelpTextProperty = BindableProperty.Create
    (
        nameof(HelpText),
        typeof(string),
        typeof(TextFieldBase),
        defaultValue: string.Empty
    );

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create
    (
        nameof(Placeholder),
        typeof(string),
        typeof(TextFieldBase),
        defaultValue: string.Empty
    );

    public static readonly BindableProperty TextProperty = BindableProperty.Create
    (
        nameof(Text),
        typeof(string),
        typeof(TextFieldBase),
        defaultValue: string.Empty
    );

    public static readonly BindableProperty KeyboardProperty = BindableProperty.Create
    (
        nameof(Keyboard),
        typeof(Keyboard),
        typeof(TextFieldBase),
        defaultValue: null
    );

    public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create
    (
        nameof(IsReadOnly),
        typeof(bool),
        typeof(TextFieldBase),
        false,
        BindingMode.TwoWay
    );

    public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create
    (
        nameof(MaxLength),
        typeof(int),
        typeof(TextFieldBase),
        defaultValue: int.MaxValue
    );

    public static readonly BindableProperty OrientationProperty = BindableProperty.Create
    (
        nameof(Orientation),
        typeof(FieldOrientation),
        typeof(TextFieldBase),
        defaultValue: FieldOrientation.Vertical
    );

    public FieldOrientation Orientation { get => (FieldOrientation)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

    #region ITextField Members

    public string Label { get => (string)GetValue(LabelProperty); set => SetValue(LabelProperty, value); }

    public string HelpText { get => (string)GetValue(HelpTextProperty); set => SetValue(HelpTextProperty, value); }

    public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }

    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

    public bool IsReadOnly { get => (bool)GetValue(IsReadOnlyProperty); set => SetValue(IsReadOnlyProperty, value); }

    public Keyboard Keyboard { get => (Keyboard)GetValue(KeyboardProperty); set => SetValue(KeyboardProperty, value); }

    public int MaxLength { get => (int)GetValue(MaxLengthProperty); set => SetValue(MaxLengthProperty, value); }

    #endregion

    protected virtual void OnCompleted(object sender, EventArgs e)
    {
        Debug.WriteLine($"Text Completed by: {nameof(sender)}");
    }
}