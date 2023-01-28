using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;


namespace LicenseMailManagement
{
    public class SQLiteMananger
    {
        private const string SQLITE_CONNECTION = @"Data Source={0}";

        private SQLiteConnection m_sqliteConnection;

        public void CreateDB(string strPath)
        {
            string strConnection = string.Format(SQLITE_CONNECTION, strPath);
            m_sqliteConnection = new SQLiteConnection(strConnection);
        }

        public bool OpenDB()
        {
            if (null == m_sqliteConnection.OpenAndReturn())
                return false;


            return true;
        }

    }
}
