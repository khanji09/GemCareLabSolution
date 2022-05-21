using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GemCare.API.Common
{
    public class GreaterThanZero : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            // return true if value is a non-null number > 0, otherwise return false
            return value != null && int.TryParse(value.ToString(), out int i) && i > 0;
        }
    }
}
