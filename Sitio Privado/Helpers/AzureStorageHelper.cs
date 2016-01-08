using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;

using System.Web;


namespace Sitio_Privado
{
    public class AzureStorageHelper
    {
        private const string ConnectionString = "StorageConnection";

        CloudBlobClient clientConnection;

        public AzureStorageHelper() {
            String connectionString = ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            clientConnection = storageAccount.CreateCloudBlobClient();
        }

        public IEnumerable<CloudBlobContainer> GetContainersList() {
            return clientConnection.ListContainers();
        }

        public IEnumerable<IListBlobItem> GetBlobsFromContainer(string containerName) {
            IEnumerable<IListBlobItem> result = null;
            CloudBlobContainer blobContainer = clientConnection.GetContainerReference(containerName);
            if (blobContainer.Exists()) {
                result = blobContainer.ListBlobs(null, true, BlobListingDetails.All);
            }
            return result;
        }

        /*public void AddBlob(HttpPostedFileBase pic)
        {
            if (pic.ContentLength > 0)
            {
                CloudBlobContainer blobcontainer = GetBLOBRef();
                CloudBlockBlob blob = blobcontainer.GetBlockBlobReference(pic.FileName);
                blob.UploadFromStream(pic.InputStream);
            }
        }

        internal void DeleteBlob(string name)
        {
            Uri ur = new Uri(name);
            string fname = Path.GetFileName(ur.LocalPath);
            CloudBlobContainer blobcontainer = GetBLOBRef();
            CloudBlockBlob blob = blobcontainer.GetBlockBlobReference(fname);
            blob.Delete();
        }*/
    }
}