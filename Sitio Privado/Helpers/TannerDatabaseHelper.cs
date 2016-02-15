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

        public static IList<TannerUserModel> GetUserList()
        {
            using( SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                IList<TannerUserModel> userList = new List<TannerUserModel>();
                try
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand("SELECT TOP 20 * FROM dbo.VW_Clientes_CBV", sqlConnection); //TODO: remove number filter
                    SqlDataReader reader = command.ExecuteReader();


                    while (reader.Read())
                    {
                        TannerUserModel user = new TannerUserModel();
                        if (reader[0] != null) user.Name = reader[0].ToString();
                        if (reader[1] != null) user.Surname = reader[1].ToString();
                        if (reader[2] != null) user.RutID = reader[2].ToString();
                        if (reader[3] != null) user.RutVD = reader[3].ToString();
                        if (reader[4] != null) user.WorkAddress = reader[4].ToString();
                        if (reader[5] != null) user.HomeAddress = reader[5].ToString();
                        if (reader[6] != null) user.City = reader[6].ToString();
                        if (reader[7] != null) user.Country = reader[7].ToString();
                        if (reader[8] != null) user.WorkPhone = reader[8].ToString();
                        if (reader[9] != null) user.HomePhone = reader[9].ToString();
                        if (reader[10] != null) user.Email = reader[10].ToString();
                        if (reader[11] != null) user.CheckingAccount = reader[11].ToString();
                        if (reader[12] != null) user.Bank = reader[12].ToString();
                        if (reader[13] != null) user.TemporalPassword = reader[13].ToString();
                        if (reader[14] != null) { string timestamp = reader[14].ToString(); }
                        userList.Add(user);
                    }
                }
                catch (Exception e)
                {

                }
                return userList;
            }
        }
    }
}