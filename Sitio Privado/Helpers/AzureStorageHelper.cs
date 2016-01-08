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
        #region Constants
        private const string ConnectionString = "StorageConnection";
        #endregion

        #region Private Fields
        public CloudBlobClient clientConnection;
        #endregion

        #region Constructors
        public AzureStorageHelper() {
            String connectionString = ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            clientConnection = storageAccount.CreateCloudBlobClient();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Method for obtaining a list with the containers available in the Azure Storage Account
        /// </summary>
        /// <returns>A list with the containers</returns>
        public IEnumerable<CloudBlobContainer> GetContainersList() {
            return clientConnection.ListContainers();
        }

        /// <summary>
        /// Gets a list of blobs from a particular container
        /// </summary>
        /// <param name="containerName">Name of the container</param>
        /// <returns>The list of blobs</returns>
        public IEnumerable<IListBlobItem> GetBlobsFromContainer(string containerName) {
            IEnumerable<IListBlobItem> result = null;
            //Reference to the container
            var blobContainer = clientConnection.GetContainerReference(containerName);

            //If the container exists, the blob list is retrieved
            if (blobContainer.Exists()) {
                result = blobContainer.ListBlobs(null, true, BlobListingDetails.All);
            }
            return result;
        }

        /// <summary>
        /// Gets a reference to a particular blob
        /// </summary>
        /// <param name="containerName">Name of the container</param>
        /// <param name="fileName">Name of the blob</param>
        /// <returns>The corresponding blob</returns>
        public ICloudBlob GetBlob(string containerName, string fileName)
        {
            // Retrieve reference to a previously created container.
            CloudBlobContainer container = clientConnection.GetContainerReference(containerName);

            // Retrieve reference to the blob
            return container.GetBlobReferenceFromServer(fileName);
        }
        #endregion
    }
}