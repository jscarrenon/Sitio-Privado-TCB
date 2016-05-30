using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sitio_Privado.Extras
{
    public class RutValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string rut = (string)value;

            if (!RutValidation(rut))
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }

        private bool RutValidation(string rut)
        {
            bool isValid = false;
            try
            {
                rut = Regex.Replace(rut, "[-.]", string.Empty).ToUpper();
                int rutValue = int.Parse(rut.Substring(0, rut.Length - 1));
                char vd = rut.Last();

                int s = 1;
                for (int m = 0; rutValue != 0; rutValue /= 10)
                {
                    s = (s + rutValue % 10 * (9 - m++ % 6)) % 11;
                }
                if (vd == (char)(s != 0 ? s + 47 : 75))
                {
                    isValid = true;
                }
            }
            catch (Exception e)
            {
                var a = 0;
            }

            return isValid;
        }
    }
}