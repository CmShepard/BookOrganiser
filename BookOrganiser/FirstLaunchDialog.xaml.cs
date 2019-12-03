using Microsoft.Win32;
using Npgsql;
using System;
using System.Windows;

namespace BookOrganiser {
    /// <summary>
    /// Interaction logic for FirstLaunchDialog.xaml
    /// </summary>
    public partial class FirstLaunchDialog : Window {
        MainWindow mw;
        public FirstLaunchDialog(MainWindow _mw) {
            InitializeComponent();
            mw = _mw;
            var postgres = Registry.CurrentUser.OpenSubKey(@"Software\pgadmin");
            if (postgres == null) {
                PGWarnText.Visibility = Visibility.Visible;
                DBHintText.Visibility = Visibility.Hidden;

                CreateDBBtn.IsEnabled = false;
                FindDBBtn.IsEnabled = false;
            }
        }

        private void CreateDBBtn_Click(object sender, RoutedEventArgs e) {
            string connStr = "Server=" + DataBaseHostTB.Text +";Port=" + DataBasePortTB.Text 
                + ";User Id=" + DataBaseUserNameTB.Text + ";Password=" + DataBasePasswordTB.Text + ";";
            NpgsqlConnection m_conn = new NpgsqlConnection(connStr);
            NpgsqlCommand m_createdb_cmd = new NpgsqlCommand(
                "CREATE DATABASE " + DataBaseNameTB.Text + " \nENCODING = 'UTF8'\nCONNECTION LIMIT = -1;", m_conn);
            m_conn.Open();
            m_createdb_cmd.ExecuteNonQuery();
            m_conn.Close();

            connStr = "Server=" + DataBaseHostTB.Text + ";Port=" + DataBasePortTB.Text
                + ";User Id=" + DataBaseUserNameTB.Text + ";Password=" + DataBasePasswordTB.Text + ";Database=" + DataBaseNameTB.Text;
            m_conn = new NpgsqlConnection(connStr);
            NpgsqlCommand m_createtbl_cmd = new NpgsqlCommand(
               "CREATE TABLE " + TableNameTB.Text + 
                " (cover_path character varying(255) COLLATE pg_catalog.\"default\", " +
                "title character varying(9999) COLLATE pg_catalog.\"default\" NOT NULL, " + 
                "location character varying(9999) COLLATE pg_catalog.\"default\" NOT NULL, " + 
                "authors character varying(9999) COLLATE pg_catalog.\"default\", " +
                "content character varying(9999) COLLATE pg_catalog.\"default\", " + 
                "annotation character varying(9999) COLLATE pg_catalog.\"default\", " +
                "genres character varying(9999) COLLATE pg_catalog.\"default\", " +
                "format character varying(9999) COLLATE pg_catalog.\"default\", " +
                "id serial NOT NULL, " +
                "publisher character varying(9999) COLLATE pg_catalog.\"default\", " +
                "series character varying(9999) COLLATE pg_catalog.\"default\", " +
                "languages character varying(9999) COLLATE pg_catalog.\"default\", " +
                "price character varying(9999) COLLATE pg_catalog.\"default\",  " +
                "currency character varying(9999) COLLATE pg_catalog.\"default\", " +
                "circulation character varying(9999) COLLATE pg_catalog.\"default\", " +
                "cover_type character varying(9999) COLLATE pg_catalog.\"default\", " +
                "page_count character varying(9999) COLLATE pg_catalog.\"default\", " +
                "year character varying(9999) COLLATE pg_catalog.\"default\", " +
                "isbn character varying(9999) COLLATE pg_catalog.\"default\", " +
                "user_comments character varying(9999) COLLATE pg_catalog.\"default\", " +
                "CONSTRAINT books_pkey PRIMARY KEY(id))"
               , m_conn);
            m_conn.Open();
            m_createtbl_cmd.ExecuteNonQuery();
            m_conn.Close();
            DataBase.ConnectToDataBase(DataBaseHostTB.Text,
                DataBasePortTB.Text, DataBaseUserNameTB.Text, DataBasePasswordTB.Text, DataBaseNameTB.Text);
            mw.AddBookBtn.IsEnabled = true;
            mw.SearchBookBtn.IsEnabled = true;
            mw.SettingsBtn.IsEnabled = true;
            mw.QuickSearchBtn.IsEnabled = true;
            mw.OrderByCB.IsEnabled = true;
            mw.SortByCB.IsEnabled = true;
            SaveSettings();
            this.Close();
        }

        void SaveSettings() {
            Properties.Settings.Default.dataBaseName = DataBaseNameTB.Text;
            Properties.Settings.Default.dataBasePassword = DataBasePasswordTB.Text;
            Properties.Settings.Default.dataBaseHost = DataBaseHostTB.Text;
            Properties.Settings.Default.dataBaseUser = DataBaseUserNameTB.Text;
            Properties.Settings.Default.tableName = TableNameTB.Text;
            Properties.Settings.Default.isFirstLaunch = false;
            try {
                Properties.Settings.Default.dataBasePort = Int32.Parse(DataBasePortTB.Text);
            } catch {
                System.Windows.Forms.MessageBox.Show("Failed to save port! Port must be an integer number!");
            }
            Properties.Settings.Default.Save();
        }

        private void FindDBBtn_Click(object sender, RoutedEventArgs e) {
            string password = DataBasePasswordTB.Text;
            string bdName = DataBaseNameTB.Text;
            if (DataBase.ConnectToDataBase(DataBaseHostTB.Text,
                DataBasePortTB.Text, DataBaseUserNameTB.Text, password, bdName)) {
                mw.AddBookBtn.IsEnabled = true;
                mw.SearchBookBtn.IsEnabled = true;
                mw.SettingsBtn.IsEnabled = true;
                mw.QuickSearchBtn.IsEnabled = true;
                mw.OrderByCB.IsEnabled = true;
                mw.SortByCB.IsEnabled = true;
                mw.UpdateData("", "");

                SaveSettings();
                this.Close();
            } else {
                System.Windows.Forms.MessageBox.Show("Error! Failed to find DB with specified parameters! try creating new DB instead.");
            }
        }
    }
}
