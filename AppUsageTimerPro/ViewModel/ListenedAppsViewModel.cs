using System.Collections.ObjectModel;

namespace AppUsageTimerPro;

public class ListenedAppsViewModel : ViewModelBase
{
    public ObservableCollection<ListenedApp> Collection { get; set; } = new();

    private object? _selectedItem;

    public object? SelectedItem
    {
        get { return _selectedItem; }
        set { _selectedItem = value; OnPropertyChanged(); }
    }

    private int _selectedIndex = 0;

    public int SelectedIndex
    {
        get { return _selectedIndex; }
        set { _selectedIndex = value; OnPropertyChanged(); }
    }
}