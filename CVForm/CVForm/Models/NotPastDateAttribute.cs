using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CVForm.Models
{
    public class NotPastDateAttribute : ValidationAttribute
    {
        private readonly int _maxValue;

        public NotPastDateAttribute()
        {
            
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((DateTime)value < DateTime.Now)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return $"Date cannot be a past date";
        }
    }
}
