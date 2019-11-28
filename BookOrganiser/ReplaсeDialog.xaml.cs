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
    /// Interaction logic for ReplaсeDialog.xaml
    /// </summary>
    public partial class ReplaсeDialog : Window {
        public ReplaсeDialog() {
            InitializeComponent();

            for(int i = 2; i< MainWindow.paramNames.Length; i++) {
                ColumnCB.Items.Add(MainWindow.paramNames[i]);
            }
            ColumnCB.SelectedIndex = 0;

        }

        private void ReplaceBtn_Click(object sender, RoutedEventArgs e) {
            this.IsEnabled = false;
            Book[] books = DataBase.ExecuteSelectQuerry(DataBase.CreateSelectQuerry(MainWindow.paramNames,
                Properties.Settings.Default.tableName, "", "", "", "", ""));
            foreach( Book b in books) {
                if (b.Id == 595.ToString()) {
                    int i = b.Params[6].IndexOf("&");
                }

                if(b.Params[ColumnCB.SelectedIndex + 2].IndexOf(OriginalTB.Text) >= 0) {
                    b.Params[ColumnCB.SelectedIndex + 2] = b.Params[ColumnCB.SelectedIndex + 2].Replace(OriginalTB.Text, SubtitutingTB.Text);
                    DataBase.ExecuteQuerryWithoutOutput(DataBase.CreateUpdateQuerry(MainWindow.paramNames,
                        Properties.Settings.Default.tableName, b.Params));
                }
            }
            this.IsEnabled = true;
        }
    }
}
