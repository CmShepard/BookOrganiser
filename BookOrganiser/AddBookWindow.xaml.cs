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
    }
}
