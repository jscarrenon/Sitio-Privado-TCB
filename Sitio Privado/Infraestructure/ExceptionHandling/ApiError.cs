using System.Collections.Generic;
using System.Net;

namespace Sitio_Privado.Infraestructure.ExceptionHandling
{
    public class ApiError
    {
        private readonly ApiErrorCode? errorCode;

        public ApiError(string message, ApiErrorCode errorCode, HttpStatusCode statusCode, string description = null)
        {
            this.Message = message;
            this.errorCode = errorCode;
            this.StatusCode = statusCode;
            this.Description = description;
        }

        /// <summary>
        /// Short message of the error
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// A description of the error (or an extended message)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A special message or hint to let the developers to understand the error
        /// </summary>
        public string DeveloperHint { get; set; }

        /// <summary>
        /// Server error code
        /// </summary>
        public ApiErrorCode? ErrorCode { get { return errorCode; } }

        /// <summary>
        /// Http status code
        /// </summary>
        public HttpStatusCode? StatusCode { get; set; }

        /// <summary>
        /// A list of sub-errors or extra information. Typically used for validation errors of each field of a resource
        /// </summary>
        public List<ApiErrorResult> Errors { get; set; }

        /// <summary>
        /// Returns the status code or a default satus code if not set.
        /// </summary>
        /// <returns></returns>
        public HttpStatusCode GetStatusCodeOrDefault()
        {
            return this.StatusCode ?? HttpStatusCode.InternalServerError;
        }
    }
}