using Newtonsoft.Json.Linq;
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
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Tracing;

namespace Sitio_Privado.Controllers
{
    [OverrideAuthorization]
    [TokenAuthorizationFilter]
    public class SyncApiController : ApiController
    {
        #region SyncAPIController Parameters
        //Keys used by our API
        private static string NameParam = "names";
        private static string SurnameParam = "surnames";
        private static string RutParam = "rut";
        private static string WorkAddressParam = "work_address";
        private static string HomeAddressParam = "home_address";
        private static string CountryParam = "country";
        private static string CityParam = "city";
        private static string WorkPhoneParam = "work_phone";
        private static string HomePhoneParam = "home_phone";
        private static string EmailParam = "email";
        private static string CheckingAccountParam = "checking_account";
        private static string BankParam = "bank";
        private static string UpdatedAtParam = "updated_at";
        private static string CanResetPasswordParam = "can_reset_password";
        #endregion

        private GraphApiClientHelper syncApiHelper = new GraphApiClientHelper();
        private ITraceWriter tracer = GlobalConfiguration.Configuration.Services.GetTraceWriter();

        [HttpPost]
        public async Task<HttpResponseMessage> CreateUser()
        {
            //Read request's parameters
            JObject requestBody = JObject.Parse(await Request.Content.ReadAsStringAsync());
            tracer.Info(Request, this.ControllerContext.ControllerDescriptor.ControllerType.FullName,
                "Content:\n{0}", new string[] { requestBody.ToString() });

            //Response
            HttpResponseMessage response = new HttpResponseMessage();

            if (!CheckNeededAttributesForCreatingUser(requestBody))
            {
                string errorMessage = GenerateJsonErrorMessage("One or more required parameters is missing");
                response.Content = new StringContent(errorMessage, Encoding.UTF8, "application/json");
                tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Completed with {0}, Content:\n{1}",
                    new string[] { response.StatusCode.ToString(), await response.Content.ReadAsStringAsync() });
                return response;
            }
 
            //Create user
            GraphUserModel graphUser = GetGraphUserCreateRequest(requestBody);

            GraphApiResponseInfo getUserResponse = await syncApiHelper.GetUserByRut(graphUser.Rut);
            if (!(getUserResponse.StatusCode == HttpStatusCode.OK))
            {
                GraphApiResponseInfo graphApiResponse = await syncApiHelper.CreateUser(graphUser, false);

                //Read result and set response
                response.StatusCode = graphApiResponse.StatusCode;
                if (graphApiResponse.StatusCode == HttpStatusCode.Created)
                {
                    string responseBody = GetUserResponseBody(graphApiResponse.User);
                    response.Content = new StringContent(responseBody, Encoding.UTF8, "application/json");
                }
                else
                {
                    string errorMessage = GenerateJsonErrorMessage(graphApiResponse.Message);
                    response.Content = new StringContent(errorMessage, Encoding.UTF8, "application/json");
                }
            }
            else
            {
                string errorMessage = GenerateJsonErrorMessage("Another object with the same value for property rut already exists.");
                response.Content = new StringContent(errorMessage, Encoding.UTF8, "application/json");
            }

            tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Completed with {0}, Content:\n{1}",
                     new string[] { response.StatusCode.ToString(), await response.Content.ReadAsStringAsync() });

