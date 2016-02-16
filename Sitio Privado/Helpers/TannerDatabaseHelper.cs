using NLog;
using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Sitio_Privado.Helpers
{
    public class TannerDatabaseHelper
    {
        private static string ConnectionString = ConfigurationManager.AppSettings["sql:ConnectionString"];
        private static int MaxConnectionAttempts = 5;
        private static int ReconnectionTime = 30000;
        private Logger logger;

        public TannerDatabaseHelper(Logger logger)
        {
            this.logger = logger;
        }

        public IList<TannerUserModel> GetUserList()
        {
            using( SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                int currentAttempt = 1;
                while(currentAttempt <= MaxConnectionAttempts)
                {
                    try
                    {
                        logger.Info("Trying to open a database connection. Attempt " + currentAttempt + ".");
                        sqlConnection.Open();
                        SqlCommand command = new SqlCommand("SELECT * FROM dbo.VW_Clientes_CBV", sqlConnection); //TODO: remove number filter
                        logger.Info("Obtaining users from database");
                        SqlDataReader reader = command.ExecuteReader();

                        IList<TannerUserModel> userList = new List<TannerUserModel>();
                        while (reader.Read())
                        {
                            TannerUserModel user = ParseUser(reader);
                            userList.Add(user);
                        }

                        logger.Info(userList.Count + " users obtained from the database.");
                        return userList;
                    }
                    catch (Exception e)
                    {
                        logger.Warn(e.Message);
                        currentAttempt++;
                        System.Threading.Thread.Sleep(ReconnectionTime);
                    }
                }

                return null;
            }
        }

        private TannerUserModel ParseUser(SqlDataReader reader)
        {
            TannerUserModel user = new TannerUserModel();
            if (reader[0] != null) user.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reader[0].ToString().Trim().ToLower());
            if (reader[1] != null) user.Surname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reader[1].ToString().Trim().ToLower());
            if (reader[2] != null) user.RutID = reader[2].ToString().Trim();
            if (reader[3] != null) user.RutVD = reader[3].ToString().Trim().ToUpper();
            if (reader[4] != null) user.WorkAddress = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reader[4].ToString().Trim().ToLower());
            if (reader[5] != null) user.HomeAddress = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reader[5].ToString().Trim().ToLower());
            if (reader[6] != null) user.City = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reader[6].ToString().Trim().ToLower());
            if (reader[7] != null) user.Country = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reader[7].ToString().Trim().ToLower());
            if (reader[8] != null) user.WorkPhone = reader[8].ToString().Trim();
            if (reader[9] != null) user.HomePhone = reader[9].ToString().Trim();
            if (reader[10] != null) user.Email = reader[10].ToString().Trim().ToLower();
            if (reader[11] != null) user.CheckingAccount = reader[11].ToString().Trim();
            if (reader[12] != null) user.Bank = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reader[12].ToString().Trim().ToLower());
            if (reader[13] != null) user.TemporalPassword = reader[13].ToString().Trim();
            if (reader[14] != null) user.UpdatedAt = reader[14].ToString().Trim();

            return user;
        }
    }
}