using Sitio_Privado.Infraestructure.ExceptionHandling;
using System.Net;
using System.Net.Http;

namespace Sitio_Privado.Services.Interfaces
{
    public interface IHttpService
    {
        /// <summary>
        /// Returns an HttpResponseMessage with standardized properties
        /// </summary>
        /// <param name="code">The internal api error code</param>
        /// <param name="request">The current request object</param>
        /// <returns>HttpResponseMessage instance</returns>
        /// <exception cref="ArgumentException"></exception>
        HttpResponseMessage GenerateErrorResponse(ApiErrorCode code, HttpRequestMessage request, string messageOverride = null);

        /// <summary>
        /// Returns an HttpResponseMessage with standardized properties
        /// </summary>
        /// <param name="apiError">An ApiError instance</param>
        /// <param name="request">The current request object</param>
        /// <returns>HttpResponseMessage instance</returns>
        /// <exception cref="ArgumentException"></exception>
        HttpResponseMessage GenerateErrorResponse(ApiError apiError, HttpRequestMessage request);

        /// <summary>
        /// Generates a standard ok response with a 200 status code
        /// </summary>
        /// <param name="request">The current request object</param>
        /// <returns>HttpResponseMessage instance</returns>
        HttpResponseMessage OkResponse(HttpRequestMessage request);

        /// <summary>
        /// Generates an response with a the specified status code
        /// </summary>
        /// <param name="statusCode">The http status code to send in the request</param>
        /// <param name="request">The current request object</param>
        /// <returns>HttpResponseMessage instance</returns>
        HttpResponseMessage EmptyResponseWithStatusCode(HttpStatusCode statusCode, HttpRequestMessage request);

        /// <summary>
        /// Generates a response with a 200 status code with the given object
        /// </summary>
        /// <param name="obj">The result object to send in the response</param>
        /// <param name="request">The current request object</param>
        /// <returns>HttpResponseMessage instance</returns>
        HttpResponseMessage OkResponseWithResult<T>(T obj, HttpRequestMessage request);

        /// <summary>
        /// Generates a response with a 201 status code with the given object
        /// </summary>
        /// <param name="obj">The result object to send in the response</param>
        /// <param name="request">The current request object</param>
        /// <returns>HttpResponseMessage instance</returns>
        HttpResponseMessage CreatedResponseWithResult<T>(T obj, HttpRequestMessage request);

        /// <summary>
        /// Extract the bearer access token from the Authentication header of the request
        /// </summary>
        /// <param name="request">The current request object</param>
        /// <returns>The access token</returns>
        string ExtractAccessToken(HttpRequestMessage request);
    }
}