using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ARS.DataAccess.DataSQL
{
   
    public class OracleDBManager : IDBManager
    {  
        public DataTable ExecuteQuery(string query,SqlParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public void InsertIntoTable(string tablename, Dictionary<string, object> columnsValues)
    
        {
            throw new NotImplementedException();
        }

        public void UpdateTableRow(string tableName, Dictionary<string, object> columnsValues, string keyName, int keyId)
        {
            throw new NotImplementedException();
        }
        public void DeleteTableRow(string tablename, Dictionary<string, object> columnsValues)
        {
            throw new NotImplementedException();
        }

        public void DeleteTableRow(string tablename, string keyName, int keyId)
        {
            throw new NotImplementedException();
        }

        public void NotifExecuteQuery()
        {
            throw new NotImplementedException();
        }

        public void ExecuteWithinTransaction(Action<SqlConnection> todo)
        {
            throw new NotImplementedException();
        }

        public DataTable ExecuteQueryWithConn(SqlConnection sc, string query, SqlParameter[] parameters,SqlTransaction tran)
        {
            throw new NotImplementedException();
        }

        public void ExecuteWithinTransaction(Action<SqlConnection, SqlTransaction> todo)
        {
            throw new NotImplementedException();
        }
    }
}
