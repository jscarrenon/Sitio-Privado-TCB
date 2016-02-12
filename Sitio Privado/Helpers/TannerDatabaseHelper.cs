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
        private SqlConnection sqlConnection;

        public TannerDatabaseHelper()
        {
            sqlConnection = new SqlConnection(ConnectionString);
        }

        public bool OpenConnection()
        {
            if(sqlConnection == null)
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
            SqlCommand command = new SqlCommand("SELECT * FROM dbo.VW_Clientes_CBV", sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            IList<TannerUserModel> userList = new List<TannerUserModel>();
            while(reader.Read())
            {
                TannerUserModel user = new TannerUserModel();
                if (reader[0] != null) user.FullName = reader[0].ToString();
                if (reader[1] != null) user.RutID = reader[1].ToString();
                if (reader[2] != null) user.RutVD = reader[2].ToString();
                if (reader[3] != null) user.WorkAddress = reader[3].ToString();
                if (reader[4] != null) user.HomeAddress = reader[4].ToString();
                userList.Add(user);
            }
            return userList;
        }
    }
}