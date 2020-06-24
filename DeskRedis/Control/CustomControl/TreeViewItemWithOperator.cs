using DeskRedis.Model;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Controls;

namespace DeskRedis.Control.CustomControl
{
    /// <summary>
    /// 包含自定义操作功能的TreeViewItem。
    /// </summary>
    public class TreeViewItemWithOperator : TreeViewItem
    {
        static TreeViewItemWithOperator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeViewItemWithOperator),
                new FrameworkPropertyMetadata(typeof(TreeViewItemWithOperator)));
        }


        public static readonly DependencyProperty ClearEnabledProperty =
            DependencyProperty.Register("ClearEnabled", typeof(bool), typeof(TreeViewItemWithOperator), new PropertyMetadata(true));

        public static readonly DependencyProperty ClearVisibilityProperty =
            DependencyProperty.Register("ClearVisibility", typeof(bool), typeof(TreeViewItemWithOperator), new PropertyMetadata(true));

        public static readonly DependencyProperty RereshEnabledProperty =
            DependencyProperty.Register("RereshEnabled", typeof(bool), typeof(TreeViewItemWithOperator), new PropertyMetadata(true));

        public static readonly DependencyProperty RereshVisibilityProperty =
            DependencyProperty.Register("RereshVisibility", typeof(bool), typeof(TreeViewItemWithOperator), new PropertyMetadata(true));


        /// <summary>
        /// 启用/禁用清空按钮。
        /// </summary>
        public bool ClearEnabled
        {
            get { return (bool)GetValue(ClearEnabledProperty); }
            set
            {
                SetValue(ClearEnabledProperty, value);
                if (this.btnClear != null)
                {
                    this.btnClear.IsEnabled = value;
                }
            }
        }

        /// <summary>
        /// 显示/隐藏清空按钮。
        /// </summary>
        public bool ClearVisibility
        {
            get { return (bool)GetValue(ClearVisibilityProperty); }
            set
            {
                SetValue(ClearVisibilityProperty, value);
                if (this.btnClear != null)
                {
                    this.btnClear.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// 启用/禁用刷新按钮。
        /// </summary>
        public bool RefreshEnabled
        {
            get { return (bool)GetValue(RereshEnabledProperty); }
            set
            {
                SetValue(RereshEnabledProperty, value);
                if (this.btnRefresh != null)
                {
                    this.btnRefresh.IsEnabled = value;
                }
            }
        }

        /// <summary>
        /// 显示/隐藏刷新按钮。
        /// </summary>
        public bool RefreshVisibility
        {
            get { return (bool)GetValue(RereshVisibilityProperty); }
            set
            {
                SetValue(RereshVisibilityProperty, value);
                if (this.btnRefresh != null)
                {
                    this.btnRefresh.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }



        #region 公共属性
        /// <summary>
        /// 节点信息。
        /// </summary>
        public NodeInfo NodeInfo { get; set; }
        #endregion



        #region 本地组件
        private Button btnRefresh;
        private Button btnClear;
        #endregion


        #region 路由事件
        /// <summary>
        /// 当点击“清空”鼠标按钮时发生。
        /// </summary>
        public event RoutedEventHandler OnClear;
        /// <summary>
        /// 当点击“刷新”鼠标按钮时发生。
        /// </summary>
        public event RoutedEventHandler OnRefresh;
        #endregion


        #region 重写方法
        public override void BeginInit()
        {
            base.BeginInit();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.btnRefresh = this.GetTemplateChild("btnRefresh") as Button;
            this.btnRefresh.Visibility = this.RefreshVisibility ? Visibility.Visible : Visibility.Collapsed;
            this.btnRefresh.IsEnabled = this.RefreshEnabled;
            this.btnClear = this.GetTemplateChild("btnClear") as Button;
            this.btnClear.Visibility = this.ClearVisibility ? Visibility.Visible : Visibility.Collapsed;
            this.btnClear.IsEnabled = this.ClearEnabled;

            this.btnRefresh.Click += (s, e) => { this.OnRefresh?.Invoke(s, new RoutedEventArgs(e.RoutedEvent, this.NodeInfo)); };
            this.btnRefresh.MouseDoubleClick += (s, e) => { return; };
            this.btnClear.Click += (s, e) => { this.OnClear?.Invoke(s, new RoutedEventArgs(e.RoutedEvent, this.NodeInfo)); };
            this.btnClear.MouseDoubleClick += (s, e) => { return; };
        }
        #endregion
    }
}
