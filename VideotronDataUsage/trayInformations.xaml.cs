using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
using VideotronDataUsage.Classes;

namespace VideotronDataUsage
{
    /// <summary>
    /// Logique d'interaction pour trayInformations.xaml
    /// </summary>
    public partial class trayInformations : Window
    {
        public Videotron toUpdate = null;

        public trayInformations()
        {
            InitializeComponent();
            this.Hide();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.Title == "Consomation")
            {
                this.Hide();
                e.Cancel = true;
            }
            base.OnClosing(e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }

        public void SetConso(Videotron data)
        {
            toUpdate = data;
            pourcentageTotal.Content = data.pourcentageTotal + "%";
            octetsMax.Content = (Int64.Parse(data.octetsMax) / 1024 / 1024 / 1024).ToString() + " Go";
            octetsTotal.Content = (Int64.Parse(data.octetsTotal) / 1024 / 1024 / 1024).ToString() + " Go";
            octetsDownload.Content = (Int64.Parse(data.octetsDownload) / 1024 / 1024 / 1024).ToString() + " Go";
            octetsUpload.Content = (Int64.Parse(data.octetsUpload) / 1024 / 1024 / 1024).ToString() + " Go";
            joursRestants.Content = data.joursRestants + " jours";
            dateReset.Content = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(data.dateReset));
            jours.Content = data.jours + " jours";
            dateDebut.Content = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(data.dateDebut));
            lastRefresh.Content = "Der.Maj : " + DateTime.Now.ToString();
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["UserKey"].Value != "" && toUpdate != null)
            {
                try
                {
                    toUpdate.getDataUsage(config.AppSettings.Settings["UserKey"].Value);
                    SetConso(toUpdate);
                } catch (Exception ex)
                {
                    // alert exception here
                }
            }
        }
    }
}