            return response;
        }

        [HttpPatch]
        public async Task<HttpResponseMessage> UpdateUser(string id)
        {
            //Read request's parameters
            JObject requestContent = (JObject)await Request.Content.ReadAsAsync(typeof(JObject));
            tracer.Info(Request, this.ControllerContext.ControllerDescriptor.ControllerType.FullName,
                "Content:\n{0}", new string[] { requestContent.ToString() });

            HttpResponseMessage response = new HttpResponseMessage();

            if (id == null || id.Length <= 0)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                string errorMessage = GenerateJsonErrorMessage("Rut param was not provided");
                response.Content = new StringContent(errorMessage, Encoding.UTF8, "application/json");
                tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Completed with {0}, Content:\n{1}",
                    new string[] { response.StatusCode.ToString(), await response.Content.ReadAsStringAsync() });
                return response;
            }

            GraphUserModel requestUser = GetUpdateUserGraphApiRequestBody(requestContent);
            if (requestUser == null) {
                response.StatusCode = HttpStatusCode.BadRequest;
                string errorMessage = GenerateJsonErrorMessage("No content provided");
                response.Content = new StringContent(errorMessage, Encoding.UTF8, "application/json");
                tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Completed with {0}, Content:\n{1}",
                    new string[] { response.StatusCode.ToString(), await response.Content.ReadAsStringAsync() });
                return response;
            }

            //Format Rut
            id = id.Trim().ToUpper();
            //Get user
            GraphApiResponseInfo getGraphResponse = await syncApiHelper.GetUserByRut(id);

            if (getGraphResponse.User == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                string errorMessage = GenerateJsonErrorMessage(getGraphResponse.Message);
                response.Content = new StringContent(errorMessage, Encoding.UTF8, "application/json");
                tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Completed with {0}, Content:\n{1}",
                    new string[] { response.StatusCode.ToString(), await response.Content.ReadAsStringAsync() });
                return response;
            }
            //Assign rut in case that email is modified
            requestUser.Rut = id;
            string userGraphId = getGraphResponse.User.ObjectId;
            GraphApiResponseInfo graphResponse = await syncApiHelper.UpdateUser(userGraphId, requestUser, false);
            response.StatusCode = graphResponse.StatusCode;

            if (graphResponse.StatusCode == HttpStatusCode.NoContent)
            {
                tracer.Info(Request, this.ControllerContext.ControllerDescriptor.ControllerType.FullName,
                    "Completed with {0}", new string[] { response.StatusCode.ToString() });
            }
            else
            {
                string errorMessage = GenerateJsonErrorMessage(graphResponse.Message);
                response.Content = new StringContent(errorMessage, Encoding.UTF8, "application/json");
                tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Completed with {0}, Content:\n{1}",
                     new string[] { response.StatusCode.ToString(), await response.Content.ReadAsStringAsync() });
            }
            return response;
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteUser(string id)
        {
            //Read request's parameters
            JObject requestContent = (JObject)await Request.Content.ReadAsAsync(typeof(JObject));
            tracer.Info(Request, this.ControllerContext.ControllerDescriptor.ControllerType.FullName,
                "Rut: {0}", new string[] { id });

            HttpResponseMessage response = new HttpResponseMessage();

            if (id == null || id.Length <= 0)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                string errorMessage = GenerateJsonErrorMessage("Rut param was not provided");
                response.Content = new StringContent(errorMessage, Encoding.UTF8, "application/json");
                tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Completed with {0}, Content:\n{1}",
                    new string[] { response.StatusCode.ToString(), await response.Content.ReadAsStringAsync() });
                return response;
            }

            //Format Rut
            id = id.Trim().ToUpper();
            //Get user
            GraphApiResponseInfo getGraphResponse = await syncApiHelper.GetUserByRut(id);

            if (getGraphResponse.User == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                string errorMessage = GenerateJsonErrorMessage(getGraphResponse.Message);
                response.Content = new StringContent(errorMessage, Encoding.UTF8, "application/json");
                tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Completed with {0}, Content:\n{1}",
                    new string[] { response.StatusCode.ToString(), await response.Content.ReadAsStringAsync() });
                return response;
            }
            string userGraphId = getGraphResponse.User.ObjectId;
            GraphApiResponseInfo graphResponse = await syncApiHelper.DeleteUserByObjectId(userGraphId);
            response.StatusCode = graphResponse.StatusCode;

            if (graphResponse.StatusCode == HttpStatusCode.NoContent)
            {
                tracer.Info(Request, this.ControllerContext.ControllerDescriptor.ControllerType.FullName,
                    "Completed with {0}", new string[] { response.StatusCode.ToString() });
            }
            else
            {
                string errorMessage = GenerateJsonErrorMessage(getGraphResponse.Message);
                response.Content = new StringContent(errorMessage, Encoding.UTF8, "application/json");
                tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Completed with {0}, Content:\n{1}",
                     new string[] { response.StatusCode.ToString(), await response.Content.ReadAsStringAsync() });
            }
            return response;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetUser(string id)
        {
            tracer.Info(Request, this.ControllerContext.ControllerDescriptor.ControllerType.FullName, "Requested Rut: " + id);

            HttpResponseMessage response = new HttpResponseMessage();

            if(id == null || id.Length <= 0)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                string errorMessage = GenerateJsonErrorMessage("Rut param was not provided");
                response.Content = new StringContent(errorMessage, Encoding.UTF8, "application/json");
                tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Completed with {0}, Content:\n{1}",
                    new string[] { response.StatusCode.ToString(), await response.Content.ReadAsStringAsync() });
                return response;
            }

            id = id.Trim().ToUpper();
            GraphApiResponseInfo graphApiResponse = await syncApiHelper.GetUserByRut(id);
            response.StatusCode = graphApiResponse.StatusCode;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseBody = GetUserResponseBody(graphApiResponse.User);
                response.Content = new StringContent(responseBody, Encoding.UTF8, "application/json");
            }
            else
            {
                string errorMessage = GenerateJsonErrorMessage(graphApiResponse.Message);
                response.Content = new StringContent(errorMessage, Encoding.UTF8, "application/json");
            }

            tracer.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Completed with {0}, Content:\n{1}",
                new string[] { response.StatusCode.ToString(), await response.Content.ReadAsStringAsync() });

            return response;
        }

        private string GetUserResponseBody(GraphUserModel user)
        {
            JObject response = new JObject();
            response.Add(NameParam, user.Name);
            response.Add(SurnameParam, user.Surname);
            response.Add(RutParam, user.Rut);
            response.Add(WorkAddressParam, user.WorkAddress);
            response.Add(HomeAddressParam, user.HomeAddress);
            response.Add(CountryParam, user.Country);
            response.Add(CityParam, user.City);
            response.Add(WorkPhoneParam, user.WorkPhone);
            response.Add(HomePhoneParam, user.HomePhone);
            response.Add(EmailParam, user.Email);
            response.Add(CheckingAccountParam, user.CheckingAccount);
            response.Add(BankParam, user.Bank);
            response.Add(UpdatedAtParam, user.UpdatedAt);
            response.Add(CanResetPasswordParam, user.CanResetPassword);
            return response.ToString();
        }

        private GraphUserModel GetUpdateUserGraphApiRequestBody(JObject content)
        {
            if (content == null)
                return null;

            GraphUserModel user = new GraphUserModel();

            if(content.GetValue(NameParam) != null)
                user.Name = GetTitleFormatString(content.GetValue(NameParam).ToString());

            if(content.GetValue(SurnameParam) != null)
                user.Surname = GetTitleFormatString(content.GetValue(SurnameParam).ToString());

            if (content.GetValue(WorkAddressParam) != null)
                user.WorkAddress = GetTitleFormatString(content.GetValue(WorkAddressParam).ToString());

            if (content.GetValue(HomeAddressParam) != null)
                user.HomeAddress = GetTitleFormatString(content.GetValue(HomeAddressParam).ToString());

            if (content.GetValue(CountryParam) != null)
                user.Country = GetTitleFormatString(content.GetValue(CountryParam).ToString());

            if (content.GetValue(CityParam) != null)
                user.City = GetTitleFormatString(content.GetValue(CityParam).ToString());

            if (content.GetValue(WorkPhoneParam) != null)
                user.WorkPhone = content.GetValue(WorkPhoneParam).ToString().Trim();

            if (content.GetValue(HomePhoneParam) != null)
                user.HomePhone = content.GetValue(HomePhoneParam).ToString().Trim();

            if (content.GetValue(EmailParam) != null)
                user.Email = content.GetValue(EmailParam).ToString().Trim().ToLower();

            if (content.GetValue(CheckingAccountParam) != null)
                user.CheckingAccount = content.GetValue(CheckingAccountParam).ToString().Trim();

            if (content.GetValue(BankParam) != null)
                user.Bank = GetTitleFormatString(content.GetValue(BankParam).ToString());

            user.UpdatedAt = DateTime.Now.ToString();

            return user;
        }

        private GraphUserModel GetGraphUserCreateRequest(JObject content)
        {
            GraphUserModel user = new GraphUserModel();

            //Retrieve formatted values
            user.Name = GetTitleFormatString(content.GetValue(NameParam).ToString());
            user.Rut = content.GetValue(RutParam).ToString().Trim().ToUpper();
            user.Email = content.GetValue(EmailParam).ToString().Trim().ToLower();
            user.TemporalPassword = PasswordGeneratorHelper.GeneratePassword();

            if (content.GetValue(SurnameParam) != null)
                user.Surname = GetTitleFormatString(content.GetValue(SurnameParam).ToString());

            if (content.GetValue(WorkAddressParam) != null)
                user.WorkAddress = GetTitleFormatString(content.GetValue(WorkAddressParam).ToString());

            if(content.GetValue(HomeAddressParam) != null)
                user.HomeAddress = GetTitleFormatString(content.GetValue(HomeAddressParam).ToString());

            if(content.GetValue(CountryParam) != null)
                user.Country = GetTitleFormatString(content.GetValue(CountryParam).ToString());

            if(content.GetValue(CityParam) != null)
                user.City = GetTitleFormatString(content.GetValue(CityParam).ToString());

            if(content.GetValue(WorkPhoneParam) != null)
                user.WorkPhone = content.GetValue(WorkPhoneParam).ToString().Trim();

            if(content.GetValue(HomePhoneParam) != null)
                user.HomePhone = content.GetValue(HomePhoneParam).ToString().Trim();

            if (content.GetValue(CheckingAccountParam) != null)
                user.CheckingAccount = content.GetValue(CheckingAccountParam).ToString().Trim();

            if (content.GetValue(BankParam) != null)
                user.Bank = GetTitleFormatString(content.GetValue(BankParam).ToString());

            user.UpdatedAt = DateTime.Now.ToString();
            return user;
        }

        private string GetTitleFormatString(string s)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.Trim().ToLower());
        }

        private bool CheckNeededAttributesForCreatingUser(JObject requestBody)
        {
            if (requestBody.GetValue(NameParam) == null || requestBody.GetValue(RutParam) == null || 
                requestBody.GetValue(EmailParam) == null)
                return false;
            return true;
        }

        private string GenerateJsonErrorMessage(string message)
        {
            JObject json = new JObject();
            json.Add("message", message);
            return json.ToString();
        }
    }
}
