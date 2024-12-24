using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AppUsageTimerPro;

public class RowIndexConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // 获取 DataGrid 行的索引
        if (value is DataGridRow row)
        {
            return row.GetIndex() + 1;  // 获取行的索引
        }
        return -1;  // 默认返回 -1，表示无效的索引
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}


public class DataGridIndexRowColumn : DataGridTemplateColumn
{
    public DataGridIndexRowColumn()
    {
        // 设置模板，这里可以选择自定义其他属性。
        this.CellTemplate = CreateCellTemplate();
    }

    private DataTemplate CreateCellTemplate()
    {
        // 创建一个DataTemplate，并在其中绑定行号
        var textBlock = new TextBlock();
        var binding = new Binding
        {
            RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(DataGridRow), 1),
            Path = new PropertyPath("."),
            Converter = new RowIndexConverter() // 使用自定义的行号转换器
        };
        textBlock.SetBinding(TextBlock.TextProperty, binding);
        textBlock.HorizontalAlignment = HorizontalAlignment.Center;

        // 将TextBlock放入一个DataTemplate中
        var dataTemplate = new DataTemplate();
        var frameworkElementFactory = new FrameworkElementFactory(typeof(TextBlock));
        frameworkElementFactory.SetBinding(TextBlock.TextProperty, binding);
        frameworkElementFactory.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);

        dataTemplate.VisualTree = frameworkElementFactory;

        return dataTemplate;
    }
}