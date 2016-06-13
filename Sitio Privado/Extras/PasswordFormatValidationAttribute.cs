using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sitio_Privado.Extras
{
    public class PasswordFormatValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string password = (string)value;

            //Validations
            bool[] validations = new bool[4];

            //1: lowercase characters
            bool validation1 = password.Any(char.IsLower);
            validations[0] = validation1;

            //2: uppercase characters
            bool validation2 = password.Any(char.IsUpper);
            validations[1] = validation2;

            //3: numbers
            bool validation3 = password.Any(char.IsDigit);
            validations[2] = validation3;

            //4: symbols
            bool validation4 = password.IndexOfAny(@"@#$%^&*\-_!+=[\]{}\|\\:´,.?\/`~""();".ToCharArray()) != -1;
            validations[3] = validation4;

            if (!(password != null && validations.Where(v => v == true).Count() >= 3))
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }

            return null;
        }
    }
}