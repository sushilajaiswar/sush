using System;
using System.Data;
using System.Configuration;
using Oracle.DataAccess.Types;
using System.Data.SqlClient;

namespace HdfcErgo.SalesPortal.Shared
{
    /// <summary>
    /// DB helper class
    /// </summary>
    public class DbHelper
    {
        #region " Oracle Helper "
       
        /// <summary>
        /// Gets the data table.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="refCursorOutParamName">Name of the ref cursor out param.</param>
        /// <param name="paramsName">Name of the params.</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string connectionString, string storedProcedureName, string refCursorOutParamName, params OracleParameter[] paramsName)
        {
            var dt = new DataTable();

            using (var con = new SqlConnection(connectionString))
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                foreach (var p in paramsName)
                {
                    com.Parameters.Add(p);
                }

                com.CommandText = storedProcedureName;

                com.Connection.Open();
                com.ExecuteNonQuery();

                var refCursor = (OracleRefCursor)com.Parameters[refCursorOutParamName].Value;
                var da = new OracleDataAdapter();
                da.Fill(dt, refCursor);

                com.Connection.Close();
            }

            return dt;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="outParamName">Name of the out param.</param>
        /// <param name="paramsName">Name of the params.</param>
        /// <returns></returns>
        public static string GetValue(string connectionString, string storedProcedureName, string outParamName, params OracleParameter[] paramsName)
        {
            string outParamValue = string.Empty;

            using (var con = new SqlConnection(connectionString))
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = storedProcedureName;

                foreach (var p in paramsName)
                {
                    com.Parameters.Add(p);
                }

                com.Connection.Open();
                com.ExecuteNonQuery();


                if (com.Parameters[outParamName].Value != null)
                {
                    outParamValue = com.Parameters[outParamName].Value.ToString();
                }
                com.Connection.Close();
            }

