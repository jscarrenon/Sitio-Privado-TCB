namespace Sitio_Privado.Infraestructure.ExceptionHandling
{
    /// <summary>
    /// Contains the definitions of the posible errors the API can send back as a response
    /// </summary>
    public enum ApiErrorCode
    {
        /// <summary>
        /// Useful when you want to send a Bad Request response without giving additional information.
        /// </summary>
        GenericBadRequest = 40000,

        /// <summary>
        /// For model/entity validation errors
        /// </summary>
        ValidationError = 40001,

        /// <summary>
        /// Useful when a POST request expects a payload, but none is received (Note: when no payload is sent, the ViewModel is null and is not possible to validate the model state without some workarounds, hence thi error).
        /// </summary>
        EmptyPayload = 40002,

        /// <summary>
        /// Used when an entity could not be created because it violated a unique index or primary key constraint
        /// </summary>
        UniqueConstraintViolation = 40006,

        /// <summary>
        /// When the TMC rate exceeds its maximum allowed value
        /// </summary>
        TMCRateExceeded = 40007,

        /// <summary>
        /// For generic 401 http response
        /// </summary>
        GenericUnauthorized = 40100,

        /// <summary>
        /// For generic 401 http response
        /// </summary>
        GenericForbidden = 40300,

        /// <summary>
        /// For generic 404 http response
        /// </summary>
        GenericNotFound = 40400,

        /// <summary>
        /// Used when an entity could not be found.
        /// </summary>
        EntityNotFound = 40401,

        /// <summary>
        /// To be used when an operation could not be completed, is not the server's fault and is not the user's fault (directly)
        /// Example: the user tries to reset password
        /// </summary>
        CannotCompleteOperation = 42201,

        /// <summary>
        /// The common generic internal server error.
        /// </summary>
        GenericError = 50000
    }
}