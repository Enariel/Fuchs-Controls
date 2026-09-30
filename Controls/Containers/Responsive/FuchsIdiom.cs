using System.Collections.ObjectModel;
using System.Collections.Specialized;
using FuchsControls.Responsive;
using Microsoft.Maui.Devices;

namespace FuchsControls;

/// <summary>
/// A responsive container that conditionally displays or hides its content
/// based on the target device idiom (e.g., Phone, Tablet, Desktop, TV).
/// </summary>
public sealed class FuchsIdiom : FuchsResponsiveBase
{
	/// <summary>
	/// Identifies the <see cref="Idiom"/> bindable property.
	/// </summary>
	public static readonly BindableProperty IdiomProperty = BindableProperty.Create(
		nameof(Idiom), typeof(DeviceIdiom), typeof(FuchsIdiom), DeviceIdiom.Unknown,
		propertyChanged: OnIdiomPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="Idioms"/> bindable property.
	/// </summary>
	public static readonly BindableProperty IdiomsProperty = BindableProperty.Create(
		nameof(Idioms), typeof(ObservableCollection<DeviceIdiom>), typeof(FuchsIdiom),
		defaultValueCreator: _ => new ObservableCollection<DeviceIdiom>(),
		propertyChanged: OnIdiomsPropertyChanged);

	/// <summary>
	/// Initializes a new instance of the <see cref="FuchsIdiom"/> class.
	/// </summary>
	public FuchsIdiom()
	{
		Idioms.CollectionChanged += OnIdiomsCollectionChanged;
	}

	/// <summary>
	/// Gets or sets a single target device idiom to match.
	/// </summary>
	public DeviceIdiom Idiom
	{
		get => (DeviceIdiom)GetValue(IdiomProperty);
		set => SetValue(IdiomProperty, value);
	}

	/// <summary>
	/// Gets or sets a collection of target device idioms to match.
	/// </summary>
	public ObservableCollection<DeviceIdiom> Idioms
	{
		get => (ObservableCollection<DeviceIdiom>)GetValue(IdiomsProperty);
		set => SetValue(IdiomsProperty, value ?? new ObservableCollection<DeviceIdiom>());
	}

	/// <inheritdoc />
	protected override bool IsConditionMet()
	{
		if (Idiom == DeviceIdiom.Unknown && Idioms.Count == 0)
		{
			return true;
		}

		var currentIdiom = DeviceInfo.Current.Idiom;
		return Idiom == currentIdiom || (Idioms.Count > 0 && Idioms.Contains(currentIdiom));
	}

	private static void OnIdiomPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		((FuchsIdiom)bindable).RefreshVisibility();
	}

	private static void OnIdiomsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var responsiveControl = (FuchsIdiom)bindable;
		if (oldValue is ObservableCollection<DeviceIdiom> oldIdioms)
		{
			oldIdioms.CollectionChanged -= responsiveControl.OnIdiomsCollectionChanged;
		}

		if (newValue is ObservableCollection<DeviceIdiom> newIdioms)
		{
			newIdioms.CollectionChanged += responsiveControl.OnIdiomsCollectionChanged;
		}
		else
		{
			responsiveControl.SetValue(IdiomsProperty, new ObservableCollection<DeviceIdiom>());
			return;
		}

		responsiveControl.RefreshVisibility();
	}

	private void OnIdiomsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		RefreshVisibility();
	}
}