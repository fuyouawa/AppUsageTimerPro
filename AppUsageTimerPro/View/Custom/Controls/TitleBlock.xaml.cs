using System.Windows;
using System.Windows.Controls;

namespace AppUsageTimerPro
{
    /// <summary>
    /// Title.xaml 的交互逻辑
    /// </summary>
    public partial class TitleBlock : UserControl
    {
        public TitleBlock()
        {
            InitializeComponent();
        }

        // 定义依赖属性
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(TitleBlock),
                new PropertyMetadata(string.Empty, OnTitleChanged));

        // CLR包装器
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        // 可选：属性更改回调
        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TitleBlock control)
            {
                // 当属性值更改时更新UI
                control.TxtTitle.Text = e.NewValue as string;
            }
        }
    }
}
