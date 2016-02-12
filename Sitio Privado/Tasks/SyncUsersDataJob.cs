using Quartz;
using Sitio_Privado.Helpers;
using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            }
        }
    }
}