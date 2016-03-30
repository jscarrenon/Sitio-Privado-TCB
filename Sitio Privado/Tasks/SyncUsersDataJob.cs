using NLog;
using Quartz;
using Sitio_Privado.Extras;
using Sitio_Privado.Helpers;
using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sitio_Privado.Tasks
{
    public class SyncUsersDataJob : IJob
    {
        private GraphApiClientHelper graphClient;
        private static Logger logger = LogManager.GetLogger("MigrationLog");

        public async void Execute(IJobExecutionContext context)
        {
            logger.Info("Starting synchronization task.");
            graphClient = new GraphApiClientHelper();
            TannerDatabaseHelper tannerDatabaseHelper = new TannerDatabaseHelper(logger);
            logger.Info("Retrieving users from database.");
            IList<TannerUserModel> userList = tannerDatabaseHelper.GetUserList();
            if(userList != null)
            {
                await ProcessUsers(userList);
            }
            else
            {
                logger.Warn("The task couldn't obtain the user list from the database");
            }
            logger.Info("Task finished");
        }

        private async Task ProcessUsers(IList<TannerUserModel> userList)
        {
            logger.Info("Processing user list.");
            foreach (var user in userList)
            {
                logger.Info("Checking if user " + user.Rut + " already exists");
                GraphApiResponseInfo response = await graphClient.GetUserByRut(user.Rut);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    //Create User
                    logger.Info("User " + user.Rut + " not found. Creating user");
                    GraphUserModel graphUser = GetGraphUserModel(user);
                    GraphApiResponseInfo createResponse = await graphClient.CreateUser(graphUser, false);
                    if(createResponse.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        logger.Info("User " + user.Rut + " created");
                    }
                    else
                    {
                        logger.Warn("User not created. Error: " + createResponse.Message);
                    }
                }

                else if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Update User
                    logger.Info("User " + user.Rut + " found. Updating user");
                    GraphUserModel graphUser = GetGraphUserModel(user);
                    GraphApiResponseInfo updateResponse = await graphClient.UpdateUser(response.User.ObjectId, graphUser, false);
                    if (updateResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        logger.Info("User " + user.Rut + " updated");
                    }
                    else
                    {
                        logger.Warn("User not updated. Error: " + updateResponse.Message);
                    }
                }
                else
                {
                    //Error
                    logger.Info("Error retrieving user. Error: " + response.Message);
                }
            }
        }

        private GraphUserModel GetGraphUserModel(TannerUserModel user)
        {
            GraphUserModel graphUser = new GraphUserModel();
            graphUser.Name = user.Name;
            graphUser.Rut = user.Rut;
            graphUser.Surname = user.Surname;
            graphUser.WorkAddress = user.WorkAddress;
            graphUser.HomeAddress = user.HomeAddress;
            graphUser.WorkPhone = user.WorkPhone;
            graphUser.HomePhone = user.HomePhone;
            graphUser.Email = user.Email;
            graphUser.Country = user.Country;
            graphUser.City = user.City;
            graphUser.Bank = user.Bank;
            graphUser.CheckingAccount = user.CheckingAccount;
            graphUser.TemporalPassword = PasswordGeneratorHelper.GeneratePassword();
            graphUser.UpdatedAt = DateTime.Now.ToString();
            return graphUser;
        }
    }
}