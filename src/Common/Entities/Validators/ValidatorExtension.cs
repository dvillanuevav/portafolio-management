using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities.Validators
{
    public static class ValidatorExtension
    {
        public static bool IsGreaterThanZero(this decimal value)
        {
            return value > 0;
        }

        public static bool IsValidDate(this string value, string format = "yyyy-MM-ddTHH:mm:ssK")
        {
            DateTime date;
            return DateTime.TryParseExact(value, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date);
        }

        public static string ToErrorsMessage(this List<ValidationFailure> errors)
        {
            return string.Join(';', errors.Select(e => $"{e.PropertyName}:{e.ErrorMessage}").ToArray());
        }
    }
}
