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

namespace BookOrganiser {
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window {
        public SettingsWindow() {
            InitializeComponent();
            DataBaseNameTB.Text = Properties.Settings.Default.dataBaseName;
            DataBaseUserNameTB.Text = Properties.Settings.Default.dataBaseUser;
            DataBasePasswordTB.Text = Properties.Settings.Default.dataBasePassword;
            DataBaseHostTB.Text = Properties.Settings.Default.dataBaseHost;
            DataBasePortTB.Text = Properties.Settings.Default.dataBasePort.ToString();
            BookRowHeightTB.Text = Properties.Settings.Default.rowHeight.ToString();
            TableNameTB.Text = Properties.Settings.Default.tableName;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e) {
            Properties.Settings.Default.dataBaseName = DataBaseNameTB.Text;
            Properties.Settings.Default.dataBasePassword = DataBasePasswordTB.Text;
            Properties.Settings.Default.dataBaseHost = DataBaseHostTB.Text;
            Properties.Settings.Default.dataBaseUser = DataBaseUserNameTB.Text;
            Properties.Settings.Default.tableName = TableNameTB.Text;
            try {
                Properties.Settings.Default.dataBasePort = Int32.Parse(DataBasePortTB.Text);
            } catch {
                System.Windows.Forms.MessageBox.Show("Failed to save port! Port must be an integer number!");
            }

            try {
                Properties.Settings.Default.rowHeight = Int32.Parse(BookRowHeightTB.Text);
            } catch {
                System.Windows.Forms.MessageBox.Show("Failed to save row height! Height must be an integer number!");
            }
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void ReplaceBtn_Click(object sender, RoutedEventArgs e) {
            ReplaсeDialog dialog = new ReplaсeDialog();
            dialog.Show();
        }
    }
}
