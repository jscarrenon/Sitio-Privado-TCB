using Microsoft.WindowsAzure.Storage.Blob;
using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitio_Privado.Controllers
{
    public class ContainersController : Controller
    {
        #region Constants
        private const string GetBlobsAction = "Index";
        private const string BlobsControllerName = "Blobs";
        #endregion

        private AzureStorageHelper azureStorageHelper = new AzureStorageHelper();

        #region Actions
        // GET: Containers
        public ActionResult Index()
        {
            List<Container> containers = new List<Container>();

            foreach (var item in azureStorageHelper.GetContainersList()) {
                //Construct the URI for retrieving the blobs.
                var uriString = Url.Action(GetBlobsAction, BlobsControllerName,
                        routeValues: new { container = item.Name },
                        protocol: Request.Url.Scheme);

                Container container = new Container() { Name = item.Name, Link = uriString};
                containers.Add(container);
            }
            return View(containers);
        }

        public ActionResult GetContainer(string name)
        {
            return null;
        }

        #endregion
    }
}