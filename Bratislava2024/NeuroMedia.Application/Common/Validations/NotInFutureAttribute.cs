using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroMedia.Application.Common.Validations
{
    public class NotInFutureAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is DateTime dateTime)
            {
                var currentDate = DateTime.Today;
                var inputDate = dateTime.Date;
                return inputDate <= currentDate;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The {name} field cannot be in the future.";
        }
    }
}
