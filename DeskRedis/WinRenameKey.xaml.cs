using DeskRedis.Exceptions;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DeskRedis
{
    /// <summary>
    /// WinRenameKey.xaml 的交互逻辑
    /// </summary>
    public partial class WinRenameKey : Window
    {
        #region 私有变量
        /// <summary>
        /// 配置id。
        /// </summary>
        private readonly string configId;
        /// <summary>
        /// 数据库索引。
        /// </summary>
        private readonly int index;
        /// <summary>
        /// 键
        /// </summary>
        private readonly string key;
        #endregion


        #region 委托、事件
        /// <summary>
        /// 当重命名完成时发生。
        /// </summary>
        public event Action<string> OnUpdatedKey;
        #endregion


        #region 构造方法
        /// <summary>
        /// 使用指定的参数创建实例。
        /// </summary>
        /// <param name="configId">配置id</param>
        /// <param name="index">数据库索引</param>
        /// <param name="key">键</param>
        public WinRenameKey(string configId, int index, string key)
        {
            InitializeComponent();

            this.configId = configId;
            this.index = index;
            this.key = key;
            this.tblockKey.Text = key;
        }
        #endregion


        /// <summary>
        /// 当鼠标点击修改按钮时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.tbNewKey.Text.Trim()))
                {
                    throw new IllegalFormDataException("请输入合法的键。");
                }

                if (GlobalBusiness.RedisCaches[configId].Get(this.tbNewKey.Text.Trim(), this.index) != null)
                {
                    MessageBox.Show("已存在键。");
                    return;
                }
                GlobalBusiness.RedisCaches[configId].RenameKey(this.key, this.tbNewKey.Text.Trim(), this.index);
                this.OnUpdatedKey?.Invoke(this.tbNewKey.Text.Trim());

                this.Close();
            }
            catch (IllegalFormDataException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (RedisException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 当鼠标点击关闭按钮时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
