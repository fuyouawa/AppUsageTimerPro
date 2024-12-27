using System.Collections.ObjectModel;

namespace AppUsageTimerPro
{
    public class TimerPageViewModel : ViewModelBase
    {
        private ObservableCollection<TimerItem> _collection;

        public ObservableCollection<TimerItem> Collection
        {
            get => _collection;
            set
            {
                _collection = value;
                OnPropertyChanged();
            }
        }

        private object? _selectedItem;

        public object? SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        private int _selectedIndex = 0;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
            }
        }

        public TimerPageViewModel()
        {

        }
    }
}