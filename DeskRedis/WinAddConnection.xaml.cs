using DeskRedis.Control.CustomControl;
using DeskRedis.Enums;
using DeskRedis.Exceptions;
using DeskRedis.Model.Configs;
using DeskRedis.Util;
using DeskRedis.Util.Redis;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace DeskRedis
{
    /// <summary>
    /// WinAddConnection.xaml 的交互逻辑
    /// </summary>
    public partial class WinAddConnection : DialogWindow
    {
        private ConfigOperationType configOperationType;
        private string configId;


        public delegate void OnSaveConnectionConfigHandler(ConnectionConfig config, ConfigOperationType configOperationType);
        /// <summary>
        /// 当保存配置信息成功时发生。
        /// </summary>
        public event OnSaveConnectionConfigHandler SavedConnectionConfig;


        /// <summary>
        /// 使用默认参数创建实例。
        /// </summary>
        public WinAddConnection(ConfigOperationType crudType) : this(null, crudType)
        { }

        /// <summary>
        /// 使用指定的参数创建实例。
        /// </summary>
        /// <param name="connId">连接名称。</param>
        public WinAddConnection(string connId, ConfigOperationType configOperationType)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.IsShowHelp = false;

            this.configOperationType = configOperationType;
            if (!string.IsNullOrWhiteSpace(connId))
            {
                this.configId = connId;
                ConnectionConfig config = GlobalBusiness.GetConnectionConfig(connId);
                this.tbConnIP.Text = config.IP;
                this.tbConnPort.Text = config.Port.ToString();
                this.tbConnName.Text = config.Name;
                this.tbConnPassword.Text = config.Password;
            }
        }


        #region 本地事件
        /// <summary>
        /// 当鼠标在标题栏按下时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridTitle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// 当鼠标点击保存按钮时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AssertUtil.FormDataValidate("名称不能为空。", () => { return string.IsNullOrWhiteSpace(this.tbConnName.Text); });
                AssertUtil.FormDataValidate("地址不能为空。", () => { return string.IsNullOrWhiteSpace(this.tbConnIP.Text); });
                AssertUtil.FormDataValidate("端口不能为空。", () => { return string.IsNullOrWhiteSpace(this.tbConnPort.Text); });
                AssertUtil.FormDataValidate("端口不合法。", () => { return !Regex.IsMatch(this.tbConnPort.Text, @"^[+-]?\d*$"); });

                ConnectionConfig config = new ConnectionConfig()
                {
                    Name = this.tbConnName.Text.Trim(),
                    IP = this.tbConnIP.Text.Trim(),
                    Port = Convert.ToInt32(this.tbConnPort.Text.Trim()),
                    Password = this.tbConnPassword.Text.Trim()
                };
                if (configOperationType == ConfigOperationType.ADD)
                {
                    config.Id = Guid.NewGuid().ToString("N");
                    GlobalBusiness.SaveConfig(config);
                }
                else
                {
                    config.Id = this.configId;
                    GlobalBusiness.UpdateConfig(config);
                }

                MessageBox.Show("保存成功！");

                this.SavedConnectionConfig?.Invoke(config, this.configOperationType);

                this.Close();
            }
            catch (IllegalFormDataException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (DuplicateMemberException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CbShowPassword_CheckChanged(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.IsChecked == false)
            {
                this.tbConnPassword.Visibility = Visibility.Collapsed;
                this.pwdConnPassword.Visibility = Visibility.Visible;
            }
            else
            {
                this.tbConnPassword.Visibility = Visibility.Visible;
                this.pwdConnPassword.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 当密码框内容变化时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PwdConnPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.pwdConnPassword.IsVisible)
            {
                this.tbConnPassword.Text = this.pwdConnPassword.Password.Trim();
            }
        }

        /// <summary>
        /// 当密码框（明文）内容变化时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbConnPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.tbConnPassword.IsVisible)
            {
                this.pwdConnPassword.Password = this.tbConnPassword.Text.Trim();
            }
        }

        /// <summary>
        /// 当鼠标点击测试连接按钮时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTestConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AssertUtil.FormDataValidate("名称不能为空。", () => { return string.IsNullOrWhiteSpace(this.tbConnName.Text); });
                AssertUtil.FormDataValidate("地址不能为空。", () => { return string.IsNullOrWhiteSpace(this.tbConnIP.Text); });
                AssertUtil.FormDataValidate("端口不能为空。", () => { return string.IsNullOrWhiteSpace(this.tbConnPort.Text); });
                AssertUtil.FormDataValidate("端口不合法。", () => { return !Regex.IsMatch(this.tbConnPort.Text, @"^[+-]?\d*$"); });


                ConnectionConfig config = new ConnectionConfig()
                {
                    Name = this.tbConnName.Text.Trim(),
                    IP = this.tbConnIP.Text.Trim(),
                    Port = Convert.ToInt32(this.tbConnPort.Text.Trim()),
                    Password = this.tbConnPassword.Text.Trim()
                };

                string host = (string.IsNullOrEmpty(config.Password) ? "" : $"{config.Password}@") + $"{config.IP}:{config.Port}";
                string[] hosts = new string[] { host };
                IRedisCache redis = new RedisCache(hosts, hosts);
                string result = redis.ConnectTest();
                if ("SUCCESS".Equals(result))
                {
                    MessageBox.Show("连接成功！");
                }
                else
                {
                    MessageBox.Show($"连接失败！（{result}）");
                }
            }
            catch (IllegalFormDataException ex)
            {
                MessageBox.Show(ex.Message);
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
