using Microsoft.WindowsAzure.Storage.Blob;
using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;
using Sitio_Privado.Filters;

namespace Sitio_Privado.Controllers
{
    [AuthorizeWithGroups]
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
            CloudBlobContainer container = azureStorageHelper.GetContainerReferenceByName(name);
            if (container.Exists())
            {
                List<AzureFolderModel> folders = new List<AzureFolderModel>();
                IEnumerable<IListBlobItem> blobs = container.ListBlobs(null, true, BlobListingDetails.All);
                blobs = blobs.OrderBy(item => item.Uri.LocalPath);

                foreach (var item in blobs)
                {
                    string[] foldersNameFromPath = item.Parent.Prefix.Split(new char[]{ '/' }, StringSplitOptions.RemoveEmptyEntries);

                    AzureFolderModel leaf = GetFolderStructure(folders, foldersNameFromPath);

                    AzureBlobModel blob = GetBlobFromUri(item.Uri.AbsoluteUri);
                    leaf.Blobs.Add(blob);
                }

                return Ok(folders);
            }

            return NotFound();
        }

        private AzureFolderModel GetFolderStructure(List<AzureFolderModel> folders, string[] foldersPath)
        {
            List<AzureFolderModel> parentFolders = folders;
            AzureFolderModel parent = null;
            foreach (var folderName in foldersPath)
            {
                AzureFolderModel folder = null;
                IEnumerable<AzureFolderModel> tempFolders = parentFolders.Where(f => f.Name == folderName);
                if (tempFolders.Count() <= 0)
                {
                    folder = new AzureFolderModel(folderName);
                    parentFolders.Add(folder);
                }
                else
                {
                    folder = tempFolders.First();
                }
                parentFolders = folder.Folders;
                parent = folder;
            }

            return parent;
        }

        private AzureBlobModel GetBlobFromUri(string absoluteUri) {
            //Get the blob.
            CloudBlockBlob blockBlob = new CloudBlockBlob(new Uri(absoluteUri));

            var url = this.Url.Link("DefaultApi", new
            {
                Controller = BlobsControllerName,
                Action = GetBlobAction,
                container = blockBlob.Container.Name,
                fileName = blockBlob.Name
            });

            string blobName = Regex.Replace(Path.GetFileNameWithoutExtension(blockBlob.Name), "[^a-zA-Z0-9\u00C0-\u017F]", " ", RegexOptions.Compiled);
            blobName = blobName.Substring(0, 1).ToUpper() + blobName.Substring(1).ToLower();

            AzureBlobModel blob = new AzureBlobModel { Name = blobName, Url = url };

            return blob;
        }

        #endregion
    }
}