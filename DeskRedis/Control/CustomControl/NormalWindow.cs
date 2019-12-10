using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DeskRedis.Control.CustomControl
{
    public class NormalWindow : Window
    {
        #region 私有变量
        private double normalTop;
        private double normalLeft;
        private double normalWidth;
        private double normalHeight;
        #endregion


        #region 公共属性
        /// <summary>
        /// 是否显示帮助按钮。
        /// </summary>
        public bool IsShowHelp { get; set; }
        /// <summary>
        /// 是否初始最大化。
        /// </summary>
        public bool IsMaxWindow { get; set; }
        #endregion


        #region 构造方法
        public NormalWindow()
        {
            InitializeStyle();
            //必须窗口加载完成后再执行InitializeEvent()方法
            this.Loaded += (s, e) =>
            {
                this.normalTop = this.Top;
                this.normalLeft = this.Left;
                this.normalWidth = this.Width;
                this.normalHeight = this.Height;

                if (this.IsMaxWindow)
                {
                    this.Top = SystemParameters.WorkArea.Top;
                    this.Left = SystemParameters.WorkArea.Left;
                    this.Width = SystemParameters.WorkArea.Width;
                    this.Height = SystemParameters.WorkArea.Height;
                }

                InitializeEvent();
            };
        }
        #endregion


        #region 私有方法
        /// <summary>
        /// 初始化样式
        /// </summary>
        private void InitializeStyle()
        {
            this.Style = (Style)App.Current.Resources["NormalWindowStyle"];
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitializeEvent()
        {
            ControlTemplate template = (ControlTemplate)App.Current.Resources["NormalWindowTemplate"];

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

            Button btnNormal = (Button)template.FindName("btnNormal", this);
            Button btnMax = (Button)template.FindName("btnMax", this);
            if (this.IsMaxWindow)
            {
                btnNormal.Visibility = Visibility.Visible;
                btnMax.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnNormal.Visibility = Visibility.Collapsed;
                btnMax.Visibility = Visibility.Visible;
            }
            btnNormal.Click += (s, e) =>
            {
                this.ChangeWindowState(btnNormal, btnMax);
            };
            btnMax.Click += (s, e) =>
            {
                this.ChangeWindowState(btnNormal, btnMax);
            };

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
            title.MouseLeftButtonDown += (s, e) =>
            {
                if (e.ClickCount == 2)
                {
                    this.ChangeWindowState(btnNormal, btnMax);
                }
            };
        }

        /// <summary>
        /// 切换窗口最大化/窗口状态。
        /// </summary>
        /// <param name="normal">常规按钮</param>
        /// <param name="max">最大化按钮</param>
        private void ChangeWindowState(Button normal, Button max)
        {
            if (this.IsMaxWindow)//如果当前是最大化，就变成常规状态
            {
                this.Top = this.normalTop;
                this.Left = this.normalLeft;
                this.Width = this.normalWidth;
                this.Height = this.normalHeight;

                normal.Visibility = Visibility.Collapsed;
                max.Visibility = Visibility.Visible;
                this.IsMaxWindow = false;
            }
            else
            {
                this.normalTop = this.Top;
                this.normalLeft = this.Left;
                this.normalWidth = this.Width;
                this.normalHeight = this.Height;

                this.Top = SystemParameters.WorkArea.Top;
                this.Left = SystemParameters.WorkArea.Left;
                this.Width = SystemParameters.WorkArea.Width;
                this.Height = SystemParameters.WorkArea.Height;

                normal.Visibility = Visibility.Visible;
                max.Visibility = Visibility.Collapsed;
                this.IsMaxWindow = true;
            }
        }
        #endregion
    }
}
