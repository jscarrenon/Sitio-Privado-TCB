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
        private AzureStorageHelper azureStorageHelper = new AzureStorageHelper();

        // GET: Blobs
        public ActionResult Index(string container)
        {
            List<Blob> blobs = new List<Blob>();
            foreach (var item in azureStorageHelper.GetBlobsFromContainer(container)) {
                CloudBlockBlob cbb = new CloudBlockBlob(new Uri(item.Uri.AbsoluteUri));
                var uriString = Url.Action("GetBlob", "Blobs",
                        routeValues: new { container = item.Container.Name, fileName = cbb.Name },
                        protocol: Request.Url.Scheme);
                Blob blob = new Blob { Name = cbb.Name, //Url = item.StorageUri.PrimaryUri.ToString()
                    Url = uriString };
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
    }
}