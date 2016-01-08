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
        private AzureStorageHelper azureStorageHelper = new AzureStorageHelper();

        // GET: Containers
        public ActionResult Index()
        {
            List<Container> containers = new List<Container>();
            foreach (CloudBlobContainer item in azureStorageHelper.GetContainersList()) {
                var uriString = Url.Action("Index", "Blobs",
                        routeValues: new { container = item.Name },
                        protocol: Request.Url.Scheme);
                Container container = new Container() { Name = item.Name, Link = uriString};
                containers.Add(container);
            }
            return View(containers);
        }
    }
}