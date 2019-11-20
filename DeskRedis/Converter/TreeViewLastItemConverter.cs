using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace DeskRedis.Converter
{
    /// <summary>
    /// 判断是否是构造当前层的最后一个元素
    /// </summary>
    public class TreeViewLastItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;//不画末位垂直线
            //return false;//画末尾垂直线
            //TreeViewItem item = (TreeViewItem)value;
            //var ic = ItemsControl.ItemsControlFromItemContainer(item);
            //return ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
