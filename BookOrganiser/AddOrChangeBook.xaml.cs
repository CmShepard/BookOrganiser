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
    /// Логика взаимодействия для AddOrChangeBook.xaml
    /// </summary>
    public partial class AddOrChangeBook : Window {
        private int _numValue = 1;
        AddBookWindow abw;
        MainWindow mw;
        Book book;
        bool isAddingBook = true;
        bool[] wasChanged = new bool[20];

        //! Constructor for adding new book
        public AddOrChangeBook(AddBookWindow _abw, MainWindow _mw) {
            InitializeComponent();
            txtNum.Text = _numValue.ToString();
            abw = _abw;
            mw = _mw;
            CoverImage.Source = null;
            this.Title = "Add Book";
            for(int i = 0; i <abw.saveBookValues.Length; i++) {
                if (abw.saveBookValues[i]) {
                    MainGrid.Children.OfType<Border>().Where(j => Grid.GetRow(j) == i).First().Background 
                            = new SolidColorBrush(Colors.PeachPuff);
                    TextBox tb = (TextBox)MainGrid.Children.OfType<Border>().Where(j => (Grid.GetColumn(j) == 1)
                            && (Grid.GetRow(j) == i)).First().Child;
                    tb.Text = abw.savedBookValues[i];
                }
            }
        }
        //! Constructor for the cange book form
        public AddOrChangeBook(Book _book, MainWindow _mw) {
            InitializeComponent();
            isAddingBook = false;
            book = _book;
            mw = _mw;
            this.Title = "Change Book";
            AddBookBtn.Content = "Change";
            DeleteBtn.Visibility = Visibility.Visible;
            NumericUpDown.Visibility = Visibility.Hidden;
            for(int i = 0; i < book.Params.Length; i++) {
                if (i != 1 && i != 0) {
                    // Load everything but cover and id
                    TextBox tb = (TextBox)MainGrid.Children.OfType<Border>().Where(j => (Grid.GetColumn(j) == 1)
                            && (Grid.GetRow(j) == i)).First().Child;
                    tb.Text = book.Params[i];
                } else if (i == 0) {
                    TextBlock tb = (TextBlock)MainGrid.Children.OfType<Border>().Where(j => (Grid.GetColumn(j) == 1)
                            && (Grid.GetRow(j) == i)).First().Child;
                    tb.Text = book.Params[i];
                } else if (book.Cover != null && book.Cover != "") {
                    // Load cover image from file
                    BitmapImage src = new BitmapImage();
                    src.BeginInit();
                    src.UriSource = new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, book.Params[i]), UriKind.Absolute);
                    src.CacheOption = BitmapCacheOption.OnLoad;
                    src.EndInit();
                    CoverImage.Source = src;
                    CoverImage.Stretch = Stretch.Uniform;
                }
            }
        }

        #region NumericUpDown
        public int NumValue {
            get { return _numValue; }
            set {
                if (value > 0) {
                    _numValue = value;
                    txtNum.Text = value.ToString();
                }
            }
        }
        private void TxtNum_TextChanged(object sender, TextChangedEventArgs e) {
            if (txtNum == null || txtNum.Text == "") {
                return;
            }
            int val = 1;
            if (int.TryParse(txtNum.Text, out val)) {
                if (val <= 0) {
                    val = 1;
                }
                txtNum.Text = val.ToString();
                _numValue = val;
            }
        }
        private void CmdUp_Click(object sender, RoutedEventArgs e) {
            NumValue++;
        }
        private void CmdDown_Click(object sender, RoutedEventArgs e) {
            NumValue--;
        }
        #endregion

        //! Save TextBox values to use for the next book
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (isAddingBook) {
                for (int i = 0; i < 20; i++) {
                    if (abw.saveBookValues[i]) {
                        TextBox tb = (TextBox)MainGrid.Children.OfType<Border>().Where(j => (Grid.GetColumn(j) == 1)
                                && (Grid.GetRow(j) == i)).First().Child;
                        abw.savedBookValues[i] = tb.Text.ToString();
                    }
                }
            }
                
        }
        //! Save values for the next book
        private void Block_MouseRightButtonDown(object sender, MouseButtonEventArgs e) {
            if (isAddingBook) {
                Border border = (Border)sender;
                int row = Grid.GetRow(border);

                if (abw.saveBookValues[row]) {
                    abw.saveBookValues[row] = false;
                    border.Background = new SolidColorBrush(Colors.LightBlue);
                } else {
                    abw.saveBookValues[row] = true;
                    border.Background = new SolidColorBrush(Colors.PeachPuff);
                }
            }
        }

        //! Open cover image context menu
        private void CoverBtn_Click(object sender, RoutedEventArgs e) {
            ContextMenu cm = this.FindResource("cmButton") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }

        //! Paste image from clipboard
        private void MenuItemPaste_Click(object sender, RoutedEventArgs e) {
            if (Clipboard.ContainsImage()) {
                System.Windows.Forms.IDataObject clipboardData = System.Windows.Forms.Clipboard.GetDataObject();
                if (clipboardData != null) {
                    if (clipboardData.GetDataPresent(System.Windows.Forms.DataFormats.Bitmap)) {
                        System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)clipboardData.GetData(System.Windows.Forms.DataFormats.Bitmap);
                        CoverImage.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    }
                }
            }
            wasChanged[1] = true;
        }

        //! Add cover image by loading from file
        private void MenuItemLoadFromFile_Click(object sender, RoutedEventArgs e) {
            string path;
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.RestoreDirectory = true;
            dialog.Title = "Select Image File";
            dialog.Multiselect = false;
            dialog.Filter = "JPG Image (*.jpg)|*.jpg|PNG Image (*.png)|*.png|All files (*.*)|*.*";
            if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                path = dialog.FileName;
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(path, UriKind.Absolute);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();
                CoverImage.Source = src;
                CoverImage.Stretch = Stretch.Uniform;
            }
            wasChanged[1] = true;
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e) {
            CoverImage.Source = null;
            wasChanged[1] = true;
        }

        private void AddBookBtn_Click(object sender, RoutedEventArgs e) {
            if (isAddingBook) {
                for(int k = 0; k < NumValue; k++) {
                    // If new book is being added
                    Book newBook;
                    string[] parameters = new string[20];
                    parameters[0] = (DataBase.GetMaxBookId() + 1).ToString();
                    if (CoverImage.Source != null) {
                        string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "images", parameters[0]);
                        parameters[1] = "data\\images\\" + parameters[0];
                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)CoverImage.Source));
                        using (System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Create))
                            encoder.Save(stream);
                    }
                    for (int i = 2; i < parameters.Length; i++) {
                        TextBox tb = (TextBox)MainGrid.Children.OfType<Border>().Where(j => (Grid.GetColumn(j) == 1)
                                && (Grid.GetRow(j) == i)).First().Child;
                        parameters[i] = tb.Text;
                    }
                    newBook = new Book(parameters);
                    DataBase.ExecuteQuerryWithoutOutput(DataBase.CreateInsertQuerry(MainWindow.paramNames, "books", newBook.Params));
                }
                mw.ClearRows();
                mw.UpdateData(mw.searchString);
                this.Close();
            } else {
                // If the book is being updated

                // If image was chaged or added - delete or create new file
                if (wasChanged[1]) {
                    if (CoverImage.Source == null) {
                        if (!isAddingBook && book.Cover != "" && book.Cover != null)
                            System.IO.File.Delete(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, book.Cover));
                        book.Cover = "";
                    } else {
                        string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "images", book.Id);
                        book.Cover = "data\\images\\" + book.Id;
                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)CoverImage.Source));
                        using (System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                            encoder.Save(stream);
                    }
                }

                    for (int i = 2; i < book.Params.Length; i++) {
                        TextBox tb = (TextBox)MainGrid.Children.OfType<Border>().Where(j => (Grid.GetColumn(j) == 1)
                                && (Grid.GetRow(j) == i)).First().Child;
                        book.Params[i] = tb.Text;
                    }
                    DataBase.ExecuteQuerryWithoutOutput(DataBase.CreateUpdateQuerry(MainWindow.paramNames, "books", book.Params));
                mw.ClearRows();
                mw.UpdateData(mw.searchString);
                this.Close();
            }
            
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e) {
            DataBase.ExecuteQuerryWithoutOutput(DataBase.CreateDeleteQuerry("books", "id", book.Id));
            mw.ClearRows();
            mw.UpdateData(mw.searchString);
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
