using Microsoft.WindowsAzure.Storage.Blob;
using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Sitio_Privado.Controllers
{
    public class BlobsController : ApiController
    {
        #region Constants
        private const string GetBlobAction = "GetBlob";
        private const string BlobsControllerName = "Blobs";
        private const int MarginTime = 3;
        #endregion

        private AzureStorageHelper azureStorageHelper = new AzureStorageHelper();

        #region Actions
        public IHttpActionResult GetBlob(string container, string fileName) {
            var blob = azureStorageHelper.GetBlob(container, fileName);

            //Set the expiry time and permissions for the blob.
            //In this case the start time is specified as a few minutes in the past, to mitigate clock skew.
            //The shared access signature will be valid immediately.
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-MarginTime);
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(MarginTime);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read;

            //Generate the shared access signature on the blob, setting the constraints directly on the signature.
            string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

            //Return the URI string for blob.
            return Redirect(blob.Uri + sasBlobToken);
        }
        #endregion
    }
}