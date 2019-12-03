using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BookOrganiser {
    /// <summary>
    /// Interaction logic for InternetSearchWindow.xaml
    /// </summary>
    public partial class InternetSearchWindow : Window {

        string searchString;
        Grid selectedGrid;
        MainWindow mw;
        AddBookWindow abw;
        public InternetSearchWindow(MainWindow _mw, AddBookWindow _abw, string _searchString) {
            InitializeComponent();
            mw = _mw;
            abw = _abw;
            searchString = _searchString;
            GetSearchResults(@"https://www.ozon.ru/category/knigi-16500/?text=" + searchString);
        }

        public void BookRow_MouseDown(object sender, MouseButtonEventArgs e) {
            Grid grid = (Grid)sender;
            if (e.ClickCount == 1) {
                SolidColorBrush sb = grid.Background as SolidColorBrush;
                if (sb.Color == Colors.White) {
                    if(selectedGrid != null) {
                        selectedGrid.Background = new SolidColorBrush(Colors.White);
                    }
                    grid.Background = new SolidColorBrush(Colors.Aqua);
                    selectedGrid = grid;
                } else {
                    grid.Background = new SolidColorBrush(Colors.White);
                    selectedGrid = null;
                }
            } else if (e.ClickCount == 2) {
                ParseBookPage(grid.Tag.ToString());
            } 
        }

        void ParseBookPage(string link) {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(link);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();

            string authors = "", price = "", currency = "", coverImageLink = "", title = "", annotation = "", content = "", isbn = "",
                pageCount = "", year = "", series = "", genres = "", format = "", publisher = "", language = "", coverType ="";

            int pos = 0;

            // Genres
            pos = result.IndexOf("ol class=", pos);
            if(pos > 0) {
                while (true) {
                    pos = result.IndexOf("span data-v-c66bfbbc data-v-c66bfbbc>", pos + 15);
                    if (pos < 0 || pos > result.IndexOf("Код")) {
                        pos = 0;
                        break;
                    }
                        
                    genres += result.Substring(pos + 37, result.IndexOf("</span>", pos) - pos - 37) + " > ";
                }
                genres = genres.Remove(genres.Length - 3, 3);
            }else{
                pos = 0;
            }

            // Title
            pos = result.IndexOf("<h1 class", pos);
            if (pos > 0) {
                pos = result.IndexOf("<span>", pos);
                title = result.Substring(pos + 6, result.IndexOf("</span", pos) - pos - 6);
            } else {
                pos = 0;
            }


            // Price
            pos = result.IndexOf("price-number");
            if(pos > 0) {
                pos = result.IndexOf("\"main\" data-v-2d02c2d5>", pos);
                price = result.Substring(pos + 23, result.IndexOf("</span", pos) - pos - 23);
            } else {
                pos = 0;
            }

            // Currency
            pos = result.IndexOf("currency", pos);
            if(pos > 0) {
                pos = result.IndexOf("2d02c2d5>", pos);
                currency = result.Substring(pos + 9, result.IndexOf("</span", pos) - pos - 9);
            } else {
                pos = 0;
            }
            // ImageURL
            pos = result.IndexOf("\"image\":\"", 0);
            if(pos > 0) {
                coverImageLink = result.Substring(pos + 9, result.IndexOf("\"", pos + 10) - pos - 9).Replace("\\\\u002F", "/");
            } else {
                pos = 0;
            }

            

            // Annotation
            pos = result.IndexOf("Описание", pos);
            if(pos > 0) {
                pos = result.IndexOf("\">", pos + 1);
                pos = result.IndexOf("\">", pos + 1);
                pos = result.IndexOf("\">", pos + 1);
                annotation = result.Substring(pos + 2, result.IndexOf("</div>", pos) - pos - 2).Replace("<br>", "\n").Replace("&quot;", "\"");
            } else {
                pos = 0;
            }

            // Content
            pos = result.IndexOf("Содержание", pos);
            if(pos > 0) {
                while (true) {
                    pos = result.IndexOf("h3 data-v-c66bfbbc>", pos + 5);
                    if(pos < 0) {
                        pos = 0;
                        break;
                    }
                    content += result.Substring(pos + 19,
                        result.IndexOf("</h3>", pos) - pos - 19).Replace("</h3>", " ").Replace("</p>", " ").Replace("<p>", "")  + " \n";
                }
                content = content.Trim();
                
            } else {
                pos = 0;
            }

            // Authors
            pos = result.IndexOf("Автор на обложке", pos);
            if (pos > 0) {
                pos = result.IndexOf("<span data-v-c66bfbbc>", pos);
                authors = result.Substring(pos + 22, result.IndexOf("</span", pos) - pos - 22);
            } else {
                pos = 0;
            }

            // Series
            pos = result.IndexOf("Серия", pos);
            if(pos > 0) {
                pos = result.IndexOf("data-v-c66bfbbc data-v-c66bfbbc>", pos);
                series = result.Substring(pos + 32, result.IndexOf("</a", pos) - pos - 32);
            } else {
                pos = 0;
            }

            // Format
            pos = result.IndexOf("Формат издания", pos);
            if(pos > 0) {
                pos = result.IndexOf("span data-v-c66bfbbc>", pos);
                format = result.Substring(pos + 21, result.IndexOf("</span", pos) - pos - 21);
            } else {
                pos = 0;
            }

            // Publisher
            pos = result.IndexOf("Издательство", pos);
            if(pos > 0) {
                int origPos = pos;
                while (pos < result.IndexOf("Год выпуска", origPos)) {
                    pos = result.IndexOf("data-v-c66bfbbc data-v-c66bfbbc>", pos + 5);
                    if (pos < 0) {
                        pos = 0;
                        break;
                    }
                    publisher += result.Substring(pos + 32,
                        result.IndexOf("</a", pos) - pos - 32) + ", ";
                }
                publisher = publisher.Remove(publisher.Length - 2, 2);
                publisher = publisher.Remove(publisher.LastIndexOf(','), publisher.Count() - publisher.LastIndexOf(","));
                pos = origPos;
            } else {
                pos = 0;
            }


            // Year
            pos = result.IndexOf("Год выпуска", pos);
            if(pos > 0) {
                pos = result.IndexOf("span data-v-c66bfbbc>", pos);
                year = result.Substring(pos + 21, result.IndexOf("</span", pos) - pos - 21);
            } else {
                pos = 0;
            }

            pos = result.IndexOf("Количество страниц", pos);
            if(pos > 0) {
                pos = result.IndexOf("span data-v-c66bfbbc>", pos);
                pageCount = result.Substring(pos + 21, result.IndexOf("</span", pos) - pos - 21);
            } else {
                pos = 0;
            }


            pos = result.IndexOf("Язык издания", pos);
            if(pos > 0) {
                pos = result.IndexOf("span data-v-c66bfbbc>", pos);
                language = result.Substring(pos + 21, result.IndexOf("</span", pos) - pos - 21);
            } else {
                pos = 0;
            }


            pos = result.IndexOf("ISBN", pos);
            if(pos > 0) {
                pos = result.IndexOf("span data-v-c66bfbbc>", pos);
                isbn = result.Substring(pos + 21, result.IndexOf("</span", pos) - pos - 21);
            } else {
                pos = 0;
            }
            pos = 0;

            pos = result.IndexOf("Тип обложки", pos);
            if(pos > 0) {
                pos = result.IndexOf("span data-v-c66bfbbc>", pos);
                coverType = result.Substring(pos + 21, result.IndexOf("</span", pos) - pos - 21);
            } else {
                pos = 0;
            }

            Book book = new Book("", coverImageLink, title, "", authors, content, annotation, genres, format, publisher, series,
                language, price, currency, "", coverType, pageCount, year, isbn, "");
            AddOrChangeBook dialog = new AddOrChangeBook(abw, mw, this, book);
            dialog.Owner = this;
            dialog.Show();
        }

        void GetSearchResults(string searchStr) {

            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(searchStr);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();
            List<Bitmap> bitmaps = new List<Bitmap>();
            List<string> resultList = new List<string>();
            List<string> titles = new List<string>();
            List<string> authors = new List<string>();
            for(int i = 0; i > -1;) {
                string s = "tile-wrapper";
                i = result.IndexOf(s, i + 10);
                if(i == -1) {
                    break;
                }
                int next = result.IndexOf("class=\"tile-wrapper\"", i + 10);
                if(next < i) {
                    next = result.Length - 1;
                }

                string link = "https://www.ozon.ru" + result.Substring(result.IndexOf("href=\"", i - 100) + 6,
                    result.IndexOf("\" class=\"", i - 30) - result.IndexOf("href=\"", i - 100) - 6);
                resultList.Add(link);


                string path = result.Substring(result.IndexOf("<img src=\"", i + 1) + 10,
                    result.IndexOf("\" srcset=\"", i) - result.IndexOf("<img src=\"", i + 1) - 10);
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(path);
                Bitmap bitmap;
                bitmap = new Bitmap(stream);
                bitmaps.Add(bitmap);


                int titleStart = result.IndexOf("tile-name", i + 1);
                titleStart = result.IndexOf("00072736>", titleStart) + 9;
                titles.Add(result.Substring(titleStart + 2, result.IndexOf("<span", titleStart) - titleStart - 2).TrimStart().TrimEnd().TrimEnd('\n'));

                int authorStart = result.IndexOf("В корзину", titleStart + 1);
                authorStart = result.IndexOf("00072736>", authorStart) + 9;
                authors.Add(result.Substring(authorStart, result.IndexOf("</span>", authorStart) - authorStart));
            }

            for(int i = 0; i < authors.Count; i++) {
                Grid g = new Grid();

                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                TextBlock tb = new TextBlock();
                TextBlock tb1 = new TextBlock();

                g.Height = Properties.Settings.Default.rowHeight;
                g.Tag = resultList[i];
                g.Margin = new Thickness(0, 5, 0, 0);
                g.MouseDown += BookRow_MouseDown;
                g.Background = new SolidColorBrush(Colors.White);

                image.Source =  System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bitmaps[i].GetHbitmap(),
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(bitmaps[i].Width, bitmaps[i].Height));

                image.HorizontalAlignment = HorizontalAlignment.Left;
                image.Margin = new Thickness(10, 0, 0, 0);
                image.Width = 100;

                tb.Width = 200;
                tb.Text = titles[i];
                tb.HorizontalAlignment = HorizontalAlignment.Left;
                tb.Margin = new Thickness(120, 0, 0, 0);
                tb.TextWrapping = TextWrapping.Wrap;
                tb.VerticalAlignment = VerticalAlignment.Center;

                tb1.Width = 200;
                tb1.Text = authors[i];
                tb1.HorizontalAlignment = HorizontalAlignment.Left;
                tb1.Margin = new Thickness(330, 0, 0, 0);
                tb1.TextWrapping = TextWrapping.Wrap;
                tb1.VerticalAlignment = VerticalAlignment.Center;

                g.Children.Add(image);
                g.Children.Add(tb);
                g.Children.Add(tb1);

                SearchResults.Children.Add(g);
            }
        }
    }
}
