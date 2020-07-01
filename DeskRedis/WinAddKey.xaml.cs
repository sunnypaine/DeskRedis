using DeskRedis.Control.CustomControl;
using DeskRedis.Exceptions;
using DeskRedis.Model;
using DeskRedis.Util;
using ServiceStack;
using ServiceStack.Redis;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace DeskRedis
{
    /// <summary>
    /// WinAddKey.xaml 的交互逻辑
    /// </summary>
    public partial class WinAddKey : DialogWindow
    {
        #region 私有变量
        private readonly NodeInfo nodeInfo;
        #endregion


        #region 事件委托
        /// <summary>
        /// 当添加新项成功时发生。
        /// </summary>
        public event Action<string> OnAdded;
        public event Action<string> OnError;
        #endregion


        #region 构造方法
        public WinAddKey(NodeInfo nodeInfo)
        {
            InitializeComponent();

            this.nodeInfo = nodeInfo;
        }

        #endregion


        #region 私有方法
        #endregion


        #region 本地事件
        /// <summary>
        /// 当鼠标点击“保存”按钮时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AssertUtil.FormDataValidate("请输入键名", () => { return string.IsNullOrEmpty(this.tbKey.Text.Trim()); });
                AssertUtil.FormDataValidate("请选择类型", () =>
                {
                    return this.cbbType.SelectedItem == null || string.IsNullOrEmpty(this.cbbType.SelectedItem.ToString());
                });
                AssertUtil.FormDataValidate("请输入值", () => { return string.IsNullOrEmpty(this.tbValue.Text.Trim()); });
                string key = this.tbKey.Text.Trim();
                string type = this.cbbType.SelectionBoxItem.ToString();
                int ttl = Convert.ToInt32(this.tbTTL.Text.Trim());
                string value = this.tbValue.Text.Trim();


                bool result = false;
                switch (type)
                {
                    case "string":
                        if (ttl == 0)
                        {
                            result = GlobalBusiness.RedisCaches[this.nodeInfo.ConfigId].Add<string>(key, value, nodeInfo.DbIndex);
                        }
                        else
                        {
                            result = GlobalBusiness.RedisCaches[this.nodeInfo.ConfigId].Add<string>(key, value, new TimeSpan(0, 0, 0, 0, ttl), nodeInfo.DbIndex);
                        }
                        break;
                    default:
                        break;
                }
                if (result)
                {
                    this.OnAdded?.Invoke(key);
                    MessageBox.Show("添加成功");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("添加失败");
                }
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

        string oldTxt = "";
        /// <summary>
        /// 当TTL文本框内容变化时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbTTL_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            int result;
            string text = this.tbTTL.Text.Trim();
            if (int.TryParse(text, out result) && result >= 0)
            {
                this.oldTxt = text;
            }
            else
            {
                this.tbTTL.Text = oldTxt;
            }
            this.tbTTL.CaretIndex = oldTxt.Length;
        }

        /// <summary>
        /// 当鼠标点击“取消”按钮时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
