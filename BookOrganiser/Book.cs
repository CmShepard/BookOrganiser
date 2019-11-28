using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookOrganiser {
    public class Book {
        //! Constructor with separate values
        public Book(string id, string cover, string title, string location, string authors, string content,
            string annotation, string genres, string format, string publisher, string series, string languages,
            string price, string currency, string circulation, string coverType, string pageCount, string year,
            string isbn, string userComments) {
            Params = new string[] {
                id, cover, title, location, authors, content, annotation, genres, format, publisher, series,
                languages, price, currency, circulation, coverType, pageCount, year, isbn, userComments
            };
        }
        //! Constructor with array as imput
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
                if (int.TryParse(value, out id)) {
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
}
