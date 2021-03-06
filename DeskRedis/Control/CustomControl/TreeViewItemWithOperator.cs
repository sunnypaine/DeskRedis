﻿using DeskRedis.Model;
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
            DependencyProperty.Register("ClearEnabled", typeof(bool), typeof(TreeViewItemWithOperator), new PropertyMetadata(false));

        public static readonly DependencyProperty ClearVisibilityProperty =
            DependencyProperty.Register("ClearVisibility", typeof(bool), typeof(TreeViewItemWithOperator), new PropertyMetadata(false));

        public static readonly DependencyProperty RereshEnabledProperty =
            DependencyProperty.Register("RereshEnabled", typeof(bool), typeof(TreeViewItemWithOperator), new PropertyMetadata(false));

        public static readonly DependencyProperty RereshVisibilityProperty =
            DependencyProperty.Register("RereshVisibility", typeof(bool), typeof(TreeViewItemWithOperator), new PropertyMetadata(false));

        public static readonly DependencyProperty DeleteEnabledProperty =
            DependencyProperty.Register("DeleteEnabled", typeof(bool), typeof(TreeViewItemWithOperator), new PropertyMetadata(false));

        public static readonly DependencyProperty DeleteVisibilityProperty =
            DependencyProperty.Register("DeleteVisibility", typeof(bool), typeof(TreeViewItemWithOperator), new PropertyMetadata(false));

        public static readonly DependencyProperty AddEnabledProperty =
            DependencyProperty.Register("AddEnabled", typeof(bool), typeof(TreeViewItemWithOperator), new PropertyMetadata(false));

        public static readonly DependencyProperty AddVisibilityProperty =
            DependencyProperty.Register("AddVisibility", typeof(bool), typeof(TreeViewItemWithOperator), new PropertyMetadata(false));

        public static readonly DependencyProperty EditEnabledProperty =
            DependencyProperty.Register("EditEnabled", typeof(bool), typeof(TreeViewItemWithOperator), new PropertyMetadata(false));

        public static readonly DependencyProperty EditVisibilityProperty =
            DependencyProperty.Register("EditVisibility", typeof(bool), typeof(TreeViewItemWithOperator), new PropertyMetadata(false));



        #region 依赖属性
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

        /// <summary>
        /// 启用/禁用删除按钮。
        /// </summary>
        public bool DeleteEnabled
        {
            get { return (bool)GetValue(DeleteEnabledProperty); }
            set
            {
                SetValue(DeleteEnabledProperty, value);
                if (this.btnDelete != null)
                {
                    this.btnDelete.IsEnabled = value;
                }
            }
        }

        /// <summary>
        /// 显示/隐藏删除按钮。
        /// </summary>
        public bool DeleteVisibility
        {
            get { return (bool)GetValue(DeleteVisibilityProperty); }
            set
            {
                SetValue(DeleteVisibilityProperty, value);
                if (this.btnDelete != null)
                {
                    this.btnDelete.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// 启用/禁用添加按钮。
        /// </summary>
        public bool AddEnabled
        {
            get { return (bool)GetValue(AddEnabledProperty); }
            set
            {
                SetValue(AddEnabledProperty, value);
                if (this.btnAdd != null)
                {
                    this.btnAdd.IsEnabled = value;
                }
            }
        }

        /// <summary>
        /// 显示/隐藏添加按钮。
        /// </summary>
        public bool AddVisibility
        {
            get { return (bool)GetValue(AddVisibilityProperty); }
            set
            {
                SetValue(AddVisibilityProperty, value);
                if (this.btnAdd != null)
                {
                    this.btnAdd.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// 启用/禁用编辑按钮。
        /// </summary>
        public bool EditEnabled
        {
            get { return (bool)GetValue(EditEnabledProperty); }
            set
            {
                SetValue(EditEnabledProperty, value);
                if (this.btnEdit != null)
                {
                    this.btnEdit.IsEnabled = value;
                }
            }
        }

        /// <summary>
        /// 显示/隐藏编辑按钮。
        /// </summary>
        public bool EditVisibility
        {
            get { return (bool)GetValue(EditVisibilityProperty); }
            set
            {
                SetValue(EditVisibilityProperty, value);
                if (this.btnEdit != null)
                {
                    this.btnEdit.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }
        #endregion



        #region 公共属性
        /// <summary>
        /// 节点信息。
        /// </summary>
        public NodeInfo NodeInfo { get; set; }
        #endregion



        #region 本地组件
        private Button btnAdd;
        private Button btnEdit;
        private Button btnRefresh;
        private Button btnClear;
        private Button btnDelete;
        #endregion


        #region 路由事件
        /// <summary>
        /// 当点击“添加键值”按钮时发生。
        /// </summary>
        public event RoutedEventHandler OnAddKeyValue;
        /// <summary>
        /// 当点击“清空”按钮时发生。
        /// </summary>
        public event RoutedEventHandler OnClear;
        /// <summary>
        /// 当点击“刷新”按钮时发生。
        /// </summary>
        public event RoutedEventHandler OnRefresh;
        /// <summary>
        /// 当点击“删除”按钮时发生。
        /// </summary>
        public event RoutedEventHandler OnDelete;
        #endregion


        #region 重写方法
        public override void BeginInit()
        {
            base.BeginInit();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.btnAdd = this.GetTemplateChild("btnAdd") as Button;
            this.btnAdd.Visibility = this.AddVisibility ? Visibility.Visible : Visibility.Collapsed;
            this.btnAdd.IsEnabled = this.AddEnabled;
            this.btnEdit = this.GetTemplateChild("btnEdit") as Button;
            this.btnEdit.Visibility = this.EditVisibility ? Visibility.Visible : Visibility.Collapsed;
            this.btnEdit.IsEnabled = this.EditEnabled;
            this.btnRefresh = this.GetTemplateChild("btnRefresh") as Button;
            this.btnRefresh.Visibility = this.RefreshVisibility ? Visibility.Visible : Visibility.Collapsed;
            this.btnRefresh.IsEnabled = this.RefreshEnabled;
            this.btnDelete = this.GetTemplateChild("btnDelete") as Button;
            this.btnDelete.Visibility = this.DeleteVisibility ? Visibility.Visible : Visibility.Collapsed;
            this.btnDelete.IsEnabled = this.DeleteEnabled;
            this.btnClear = this.GetTemplateChild("btnClear") as Button;
            this.btnClear.Visibility = this.ClearVisibility ? Visibility.Visible : Visibility.Collapsed;
            this.btnClear.IsEnabled = this.ClearEnabled;

            this.btnAdd.Click += (s, e) => { this.OnAddKeyValue?.Invoke(s, new RoutedEventArgs(e.RoutedEvent, this.NodeInfo)); };
            this.btnAdd.MouseDoubleClick += (s, e) => { return; };
            this.btnRefresh.Click += (s, e) => { this.OnRefresh?.Invoke(s, new RoutedEventArgs(e.RoutedEvent, this.NodeInfo)); };
            this.btnRefresh.MouseDoubleClick += (s, e) => { return; };
            this.btnDelete.Click += (s, e) => { this.OnDelete?.Invoke(s, new RoutedEventArgs(e.RoutedEvent, this.NodeInfo)); };
            this.btnDelete.MouseDoubleClick += (s, e) => { return; };
            this.btnClear.Click += (s, e) => { this.OnClear?.Invoke(s, new RoutedEventArgs(e.RoutedEvent, this.NodeInfo)); };
            this.btnClear.MouseDoubleClick += (s, e) => { return; };
        }
        #endregion
    }
}
