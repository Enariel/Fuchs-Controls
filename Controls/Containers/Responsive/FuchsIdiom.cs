using System.Collections.ObjectModel;
using System.Collections.Specialized;
using FuchsControls.Responsive;
using Microsoft.Maui.Devices;

namespace FuchsControls;

public sealed class FuchsIdiom : FuchsResponsiveBase
{
	public static readonly BindableProperty IdiomProperty = BindableProperty.Create(
		nameof(Idiom), typeof(DeviceIdiom), typeof(FuchsIdiom), DeviceIdiom.Unknown,
		propertyChanged: OnIdiomPropertyChanged);

	public static readonly BindableProperty IdiomsProperty = BindableProperty.Create(
		nameof(Idioms), typeof(ObservableCollection<DeviceIdiom>), typeof(FuchsIdiom),
		defaultValueCreator: _ => new ObservableCollection<DeviceIdiom>(),
		propertyChanged: OnIdiomsPropertyChanged);

	public FuchsIdiom()
	{
		Idioms.CollectionChanged += OnIdiomsCollectionChanged;
	}

	public DeviceIdiom Idiom
	{
		get => (DeviceIdiom)GetValue(IdiomProperty);
		set => SetValue(IdiomProperty, value);
	}

	public ObservableCollection<DeviceIdiom> Idioms
	{
		get => (ObservableCollection<DeviceIdiom>)GetValue(IdiomsProperty);
		set => SetValue(IdiomsProperty, value ?? new ObservableCollection<DeviceIdiom>());
	}

	protected override bool IsConditionMet()
	{
		if (Idiom == DeviceIdiom.Unknown && Idioms.Count == 0)
		{
			return true;
		}

		var currentIdiom = DeviceInfo.Current.Idiom;
		return Idiom == currentIdiom || Idioms.Contains(currentIdiom);
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