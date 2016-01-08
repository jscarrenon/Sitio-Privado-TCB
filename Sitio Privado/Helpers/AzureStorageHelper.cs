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

        public CloudBlobClient clientConnection;

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
            var blobContainer = clientConnection.GetContainerReference(containerName);
            if (blobContainer.Exists()) {
                result = blobContainer.ListBlobs(null, true, BlobListingDetails.All);
            }
            return result;
        }

        public ICloudBlob GetBlob(string containerName, string fileName)
        {
            // Retrieve reference to a previously created container.
            CloudBlobContainer container = clientConnection.GetContainerReference(containerName);

            // Retrieve reference to a blob named "photo1.jpg".
            return container.GetBlobReferenceFromServer(fileName);
        }
    }
}