            return outParamValue;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="outParamName">Name of the out param.</param>
        /// <param name="paramsName">Name of the params.</param>
        /// <returns></returns>
        public static string GetValue(string storedProcedureName, string outParamName, params OracleParameter[] paramsName)
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["CIMA_DB"].ConnectionString;
            return GetValue(strConnectionString, storedProcedureName, outParamName, paramsName);
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="paramsName">Name of the params.</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, string storedProcedureName, params OracleParameter[] paramsName)
        {
            int affectedRows;
            using (var con = new SqlConnection(connectionString))
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = storedProcedureName;

                foreach (var p in paramsName)
                {
                    com.Parameters.Add(p);
                }

                com.Connection.Open();

                affectedRows = com.ExecuteNonQuery();

                com.Connection.Close();
            }
            return affectedRows;
        }

        public static int ExecuteNonQuery(string connectionString, string query)
        {
            int affectedRows;
            using (var con = new SqlConnection(connectionString))
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.Text;
                com.CommandText = query;

                com.Connection.Open();

                affectedRows = com.ExecuteNonQuery();

                com.Connection.Close();
            }
            return affectedRows;
        }

        /// <summary>
        /// Executes the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="paramsName">Name of the params.</param>
        public static void Execute(string connectionString, string storedProcedureName, params OracleParameter[] paramsName)
        {
            using (var con = new SqlConnection(connectionString))
            {
                OracleCommand com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = storedProcedureName;

                foreach (OracleParameter p in paramsName)
                {
                    com.Parameters.Add(p);
                }

                com.Connection.Open();
                com.ExecuteNonQuery();

                com.Connection.Close();
            }

        }

        /// <summary>
        /// Gets the data set.
        /// </summary>
        /// <param name="strConnectionString">The STR connection string.</param>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="paramsName">Name of the params.</param>
        /// <returns></returns>
        public static DataSet GetDataSet(string strConnectionString, String storedProcedureName, params OracleParameter[] paramsName)
        {
            var ds = new DataSet();

            using (var con = new SqlConnection(strConnectionString))
            {
                OracleCommand com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = storedProcedureName;
                foreach (OracleParameter p in paramsName)
                {
                    com.Parameters.Add(p);
                }
                com.Connection.Open();

                var da = new OracleDataAdapter(com);
                da.Fill(ds);
                com.Connection.Close();
            }
            return ds;
        }

        public static DataSet GetDataSetFromQuery(string strConnectionString, String query)
        {
            var ds = new DataSet();

            using (var con = new SqlConnection(strConnectionString))
            {
                OracleCommand com = con.CreateCommand();
                com.CommandType = CommandType.Text;
                com.CommandText = query;
                com.Connection.Open();

                var da = new OracleDataAdapter(com);
                da.Fill(ds);
                com.Connection.Close();
            }
            return ds;
        }

        public static void ExecuteTextQuery(string queryText, params OracleParameter[] paramsName)
        {

            string strConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            using (SqlConnection con = new SqlConnection(strConnectionString))
            {
                OracleCommand com = con.CreateCommand();
                com.CommandType = System.Data.CommandType.Text;
                com.CommandText = queryText;

                if (paramsName != null)
                {
                    foreach (OracleParameter p in paramsName)
                    {
                        com.Parameters.Add(p);
                    }
                }
                com.Connection.Open();
                com.ExecuteNonQuery();

                com.Connection.Close();
            }

        }
        /// <summary>
        /// Gets the Data Table
        /// </summary>
        /// <param name="strConnectionString">the connection string</param>
        /// <param name="query">the query</param>
        /// <returns></returns>
        public static DataTable GetDataTableFromQuery(string strConnectionString, String query)
        {
            DataTable dt = null;
            SqlConnection myConnection = new SqlConnection(strConnectionString);
            myConnection.Open();
            OracleCommand ocmd = new OracleCommand();
            ocmd.Connection = myConnection;
            ocmd.CommandText = query;
            ocmd.CommandType = CommandType.Text;
            OracleDataAdapter da = new OracleDataAdapter(ocmd);
            dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Gets Dataset with Parameter Array as input
        /// </summary>
        /// <param name="strConnectionString">the Connection String</param>
        /// <param name="storedProcedureName">the Procedure Name</param>
        /// <param name="paramsName">the Param Name</param>
        /// <returns>the Data Set</returns>
        public static DataSet GetDataSetWithParameterArray(string strConnectionString, String storedProcedureName, OracleParameter[] paramsName)
        {
            var ds = new DataSet();

            using (var con = new SqlConnection(strConnectionString))
            {
                OracleCommand com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = storedProcedureName;
                foreach (OracleParameter p in paramsName)
                {
                    com.Parameters.Add(p);
                }
                com.Connection.Open();

                var da = new OracleDataAdapter(com);
                da.Fill(ds);
                com.Connection.Close();
            }
            return ds;
        }

        #endregion

        #region " SQL Helper "

        /// <summary>
        /// Gets Data Set with Parameter Array Sql
        /// </summary>
        /// <param name="strConnectionString">the sql connection string</param>
        /// <param name="storedProcedureName">the procedure name</param>
        /// <param name="paramsName">the param name</param>
        /// <returns></returns>
        public static DataSet GetDataSetWithParameterArraySql(string strConnectionString, string storedProcedureName, params SqlParameter[] paramsName)
        {
            var ds = new DataSet();

            SqlConnection objcon = new SqlConnection(strConnectionString);
            SqlCommand objCommand = new SqlCommand(storedProcedureName, objcon);
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandTimeout = 0;

            foreach (SqlParameter p in paramsName)
            {
                objCommand.Parameters.Add(p);
            }

            SqlDataAdapter objDataAdapter = new SqlDataAdapter(objCommand);
            objDataAdapter.Fill(ds);
            objcon.Close();

            return ds;
        }

        /// <summary>
        /// Gets Data Table with Parameter Array Sql
        /// </summary>
        /// <param name="strConnectionString">the sql connection string</param>
        /// <param name="storedProcedureName">the procedure name</param>
        /// <param name="paramsName">the param name</param>
        /// <returns></returns>
        public static DataTable GetDataTableWithParameterArraySql(string strConnectionString, string storedProcedureName, params SqlParameter[] paramsName)
        {
            var dt = new DataTable();

            SqlConnection objcon = new SqlConnection(strConnectionString);
            SqlCommand objCommand = new SqlCommand(storedProcedureName, objcon);
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandTimeout = 0;

            foreach (SqlParameter p in paramsName)
            {
                objCommand.Parameters.Add(p);
            }

            SqlDataAdapter objDataAdapter = new SqlDataAdapter(objCommand);
            objDataAdapter.Fill(dt);
            objcon.Close();

            return dt;
        }

        /// <summary>
        /// Execute Non Query for Sql to get Output parameter
        /// </summary>
        /// <param name="strConnectionString"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="paramsName"></param>
        /// <returns></returns>
        public static string ExecuteNonQuerySql(string strConnectionString, string storedProcedureName, params SqlParameter[] paramsName)
        {
            var dt = new DataTable();

            SqlConnection objcon = new SqlConnection(strConnectionString);
            SqlCommand objCommand = new SqlCommand(storedProcedureName, objcon);
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandTimeout = 0;
            string _outputParameterName = string.Empty;
            string result = string.Empty;

            foreach (SqlParameter p in paramsName)
            {
                if (p.Direction==ParameterDirection.Output)
                {
                    _outputParameterName = p.ParameterName;
                }
                objCommand.Parameters.Add(p);
            }

            objcon.Open();
            result = objCommand.ExecuteNonQuery().ToString();
            if (!string.IsNullOrEmpty(_outputParameterName))
            {
                result = Convert.ToString(objCommand.Parameters[_outputParameterName].Value);
            }
            objcon.Close();

            return result;
        }

        /// <summary>
        /// Executes Text Query Sql
        /// </summary>
        /// <param name="strConnectionString"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataTable ExecuteTextQuerySql(string strConnectionString, string query)
        {
            var dt = new DataTable();

            SqlConnection objcon = new SqlConnection(strConnectionString);
            SqlCommand objCommand = new SqlCommand(query, objcon);

            SqlDataAdapter da = new SqlDataAdapter(objCommand);
            da.Fill(dt);

            return dt;
        }

        #endregion

    }
}
