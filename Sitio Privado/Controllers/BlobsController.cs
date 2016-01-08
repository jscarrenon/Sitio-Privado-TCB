using Microsoft.WindowsAzure.Storage.Blob;
using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitio_Privado.Controllers
{
    public class BlobsController : Controller
    {
        #region Constants
        private const string GetBlobAction = "GetBlob";
        private const string BlobsControllerName = "Blobs";
        private const int MarginTime = 3;
        #endregion

        private AzureStorageHelper azureStorageHelper = new AzureStorageHelper();

        #region Actions
        // GET: Blobs
        public ActionResult Index(string container)
        {
            List<Blob> blobs = new List<Blob>();

            foreach (var item in azureStorageHelper.GetBlobsFromContainer(container)) {
                //Get the blob.
                CloudBlockBlob blockBlob = new CloudBlockBlob(new Uri(item.Uri.AbsoluteUri));

                //Construct the URI for accessing the blob through the controller.
                var uriString = Url.Action(GetBlobAction, BlobsControllerName,
                        routeValues: new { container = item.Container.Name, fileName = blockBlob.Name },
                        protocol: Request.Url.Scheme);

                Blob blob = new Blob { Name = blockBlob.Name, Url = uriString };
                blobs.Add(blob);
            }

            return View(blobs);
        }

        public ActionResult GetBlob(string container, string fileName) {
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