using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ConsoleApp.Models;

public class NoWhitespaces : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;
        if (value is IEnumerable enumerable and not string)
        {
            foreach (var item in enumerable)
            {
                if (item is string strValue && string.IsNullOrWhiteSpace(strValue))
                {
                    return new("List cannot contain an empty or whitespace string.");
                }
            }
        }
        else if (value is string strValue)
        {
            if (string.IsNullOrWhiteSpace(strValue))
            {
                return new("Line cannot be empty or whitespace.");
            }
        }

        return ValidationResult.Success;
    }
    
}