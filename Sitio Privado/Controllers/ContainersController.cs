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
            CloudBlobContainer container = azureStorageHelper.GetContanerReferenceByName(name);
            if (container.Exists())
            {
                List<AzureFolder> folders = new List<AzureFolder>();
                IEnumerable<IListBlobItem> blobs = container.ListBlobs(null, true, BlobListingDetails.All);

                foreach (var item in blobs)
                {
                    string[] foldersNameFromPath = item.Parent.Prefix.Split(new char[]{ '/' }, StringSplitOptions.RemoveEmptyEntries);
                    string rootName = foldersNameFromPath[0];
                    AzureFolder folder = null;
                    IEnumerable<AzureFolder> auxFolders = folders.Where(f => f.Name == rootName);
                    if (auxFolders.Count() <= 0)
                    {

                        folder = new AzureFolder(rootName);
                        folders.Add(folder);
                    }
                    else
                    {
                        folder = auxFolders.First();
                    }

                    AzureFolder parent = folder;

                    for (int i = 1; i < foldersNameFromPath.Length; i++) {
                        AzureFolder child = new AzureFolder(foldersNameFromPath[i]);
                        parent.Folders.Add(child);
                        parent = child;
                    }

                    AzureBlob blob = GetBlobFromUri(item.Uri.AbsoluteUri);
                    parent.Blobs.Add(blob);
                }

                return Ok(folders);
            }

            return NotFound();
        }

        private AzureBlob GetBlobFromUri(string absoluteUri) {
            //Get the blob.
            CloudBlockBlob blockBlob = new CloudBlockBlob(new Uri(absoluteUri));

            var url = this.Url.Link("DefaultApi", new
            {
                Controller = BlobsControllerName,
                Action = GetBlobAction,
                container = blockBlob.Container.Name,
                fileName = blockBlob.Name
            });

            AzureBlob blob = new AzureBlob { Name = Path.GetFileNameWithoutExtension(blockBlob.Name), Url = url };

            return blob;
        }

        #endregion
    }
}