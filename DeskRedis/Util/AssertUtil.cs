using DeskRedis.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskRedis.Util
{
    public static class AssertUtil
    {
        /// <summary>
        /// 校验是否为null或空字符串（如果被校验对象为String类型）。
        /// </summary>
        /// <example>
        /// 指定的参数{0}不能为空。不合法的参数将自动替换{0}占位符。
        /// </example>
        /// <param name="tip">如果为null或空字符串，将打印的错误信息。</param>
        /// <param name="obj">被校验的对象。</param>
        public static void IsNullOrEmpty(string tip, params object[] obj)
        {
            if (obj == null || obj.Length <= 0)
            {
                throw new ArgumentException("指定的参数 obj 为null或成员不存在。");
            }

            foreach (object item in obj)
            {
                if (item == null)
                {
                    throw new ArgumentNullException(string.Format(tip, obj.ToString()));
                }
                if (item is string && string.IsNullOrEmpty(item.ToString()))
                {
                    throw new StringArgumentEmptyException(string.Format(tip, obj.ToString()));
                }
            }
        }

        /// <summary>
        /// 表单校验
        /// </summary>
        /// <param name="tip">提示信息。当func委托返回true时生效。</param>
        /// <param name="func">指定用于校验参数是否合法的委托。</param>
        public static void FormDataValidate(string tip, Func<bool> func)
        {
            if (func())
            {
                throw new IllegalFormDataException(tip);
            }
        }
    }
}
