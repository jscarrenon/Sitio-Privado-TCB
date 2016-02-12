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
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
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
                if (reader[5] != null) user.City = reader[5].ToString();
                if (reader[6] != null) user.Country = reader[6].ToString();
                if (reader[7] != null) user.WorkPhone = reader[7].ToString();
                if (reader[8] != null) user.HomePhone = reader[8].ToString();
                if (reader[9] != null) user.Email = reader[9].ToString();
                if (reader[10] != null) user.CheckingAccount = reader[10].ToString();
                if (reader[11] != null) { user.Bank = reader[11].ToString(); }
                if (reader[12] != null) user.TemporalPassword = reader[12].ToString();
                if (reader[13] != null) { string timestamp = reader[13].ToString(); }
                userList.Add(user);
            }
            return userList;
        }
    }
}