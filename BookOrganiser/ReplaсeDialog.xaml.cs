using System.Windows;

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
