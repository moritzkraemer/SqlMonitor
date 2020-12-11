using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlMonitor
{


    class DatabaseHandler
    {
        SqlConnection SqlConnection;

        public DatabaseHandler(string host, string username, string password)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            sqlConnectionStringBuilder.DataSource = host;
            sqlConnectionStringBuilder.UserID = username;
            sqlConnectionStringBuilder.Password = password;
            SqlConnection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
        }

        public void setDatabase(string database)
        {
            SqlCommand command = new SqlCommand("USE " + database, SqlConnection);
            //command.Parameters.Add("@db", SqlDbType.VarChar);
            //command.Parameters["@db"].Value = database;

            try
            {
                SqlConnection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public DataTable RunQuery(string query)
        {
            DataTable result = new DataTable();
            SqlCommand command = new SqlCommand(query, SqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            try
            {
                sqlDataAdapter.Fill(result);
            }
            catch (Exception ex)
            {
                sqlDataAdapter.Dispose();
                throw ex;
            }

            sqlDataAdapter.Dispose();
            return result;
        }
    }
}
