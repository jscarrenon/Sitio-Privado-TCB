using Microsoft.WindowsAzure.Storage.Blob;
using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
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
                Blob blob = new Blob { Name = cbb.Name };
                blobs.Add(blob);
            }
            return View(blobs);
        }

        /*[HttpPost]
        public ActionResult Add(HttpPostedFileBase pic)
        {
            objbl.AddBlob(pic);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public string Remove(string name)
        {
            objbl.DeleteBlob(name);
            return "Blob Removed Successfully";
        }*/
    }
}