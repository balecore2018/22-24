using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static _22_24.Classes.Common;

namespace _22_24.Classes
{
    public class Connection
    {
        public List<User> users = new List<User>();
        public List<Call> calls = new List<Call>();

        public enum tabels
        {
            users, calls
        }

        public string localPath = "";

        // МЕТОД QUERYACCESS (для SELECT запросов)
        public OleDbDataReader QueryAccess(string query)
        {
            try
            {
                localPath = Directory.GetCurrentDirectory();
                string dbPath = localPath + "\\accesbase.accdb";
                string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbPath;

                OleDbConnection connect = new OleDbConnection(connString);
                connect.Open();
                OleDbCommand cmd = new OleDbCommand(query, connect);
                OleDbDataReader reader = cmd.ExecuteReader();
                return reader;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка QueryAccess: " + ex.Message);
                return null;
            }
        }

        // МЕТОД EXECUTENONQUERY (для INSERT/UPDATE/DELETE запросов)
        public bool ExecuteNonQuery(string query)
        {
            try
            {
                localPath = Directory.GetCurrentDirectory();
                string dbPath = localPath + "\\accesbase.accdb";
                string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbPath;

                using (OleDbConnection connect = new OleDbConnection(connString))
                {
                    connect.Open();

                    // Добавляем точку с запятой, если её нет
                    if (!query.Trim().EndsWith(";"))
                    {
                        query = query.Trim() + ";";
                    }

                    OleDbCommand cmd = new OleDbCommand(query, connect);
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка ExecuteNonQuery: " + ex.Message + "\n\nЗапрос: " + query);
                return false;
            }
        }

        public void LoadData(tabels zap)
        {
            try
            {
                string dbPath = Directory.GetCurrentDirectory() + "\\accesbase.accdb";
                string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbPath;

                using (OleDbConnection connect = new OleDbConnection(connString))
                {
                    connect.Open();
                    string query = "SELECT * FROM [" + zap.ToString() + "]";
                    OleDbCommand cmd = new OleDbCommand(query, connect);
                    OleDbDataReader reader = cmd.ExecuteReader();

                    if (zap.ToString() == "users")
                    {
                        users.Clear();
                        while (reader.Read())
                        {
                            User newEl = new User();
                            newEl.id = Convert.ToInt32(reader["Код"]);
                            newEl.phone_num = Convert.ToString(reader["phone_num"]);
                            newEl.fio_user = Convert.ToString(reader["FIO_user"]);
                            newEl.pasport_data = Convert.ToString(reader["pasport_data"]);
                            users.Add(newEl);
                        }
                    }

                    if (zap.ToString() == "calls")
                    {
                        calls.Clear();
                        while (reader.Read())
                        {
                            Call newEl = new Call();
                            newEl.id = Convert.ToInt32(reader["Код"]);
                            newEl.user_id = Convert.ToInt32(reader["user_id"]);
                            newEl.category_call = Convert.ToInt32(reader["category_call"]);
                            newEl.date = Convert.ToString(reader["date"]);
                            newEl.time_start = Convert.ToString(reader["time_start"]);
                            newEl.time_end = Convert.ToString(reader["time_end"]);
                            calls.Add(newEl);
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка LoadData: " + ex.Message);
            }
        }

        public int SetLastId(tabels tabel)
        {
            try
            {
                LoadData(tabel);
                switch (tabel.ToString())
                {
                    case "users":
                        if (users.Count >= 1)
                        {
                            int max_status = users.Max(x => x.id);
                            return max_status + 1;
                        }
                        else return 1;
                    case "calls":
                        if (calls.Count >= 1)
                        {
                            int max_status = calls.Max(x => x.id);
                            return max_status + 1;
                        }
                        else return 1;
                }
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка в SetLastId: " + ex.Message);
                return 1;
            }
        }
    }
}

