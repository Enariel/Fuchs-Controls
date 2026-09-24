#region Meta

// FuchsControls
// Created: 24/09/2026
// Modified: 24/09/2026

#endregion

namespace FuchsControls.Controls;

public sealed class FuchsTab : FuchsComponent
{
	public static readonly BindableProperty HeaderProperty =
		BindableProperty.Create(nameof(Header), typeof(string), typeof(FuchsTab), string.Empty);

	public string Header
	{
		get => (string)GetValue(HeaderProperty);
		set => SetValue(HeaderProperty, value);
	}

	internal bool IsSelected { get; private set; }

	public FuchsTab()
	{
		IsVisible = false;
		IsEnabled = true;
		ApplyTheme();
	}

	protected override void ApplyTheme()
	{
		IsEnabled = !IsDisabled;
		Opacity = IsDisabled ? 0.65 : 1;
	}

	internal void SetSelectedState(bool isSelected)
	{
		IsSelected = isSelected;
		IsVisible = isSelected;
		AutomationProperties.SetIsInAccessibleTree(this, isSelected);
	}

	internal void RefreshTheme() => ApplyTheme();
}