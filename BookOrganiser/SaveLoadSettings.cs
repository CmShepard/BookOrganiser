using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookOrganiser {
    static class SaveLoadSettings {

        public static int addBookColumnWidth;
        public static int[] bookColumnWidths;

        public static int bookRowHeight;

        public static void Save() {
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings", "settings.csv");


        }
    }
}
