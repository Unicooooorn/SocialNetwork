using System.Globalization;
using System.Windows.Controls;

namespace SocialNetwork.Desktop.Validations
{
    public class IsValidRegistrationForm : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Field is required")
                : ValidationResult.ValidResult;
        }
    }
}
