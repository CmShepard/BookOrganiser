using System.Windows;

namespace BookOrganiser {
    /// <summary>
    /// Логика взаимодействия для AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window {

        public string[] savedBookValues;
        public bool[] saveBookValues;
        public int lastID;
        MainWindow mw;
        public AddBookWindow(MainWindow _mw) {
            InitializeComponent();
            mw = _mw;
            savedBookValues = new string[20];
            saveBookValues = new bool[20];
        }

        private void ManualAddBtn_Click(object sender, RoutedEventArgs e) {
            AddOrChangeBook dialog = new AddOrChangeBook(this, mw);
            dialog.Show();
        }

        private void BtnInternetSearch_Click(object sender, RoutedEventArgs e) {
            InternetSearchWindow dialog = new InternetSearchWindow(mw, this, InternetSearchTB.Text);
            dialog.Show();
        }
    }
}
