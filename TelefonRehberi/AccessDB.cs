using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
namespace TelefonRehberi
{
    public partial class AccessDB
    {
        public Dictionary<string, string> tables = new Dictionary<string, string>() { };
        string dbFile;
        OleDbConnection connection;
        public AccessDB(string dbFile)
        {
            this.dbFile = dbFile;
            connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbFile);
            connection.Open();
        }
        public void AddNewTable(string key, string table)
        {
            tables[key] = table;
        }

        public string SubString(string text, int start, int end)
        {
            string last = "";
            for (int i = start; i < end && i < text.Length; i++)
            {
                last += text[i];
            }
            return last;
        }

        public DataTable GetRows(string tableKey, string condition = "")
        {
            string table = tables[tableKey];
            DataTable dt = new DataTable();
            new OleDbDataAdapter("SELECT * FROM " + table + " " + condition, this.connection).Fill(dt);
            return dt;
        }
        public void ExecuteQuery(string query)
        {
            new OleDbCommand(query, connection).ExecuteNonQuery();
        }
        public string CreateExactCondition(Dictionary<string, string> data)
        {
            string sqlString = " WHERE ";
            if (data.Keys.Count < 1)
                return "";
            foreach (var item in data)
            {
                sqlString += (item.Key + "='" + item.Value + "' and ");
            };
            sqlString = this.SubString(sqlString, 0, sqlString.Length - 5);
            return sqlString;
        }
        public string CreateLikeCondition(string key, string value)
        {
            string sqlString = " WHERE " + key + " LIKE '*"+ value + "*'";
            return sqlString;
        } 
        public string CreateLikeCondition(string[] keys, string value)
        {
            string sqlString = " WHERE ";
            foreach (string key in keys)
            {
                sqlString += (key + " LIKE '%" + value + "%' or ");
            }
            sqlString = this.SubString(sqlString, 0, sqlString.Length - 3);
            return sqlString;
        }
        public string CreateDeleteQueryString(string tableKey, string condition = "")
        {
            string table = tables[tableKey];
            return "DELETE FROM " + table + " " + condition;
        }
        public string CreateUpdateQueryString(string tableKey, Dictionary<string, string> data, string condition = "")
        {
            string sqlString = "UPDATE " + tableKey + " SET ";
            foreach (var item in data)
            {
                sqlString += (item.Key + "='" + item.Value + "', ");
            }
            sqlString = this.SubString(sqlString, 0, sqlString.Length - 2) + " " + condition;
            return sqlString;
        }
        public string CreateInsertIntoQueryString(string tableKey, Dictionary<string, string> data)
        {
            string table = tables[tableKey];
            string sqlString = "INSERT INTO " + table + " (";
            foreach (var item in data)
            {
                sqlString += item.Key + ", ";
            }
            sqlString = SubString(sqlString, 0, sqlString.Length - 2) + ") VALUES (";
            foreach (var item in data)
            {
                sqlString += "'" + item.Value + "', ";
            }
            sqlString = SubString(sqlString, 0, sqlString.Length - 2) + ")";
            sqlString.Substring(0, sqlString.Length - 2);
            return sqlString;
        }
    }
}