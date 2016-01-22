using System;
using System.Web.Mvc;

namespace Sitio_Privado.Controllers
{
    public class JasmineController : Controller
    {
        public ViewResult Run()
        {
            return View("SpecRunner");
        }
    }
}
