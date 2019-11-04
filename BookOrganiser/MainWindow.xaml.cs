using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookOrganiser {
    public class Book {
        public Book(string id, string cover, string title, string location, string authors, string content,
            string annotation, string genres, string format, string publisher, string series, string languages,
            string price, string currency, string circulation, string coverType, string pageCount, string year,
            string isbn, string userComments) {
            Params = new string[] {
                id, cover, title, location, authors, content, annotation, genres, format, publisher, series,
                languages, price, currency, circulation, coverType, pageCount, year, isbn, userComments
            };
        }
        public Book(string[] parameters) {
            Params = parameters;
        }
        public string[] Params {
            get;
        }
        public string Id {
            get {
                return Params[0];
            }
            set {
                int id = 0;
                if(int.TryParse(value, out id)) {
                    Params[0] = id.ToString();
                }
            }
        }
        public string Cover {
            get {
                return Params[1];
            }
            set {
                Params[1] = value;
            }
        }
        public string Title {
            get {
                return Params[2];
            }
            set {
                Params[2] = value;
            }
        }
        public string Location {
            get {
                return Params[3];
            }
            set {
                Params[3] = value;
            }
        }
        public string Authors {
            get {
                return Params[4];
            }
            set {
                Params[4] = value;
            }
        }
        public string Content {
            get {
                return Params[5];
            }
            set {
                Params[5] = value;
            }
        }
        public string Annotation {
            get {
                return Params[6];
            }
            set {
                Params[6] = value;
            }
        }
        public string Genres {
            get {
                return Params[7];
            }
            set {
                Params[7] = value;
            }
        }
        public string Format {
            get {
                return Params[8];
            }
            set {
                Params[8] = value;
            }
        }
        public string Publisher {
            get {
                return Params[9];
            }
            set {
                Params[9] = value;
            }
        }
        public string Series {
            get {
                return Params[10];
            }
            set {
                Params[10] = value;
            }
        }
        public string Languages {
            get {
                return Params[11];
            }
            set {
                Params[11] = value;
            }
        }
        public string Price {
            get {
                return Params[12];
            }
            set {
                Params[12] = value;
            }
        }
        public string Currency {
            get {
                return Params[13];
            }
            set {
                Params[13] = value;
            }
        }
        public string Circulation {
            get {
                return Params[14];
            }
            set {
                Params[14] = value;
            }
        }
        public string CoverType {
            get {
                return Params[15];
            }
            set {
                Params[15] = value;
            }
        }
        public string PageCount {
            get {
                return Params[16];
            }
            set {
                Params[16] = value;
            }
        }
        public string Year {
            get {
                return Params[17];
            }
            set {
                Params[17] = value;
            }
        }
        public string ISBN {
            get {
                return Params[18];
            }
            set {
                Params[18] = value;
            }
        }
        public string UserComments {
            get {
                return Params[19];
            }
            set {
                Params[19] = value;
            }
        }
    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        Dictionary<Grid, Grid[]> booksGrid = new Dictionary<Grid, Grid[]>();
        private int rowThikness = 100;
        Grid selectedGrid;
        public static readonly string[] paramNames = new string[] {"id", "cover_path", "title", "location", "authors", "content",
            "annotation", "genres", "format", "publisher", "series", "languages", "price", "currency", "circulation",
            "cover_type", "page_count", "year", "isbn", "user_comments"};

        public MainWindow() {
            InitializeComponent();

            if(!DataBase.ConnectToDataBase("localhost", "5432", "postgres", "1", "Books")){
                MessageBox.Show("Failed to connect to the database!");
            }
            UpdateData();
            //
            Book book = new Book("1", "data\\images\\1.jpg", "Anna Karenina", "1 шкаф 2 стелаж 9 полка", "Leo Tolstoi", "Anna Karenina", "Book about Anna",
                "Classic literature", "9x15", "Yuzhno-Uralskoye Izdatelstvo", "Klassiki", "Russian", "180", "Rub", "15000", "Hard", "250",
                "1978", "", "");
            Book book2 = new Book("2", "data\\images\\1.jpg", "Anna Karenina", "1 шкаф 2 стелаж 9 полка", "Leo Tolstoi", "Anna Karenina", "Book about Anna",
                "Classic literature", "9x15", "Yuzhno-Uralskoye Izdatelstvo", "Klassiki", "Russian", "180", "Rub", "15000", "Hard", "250",
                "1978", "", "");
            Book book3 = new Book("3", "data\\images\\1", "Anna Karenina", "1 шкаф 2 стелаж 9 полка", "Leo Tolstoi", "Anna Karenina", "Book about Anna",
                "Classic literature", "9x15", "Yuzhno-Uralskoye Izdatelstvo", "Klassiki", "Russian", "180", "Rub", "15000", "Hard", "250",
                "1978", "", "");
            Book book4 = new Book("4", "data\\images\\1.jpg", "Anna Karenina", "1 шкаф 2 стелаж 9 полка", "Leo Tolstoi", "Anna Karenina", "Book about Anna",
                "Classic literature", "9x15", "Yuzhno-Uralskoye Izdatelstvo", "Klassiki", "Russian", "180", "Rub", "15000", "Hard", "250",
                "1978", "", "");
            AddBookRows("1 шкаф 2 стелаж 9 полка", new Book[] { book, book2, book3, book4 });
            AddBookRows("1 шкаф 3 стелаж 9 полка", new Book[] { book, book2, book3, book4 });
        }

        public void UpdateData() {
            string[] locations = DataBase.GetDistinctValues("location", "books");
            foreach (string loc in locations) {
                Book[] books = DataBase.ExecuteSelectQuerry(DataBase.CreateSelectQuerry(paramNames, "books", "title", "location", loc));
                AddBookRows(loc, books);
            }
            GC.Collect();
        }
        public void ButtonExpand_Click(Object sender, RoutedEventArgs e) {
            Button btn = (Button)sender;
            Grid grd = (Grid)btn.Parent;
            ResizeRows(btn, grd);
        }
        public void MainParameterGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 2) {
                Grid grd = (Grid)sender;
                Button btn = (Button)grd.Children[0];
                ResizeRows(btn, grd);
            }

        }
        void ResizeRows(Button btn, Grid grd) {
            if (grd.Tag.ToString() == "+") {
                for (int i = 0; i < booksGrid[grd].Count(); i++) {
                    booksGrid[grd][i].Height = rowThikness;
                }
                btn.Content = "-";
                grd.Tag = "-";

            } else {
                for (int i = 0; i < booksGrid[grd].Count(); i++) {
                    booksGrid[grd][i].Height = 0;
                }
                btn.Content = "+";
                grd.Tag = "+";
            }
        }
        //! Call thanever Main field must be cleared
        public void ClearRows() {
            ElementStack.Children.Clear();
        }

        //! Add books rows for one main parameter. mainParameter - SORT BY parameter
        public void AddBookRows(string mainParameter, Book[] books) {
            #region AddButton
            Border mainParamBorder = new Border();
            mainParamBorder.BorderBrush = new SolidColorBrush(Colors.Gray);
            mainParamBorder.BorderThickness = new Thickness(1);

            Grid mainParameterGrid = new Grid();
            mainParameterGrid.Height = 20;
            mainParameterGrid.Width = Headers.Width - 40;
            mainParameterGrid.Background = new SolidColorBrush(Colors.LightBlue);
            mainParameterGrid.Tag = "+";
            mainParameterGrid.MouseDown += MainParameterGrid_MouseDoubleClick;


            Button expandButton = new Button();
            expandButton.Width = 11;
            expandButton.Height = 11;
            expandButton.HorizontalAlignment = HorizontalAlignment.Left;
            expandButton.Margin = new Thickness(5, 0, 0, 0);
            expandButton.Content = "+";
            expandButton.FontSize = 8;
            expandButton.Padding = new Thickness(0);
            expandButton.Click += ButtonExpand_Click;

            TextBlock mainParameterTextBlock = new TextBlock();
            mainParameterTextBlock.Text = mainParameter;
            mainParameterTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
            mainParameterTextBlock.Margin = new Thickness(25, 0, 0, 0);
            mainParameterTextBlock.FontWeight = FontWeights.Bold;

            mainParameterGrid.Children.Add(expandButton);
            mainParameterGrid.Children.Add(mainParameterTextBlock);
            mainParamBorder.Child = mainParameterGrid;

            ElementStack.Children.Add(mainParamBorder);
            #endregion

            #region AddBooksRows
            Grid[] booksGr = new Grid[books.Length];
            for (int i = 0; i < books.Length; i++) {
                booksGr[i] = new Grid();
                booksGr[i].Height = 0;
                booksGr[i].Tag = books[i];
                booksGr[i].MouseDown += BookGrid_MouseClick;
                booksGr[i].Background = new SolidColorBrush(Colors.Azure);
                for(int j = 0; j < 20; j++) {
                    Binding bnd = new Binding("Width") { ElementName = ("HeaderColumn" + j.ToString()) };
                    ColumnDefinition cd = new ColumnDefinition();
                    cd.SetBinding(ColumnDefinition.WidthProperty, bnd);
                    booksGr[i].ColumnDefinitions.Add(cd);

                    Border bookParamBorder = new Border();
                    bookParamBorder.BorderBrush = new SolidColorBrush(Colors.Gray);
                    bookParamBorder.BorderThickness = new Thickness(0,0,1,1);
                    Grid.SetColumn(bookParamBorder, j);

                    if(j != 1) {
                        TextBlock tb = new TextBlock();
                        tb.TextWrapping = TextWrapping.Wrap;
                        tb.Text = books[i].Params[j];
                        tb.Margin = new Thickness(3);
                        bookParamBorder.Child = tb;
                    } else {
                        Image im = new Image();
                        if (books[i].Cover != null && books[i].Cover != "") {
                            BitmapImage src = new BitmapImage();
                            src.BeginInit();
                            src.UriSource = new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, books[i].Cover), UriKind.Absolute);
                            src.CacheOption = BitmapCacheOption.OnLoad;
                            src.EndInit();
                            im.Source = src;
                            im.Stretch = Stretch.Uniform;
                            im.Margin = new Thickness(3);
                        }
                        bookParamBorder.Child = im;
                    }
                    booksGr[i].Children.Add(bookParamBorder);
                }
                ElementStack.Children.Add(booksGr[i]);
            }
            #endregion
            booksGrid.Add(mainParameterGrid, booksGr);
        }
        //! Synchronise scroll viewer for headers and content
        private void ContenScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e) {
            HeaderScroll.ScrollToHorizontalOffset(e.HorizontalOffset);
        }

        private void AddBookBtn_Click(object sender, RoutedEventArgs e) {
            AddBookWindow dialog = new AddBookWindow(this);
            dialog.Show();
        }

        private void BookGrid_MouseClick(object sender, MouseButtonEventArgs e) {
            Grid grid = sender as Grid;
             if(e.ClickCount == 1) {
                if (selectedGrid != null) {
                    selectedGrid.Background = new SolidColorBrush(Colors.Azure);
                }
                grid.Background = new SolidColorBrush(Colors.Aqua);
                selectedGrid = grid;
            } else if (e.ClickCount == 2) {
                AddOrChangeBook dialog = new AddOrChangeBook((Book)grid.Tag, this);
                dialog.Owner = this;
                dialog.Show();
                dialog.Focus();
            }
        }
    }
}
