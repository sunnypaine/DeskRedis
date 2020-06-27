using DeskRedis.Comm.Factory;
using DeskRedis.Control.CustomControl;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DeskRedis.Converter
{
    /// <summary>
    /// TreeViewItem的图标转换器
    /// </summary>
    public class TreeViewItemIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TreeViewItemWithOperator item = (TreeViewItemWithOperator)value;
            if (item.NodeInfo == null)
            {
                return new GeometryGroup();
            }

            if (item.NodeInfo.NodeType == Enums.NodeType.Connection)
            {
                return PathFactory.CreateConnection(PathFactory.Connection);
            }
            else if (item.NodeInfo.NodeType == Enums.NodeType.DataBase)
            {
                return PathFactory.CreateConnection(PathFactory.DataBase);
            }
            else if (item.NodeInfo.NodeType == Enums.NodeType.Key)
            {
                return PathFactory.CreateConnection(PathFactory.Key);
            }
            else if (item.NodeInfo.NodeType == Enums.NodeType.Folder)
            {
                return PathFactory.CreateConnection(PathFactory.Folder);
            }

            return new GeometryGroup();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
