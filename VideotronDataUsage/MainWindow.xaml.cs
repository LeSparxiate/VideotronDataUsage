using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VideotronDataUsage.Classes;

namespace VideotronDataUsage
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Videotron data = new Videotron();
        public trayInformations conso = new trayInformations();

        public MainWindow()
        {
            InitializeComponent();
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            userkey.Text = config.AppSettings.Settings["UserKey"].Value;

            if (config.AppSettings.Settings["UserKey"].Value != "")
            {
                try
                {
                    data.getDataUsage(userkey.Text);
                    conso.SetConso(data);
                    this.WindowState = WindowState.Minimized;
                    this.Hide();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new System.Drawing.Icon("../../Assets/appIcon.ico");
            ni.Visible = true; 
            ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    conso.Hide();
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };
            ni.Click +=
                delegate (object sender, EventArgs args)
                {
                    conso.Show();
                };
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();
            base.OnStateChanged(e);
        }

        /// <summary>
        /// Enregistre la clé Videotron et actualise les informations de consomation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (userkey.Text != "")
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                string oldValue = config.AppSettings.Settings["UserKey"].Value;
                if (oldValue != userkey.Text)
                {
                    config.AppSettings.Settings.Remove("UserKey");
                    config.AppSettings.Settings.Add("UserKey", userkey.Text);
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                    if (config.AppSettings.Settings["UserKey"].Value != "")
                    {
                        try
                        {
                            string keyTmp = userkey.Text;
                            Thread requestThread = new Thread(new ThreadStart(() => { data.getDataUsage(keyTmp); }));
                            requestThread.Start();

                            conso.SetConso(data);
                        } catch (Exception exe)
                        {
                            MessageBox.Show(exe.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez entrer votre clé Vidéotron !");
            }
        }
    }
}
