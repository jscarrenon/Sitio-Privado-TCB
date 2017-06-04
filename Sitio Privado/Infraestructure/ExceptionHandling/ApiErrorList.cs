using System.Collections.Generic;
using System.Net;

namespace Sitio_Privado.Infraestructure.ExceptionHandling
{
    /// <summary>
    /// List of ApiErrors with prepopulated information
    /// </summary>
    public class ApiErrorList
    {
        private static readonly Dictionary<ApiErrorCode, ApiError> errorList = new Dictionary<ApiErrorCode, ApiError>()
        {
            {
                ApiErrorCode.GenericBadRequest,
                new ApiError("The request was not understood by the server", ApiErrorCode.GenericBadRequest, HttpStatusCode.BadRequest)
            },
            {
                ApiErrorCode.ValidationError,
                new ApiError("There was a validation error in the requested action", ApiErrorCode.ValidationError, HttpStatusCode.BadRequest)
            },
            {
                ApiErrorCode.EmptyPayload,
                new ApiError("A request payload or query was expected but none was received ", ApiErrorCode.EmptyPayload, HttpStatusCode.BadRequest)
            },
            {
                ApiErrorCode.UniqueConstraintViolation,
                new ApiError("The entity already exists", ApiErrorCode.UniqueConstraintViolation, HttpStatusCode.BadRequest)
            },
            {
                ApiErrorCode.TMCRateExceeded,
                new ApiError("The TMC exceeds the maximum allowed value", ApiErrorCode.TMCRateExceeded, HttpStatusCode.BadRequest)
            },
            {
                ApiErrorCode.GenericUnauthorized,
                new ApiError("Authentication failed for this request", ApiErrorCode.GenericUnauthorized, HttpStatusCode.Unauthorized)
            },
            {
                ApiErrorCode.GenericForbidden,
                new ApiError("User does not have enough permissions to do the required action", ApiErrorCode.GenericForbidden, HttpStatusCode.Forbidden)
            },
            {
                ApiErrorCode.GenericNotFound,
                new ApiError("The requested resource could not be found", ApiErrorCode.GenericNotFound, HttpStatusCode.NotFound)
            },
            {
                ApiErrorCode.EntityNotFound,
                new ApiError("The requested entity could not be found", ApiErrorCode.EntityNotFound, HttpStatusCode.NotFound)
            },
            {
                ApiErrorCode.CannotCompleteOperation,
                new ApiError("The operation could not be completed", ApiErrorCode.CannotCompleteOperation, (HttpStatusCode)422)
            },
            {
                ApiErrorCode.GenericError,
                new ApiError("An error has ocurred with the server", ApiErrorCode.GenericError, HttpStatusCode.InternalServerError)
            }
        };

        public static ApiError GetError(ApiErrorCode errorCode)
        {
            return errorList[errorCode];
        }
    }
}