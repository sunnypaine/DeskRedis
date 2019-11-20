using DeskRedis.Comm.Factory;
using DeskRedis.Model;
using System;
using System.Globalization;
using System.Windows.Controls;
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
            TreeViewItem item = (TreeViewItem)value;
            if (item.Tag == null)
            {
                return new GeometryGroup();
            }
            NodeInfo info = item.Tag as NodeInfo;
            if (info == null)
            {
                return new GeometryGroup();
            }

            if (info.NodeType == Enums.NodeType.Connection)
            {
                return PathFactory.CreateConnection(PathFactory.Connection);
            }
            else if (info.NodeType == Enums.NodeType.DataBase)
            {
                return PathFactory.CreateConnection(PathFactory.DataBase);
            }
            else if (info.NodeType == Enums.NodeType.Key)
            {
                return PathFactory.CreateConnection(PathFactory.Key);
            }
            else if (info.NodeType == Enums.NodeType.Folder)
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
