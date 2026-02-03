#region Meta
// FuchsControls
// Created: 10/01/2026
// Modified: 10/01/2026
#endregion

using System.Diagnostics;

namespace FuchsControls.Fields;

public abstract class TextFieldBase : FieldBase, ITextField
{
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

    #region ITextField Members

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