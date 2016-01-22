using Microsoft.WindowsAzure.Storage.Blob;
using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
        [HttpGet]
        public IHttpActionResult GetContainer(string name)
        {
            //TODO: Check container not found
            List<AzureFolder> folders = new List<AzureFolder>();

            CloudBlobContainer container = azureStorageHelper.GetContanerReferenceByName(name);
            IEnumerable<IListBlobItem> blobs = container.ListBlobs(null, true, BlobListingDetails.All);

            foreach (var item in blobs)
            {
                string folderName = item.Parent.Prefix.Replace("/", "");
                AzureFolder folder = null;
                IEnumerable<AzureFolder> auxFolders = folders.Where(f => f.Name == folderName);
                if (auxFolders.Count() <= 0)
                {
                    
                    folder = new AzureFolder(folderName);
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

                AzureBlob blob = new AzureBlob { Name = Path.GetFileNameWithoutExtension(blockBlob.Name), Url = url };
                folder.Blobs.Add(blob);
            }

            return Ok(folders);
        }

        #endregion
    }
}