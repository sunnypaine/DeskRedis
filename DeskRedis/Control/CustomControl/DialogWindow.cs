using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DeskRedis.Control.CustomControl
{
    public class DialogWindow : Window
    {
        #region 公共属性
        /// <summary>
        /// 是否显示帮助按钮
        /// </summary>
        public bool IsShowHelp { get; set; }
        #endregion


        #region 构造方法
        public DialogWindow()
        {
            InitializeStyle();
            //必须窗口加载完成后再执行InitializeEvent()方法
            this.Loaded += (s, e) => { InitializeEvent(); };
        }
        #endregion


        #region 私有方法
        /// <summary>
        /// 初始化样式
        /// </summary>
        private void InitializeStyle()
        {
            this.Style = (Style)App.Current.Resources["DialogWindowStyle"];
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitializeEvent()
        {
            ControlTemplate template = (ControlTemplate)App.Current.Resources["DialogWindowTemplate"];

            if (this.IsShowHelp)
            {
                Button btnHelp = (Button)template.FindName("btnHelp", this);
                btnHelp.Visibility = Visibility.Visible;
                btnHelp.Click += (s, e) =>
                {
                    //TODO 显示帮助信息
                };
            }

            Button btnMin = (Button)template.FindName("btnMin", this);
            btnMin.Click += (s, e) => { this.WindowState = WindowState.Minimized; };

            Button btnClose = (Button)template.FindName("btnClose", this);
            btnClose.Click += (s, e) => { this.Close(); };

            UniformGrid title = (UniformGrid)template.FindName("title", this);
            title.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };
        }
        #endregion
    }
}
