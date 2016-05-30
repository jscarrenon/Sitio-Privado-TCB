using Sitio_Privado.Extras;
using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sitio_Privado.Controllers
{
    public class UsersController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult PasswordRecovery(PasswordRecoveryModel model)
        {
            //Rut validation
            if (ModelState.IsValid)
            {
                string id = ExtraHelpers.FormatRutToId(model.Rut);

                //Check rut in db
                if (model.Rut != null)
                {
                    //TODO Send mail

                    return Json("Una nueva contraseña temporal ha sido enviada a su correo electrónico.");
                }
                else
                {
                    string message = "El Rut ingresado no es Cliente de Tanner, para cualquier duda contacte a mes de atención de clientes al NNNN.";

                    var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(message),
                        ReasonPhrase = "Rut no encontrado"
                    };

                    throw new HttpResponseException(resp);
                }
            }
            else
            {
                string message = string.Join(". ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(message),
                    ReasonPhrase = "Rut inválido"
                };
                
                throw new HttpResponseException(resp);
            }
        }
    }
}
