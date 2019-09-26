using DeskRedis.Exceptions;
using DeskRedis.Model;
using DeskRedis.Util;
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
    /// WinSearchKey.xaml 的交互逻辑
    /// </summary>
    public partial class WinSearchKey : Window
    {
        #region 私有变量
        /// <summary>
        /// 配置id
        /// </summary>
        private readonly string configId;

        /// <summary>
        /// 数据库索引
        /// </summary>
        private readonly int dbIndex;
        #endregion


        #region 委托、事件
        public event Action<string> OnError;
        #endregion


        #region 构造方法
        public WinSearchKey()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 库索引
        /// </summary>
        /// <param name="configId">redis配置id</param>
        /// <param name="dbIndex">数据库索引</param>
        public WinSearchKey(string configId, int dbIndex = 0)
        {
            InitializeComponent();

            this.configId = configId;
            this.dbIndex = dbIndex;
        }
        #endregion


        #region 本地事件
        /// <summary>
        /// 当鼠标点击查询按钮时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AssertUtil.FormDataValidate("请输入键", () => { return !string.IsNullOrEmpty(this.tbKey.Text.Trim()); });
                string key = this.tbKey.Text.Trim();

                RedisValue redisValue = GlobalBusiness.RedisCaches[this.configId].Get(key, this.dbIndex);
                this.tbValue.Text = redisValue.Value;
            }
            catch (IllegalFormDataException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (RedisException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                this.OnError?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// 当鼠标点击关闭按钮时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
