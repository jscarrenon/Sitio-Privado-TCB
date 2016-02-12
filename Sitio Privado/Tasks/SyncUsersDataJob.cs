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
        public void Execute(IJobExecutionContext context)
        {
            //TODO: Implement task
            System.Diagnostics.Debug.WriteLine("Test Message");
            TannerDatabaseHelper dbHelper = new TannerDatabaseHelper();

            if (dbHelper.OpenConnection())
            {
                IList<TannerUserModel> userList = dbHelper.GetUserList();
                dbHelper.CloseConnection();
                ProcessUsers(userList);
            }
        }

        private async Task ProcessUsers(IList<TannerUserModel> userList)
        {
            GraphApiClientHelper graphClient = new GraphApiClientHelper();

            foreach(var user in userList)
            {
                GraphApiResponseInfo response = await graphClient.GetUserByRut(user.Rut);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    //Create User
                    System.Diagnostics.Debug.WriteLine("Creating");
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
    }
}