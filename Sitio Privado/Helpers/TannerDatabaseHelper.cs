using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sitio_Privado.Helpers
{
    public class TannerDatabaseHelper
    {
        private static string ConnectionString = ConfigurationManager.AppSettings["sql:ConnectionString"];
        private static string UserViewTableName = "dbo.VW_Clientes_CBV";
        private SqlConnection sqlConnection;

        public bool OpenConnection()
        {
            if(sqlConnection != null)
                sqlConnection = new SqlConnection(ConnectionString);

            try
            {
                sqlConnection.Open();
            } 
            catch(Exception e)
            {
                return false;
            }

            return true;
        }

        public void CloseConnection()
        {
            if(sqlConnection != null)
                sqlConnection.Close();
        }

        public IList<TannerUserModel> GetUserList()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM @userViewTable", sqlConnection);
            command.Parameters.Add(new SqlParameter("userViewTable", UserViewTableName));
            SqlDataReader reader = command.ExecuteReader();

            IList<TannerUserModel> userList = new List<TannerUserModel>();
            while(reader.Read())
            {
                TannerUserModel user = new TannerUserModel();
                if (reader[0] != null) user.FullName = (string)reader[0];
                if (reader[1] != null) user.RutID = ((Int32)reader[1]).ToString();
                if (reader[2] != null) user.RutVD = ((Char)reader[2]).ToString();
                if (reader[3] != null) user.WorkAddress = (string)reader[3];
                if (reader[4] != null) user.HomeAddress = (string)reader[4];
            }
            return userList;
        }
    }
}