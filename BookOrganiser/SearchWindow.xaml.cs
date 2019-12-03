using System.Windows;

namespace BookOrganiser {
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window {
        MainWindow mw;
        public SearchWindow(MainWindow _mw) {
            InitializeComponent();
            mw = _mw;
            // Fill in Combo boxes
            if(DataBase.GetDistinctValues(MainWindow.paramNames[3], Properties.Settings.Default.tableName) != null) {
                foreach (string s in DataBase.GetDistinctValues(MainWindow.paramNames[3], Properties.Settings.Default.tableName))
                    LocationComboBox.Items.Add(s);
                foreach (string s in DataBase.GetDistinctValues(MainWindow.paramNames[4], Properties.Settings.Default.tableName))
                    AuthorComboBox.Items.Add(s);
                foreach (string s in DataBase.GetDistinctValues(MainWindow.paramNames[7], Properties.Settings.Default.tableName))
                    GenresComboBox.Items.Add(s);
                foreach (string s in DataBase.GetDistinctValues(MainWindow.paramNames[8], Properties.Settings.Default.tableName))
                    FormatComboBox.Items.Add(s);
                foreach (string s in DataBase.GetDistinctValues(MainWindow.paramNames[9], Properties.Settings.Default.tableName))
                    PublisherComboBox.Items.Add(s);
                foreach (string s in DataBase.GetDistinctValues(MainWindow.paramNames[10], Properties.Settings.Default.tableName))
                    SeriesComboBox.Items.Add(s);
                foreach (string s in DataBase.GetDistinctValues(MainWindow.paramNames[11], Properties.Settings.Default.tableName))
                    LanguagesComboBox.Items.Add(s);
                foreach (string s in DataBase.GetDistinctValues(MainWindow.paramNames[13], Properties.Settings.Default.tableName))
                    CurrencyComboBox.Items.Add(s);
                foreach (string s in DataBase.GetDistinctValues(MainWindow.paramNames[15], Properties.Settings.Default.tableName))
                    CoverTypeComboBox.Items.Add(s);
            }
            
        }
        //! Create advanced search string and search for books in the book view
        private void SearchBtn_Click(object sender, RoutedEventArgs e) {
            string searchStr = " AND " + MainWindow.paramNames[2] + " ILIKE('%" + TitleTextBox.Text + "%')";
            searchStr += " AND " + MainWindow.paramNames[3] + " ILIKE('%" + LocationComboBox.Text + "%')";
            searchStr += " AND " + MainWindow.paramNames[4] + " ILIKE('%" + AuthorComboBox.Text + "%')";
            searchStr += " AND " + MainWindow.paramNames[5] + " ILIKE('%" + ContentTextBox.Text + "%')";
            searchStr += " AND " + MainWindow.paramNames[6] + " ILIKE('%" + AnnotationTextBox.Text + "%')";
            searchStr += " AND " + MainWindow.paramNames[7] + " ILIKE('%" + GenresComboBox.Text + "%')";
            searchStr += " AND " + MainWindow.paramNames[8] + " ILIKE('%" + FormatComboBox.Text + "%')";
            searchStr += " AND " + MainWindow.paramNames[9] + " ILIKE('%" + PublisherComboBox.Text + "%')";
            searchStr += " AND " + MainWindow.paramNames[10] + " ILIKE('%" + SeriesComboBox.Text + "%')";
            searchStr += " AND " + MainWindow.paramNames[11] + " ILIKE('%" + LanguagesComboBox.Text + "%')";
            searchStr += " AND " + MainWindow.paramNames[12] + " ILIKE('%" + PriceTextBox.Text + "%')";
            searchStr += " AND " + MainWindow.paramNames[13] + " ILIKE('%" + CurrencyComboBox.Text + "%')";
            searchStr += " AND " + MainWindow.paramNames[14] + " ILIKE('%" + CirculationTextBox.Text + "%')";
            searchStr += " AND " + MainWindow.paramNames[15] + " ILIKE('%" + CoverTypeComboBox.Text + "%')";
            searchStr += " AND " + MainWindow.paramNames[16] + " ILIKE('%" + PageCountTextBox.Text + "%')";
            searchStr += " AND " + MainWindow.paramNames[17] + " ILIKE('%" + YearTextBox.Text + "%')";
            searchStr += " AND " + MainWindow.paramNames[18] + " ILIKE('%" + ISBNTextBox.Text + "%')";
            searchStr += " AND " + MainWindow.paramNames[19] + " ILIKE('%" + UserCommentsTextBox.Text + "%')";
            MainWindow.advancedSearchString = searchStr;
            MainWindow.searchString = "";
            mw.QuickSearchTB.Text = "";
            mw.UpdateData("", searchStr);
            
        }
        //! Close the dialogue
        private void CancelBtn_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
