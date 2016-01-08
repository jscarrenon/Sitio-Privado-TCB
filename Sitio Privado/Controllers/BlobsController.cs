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
        private const string GetBlobAction = "GetBlob2";
        private const string BlobsControllerName = "Blobs";

        private AzureStorageHelper azureStorageHelper = new AzureStorageHelper();

        // GET: Blobs
        public ActionResult Index(string container)
        {
            List<Blob> blobs = new List<Blob>();
            foreach (var item in azureStorageHelper.GetBlobsFromContainer(container)) {
                CloudBlockBlob cbb = new CloudBlockBlob(new Uri(item.Uri.AbsoluteUri));
                var uriString = Url.Action(GetBlobAction, BlobsControllerName,
                        routeValues: new { container = item.Container.Name, fileName = cbb.Name },
                        protocol: Request.Url.Scheme);
                Blob blob = new Blob { Name = cbb.Name, Url = uriString };
                blobs.Add(blob);
            }
            return View(blobs);
        }

        public ActionResult GetBlob(string container, string fileName)
        {
            var blob = azureStorageHelper.GetBlob(container, fileName);
            Response.AddHeader("Content-Disposition", "filename=" + fileName);
            Response.AddHeader("Content-Length", blob.Properties.Length.ToString());
            Response.ContentType = blob.Properties.ContentType;
            blob.DownloadToStream(Response.OutputStream);
            return new EmptyResult();
        }

        public ActionResult GetBlob2(string container, string fileName) {
            // Set the expiry time and permissions for the blob.
            //In this case the start time is specified as a few minutes in the past, to mitigate clock skew.
            //The shared access signature will be valid immediately.
            var blob = azureStorageHelper.GetBlob(container, fileName);
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5);
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(5);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read;

            //Generate the shared access signature on the blob, setting the constraints directly on the signature.
            string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

            //Return the URI string for the container, including the SAS token.
            return Redirect(blob.Uri + sasBlobToken);
        }
    }
}