using System.Collections.ObjectModel;

namespace AppUsageTimerPro;

public class ListenedProcessesViewModel : ViewModelBase
{
    public ObservableCollection<ListenedProcess> Collection { get; set; } = new()
    {
        new ListenedProcess("asda.exe"),
        new ListenedProcess("assdfda345.exe"),
    };

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