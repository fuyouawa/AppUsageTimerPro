using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppUsageTimerPro.View.Tools
{
    /// <summary>
    /// DisplayTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class DisplayTextBox : UserControl, INotifyPropertyChanged
    {
        public DisplayTextBox()
        {
            InitializeComponent();
            DataContext = this;
        }

        private string _display = "";

        public string Display
        {
            get { return _display; }
            set { _display = value; OnPropertyChanged(); }
        }

        private double _displayWidth = 80.0;

        public double DisplayWidth
        {
            get { return _displayWidth; }
            set { _displayWidth = value; OnPropertyChanged(); }
        }

        private double _displayHeight = 20.0;

        public double DisplayHeight
        {
            get { return _displayHeight; }
            set { _displayHeight = value; OnPropertyChanged(); }
        }

        private double _displayFontSize = 14.5;

        public double DisplayFontSize
        {
            get { return _displayFontSize; }
            set { _displayFontSize = value; OnPropertyChanged(); }
        }

        private Thickness _displayMargin;

        public Thickness DisplayMargin
        {
            get { return _displayMargin; }
            set { _displayMargin = value; OnPropertyChanged(); }
        }


        private string _placeHolder = "";

        public string PlaceHolder
        {
            get { return _placeHolder; }
            set { _placeHolder = value; OnPropertyChanged(); }
        }

        private string _text = "";

        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged(); }
        }

        private double _textWidth = 150.0;

        public double TextWidth
        {
            get { return _textWidth; }
            set { _textWidth = value; OnPropertyChanged(); }
        }

        private double _textHeight = 20.0;

        public double TextHeight
        {
            get { return _textHeight; }
            set { _textHeight = value; OnPropertyChanged(); }
        }

        private double _textFontSize = 14.5;

        public double TextFontSize
        {
            get { return _textFontSize; }
            set { _textFontSize = value; OnPropertyChanged(); }
        }

        private Thickness _textMargin;

        public Thickness TextMargin
        {
            get { return _textMargin; }
            set { _textMargin = value; OnPropertyChanged(); }
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
