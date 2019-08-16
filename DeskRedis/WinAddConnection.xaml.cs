using DeskRedis.Exceptions;
using DeskRedis.Model.Configs;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DeskRedis
{
    /// <summary>
    /// WinAddConnection.xaml 的交互逻辑
    /// </summary>
    public partial class WinAddConnection : Window
    {
        public Action<ConnectionConfig> SavedConnectionConfig;

        /// <summary>
        /// 使用默认参数创建实例。
        /// </summary>
        public WinAddConnection() : this(null)
        { }

        /// <summary>
        /// 使用指定的参数创建实例。
        /// </summary>
        /// <param name="connName">连接名称。</param>
        public WinAddConnection(string connName)
        {
            InitializeComponent();

            if (!string.IsNullOrWhiteSpace(connName))
            {
                ConnectionConfig config = GlobalBusiness.GetConnectionConfig(connName);
                this.tbConnIP.Text = config.IP;
                this.tbConnPort.Text = config.Port.ToString();
                this.tbConnName.Text = config.Name;
                this.tbConnPassword.Text = config.Password;
            }
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
                if (string.IsNullOrWhiteSpace(this.tbConnName.Text))
                {
                    throw new IllegalFormDataException("名称不能为空。");
                }
                if (string.IsNullOrWhiteSpace(this.tbConnIP.Text))
                {
                    throw new IllegalFormDataException("地址不能为空。");
                }
                if (string.IsNullOrWhiteSpace(this.tbConnPort.Text))
                {
                    throw new IllegalFormDataException("端口不能为空。");
                }


                ConnectionConfig config = new ConnectionConfig()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = this.tbConnName.Text.Trim(),
                    IP = this.tbConnIP.Text.Trim(),
                    Port = Convert.ToInt32(this.tbConnPort.Text.Trim()),
                    Password = this.tbConnPassword.Text.Trim()
                };

                GlobalBusiness.SaveConfig(config);

                MessageBox.Show("保存成功！");

                this.SavedConnectionConfig?.Invoke(config);

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
        }

        /// <summary>
        /// 当鼠标点击取消按钮时发生。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancle_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        private void PwdConnPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.pwdConnPassword.IsVisible)
            {
                this.tbConnPassword.Text = this.pwdConnPassword.Password.Trim();
            }
        }

        private void TbConnPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.tbConnPassword.IsVisible)
            {
                this.pwdConnPassword.Password = this.tbConnPassword.Text.Trim();
            }
        }
    }
}
