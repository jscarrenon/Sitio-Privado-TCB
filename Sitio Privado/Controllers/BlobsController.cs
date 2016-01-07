using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitio_Privado.Controllers
{
    public class BlobsController : Controller
    {
        BlBlobs objbl = new BlBlobs();
        // GET: Blobs
        public ActionResult Index()
        {
            //return View(objbl.GetBlobList());
            return View(BlBlobs.GetContainerList());
        }
        [HttpPost]
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
        }
    }
}