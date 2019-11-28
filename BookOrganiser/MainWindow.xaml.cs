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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookOrganiser {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        Dictionary<Grid, Book[]> booksGrid = new Dictionary<Grid, Book[]>();
        List<string> openRows = new List<string>();
        Grid selectedGrid;
        public static string searchString;
        public static string advancedSearchString;
        public static readonly string[] paramNames = new string[] {"id", "cover_path", "title", "location", "authors", "content",
            "annotation", "genres", "format", "publisher", "series", "languages", "price", "currency", "circulation",
            "cover_type", "page_count", "year", "isbn", "user_comments"};
        public static string tableName = "books";

        public MainWindow() {
            InitializeComponent();
            ReadSettings();
            if (!Properties.Settings.Default.isFirstLaunch) {
                if (!DataBase.ConnectToDataBase(Properties.Settings.Default.dataBaseHost,
                    Properties.Settings.Default.dataBasePort.ToString(), Properties.Settings.Default.dataBaseUser,
                    Properties.Settings.Default.dataBasePassword, Properties.Settings.Default.dataBaseName)) {
                    MessageBox.Show("Failed to connect to the database!");
                }
                UpdateData("", "");
            } else {
                FirstLaunchDialog fld = new FirstLaunchDialog(this);
                fld.Topmost = true;
                fld.Owner = this;
                fld.Show();
            }
            
        }

        void ReadSettings() {
            this.Width = Properties.Settings.Default.mainWindowWidth;
            this.Height = Properties.Settings.Default.mainWindowHeight;
            tableName = Properties.Settings.Default.tableName;
            if (Properties.Settings.Default.mainWindowIsFullScreen) {
                this.WindowState = WindowState.Maximized;
            }

            for(int i = 0; i < 20; i++) {
                Headers.ColumnDefinitions[i].Width = new GridLength((int)Properties.Settings.Default["columnWidth" + i.ToString()]);
            }
        }

        #region TEMP IMPORT
        void ImportFromLiba() {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "HTML file (*.html)|*.html";
            dialog.Title = "Select LibaBook backup file";
            if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                string file = dialog.FileName;
                string[] lines = System.IO.File.ReadAllLines(file);

                for(int i = 69; i < lines.Length - 10; i+= 28) {
                    string[] parameters = new string[20];
                    parameters[0] = lines[i].Substring(30, lines[i].IndexOf("</TD>") - 30).Replace("&quot;", "\"");
                    if(lines[i + 1].IndexOf("&nbsp") == -1)
                        parameters[1] = "data\\images\\"  + lines[i + 1].Substring(77, lines[i + 1].IndexOf("'> <img") - 77);
                    parameters[2] = lines[i + 2].Substring(30, lines[i + 2].IndexOf("</TD>") - 30).Replace("&quot;", "\"");
                    parameters[3] = lines[i + 3].Substring(30, lines[i + 3].IndexOf("</TD>") - 30).Replace("&quot;", "\"");
                    int[] rows = new int[] { 5, 6, 7, 8, 11, 12, 13, 14, 15, 18, 16, 17, 19, 21, 22, 20 };
                    int k = 4;
                    foreach(int j in rows) {
                        string line = lines[i + j];
                        while (lines[i + j].IndexOf("</TD>") == -1) {
                            i++;
                            line += lines[i + j];
                        }
                        if (line.Substring(30, line.IndexOf("</TD>") - 30) != "&nbsp") {
                            parameters[k] = line.Substring(30, line.IndexOf("</TD>") - 30).Replace("&quot;", "\"");
                        }
                        k++;
                    }
                    DataBase.ExecuteQuerryWithoutOutput(DataBase.CreateInsertQuerry(paramNames, "books", parameters));
                }
                ClearRows();
            }
        }
        #endregion

        //! Updates rows based on search
        public void UpdateData(string search, string advancedSearch) {
            ClearRows();
            string[] locations = DataBase.GetDistinctValues("location", tableName);
            foreach (string loc in locations) {
                Book[] books = DataBase.ExecuteSelectQuerry(DataBase.CreateSelectQuerry(paramNames, tableName, "title", "location",
                    loc, search, advancedSearch));
                if (books != null && books.Length > 0) {
                    Grid grid;
                    AddMainParameterRows(loc, books, out grid);
                    if(openRows.IndexOf(loc) >= 0) {
                        ShowHideBooksRows((Button)grid.Children[0], grid);
                    }
                }
            }
            GC.Collect();
        }
        //! Expand main parameter to book rows by pressing button
        public void ButtonExpand_Click(Object sender, RoutedEventArgs e) {
            Button btn = (Button)sender;
            Grid grd = (Grid)btn.Parent;
            ShowHideBooksRows(btn, grd);
        }
        //! Expand main parameter to book rows by double clicking
        public void MainParameterGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 2) {
                Grid grd = (Grid)sender;
                Button btn = (Button)grd.Children[0];
                ShowHideBooksRows(btn, grd);
            }

        }
        //! Creates and deletes books rows
        void ShowHideBooksRows(Button btn, Grid grd) {
            if (grd.Tag.ToString() == "+") {
                AddBooksRows(booksGrid[grd], grd);
                TextBlock tb = (TextBlock)grd.Children[1];
                openRows.Add(tb.Text);
                btn.Content = "-";
                grd.Tag = "-";

            } else {
                ElementStack.Children.RemoveRange(ElementStack.Children.IndexOf(grd.Parent as Border) + 1, booksGrid[grd].Count());
                TextBlock tb = (TextBlock)grd.Children[1];
                openRows.Remove(tb.Text);
                btn.Content = "+";
                grd.Tag = "+";
            }
        }
        //! Call thanever Main field must be cleared
        public void ClearRows() {
            ElementStack.Children.Clear();
        }

        //! Insert books rows to the Stack after the pain parameter grid
        public void AddBooksRows(Book[] books, Grid grid) {
            #region AddBooksRows
            Grid[] booksGr = new Grid[books.Length];
            for (int i = 0; i < books.Length; i++) {
                booksGr[i] = new Grid();
                booksGr[i].Height = Properties.Settings.Default.rowHeight;
                booksGr[i].Tag = books[i];
                booksGr[i].MouseDown += BookGrid_MouseClick;
                booksGr[i].Background = new SolidColorBrush(Colors.Azure);
                for (int j = 0; j < 20; j++) {
                    Binding bnd = new Binding("Width") { ElementName = ("HeaderColumn" + j.ToString()) };
                    ColumnDefinition cd = new ColumnDefinition();
                    cd.SetBinding(ColumnDefinition.WidthProperty, bnd);
                    booksGr[i].ColumnDefinitions.Add(cd);

                    Border bookParamBorder = new Border();
                    bookParamBorder.BorderBrush = new SolidColorBrush(Colors.Gray);
                    bookParamBorder.BorderThickness = new Thickness(0, 0, 1, 1);
                    Grid.SetColumn(bookParamBorder, j);

                    if (j != 1) {
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
                ElementStack.Children.Insert(ElementStack.Children.IndexOf(grid.Parent as Border) + 1, booksGr[i]);
            }
            #endregion
        }
        //! Add books rows for one main parameter. mainParameter - SORT BY parameter
        public void AddMainParameterRows(string mainParameter, Book[] books, out Grid mainParameterGrid) {
            #region AddMainParameterGrid
            Border mainParamBorder = new Border();
            mainParamBorder.BorderBrush = new SolidColorBrush(Colors.Gray);
            mainParamBorder.BorderThickness = new Thickness(1);

            mainParameterGrid = new Grid();
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

            booksGrid.Add(mainParameterGrid, books);
        }
        //! Synchronise scroll viewer for headers and content
        private void ContenScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e) {
            HeaderScroll.ScrollToHorizontalOffset(e.HorizontalOffset);
        }

        //! Open AddBook dialogue
        private void AddBookBtn_Click(object sender, RoutedEventArgs e) {
            AddBookWindow dialog = new AddBookWindow(this);
            dialog.Show();
        }

        //! Selecting book row on single click, opening change book dialog on double click
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
        //! Quick search by word in TextBox
        private void QuickSearchBtn_Click(object sender, RoutedEventArgs e) {
            searchString = QuickSearchTB.Text;
            advancedSearchString = "";
            UpdateData(searchString, "");
        }
        //! Quick search by word in TextBox by pressing Enter key
        private void QuickSearchTB_KeyDown(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter) {
                searchString = QuickSearchTB.Text;
                advancedSearchString = "";
                UpdateData(searchString, "");
            }
        }

        private void SearchBookBtn_Click(object sender, RoutedEventArgs e) {
            SearchWindow dialog = new SearchWindow(this);
            dialog.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            DataBase.DisconnectDatabase();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
            Properties.Settings.Default.mainWindowWidth = (int)e.NewSize.Width;
            Properties.Settings.Default.mainWindowHeight = (int)e.NewSize.Height;
            if (this.WindowState == WindowState.Maximized)
                Properties.Settings.Default.mainWindowIsFullScreen = true;
            else if(this.WindowState == WindowState.Normal)
                Properties.Settings.Default.mainWindowIsFullScreen = false;

            Properties.Settings.Default.Save();
        }

        private void GridSplitter_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e) {
            GridSplitter gs = (GridSplitter)sender;
            Properties.Settings.Default["columnWidth" + Grid.GetColumn(gs).ToString()]
                = (int)Headers.ColumnDefinitions[Grid.GetColumn(gs)].Width.Value;
            Properties.Settings.Default.Save();
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e) {
            SettingsWindow dialog = new SettingsWindow();
            dialog.Show();
        }
    }
}
