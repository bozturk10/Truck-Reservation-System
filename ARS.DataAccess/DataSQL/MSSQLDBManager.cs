using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace ARS.DataAccess.DataSQL
{
    public class MSSQLDBManager : IDBManager
    {
        String cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ars_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //String connectionString = " Data Source=sqltest.hayat.com.tr,1434;Initial Catalog=KEAS_ARANDEVU_TEST;User Id=user_keasarandevu;Password=Hke450Gh90;Max Pool Size=1000";

        public MSSQLDBManager(string cs)
        {
            this.cs = cs;
        }

        public MSSQLDBManager() : this(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ars_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
        {
        }

        public DataTable ExecuteQuery(string query, SqlParameter[] parameters)
        {
            SqlConnection sc = new SqlConnection(cs);
            sc.Open();


            SqlCommand scmd = new SqlCommand(query, sc);
            if (parameters != null)
                foreach (var item in parameters)
                    scmd.Parameters.Add(item);
            SqlDataReader dataReader = scmd.ExecuteReader();



            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);

            dataReader.Close();
            sc.Close();
            return dataTable;
        }

        //public void ExecuteUpdate(string query, SqlParameter[] parameters)
        //{
        //    SqlConnection sc = new SqlConnection(cs);
        //    sc.Open();
        //    SqlCommand scmd = new SqlCommand(query, sc);
        //    if (parameters != null)
        //        foreach (var item in parameters)
        //            scmd.Parameters.Add(item);

        //    sc.Open();
        //    scmd.ExecuteNonQuery();
        //    sc.Close();

        //}

        public void InsertIntoTable(string tablename , Dictionary<string, object> columnsValues)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            foreach (var column in columnsValues)
            {
                sqlParams.Add(new SqlParameter("@" + column.Key, column.Value));
            }

            StringBuilder columnNames = new StringBuilder("(");
            StringBuilder columnValues = new StringBuilder();
            foreach (var column in columnsValues)
            {
                columnNames.Append(column.Key + ",");
                columnValues.Append("@"+ column.Value + ",");

            }
            columnNames.Remove(columnNames.Length-1, 1); columnNames.Append(")");
            columnValues.Remove(columnValues.Length - 1, 1); columnValues.Append(")");


            String query = $@"INSERT INTO {tablename} "+ columnNames +" VALUES "+ columnsValues;

            SqlConnection sc = new SqlConnection(cs);
            sc.Open();
            SqlCommand scmd = new SqlCommand(query, sc);
            if (sqlParams != null)
                foreach (var item in sqlParams)
                    scmd.Parameters.Add(item);

            scmd.ExecuteNonQuery();
        }

        public void UpdateTableRow(string tableName, Dictionary<string,object> columnsValues, string keyName, int keyId)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            foreach (var column in columnsValues)
            {
                sqlParams.Add(new SqlParameter("@" + column.Key, column.Value));
            }

            StringBuilder setString = new StringBuilder();
            StringBuilder whereString = new StringBuilder();
            foreach (var column in columnsValues)
            {
                setString.Append("," + column.Key + "=@" + column.Key);
            }
            setString.Remove(0, 1);

            string query = $@"UPDATE {tableName} SET " + setString + " WHERE " + keyName + "=" + keyId;

            SqlConnection sc = new SqlConnection(cs);
            sc.Open();
            SqlCommand scmd = new SqlCommand(query, sc);
            if (sqlParams.Count > 0)
                foreach (var item in sqlParams)
                    scmd.Parameters.Add(item);

            scmd.ExecuteNonQuery();
        }

        public void DeleteTableRow(string tablename, string keyName, int keyId)
        {
            //List<SqlParameter> sqlParams = new List<SqlParameter>();
            //foreach (var column in columnsValues)
            //{
            //    sqlParams.Add(new SqlParameter("@" + column.Key, column.Value));
            //}
            //StringBuilder whereString = new StringBuilder();
            //foreach (var column in columnsValues)
            //{
            //    whereString.Append(column.Key + " = @" + column.Key);
            //}
            //whereString.Remove(0, 1);

            string query = $@"DELETE FROM  {tablename} WHERE " + keyName + "=" + keyId;

            SqlConnection sc = new SqlConnection(cs);
            sc.Open();
            SqlCommand scmd = new SqlCommand(query, sc);
            
            //if (sqlParams != null)
            //    foreach (var item in sqlParams)
            //        scmd.Parameters.Add(item);

            scmd.ExecuteNonQuery();
        }

        


        public void NotifExecuteQuery()
        {
            while (true)
            {
                SqlConnection sc = new SqlConnection(cs);
                sc.Open();
                SqlCommand scmd = new SqlCommand("dbo.DequeueNotif", sc);
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.ExecuteNonQuery();

                Thread.Sleep(10000);
            }
        }

        public DataTable ExecuteQueryWithConn(SqlConnection sc, string query, SqlParameter[] parameters, SqlTransaction tran)
        {
            SqlCommand scmd = new SqlCommand(query, sc, tran);
            if (parameters != null)
                foreach (var item in parameters)
                    scmd.Parameters.Add(item);
            SqlDataReader dataReader = scmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);
            dataReader.Close();
            return dataTable;
        }

        public void ExecuteWithinTransaction(Action<SqlConnection,SqlTransaction> todo)
        {
            SqlConnection sc = new SqlConnection(cs);
            sc.Open();
            var tran = sc.BeginTransaction();
            try
            {
                todo?.Invoke(sc,tran);
                tran.Commit();
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                sc.Close();
            }
        }

    }
}
