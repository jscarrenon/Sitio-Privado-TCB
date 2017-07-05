using Sitio_Privado.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Sitio_Privado.Infraestructure.ExceptionHandling;
using System.Net;
using System.Net.Http;

namespace Sitio_Privado.Services
{
    public class HttpService : IHttpService
    {
        /// <summary>
        /// Returns an HttpResponseMessage with standardized properties
        /// </summary>
        /// <param name="code">The internal api error code</param>
        /// <param name="request">The current request object</param>
        /// <returns>HttpResponseMessage instance</returns>
        /// <exception cref="ArgumentException"></exception>
        public virtual HttpResponseMessage GenerateErrorResponse(ApiErrorCode code, HttpRequestMessage request, string messageOverride = null)
        {
            if (!ApiErrorCode.IsDefined(typeof(ApiErrorCode), code))
                throw new ArgumentException("code");

            ApiError apiError = ApiErrorList.GetError(code);

            if (messageOverride != null)
            {
                apiError.Message = messageOverride;
            }

            HttpResponseMessage response = request.CreateResponse(apiError.GetStatusCodeOrDefault(), apiError);

            return response;
        }

        /// <summary>
        /// Returns an HttpResponseMessage with standardized properties
        /// </summary>
        /// <param name="apiError">An ApiError instance</param>
        /// <param name="request">The current request object</param>
        /// <returns>HttpResponseMessage instance</returns>
        /// <exception cref="ArgumentException"></exception>
        public virtual HttpResponseMessage GenerateErrorResponse(ApiError apiError, HttpRequestMessage request)
        {
            if (apiError == null)
                throw new ArgumentException("apiError");

            HttpResponseMessage response = request.CreateResponse(apiError.GetStatusCodeOrDefault(), apiError);
            return response;
        }

        /// <summary>
        /// Generates a standard ok response with a 200 status code
        /// </summary>
        /// <param name="request">The current request object</param>
        /// <returns>HttpResponseMessage instance</returns>
        public virtual HttpResponseMessage OkResponse(HttpRequestMessage request)
        {
            HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            return response;
        }

        /// <summary>
        /// Generates a response with a 200 status code with the given object
        /// </summary>
        /// <param name="obj">The result object to send in the response</param>
        /// <param name="request">The current request object</param>
        /// <returns>HttpResponseMessage instance</returns>
        public virtual HttpResponseMessage OkResponseWithResult<T>(T obj, HttpRequestMessage request)
        {
            HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, obj);
            return response;
        }

        /// <summary>
        /// Generates an response with a the specified status code
        /// </summary>
        /// <param name="request">The current request object</param>
        /// <returns>HttpResponseMessage instance</returns>
        public virtual HttpResponseMessage EmptyResponseWithStatusCode(HttpStatusCode statusCode, HttpRequestMessage request)
        {
            HttpResponseMessage response = request.CreateResponse(statusCode);
            return response;
        }

        /// <summary>
        /// Generates a response with a 201 status code with the given object
        /// </summary>
        /// <param name="obj">The result object to send in the response</param>
        /// <param name="request">The current request object</param>
        /// <returns>HttpResponseMessage instance</returns>
        public HttpResponseMessage CreatedResponseWithResult<T>(T obj, HttpRequestMessage request)
        {
            return request.CreateResponse(HttpStatusCode.Created, obj);
        }

        public string ExtractAccessToken(HttpRequestMessage request)
        {
            IEnumerable<string> headerValues;

            if (request.Headers.TryGetValues("Authorization", out headerValues))    // Note: "Bearer " takes 7 characters
            {
                string header = headerValues.FirstOrDefault();
                if (header.Length > 7 && header.Substring(0, 7) == "Bearer ")
                {
                    return header.Substring(7);
                }
            }

            return null;
        }
    }
}