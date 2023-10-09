using E_Commmerce.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace E_Commmerce.CustomValidation;

public class NotEqual: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        ProductViewModel model = (ProductViewModel)validationContext.ObjectInstance;
        string Description = value.ToString();
        if (Description != null)
        {
           
            if (Description == model.Name)
            {
                return new ValidationResult("Description cannot be equal to name");
            }
        }
        return ValidationResult.Success;
    }
}