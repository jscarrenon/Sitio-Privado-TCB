using Sitio_Privado.Extras;
using Sitio_Privado.Filters;
using Sitio_Privado.Helpers;
using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sitio_Privado.Controllers
{
    public class UsersController : ApiController
    {
        private GraphApiClientHelper graphApiClient = new GraphApiClientHelper();
        private SignInHelper signInHelper = new SignInHelper();
        private static readonly double passwordExpiresInHours = double.Parse(Startup.temporalPasswordTimeout, CultureInfo.InvariantCulture);

        [AllowCrossSiteJsonApi]
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
                    getUserResponse.User.IsTemporalPassword = true;
                    getUserResponse.User.TemporalPasswordTimestamp = DateTime.Now.ToString();

                    //Reset user password
                    var apiResponse = await graphApiClient.ResetUserPassword(getUserResponse.User.ObjectId, getUserResponse.User);
                    if (apiResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        //Send mail
                        try
                        {
                            SendMail(getUserResponse.User);
                        }
                        catch(Exception e)
                        {
                            //TODO
                        }

                        //Success!
                        return Json("Una nueva contraseña temporal ha sido enviada a su correo electrónico.");
                    }
                    else
                    {
                        var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {
                            Content = new StringContent("Error al intentar cambiar la contraseña. B2C Status Code: " + apiResponse.StatusCode),
                            ReasonPhrase = "Error al intentar cambiar la contraseña. Intente otra vez."
                        };
                        throw new HttpResponseException(resp);
                    }
                }
                else
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("RUT no encontrado en el sistema."),
                        ReasonPhrase = "El RUT ingresado no es Cliente de Tanner, para cualquier duda contacte a mesa de atención de clientes al NNNN."
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
                    ReasonPhrase = "RUT no válido. Por favor intente nuevamente."
                };
                throw new HttpResponseException(resp);
            }
        }

        [AllowCrossSiteJsonApi]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> SignInExternal(LoginModel model)
        {
            //Rut validation
            if (ModelState.IsValid)
            {
                string id = ExtraHelpers.FormatRutToId(model.Rut);
                GraphApiResponseInfo getUserResponse = await graphApiClient.GetUserByRut(id);

                //Check rut in db
                if (getUserResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Try to log in
                    model.Rut = id;
                    IdToken token = await signInHelper.GetToken(model);

                    if (token != null)
                    {
                        //Check temporary password
                        if(getUserResponse.User.IsTemporalPassword == false)
                        {
                            //Success!
                            return Json(token);
                        }
                        else
                        {
                            DateTime temporalPasswordTimestamp = DateTime.Parse(getUserResponse.User.TemporalPasswordTimestamp);
                            DateTime limit = temporalPasswordTimestamp.AddHours(passwordExpiresInHours);

                            if (DateTime.Now <= limit)
                            {
                                //Success
                                return Json(token);
                            }
                            else
                            {
                                var resp = new HttpResponseMessage(HttpStatusCode.Forbidden)
                                {
                                    Content = new StringContent("Contraseña temporal caducada."),
                                    ReasonPhrase = "Su contraseña temporal ha caducado. Por favor solicite una nueva."
                                };
                                throw new HttpResponseException(resp);
                            }
                        }
                    }
                    else
                    {
                        var resp = new HttpResponseMessage(HttpStatusCode.BadGateway)
                        {
                            Content = new StringContent("Token nulo."),
                            ReasonPhrase = "No se pudo iniciar sesión. Por favor revise su RUT y contraseña e intente nuevamente."
                        };
                        throw new HttpResponseException(resp);
                    }
                }
                else
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("RUT no encontrado en el sistema."),
                        ReasonPhrase = "El RUT ingresado no es Cliente de Tanner, para cualquier duda contacte a mesa de atención de clientes al NNNN."
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
                    ReasonPhrase = "RUT o contraseña no válidos. Por favor intente nuevamente."
                };
                throw new HttpResponseException(resp);
            }
		}

        private void SendMail(GraphUserModel user)
        {
            SmtpSection settings = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            var email = new PasswordRecoveryEmailModel
            {
                From = settings.From,
                User = user
            };
            email.Send();
        }
    }
}
