using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ARS.DataAccess.DataSQL
{
    public interface IDBManager
    {
        void ExecuteWithinTransaction(Action<SqlConnection, SqlTransaction> todo);
        DataTable ExecuteQuery(string query, SqlParameter[] parameters);
        DataTable ExecuteQueryWithConn(SqlConnection sc, string query, SqlParameter[] parameters, SqlTransaction tran);
        //void ExecuteUpdate(string query, SqlParameter[] parameters);
        void InsertIntoTable(string tablename, Dictionary<string, object> columnsValues);
        void UpdateTableRow(string tableName, Dictionary<string, object> columnsValues , string keyName, int keyId);
        void DeleteTableRow(string tablename, string keyName, int keyId);
        void NotifExecuteQuery();
    }
}
