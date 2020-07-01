using DeskRedis.Control.CustomControl;
using DeskRedis.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskRedis.Util
{
    public static class TreeViewItemExtension
    {
        public static void UseOperation(this TreeViewItemWithOperator item, OperateType[] operateTypes)
        {
            foreach (OperateType type in operateTypes)
            {
                switch (type)
                {
                    case OperateType.OPEN:
                        break;
                    case OperateType.ADD:
                        item.AddVisibility = true;
                        item.AddEnabled = true;
                        break;
                    case OperateType.DELETE:
                        item.DeleteVisibility = true;
                        item.DeleteEnabled = true;
                        break;
                    case OperateType.EDIT:
                        item.EditVisibility = true;
                        item.EditEnabled = true;
                        break;
                    case OperateType.REFRESH:
                        item.RefreshVisibility = true;
                        item.RefreshEnabled = true;
                        break;
                    case OperateType.SEARCH:
                        break;
                    case OperateType.CLOSE:
                        break;
                    case OperateType.FLUSH:
                        item.ClearVisibility = true;
                        item.ClearEnabled = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
