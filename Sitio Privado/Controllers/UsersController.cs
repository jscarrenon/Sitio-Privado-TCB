using Microsoft.Owin.Security;
using NLog;
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
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Sitio_Privado.Controllers
{
    public class UsersController : ApiController
    {
        private GraphApiClientHelper graphApiClient = new GraphApiClientHelper();
        private SignInHelper signInHelper = new SignInHelper();
        private static readonly double passwordExpiresInHours = double.Parse(Startup.temporalPasswordTimeout, CultureInfo.InvariantCulture);
        private static readonly string contactPhoneNumber = ConfigurationManager.AppSettings["web:ContactPhoneNumber"];
        private static Logger logger = LogManager.GetLogger("SessionLog");

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
                        //Log
                        logger.Info("Temporary password set => Rut: " + getUserResponse.User.Rut + "; Email: " +
                                getUserResponse.User.Email + "; IP: " + Request.GetClientIpAddress() + ";");

                        //Send mail
                        try
                        {
                            SendPasswordRecoveryMail(getUserResponse.User);
                            logger.Info("Temporary password mail sent => Rut: " + getUserResponse.User.Rut + "; Email: " +
                                getUserResponse.User.Email + "; IP: " + Request.GetClientIpAddress() + ";");
                        }
                        catch(Exception e)
                        {
                            logger.Error(e, "Temporary password mail error => Rut: " + getUserResponse.User.Rut + "; Email: " +
                                getUserResponse.User.Email + "; IP: " + Request.GetClientIpAddress() + ";");
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
                        ReasonPhrase = "El RUT ingresado no es Cliente de Tanner, para cualquier duda contacte a mesa de atención de clientes al " + contactPhoneNumber + "."
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
                    string token = await signInHelper.GetToken(model);

                    if (token != null)
                    {
                        //Check temporary password
                        if(getUserResponse.User.IsTemporalPassword == false)
                        {
                            //Success!
                            logger.Info("User signed in => Rut: " + getUserResponse.User.Rut + "; Email: " +
                                        getUserResponse.User.Email + "; IP: " + Request.GetClientIpAddress() + ";");
                            return Json(token);
                        }
                        else
                        {
                            DateTime temporalPasswordTimestamp = DateTime.Parse(getUserResponse.User.TemporalPasswordTimestamp);
                            DateTime limit = temporalPasswordTimestamp.AddHours(passwordExpiresInHours);

                            if (DateTime.Now <= limit)
                            {
                                //Success
                                logger.Info("User signed in => Rut: " + getUserResponse.User.Rut + "; Email: " +
                                            getUserResponse.User.Email + "; IP: " + Request.GetClientIpAddress() + ";");
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
                        ReasonPhrase = "El RUT ingresado no es Cliente de Tanner, para cualquier duda contacte a mesa de atención de clientes al " + contactPhoneNumber + "."
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

        [SkipTemporaryPassword]
        [HttpPost]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordModel model)
        {
            IPrincipal user = this.User;

            if (ModelState.IsValid)
            {
                //Retrieve user info
                Claim idClaim = ((ClaimsIdentity)user.Identity).Claims.Where(c => c.Type == Startup.objectIdClaimKey).First();
                GraphApiResponseInfo getUserResponse = await graphApiClient.GetUserByObjectId(idClaim.Value);
                if (getUserResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Try to login with old password
                    LoginModel loginModel = new LoginModel() { Rut = getUserResponse.User.Rut, Password = model.OldPassword };
                    var loginResponse = await this.SignInExternal(loginModel);

                    //If it's ok then try to change the password
                    getUserResponse.User.TemporalPassword = model.Password;
                    getUserResponse.User.IsTemporalPassword = false;
                    getUserResponse.User.TemporalPasswordTimestamp = DateTime.MinValue.ToString();

                    var apiResponse = await graphApiClient.ResetUserPassword(getUserResponse.User.ObjectId, getUserResponse.User);

                    if (apiResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        //Success!
                        string ip = Request.GetClientIpAddress();
                        DateTime timestamp = DateTime.Now;

                        //Log
                        logger.Info("Password changed => Rut: " + getUserResponse.User.Rut + "; Email: " +
                                    getUserResponse.User.Email + "; IP: " + ip + ";");

                        Claim isTemporalPasswordClaim = ((ClaimsIdentity)user.Identity).Claims.Where(c => c.Type == Startup.isTemporalPasswordClaimKey).First();
                        Claim temporalPasswordTimestampClaim = ((ClaimsIdentity)user.Identity).Claims.Where(c => c.Type == Startup.temporalPasswordTimestampClaimKey).First();

                        //Update claims
                        ((ClaimsIdentity)user.Identity).RemoveClaim(isTemporalPasswordClaim);
                        ((ClaimsIdentity)user.Identity).AddClaim(new Claim(Startup.isTemporalPasswordClaimKey, bool.FalseString));

                        ((ClaimsIdentity)user.Identity).RemoveClaim(temporalPasswordTimestampClaim);
                        ((ClaimsIdentity)user.Identity).AddClaim(new Claim(Startup.temporalPasswordTimestampClaimKey, DateTime.MinValue.ToString()));

                        var ctx = Request.GetOwinContext();
                        var authManager = ctx.Authentication;
                        authManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(user.Identity), new AuthenticationProperties() { IsPersistent = true });

                        //Sign out B2C TODO

                        //Send mail
                        try
                        {
                            SendChagePasswordMail(getUserResponse.User, ip, timestamp);
                            logger.Info("Password changed mail sent => Rut: " + getUserResponse.User.Rut + "; Email: " +
                                getUserResponse.User.Email + "; IP: " + ip + ";");
                        }
                        catch (Exception e)
                        {
                            logger.Error(e, "Password changed mail error => Rut: " + getUserResponse.User.Rut + "; Email: " +
                                getUserResponse.User.Email + "; IP: " + ip + ";");
                        }

                        return Json("Contraseña modificada con éxito.");
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
                        Content = new StringContent("Usuario no encontrado en el sistema."),
                        ReasonPhrase = "No se encontró al usuario en el sistema."
                    };
                    throw new HttpResponseException(resp);
                }
            }
            else
            {
                return BadRequest(ModelState);

            }
        }

        private void SendPasswordRecoveryMail(GraphUserModel user)
        {
            SmtpSection settings = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            var email = new PasswordRecoveryEmailModel
            {
                From = settings.From,
                User = user
            };
            email.Send();
        }

        private void SendChagePasswordMail(GraphUserModel user, string ip, DateTime timestamp)
        {
            SmtpSection settings = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            var email = new ChangePasswordEmailModel
            {
                From = settings.From,
                User = user,
                IP = ip,
                Timestamp = timestamp
            };
            email.Send();
        }
    }
}
