using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DeskRedis.Control.CustomControl
{
    /// <summary>
    /// 提供纯图片的按钮。
    /// </summary>
    public class ImageButton : Button
    {
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton),
                new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        public static readonly DependencyProperty PathDataProperty =
           DependencyProperty.Register("PathData", typeof(GeometryGroup), typeof(ImageButton));
        /// <summary>
        /// 表示用于描述图片路径的path数据。
        /// </summary>
        public GeometryGroup PathData
        {
            get { return (GeometryGroup)GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ImageButton));
        /// <summary>
        /// 按钮的圆角度
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty ImageWidhtProperty =
            DependencyProperty.Register("ImageWidht", typeof(double), typeof(ImageButton));
        /// <summary>
        /// 图片的宽度
        /// </summary>
        public double ImageWidht
        {
            get { return (double)GetValue(ImageWidhtProperty); }
            set { SetValue(ImageWidhtProperty, value); }
        }

        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register("ImageHeight", typeof(double), typeof(ImageButton));
        /// <summary>
        /// 图片的高度
        /// </summary>
        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public static readonly DependencyProperty MouseHoverColorProperty =
            DependencyProperty.Register("MouseHoverColor", typeof(Brush), typeof(ImageButton));
        /// <summary>
        /// 鼠标悬浮在按钮可见区域时按钮的颜色
        /// </summary>
        public Brush MouseHoverColor
        {
            get { return (Brush)GetValue(MouseHoverColorProperty); }
            set { SetValue(MouseHoverColorProperty, value); }
        }

        public static readonly DependencyProperty MouseClickColorProperty =
            DependencyProperty.Register("MouseClickColor", typeof(Brush), typeof(ImageButton));
        /// <summary>
        /// 鼠标按下后按钮的颜色
        /// </summary>
        public Brush MouseClickColor
        {
            get { return (Brush)GetValue(MouseClickColorProperty); }
            set { SetValue(MouseClickColorProperty, value); }
        }
    }
}
