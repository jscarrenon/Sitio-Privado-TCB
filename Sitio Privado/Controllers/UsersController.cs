﻿using Sitio_Privado.Extras;
using Sitio_Privado.Helpers;
using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sitio_Privado.Controllers
{
    public class UsersController : ApiController
    {
        private GraphApiClientHelper graphApiClient = new GraphApiClientHelper();

        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> PasswordRecovery(PasswordRecoveryModel model)
        {
            //Rut validation
            if (ModelState.IsValid)
            {
                string id = ExtraHelpers.FormatRutToId(model.Rut);
                GraphApiResponseInfo getUserResponse = await graphApiClient.GetUserByRut(id);

                //Check rut in db
                if (getUserResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string tempPassword = PasswordGeneratorHelper.GeneratePassword();
                    getUserResponse.User.TemporalPassword = tempPassword;

                    var apiResponse = await graphApiClient.ResetUserPassword(getUserResponse.User.ObjectId, getUserResponse.User);

                    if (apiResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
                    {
                        string message = "Error al intentar cambiar la contraseña. Intente otra vez.";

                        var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {
                            Content = new StringContent(message),
                            ReasonPhrase = "Error al intentar cambiar la contraseña"
                        };

                        throw new HttpResponseException(resp);
                    }

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
