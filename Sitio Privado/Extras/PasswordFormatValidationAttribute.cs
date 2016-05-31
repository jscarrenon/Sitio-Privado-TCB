using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sitio_Privado.Extras
{
    public class PasswordFormatValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string password = (string)value;

            if (!(password != null && password.Any(char.IsUpper) && password.Any(char.IsLower)
                && password.Any(char.IsDigit)))
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }
    }
}