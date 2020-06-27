using DeskRedis.Control.CustomControl;
using DeskRedis.Enums;
using DeskRedis.Model;
using DeskRedis.Model.Configs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DeskRedis
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : NormalWindow
    {
        #region 私有变量
        /// <summary>
        /// 当前选中的节点。
        /// </summary>
        private TreeViewItemWithOperator currentSelectedTreeViewItem;
        #endregion


        #region 构造方法
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.IsShowHelp = false;
            this.IsMaxWindow = false;
            this.Loaded += MainWindow_Loaded;
        }
        #endregion


        #region 私有方法
        /// <summary>
        /// 初始化连接信息树。
        /// </summary>
        private void InitTree()
        {
            List<ConnectionConfig> configs = GlobalBusiness.GetAllConnectionConfig();
            foreach (ConnectionConfig config in configs)
            {
                TreeView tree = this.CreateRootNode(config);
                this.gridRedisList.Children.Add(tree);
            }
        }

        /// <summary>
        /// 创建根节点。
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private TreeView CreateRootNode(ConnectionConfig config)
        {
            TreeView tree = new TreeView();
            tree.Tag = config.Id;
            tree.Margin = new Thickness(2);
            tree.FontSize = 14;
            tree.FontFamily = new FontFamily("courier new;microsoft yahei ui;宋体");

            TreeViewItemWithOperator root = new TreeViewItemWithOperator();
            NodeInfo nodeInfo = new NodeInfo() { NodeType = NodeType.Connection, ConfigId = config.Id, Header = config.Name };
            root.Name = "root_" + config.Id;
            root.Header = config.Name;
            root.NodeInfo = nodeInfo;
            root.Margin = new Thickness(0, 2, 0, 2);
            root.FontWeight = FontWeights.Bold;
            root.ClearVisibility = false;
            root.DeleteVisibility = false;
            root.MouseUp += Node_MouseUp;
            root.MouseDoubleClick += Root_MouseDoubleClick;
            root.OnClear += TreeViewItem_Clear;
            root.OnRefresh += TreeViewItem_Refresh;
            root.ContextMenu = this.CreateRootContextMenu(nodeInfo);
            tree.Items.Add(root);
            return tree;
        }

        /// <summary>
        /// 创建DB列表节点
        /// </summary>
        /// <param name="parent"></param>
        private void CreateDBNode(TreeViewItemWithOperator parent)
        {
            NodeInfo nodeInfo = parent.NodeInfo;
            ConnectionConfig config = GlobalBusiness.DictConnectionConfig[nodeInfo.ConfigId];
            Task.Run(() =>
            {
                this.Dispatcher.Invoke(() => { this.gridLoading.Visibility = Visibility.Visible; });
                this.SetLog(this.tbLog, $"正在打开连接 {config.Name}({config.IP}:{config.Port}) ...");
                string result = GlobalBusiness.RedisCaches[config.Id].ConnectTest();
                if ("SUCCESS".Equals(result))
                {
                    int count = GlobalBusiness.RedisCaches[config.Id].GetDataBaseCount();
                    for (int i = 0; i < count; i++)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            TreeViewItemWithOperator item = new TreeViewItemWithOperator();
                            NodeInfo info = new NodeInfo() { NodeType = NodeType.DataBase, ConfigId = config.Id, DbIndex = i, Header = "db" + i };
                            item.Name = parent.Name + "_" + i;
                            item.Header = "db" + i;
                            item.NodeInfo = info;
                            item.Margin = new Thickness(0, 2, 0, 2);
                            item.FontWeight = FontWeights.Normal;
                            item.DeleteVisibility = false;
                            item.Foreground = new SolidColorBrush(Colors.DarkBlue);
                            item.ContextMenu = this.CreateDBContextMenu(info);
                            item.MouseDoubleClick += DB_MouseDoubleClick;
                            item.OnClear += TreeViewItem_Clear;
                            item.OnRefresh += TreeViewItem_Refresh;
                            parent.Items.Add(item);
                        });
                    }
                    this.SetLog(this.tbLog, $"成功打开连接 {config.Name}({config.IP}:{config.Port})。");
                }
                else
                {
                    MessageBox.Show(result);
                    this.SetLog(this.tbLog, $"打开连接 {config.Name}({config.IP}:{config.Port})失败。");
                }
                this.Dispatcher.Invoke(() => { this.gridLoading.Visibility = Visibility.Collapsed; });
            });
        }

        /// <summary>
        /// 创建键节点
        /// </summary>
        /// <param name="parent"></param>
        private void CreateKeyNode(TreeViewItemWithOperator parent, List<string> keys)
        {
            NodeInfo nodeInfo = parent.NodeInfo;
            List<KeyInfo> keyInfos = this.ParseKeys(keys);

            foreach (KeyInfo info in keyInfos)
            {
                if (info.IsKey)//键节点
                {
                    TreeViewItemWithOperator keyItem = new TreeViewItemWithOperator();
                    NodeInfo keyNodeInfo = new NodeInfo() { NodeType = NodeType.Key, ConfigId = nodeInfo.ConfigId, Header = info.Header, DbIndex = nodeInfo.DbIndex, Key = info.Key };
                    keyItem.Header = info.Header;
                    keyItem.NodeInfo = keyNodeInfo;
                    keyItem.Margin = new Thickness(0, 2, 0, 2);
                    keyItem.FontWeight = FontWeights.Normal;
                    keyItem.ClearVisibility = false;
                    keyItem.ContextMenu = this.CreateKeyContextMenu(keyNodeInfo);
                    keyItem.MouseLeftButtonUp += KeyItem_MouseLeftButtonUp;
                    keyItem.OnDelete += TreeViewItem_Delete;
                    keyItem.OnRefresh += TreeViewItem_Refresh;
                    parent.Items.Add(keyItem);
                }
                else//键文件夹节点
                {
                    TreeViewItemWithOperator item = new TreeViewItemWithOperator();
                    NodeInfo itemNodeInfo = new NodeInfo() { NodeType = NodeType.Folder, ConfigId = nodeInfo.ConfigId, Header = info.Header, DbIndex = nodeInfo.DbIndex, Key = info.Header };
                    item.Header = info.Header;
                    item.NodeInfo = itemNodeInfo;
                    item.Margin = new Thickness(0, 2, 0, 2);
                    item.ContextMenu?.Items.Clear();
                    item.FontWeight = FontWeights.Normal;
                    item.Foreground = new SolidColorBrush(Colors.Orange);
                    item.DeleteVisibility = false;
                    item.OnRefresh += TreeViewItem_Refresh;
                    item.OnClear += TreeViewItem_Clear;
                    item.ContextMenu = this.CreateKeyFolderContextMenu(itemNodeInfo);

                    if (info.Keys != null && info.Keys.Count > 0)
                    {
                        foreach (KeyInfo key in info.Keys)
                        {
                            TreeViewItemWithOperator keyItem = new TreeViewItemWithOperator();
                            NodeInfo keyNodeInfo = new NodeInfo() { NodeType = NodeType.Key, ConfigId = nodeInfo.ConfigId, Header = key.Header, DbIndex = nodeInfo.DbIndex, Key = key.Key };
                            keyItem.Header = key.Header;
                            keyItem.NodeInfo = keyNodeInfo;
                            keyItem.Margin = new Thickness(0, 2, 0, 2);
                            keyItem.FontWeight = FontWeights.Normal;
                            keyItem.ClearVisibility = false;
                            keyItem.ContextMenu = this.CreateKeyContextMenu(keyNodeInfo);
                            keyItem.MouseLeftButtonUp += KeyItem_MouseLeftButtonUp;
                            keyItem.OnRefresh += TreeViewItem_Refresh;
                            keyItem.OnDelete += TreeViewItem_Clear;
                            item.Items.Add(keyItem);
                        }
                    }
                    parent.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// 创建根节点右键菜单。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ContextMenu CreateRootContextMenu(NodeInfo nodeInfo)
        {
            ContextMenu menu = new ContextMenu();
            MenuItem itemOpen = new MenuItem();
            itemOpen.Name = "MenuItem_Root_Open_" + nodeInfo.ConfigId;
            itemOpen.Tag = new NodeInfo() { ConfigId = nodeInfo.ConfigId, DbIndex = nodeInfo.DbIndex, Header = nodeInfo.Header, Key = nodeInfo.Key, Type = MenuItemType.OPEN };
            itemOpen.Header = "打开";
            itemOpen.Click += MenuItem_Root_Click;
            menu.Items.Add(itemOpen);

            MenuItem itemRefresh = new MenuItem();
            itemRefresh.Name = "MenuItem_Root_Refresh_" + nodeInfo.ConfigId;
            itemRefresh.Tag = new NodeInfo() { ConfigId = nodeInfo.ConfigId, DbIndex = nodeInfo.DbIndex, Header = nodeInfo.Header, Key = nodeInfo.Key, Type = MenuItemType.REFRESH };
            itemRefresh.Header = "刷新";
            itemRefresh.Click += MenuItem_Root_Click;
            menu.Items.Add(itemRefresh);

            Separator separator1 = new Separator();
            menu.Items.Add(separator1);

            MenuItem itemEdit = new MenuItem();
            itemEdit.Name = "MenuItem_Root_Edit_" + nodeInfo.ConfigId;
            itemEdit.Tag = new NodeInfo() { ConfigId = nodeInfo.ConfigId, DbIndex = nodeInfo.DbIndex, Header = nodeInfo.Header, Key = nodeInfo.Key, Type = MenuItemType.EDIT };
            itemEdit.Header = "编辑";
            itemEdit.Click += MenuItem_Root_Click;
            menu.Items.Add(itemEdit);

            MenuItem itemDelete = new MenuItem();
            itemDelete.Name = "MenuItem_Root_Delete_" + nodeInfo.ConfigId;
            itemDelete.Tag = new NodeInfo() { ConfigId = nodeInfo.ConfigId, DbIndex = nodeInfo.DbIndex, Header = nodeInfo.Header, Key = nodeInfo.Key, Type = MenuItemType.DELETE };
            itemDelete.Header = "删除";
            itemDelete.Click += MenuItem_Root_Click;
            menu.Items.Add(itemDelete);

            Separator separator2 = new Separator();
            menu.Items.Add(separator2);

            MenuItem itemClose = new MenuItem();
            itemClose.Name = "MenuItem_Root_Close_" + nodeInfo.ConfigId;
            itemClose.Tag = new NodeInfo() { ConfigId = nodeInfo.ConfigId, DbIndex = nodeInfo.DbIndex, Header = nodeInfo.Header, Key = nodeInfo.Key, Type = MenuItemType.CLOSE };
            itemClose.Header = "关闭";
            itemClose.Click += MenuItem_Root_Click;
            menu.Items.Add(itemClose);

            return menu;
        }

        /// <summary>
        /// 创建db（二级列表）的右键菜单。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ContextMenu CreateDBContextMenu(NodeInfo nodeInfo)
        {
            ContextMenu menu = new ContextMenu();
            MenuItem itemOpen = new MenuItem();
            itemOpen.Name = "MenuItem_DB_Open_" + nodeInfo.ConfigId + "_" + nodeInfo.DbIndex;
            itemOpen.Tag = new NodeInfo() { ConfigId = nodeInfo.ConfigId, DbIndex = nodeInfo.DbIndex, Header = nodeInfo.Header, Key = nodeInfo.Key, Type = MenuItemType.OPEN };
            itemOpen.Header = "打开";
            itemOpen.Click += MenuItem_DB_Click;
            menu.Items.Add(itemOpen);

            MenuItem itemRefresh = new MenuItem();
            itemRefresh.Name = "MenuItem_DB_Refresh_" + nodeInfo.ConfigId + "_" + nodeInfo.DbIndex;
            itemRefresh.Tag = new NodeInfo() { ConfigId = nodeInfo.ConfigId, DbIndex = nodeInfo.DbIndex, Header = nodeInfo.Header, Key = nodeInfo.Key, Type = MenuItemType.REFRESH };
            itemRefresh.Header = "刷新";
            itemRefresh.Click += MenuItem_DB_Click;
            menu.Items.Add(itemRefresh);

            MenuItem itemSearch = new MenuItem();
            itemSearch.Name = "MenuItem_DB_Search_" + nodeInfo.ConfigId + "_" + nodeInfo.DbIndex;
            itemSearch.Tag = new NodeInfo() { ConfigId = nodeInfo.ConfigId, DbIndex = nodeInfo.DbIndex, Header = nodeInfo.Header, Key = nodeInfo.Key, Type = MenuItemType.SEARCH };
            itemSearch.Header = "搜索";
            itemSearch.Click += MenuItem_DB_Click;
            menu.Items.Add(itemSearch);

            Separator separator = new Separator();
            menu.Items.Add(separator);

            MenuItem itemClose = new MenuItem();
            itemClose.Name = "MenuItem_DB_Close_" + nodeInfo.ConfigId + "_" + nodeInfo.DbIndex;
            itemClose.Tag = new NodeInfo() { ConfigId = nodeInfo.ConfigId, DbIndex = nodeInfo.DbIndex, Header = nodeInfo.Header, Key = nodeInfo.Key, Type = MenuItemType.CLOSE };
            itemClose.Header = "关闭";
            itemClose.Click += MenuItem_DB_Click;
            menu.Items.Add(itemClose);

            separator = new Separator();
            menu.Items.Add(separator);

            MenuItem itemDelete = new MenuItem();
            itemDelete.Name = "MenuItem_DB_Flush_" + nodeInfo.ConfigId + "_" + nodeInfo.DbIndex;
            itemDelete.Tag = new NodeInfo() { ConfigId = nodeInfo.ConfigId, DbIndex = nodeInfo.DbIndex, Header = nodeInfo.Header, Key = nodeInfo.Key, Type = MenuItemType.FLUSH };
            itemDelete.Header = "清空";
            itemDelete.Click += MenuItem_DB_Click;
            menu.Items.Add(itemDelete);

            return menu;
        }

        /// <summary>
        /// 创建键文件夹的右键菜单。
        /// </summary>
        /// <param name="nodeInfo"></param>
        /// <returns></returns>
        private ContextMenu CreateKeyFolderContextMenu(NodeInfo nodeInfo)
        {
            ContextMenu menu = new ContextMenu();
            MenuItem itemOpen = new MenuItem();
            itemOpen.Name = "MenuItem_Key_Folder_Delete_" + nodeInfo.ConfigId;
            itemOpen.Tag = new NodeInfo() { ConfigId = nodeInfo.ConfigId, DbIndex = nodeInfo.DbIndex, Header = nodeInfo.Header, Key = nodeInfo.Key, Type = MenuItemType.DELETE };
            itemOpen.Header = "删除";
            itemOpen.Click += MenuItem_KeyFolder_Click;
            menu.Items.Add(itemOpen);

            return menu;
        }

        /// <summary>
        /// 创建键的右键菜单。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ContextMenu CreateKeyContextMenu(NodeInfo nodeInfo)
        {
            ContextMenu menu = new ContextMenu();
            MenuItem itemOpen = new MenuItem();
            itemOpen.Name = "MenuItem_Key_Delete_" + nodeInfo.ConfigId;
            itemOpen.Tag = new NodeInfo() { ConfigId = nodeInfo.ConfigId, DbIndex = nodeInfo.DbIndex, Header = nodeInfo.Header, Key = nodeInfo.Key, Type = MenuItemType.DELETE };
            itemOpen.Header = "删除";
            itemOpen.Click += MenuItem_Key_Click;
            menu.Items.Add(itemOpen);

            return menu;
        }

        /// <summary>
        /// 打开数据库节点。
        /// </summary>
        /// <param name="nodeInfo"></param>
        /// <param name="item">子节点</param>
        private void OpenDB(NodeInfo nodeInfo, TreeViewItemWithOperator item)
        {
            Task.Run(() =>
            {
                this.Dispatcher.Invoke(() => { this.gridLoading.Visibility = Visibility.Visible; });
                this.SetLog(this.tbLog, $"正在打开 {nodeInfo.Header} 数据库...");
                List<string> keys = GlobalBusiness.RedisCaches[nodeInfo.ConfigId].GetAllKeys(nodeInfo.DbIndex);
                if (keys == null || keys.Count <= 0)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.gridLoading.Visibility = Visibility.Collapsed;
                        MessageBox.Show("该库没有任何数据！");
                    });
                    return;
                }
                this.Dispatcher.Invoke(() => { this.CreateKeyNode(item, keys); });
                this.SetLog(this.tbLog, $"完成打开 {nodeInfo.Header} 数据库。");
                this.Dispatcher.Invoke(() => { this.gridLoading.Visibility = Visibility.Collapsed; });
            });
        }

        /// <summary>
        /// 解析键信息。
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        private List<KeyInfo> ParseKeys(List<string> keys)
        {
            List<KeyInfo> keyInfos = new List<KeyInfo>();
            foreach (string key in keys)
            {
                if (key.Contains(":"))
                {
                    string[] strs = key.Split(':');
                    if (strs.Length > 1)
                    {
                        KeyInfo info = keyInfos.Find(p => p.Header.Equals(strs[0]));
                        if (info == null)
                        {
                            info = new KeyInfo()
                            {
                                IsKey = false,
                                Header = strs[0],
                                Keys = new List<KeyInfo>()
                            };
                            keyInfos.Add(info);
                        }
                        info.Keys.Add(new KeyInfo() { IsKey = true, Key = key, Header = key });
                    }
                }
                else
                {
                    keyInfos.Add(new KeyInfo() { IsKey = true, Key = key, Header = key });
                }
            }
            return keyInfos;
        }

        /// <summary>
        /// 设置日志内容。
        /// </summary>
        /// <param name="log"></param>
        private void SetLog(TextBox tb, string log)
        {
            tb.Dispatcher.Invoke(() =>
            {
                tb.AppendText("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]    " + log + Environment.NewLine);
            });
        }

        /// <summary>
        /// 删除指定的键。
        /// </summary>
        /// <param name="configId">配置id</param>
        /// <param name="dbIndex">redis的数据库索引</param>
        /// <param name="key">键</param>
        private void DeleteKey(string configId, int dbIndex, string key)
        {
            if (MessageBox.Show($"确定要删除 {key} 吗？", "删除提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (GlobalBusiness.RedisCaches[configId].Remove(key, dbIndex))
                {
                    TreeViewItemWithOperator parent = this.currentSelectedTreeViewItem.Parent as TreeViewItemWithOperator;
                    parent.Items.Remove(this.currentSelectedTreeViewItem);

                    this.tbKey.Clear();
                    this.tbValue.Clear();
                }
            }
        }

        /// <summary>
        /// 读取值。
        /// </summary>
        /// <param name="nodeInfo"></param>
        private void ReadValue(NodeInfo nodeInfo)
        {
            Task.Run(() =>
            {
                try
                {
                    this.SetLog(this.tbLog, $"正在读取 {nodeInfo.Key} 的值...");
                    this.Dispatcher.Invoke(() =>
                    {
                        this.tbValue.Clear();
                        this.gridLoading.Visibility = Visibility.Visible;
                        this.tbKey.Text = nodeInfo.Key;
                    });
                    RedisValue redisValue = GlobalBusiness.RedisCaches[nodeInfo.ConfigId].Get(nodeInfo.Key, nodeInfo.DbIndex);
                    this.Dispatcher.Invoke(() =>
                    {
                        this.tblockTTL.Text = redisValue.TTL.ToString();
                        this.tbValue.Text = redisValue.Value;
                    });
                    this.Dispatcher.Invoke(() => { this.gridLoading.Visibility = Visibility.Collapsed; });
                    this.SetLog(this.tbLog, $"完成读取 {nodeInfo.Key} 的值。");
                }
                catch (Exception e)
                {
                    this.Dispatcher.Invoke(() => { this.gridLoading.Visibility = Visibility.Collapsed; });
                    this.SetLog(this.tbLog, $"完成读取 {nodeInfo.Key} 的值。（附加信息：{nodeInfo.Key} 的值不是合法的。{e.Message}）");
                }
            });
        }

        /// <summary>
        /// 刷新数据库列表
        /// </summary>
        private void RefreshRoot()
        {
            this.currentSelectedTreeViewItem.Items.Clear();
            this.CreateDBNode(this.currentSelectedTreeViewItem);
            this.currentSelectedTreeViewItem.IsExpanded = true;
        }

        /// <summary>
        /// 刷新数据库。
        /// </summary>
        /// <param name="nodeInfo"></param>
        private void RefreshDB(NodeInfo nodeInfo)
        {
            Task.Run(() =>
            {
                this.Dispatcher.Invoke(() => { this.gridLoading.Visibility = Visibility.Visible; });
                List<string> keys = GlobalBusiness.RedisCaches[nodeInfo.ConfigId].GetAllKeys(nodeInfo.DbIndex);
                if (keys == null || keys.Count <= 0)
                {
                    this.Dispatcher.Invoke(() => { this.gridLoading.Visibility = Visibility.Collapsed; });
                    MessageBox.Show("该库没有任何数据！");
                    return;
                }
                this.Dispatcher.Invoke(() => { this.CreateKeyNode(this.currentSelectedTreeViewItem, keys); });
                this.Dispatcher.Invoke(() =>
                {
                    this.currentSelectedTreeViewItem.IsExpanded = true;
                    this.gridLoading.Visibility = Visibility.Collapsed;
                });
            });
        }

        /// <summary>
        /// 刷新键/键文件夹节点
        /// </summary>
        /// <param name="nodeInfo"></param>
        private void RefreshKey(NodeInfo nodeInfo)
        {
            this.gridLoading.Visibility = Visibility.Visible;
            this.currentSelectedTreeViewItem.Items.Clear();
            List<string> keys = GlobalBusiness.RedisCaches[nodeInfo.ConfigId].GetAllKeys(nodeInfo.DbIndex);
            this.CreateKeyNode(this.currentSelectedTreeViewItem, keys);
            this.currentSelectedTreeViewItem.IsExpanded = true;
            this.gridLoading.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 清空数据库。
        /// </summary>
        /// <param name="nodeInfo"></param>
        private void FlushDB(NodeInfo nodeInfo)
        {
            Task.Run(() =>
            {
                this.Dispatcher.Invoke(() => { this.gridLoading.Visibility = Visibility.Visible; });
                GlobalBusiness.RedisCaches[nodeInfo.ConfigId].FlushDb(nodeInfo.DbIndex);
                this.Dispatcher.Invoke(() => { this.gridLoading.Visibility = Visibility.Collapsed; });
            });
        }

        /// <summary>
        /// 清空键文件夹。
        /// </summary>
        /// <param name="nodeInfo"></param>
        private void FlushKeyFolder(NodeInfo nodeInfo)
        {
            if (MessageBox.Show($"确定要删除 {nodeInfo.Key} 下所有的键吗？", "删除提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.SetLog(this.tbLog, $"正在批量删除 {nodeInfo.Key} 下所有的键...");
                //删除下级所有的键
                IList<string> keys = new List<string>();
                foreach (TreeViewItemWithOperator item in this.currentSelectedTreeViewItem.Items)
                {
                    keys.Add(item.NodeInfo.Key);
                }

                GlobalBusiness.RedisCaches[nodeInfo.ConfigId].RemoveAll(keys, nodeInfo.DbIndex);
                TreeViewItemWithOperator parent = this.currentSelectedTreeViewItem.Parent as TreeViewItemWithOperator;
                parent.Items.Remove(this.currentSelectedTreeViewItem);
            }
        }
        #endregion


        #region 委托方法
        /// <summary>
        /// 当连接信息保存完毕时发生。
        /// </summary>
        /// <param name="config"></param>
        private void WinAddConnection_SavedConnectionConfig(ConnectionConfig config, ConfigOperationType crudType)
        {
            if (crudType == ConfigOperationType.ADD)
            {
                TreeView tree = this.CreateRootNode(config);
                this.gridRedisList.Children.Add(tree);
            }
            else if (crudType == ConfigOperationType.EDIT)
            {
                this.currentSelectedTreeViewItem.Items.Clear();
                this.currentSelectedTreeViewItem.NodeInfo = new NodeInfo() { ConfigId = config.Id, Header = config.Name };
                this.currentSelectedTreeViewItem.Header = config.Name;
            }
        }

        /// <summary>
        /// 当键重命名完成时发生。
        /// </summary>
        /// <param name="obj"></param>
        private void WinRenameKey_OnUpdatedKey(string newKey)
        {
            this.currentSelectedTreeViewItem.Header = newKey;
            NodeInfo nodeInfo = this.currentSelectedTreeViewItem.NodeInfo;
            nodeInfo.Header = newKey;
            nodeInfo.Key = newKey;
        }

        /// <summary>
        /// 当其它窗口业务出现错误是发生。
        /// </summary>
        /// <param name="message"></param>
        private void Win_OnError(string message)
        {
            this.SetLog(this.tbLog, message);
        }
        #endregion


        #region 本地事件
        /// <summary>
        /// 当窗口加载完成时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.InitTree();
        }

        /// <summary>
        /// 当时鼠标在标题栏按下时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// 当时鼠标点击“刷新”按钮时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            this.gridRedisList.Children.Clear();
            this.InitTree();
        }

        /// <summary>
        /// 当鼠标在点按下并释放时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Node_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.currentSelectedTreeViewItem = e.Source as TreeViewItemWithOperator;
            this.currentSelectedTreeViewItem.IsSelected = true;
        }

        /// <summary>
        /// 当鼠标点击树形列表右键菜单的选项时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Root_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            NodeInfo nodeInfo = item.Tag as NodeInfo;
            ConnectionConfig config = GlobalBusiness.DictConnectionConfig[nodeInfo.ConfigId];

            if (MenuItemType.OPEN.Equals(nodeInfo.Type))
            {
                if (this.currentSelectedTreeViewItem.HasItems)
                {
                    return;
                }
                this.CreateDBNode(this.currentSelectedTreeViewItem);
                this.currentSelectedTreeViewItem.IsExpanded = true;

                return;
            }
            if (MenuItemType.REFRESH.Equals(nodeInfo.Type))
            {
                this.RefreshRoot();

                return;
            }
            if (MenuItemType.EDIT.Equals(nodeInfo.Type))
            {
                if (this.currentSelectedTreeViewItem.HasItems
                    && MessageBox.Show("该操作将关闭连接！是否继续？", "操作提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    this.currentSelectedTreeViewItem.Items.Clear();
                }

                WinAddConnection winAddConnection = new WinAddConnection(nodeInfo.ConfigId, ConfigOperationType.EDIT);
                winAddConnection.SavedConnectionConfig += this.WinAddConnection_SavedConnectionConfig;
                winAddConnection.ShowDialog();
                winAddConnection.SavedConnectionConfig -= this.WinAddConnection_SavedConnectionConfig;

                return;
            }
            if (MenuItemType.DELETE.Equals(nodeInfo.Type))
            {
                if (MessageBox.Show($"确定删除{config.Name}连接配置吗？", "删除提示",
                    MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    TreeView tree = this.currentSelectedTreeViewItem.Parent as TreeView;
                    this.gridRedisList.Children.Remove(tree);
                    GlobalBusiness.RemoveConfig(config.Id);
                }

                return;
            }
            if (MenuItemType.CLOSE.Equals(nodeInfo.Type))
            {
                this.currentSelectedTreeViewItem.Items.Clear();
                this.currentSelectedTreeViewItem.IsExpanded = false;

                return;
            }
        }

        /// <summary>
        /// 当鼠标点击树形列表右键菜单的选项时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_DB_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            NodeInfo nodeInfo = item.Tag as NodeInfo;

            if (MenuItemType.OPEN.Equals(nodeInfo.Type))
            {
                if (this.currentSelectedTreeViewItem.HasItems)
                {
                    return;
                }

                this.RefreshDB(nodeInfo);
            }
            if (MenuItemType.REFRESH.Equals(nodeInfo.Type))
            {
                this.RefreshKey(nodeInfo);
            }
            if (MenuItemType.SEARCH.Equals(nodeInfo.Type))
            {
                WinSearchKey winSearchKey = new WinSearchKey(nodeInfo.ConfigId, nodeInfo.DbIndex);
                winSearchKey.OnError += this.Win_OnError;
                winSearchKey.ShowDialog();
                winSearchKey.OnError -= this.Win_OnError;
            }
            if (MenuItemType.CLOSE.Equals(nodeInfo.Type))
            {

            }
            if (MenuItemType.FLUSH.Equals(nodeInfo.Type))
            {
                this.FlushDB(nodeInfo);
            }
        }

        /// <summary>
        /// 当鼠标点击树形列表右键菜单的选项时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_KeyFolder_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            NodeInfo nodeInfo = menuItem.Tag as NodeInfo;

            try
            {
                this.FlushKeyFolder(nodeInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.SetLog(this.tbLog, $"完成批量删除 {nodeInfo.Key} 下所有的键。");
            }
        }

        /// <summary>
        /// 当鼠标点击树形列表右键菜单的选项时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Key_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            NodeInfo nodeInfo = item.Tag as NodeInfo;

            this.DeleteKey(nodeInfo.ConfigId, nodeInfo.DbIndex, nodeInfo.Key);
        }

        /// <summary>
        /// 当鼠标双击根节点时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Root_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TreeViewItemWithOperator item = sender as TreeViewItemWithOperator;
                if (item.HasItems)
                {
                    return;
                }
                this.CreateDBNode(item);
            }
            catch (Exception ex)
            {
                this.SetLog(this.tbLog, $"打开连接错误：{ex.Message}");
            }
        }

        /// <summary>
        /// 当鼠标双击DB节点时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItemWithOperator item = sender as TreeViewItemWithOperator;
            if (item.HasItems)
            {
                return;
            }

            this.OpenDB(item.NodeInfo, item);
        }

        /// <summary>
        /// 当鼠标在键的菜单项点下并释放按钮后发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TreeViewItemWithOperator item = sender as TreeViewItemWithOperator;
            NodeInfo nodeInfo = item.NodeInfo;

            if (nodeInfo == null)
            {
                return;
            }
            if (string.Compare(nodeInfo.Key, this.tbKey.Text) == 0)
            {
                return;
            }

            this.ReadValue(nodeInfo);
        }


        /// <summary>
        /// 当鼠标点击菜单项的清空按钮时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Clear(object sender, RoutedEventArgs e)
        {
            NodeInfo nodeInfo = e.Source as NodeInfo;
            switch (nodeInfo.NodeType)
            {
                case NodeType.DataBase:
                    this.FlushDB(nodeInfo);
                    break;
                case NodeType.Folder:
                    this.FlushKeyFolder(nodeInfo);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 当鼠标点击菜单项的删除按钮时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Delete(object sender, RoutedEventArgs e)
        {
            NodeInfo nodeInfo = e.Source as NodeInfo;
            this.DeleteKey(nodeInfo.ConfigId, nodeInfo.DbIndex, nodeInfo.Key);
        }

        /// <summary>
        /// 当鼠标点击菜单项的刷新按钮时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Refresh(object sender, RoutedEventArgs e)
        {
            NodeInfo nodeInfo = e.Source as NodeInfo;
            switch (nodeInfo.NodeType)
            {
                case NodeType.Connection:
                    this.RefreshRoot();
                    break;
                case NodeType.DataBase:
                    this.RefreshKey(nodeInfo);
                    break;
                case NodeType.Folder:

                    break;
                case NodeType.Key:
                    this.ReadValue(nodeInfo);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 当鼠标点击添加按钮时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            WinAddConnection winAddConnection = new WinAddConnection(ConfigOperationType.ADD);
            winAddConnection.SavedConnectionConfig += this.WinAddConnection_SavedConnectionConfig;
            winAddConnection.ShowDialog();
            winAddConnection.SavedConnectionConfig -= this.WinAddConnection_SavedConnectionConfig;
        }

        /// <summary>
        /// 当日志文本框内容变化时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbLog_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.tbLog.ScrollToEnd();
        }

        /// <summary>
        /// 当鼠标点击重命名按钮时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRenameKey_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.tbKey.Text.Trim()))
            {
                return;
            }

            NodeInfo nodeInfo = this.currentSelectedTreeViewItem.NodeInfo;
            WinRenameKey winRenameKey = new WinRenameKey(nodeInfo.ConfigId, nodeInfo.DbIndex, nodeInfo.Key);
            winRenameKey.OnUpdatedKey += WinRenameKey_OnUpdatedKey;
            winRenameKey.ShowDialog();
            winRenameKey.OnUpdatedKey -= WinRenameKey_OnUpdatedKey;
        }

        /// <summary>
        /// 当鼠标点击删除按钮时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteKey_Click(object sender, RoutedEventArgs e)
        {
            NodeInfo nodeInfo = this.currentSelectedTreeViewItem.NodeInfo;
            this.DeleteKey(nodeInfo.ConfigId, nodeInfo.DbIndex, nodeInfo.Key);
        }

        /// <summary>
        /// 当鼠标点击刷新按钮时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            NodeInfo nodeInfo = this.currentSelectedTreeViewItem.NodeInfo;
            this.ReadValue(nodeInfo);
        }
        #endregion
    }
}
