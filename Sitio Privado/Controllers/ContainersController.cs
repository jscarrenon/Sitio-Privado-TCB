using Microsoft.WindowsAzure.Storage.Blob;
using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Sitio_Privado.Controllers
{
    public class ContainersController : ApiController
    {
        #region Constants
        private const string GetBlobAction = "GetBlob";
        private const string BlobsControllerName = "Blobs";
        #endregion

        private AzureStorageHelper azureStorageHelper = new AzureStorageHelper();

        #region Actions
        // GET: Containers
        /*public ActionResult Index()
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
        }*/

        [HttpGet]
        public IHttpActionResult GetContainer(string name)
        {
            List<AzureFolder> folders = new List<AzureFolder>();

            CloudBlobContainer container = azureStorageHelper.GetContanerReferenceByName(name);
            IEnumerable<IListBlobItem> blobs = container.ListBlobs(null, true, BlobListingDetails.All);

            foreach (var item in blobs)
            {
                AzureFolder folder = null;
                IEnumerable<AzureFolder> auxFolders = folders.Where(f => f.Name == item.Parent.Prefix.Substring(0, item.Parent.Prefix.Length - 1));
                if (auxFolders.Count() <= 0)
                {
                    folder = new AzureFolder(item.Parent.Prefix.Substring(0, item.Parent.Prefix.Length-1));
                    folders.Add(folder);
                }
                else
                {
                    folder = auxFolders.First();
                }

                //Get the blob.
                CloudBlockBlob blockBlob = new CloudBlockBlob(new Uri(item.Uri.AbsoluteUri));

                var url = this.Url.Link("DefaultApi", new { Controller = BlobsControllerName, Action = GetBlobAction,
                    container = item.Container.Name,
                    fileName = blockBlob.Name
                });

                Blob blob = new Blob { Name = blockBlob.Name.Substring(item.Parent.Prefix.Length, blockBlob.Name.Length - item.Parent.Prefix.Length), Url = url };
                folder.Blobs.Add(blob);
            }

            return Ok(folders);
        }

        #endregion
    }
}