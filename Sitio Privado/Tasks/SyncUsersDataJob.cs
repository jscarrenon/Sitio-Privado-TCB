using Quartz;
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
        GraphApiClientHelper graphClient;

        public async void Execute(IJobExecutionContext context)
        {
            //TODO: Implement task
            System.Diagnostics.Debug.WriteLine("Test Message");
            graphClient = new GraphApiClientHelper();
            IList<TannerUserModel> userList = TannerDatabaseHelper.GetUserList();
            await ProcessUsers(userList);
        }

        private async Task ProcessUsers(IList<TannerUserModel> userList)
        {
            foreach(var user in userList)
            {
                GraphApiResponseInfo response = await graphClient.GetUserByRut(user.Rut);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    //Create User
                    System.Diagnostics.Debug.WriteLine("Creating");
                    await CreateUser(user);
                }

                else if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Update User
                    System.Diagnostics.Debug.WriteLine("Updating");
                }
                else
                {
                    //Error
                    System.Diagnostics.Debug.WriteLine("Error");
                }
            }
        }

        private async Task CreateUser(TannerUserModel user)
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
            graphUser.TemporalPassword = user.TemporalPassword + "Kunder2015";//TODO

            GraphApiResponseInfo response = await graphClient.CreateUser(graphUser);
        }
    }
}