using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace BookOrganiser {
    static class DataBase {
        static NpgsqlConnection connection;
        //! Connect to the postgreSQL database
        public static bool ConnectToDataBase(string server, string port, string user, string password, string databaseName) {
            string connectionParameters = "Server=" + server +
                ";Port=" + port + ";User Id=" + user + ";Password=" + password + ";Database=" + databaseName + ";";
            try {
                connection = new NpgsqlConnection(connectionParameters);
                connection.Open();
            } catch {
                return false;
            }
            return true;
            
        }
        //! Get all distinct values of some column used to group books
        public static string[] GetDistinctValues(string column, string table) {
            List<string> ret = new List<string>();
            string querry = "SELECT DISTINCT(" + column + ") FROM " + table + " \nORDER BY " + column + ";";
            NpgsqlCommand npgSqlCommand = new NpgsqlCommand(querry, connection);
            NpgsqlDataReader npgSqlDataReader = npgSqlCommand.ExecuteReader();
            if (npgSqlDataReader.HasRows) {
                foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                    ret.Add(dbDataRecord[column] as string);
                npgSqlDataReader.Close();
                return ret.ToArray();
            } else {
                npgSqlDataReader.Close();
                return null;
            }
        }

        //! Returns the maximum ID of the table
        public static int GetMaxBookId() {
            string querry = "SELECT MAX(id) FROM books";
            NpgsqlCommand npgSqlCommand = new NpgsqlCommand(querry, connection);
            NpgsqlDataReader npgSqlDataReader = npgSqlCommand.ExecuteReader();
            int id = 0;
            if (npgSqlDataReader.HasRows) {
                foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                    id =  int.Parse(dbDataRecord[0].ToString());
            }
            npgSqlDataReader.Close();
            return id;
             
        }

        //! Returns querry for the SELECT command
        public static string CreateSelectQuerry(string[] columns, string table, string orderByColumn, string whereColumn, string whereValue) {
            string querry = "SELECT ";
            for(int i = 0; i < columns.Length; i++) {
                if (i != columns.Length - 1)
                    querry += columns[i] + ", ";
            }
            querry += columns[columns.Length - 1];
            querry += " FROM ";
            querry += table;
            if (whereColumn.Length > 0)
                querry += "\nWHERE " + whereColumn + " = '" + whereValue + "'";
            if (orderByColumn.Length > 0)
                querry += "\nORDER BY " + orderByColumn;
            querry += ";";
            return querry;
        }

        public static string CreateInsertQuerry(string[] columns, string table, string[] values) {
            string querry = "INSERT INTO " + table + "(";
            querry += columns[0];
            for(int i = 1; i < columns.Length; i++) {
                querry += ", " + columns[i];
            }
            querry += ") \n VALUES ('" + values[0] + "'";
            for (int i = 1; i < values.Length; i++) {
                querry += ", '" + values[i] + "'";
            }
            querry += ");";
            return querry;
        }

        // Create delete querry
        public static string CreateDeleteQuerry(string table, string whereColumn, string whereValue) {
            string querry = "DELETE FROM " + table + " \nWHERE ";
            querry += whereColumn + " = " + whereValue + ";";
            return querry;
        }

        public static string CreateUpdateQuerry(string[] columns, string table, string[] values) {
            string querry = "UPDATE " + table + " \nSET ";
            querry += columns[0] + " = " + values[0];
            for(int i = 1; i < columns.Length; i++) {
                querry += ", " + columns[i] + " = '" + values[i] + "'";
            }
            querry += " \n WHERE " + columns[0] + " = '" + values[0] + "';";
            return querry;
        }

        public static void ExecuteQuerryWithoutOutput(string querry) {
            NpgsqlCommand npgSqlCommand = new NpgsqlCommand(querry, connection);
            npgSqlCommand.ExecuteNonQuery();
        }
        //! Execute SELECT querry, return Book class
        public static Book[] ExecuteSelectQuerry(string querry) {
            List<Book> books = new List<Book>();
            NpgsqlCommand npgSqlCommand = new NpgsqlCommand(querry, connection);
            NpgsqlDataReader npgSqlDataReader = npgSqlCommand.ExecuteReader();
            if (npgSqlDataReader.HasRows) {
                foreach (DbDataRecord dbDataRecord in npgSqlDataReader)
                    books.Add(new Book(dbDataRecord[0].ToString(), dbDataRecord[1].ToString(), dbDataRecord[2].ToString(),
                        dbDataRecord[3].ToString(), dbDataRecord[4].ToString(), dbDataRecord[5].ToString(),
                        dbDataRecord[6].ToString(), dbDataRecord[7].ToString(), dbDataRecord[8].ToString(),
                        dbDataRecord[9].ToString(), dbDataRecord[10].ToString(), dbDataRecord[11].ToString(),
                        dbDataRecord[12].ToString(), dbDataRecord[13].ToString(), dbDataRecord[14].ToString(),
                        dbDataRecord[15].ToString(), dbDataRecord[16].ToString(), dbDataRecord[17].ToString(),
                        dbDataRecord[18].ToString(), dbDataRecord[19].ToString()));
            }
            npgSqlDataReader.Close();
            return books.ToArray();
        }

        public static void DisconnectDatabase() {
            connection.Close();
        }
    }
}
