using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace FuchsControls;

public sealed class FuchsAutocomplete : FuchsTextBase
{
	public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
		nameof(ItemsSource), typeof(IEnumerable<string>), typeof(FuchsAutocomplete), null, propertyChanged: OnItemsSourceChanged);

	private readonly ObservableCollection<string> _filteredItems = new();
	private readonly CollectionView _suggestionsView;
	private bool _isFieldFocused;
	private bool _isCommittingSuggestion;

	public FuchsAutocomplete()
		: base(new Entry().ApplyFuchsEntryStyle())
	{
		_suggestionsView = new CollectionView
						   {
							   ItemsSource = _filteredItems
							   , SelectionMode = SelectionMode.Single
							   , ItemTemplate = new DataTemplate(() =>
							   {
								   var suggestion = new Label();
								   suggestion.SetBinding(Microsoft.Maui.Controls.Label.TextProperty, ".");
								   return suggestion.ApplyFuchsStyle("FuchsAutocompleteSuggestionStyle");
							   })
						   };
		_suggestionsView.SelectionChanged += OnSuggestionSelected;
		PostInputContent.Children.Add(new Border { Content = _suggestionsView }.ApplyFuchsStyle("FuchsAutocompleteSuggestionsStyle"));
		Input.TextChanged += (_, _) => RefreshSuggestions();
	}

	public IEnumerable<string>? ItemsSource
	{
		get => (IEnumerable<string>?)GetValue(ItemsSourceProperty);
		set => SetValue(ItemsSourceProperty, value);
	}

	protected override void UpdateReturnCommand(ICommand? command) => ((Entry)Input).ReturnCommand = command;

	protected override void OnInputFocusChanged(bool isFocused)
	{
		_isFieldFocused = isFocused;
		RefreshSuggestions();
	}

	private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var autocomplete = (FuchsAutocomplete)bindable;
		autocomplete.OnItemsSourceChanged(oldValue as INotifyCollectionChanged, newValue as INotifyCollectionChanged);
	}

	private void OnItemsSourceChanged(INotifyCollectionChanged? oldSource, INotifyCollectionChanged? newSource)
	{
		if (oldSource is not null)
		{
			oldSource.CollectionChanged -= OnItemsCollectionChanged;
		}

		if (newSource is not null)
		{
			newSource.CollectionChanged += OnItemsCollectionChanged;
		}

		RefreshSuggestions();
	}

	private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => RefreshSuggestions();

	private void OnSuggestionSelected(object? sender, SelectionChangedEventArgs e)
	{
		if (_isCommittingSuggestion || e.CurrentSelection.FirstOrDefault() is not string selected)
		{
			return;
		}

		_isCommittingSuggestion = true;
		try
		{
			Value = selected;
		}
		finally
		{
			_isCommittingSuggestion = false;
			_suggestionsView.SelectedItem = null;
			PostInputContent.IsVisible = false;
		}
	}

	private void RefreshSuggestions()
	{
		var query = Value ?? string.Empty;
		_filteredItems.Clear();
		if (!string.IsNullOrEmpty(query) && ItemsSource is not null)
		{
			foreach (var item in ItemsSource)
			{
				if (item is not null && item.Contains(query, StringComparison.OrdinalIgnoreCase))
				{
					_filteredItems.Add(item);
				}
			}
		}

		PostInputContent.IsVisible = _isFieldFocused && !_isCommittingSuggestion && !string.IsNullOrEmpty(query) && _filteredItems.Count > 0;
	}
}