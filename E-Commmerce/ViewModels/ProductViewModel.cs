using E_Commmerce.CustomValidation;
using E_Commmerce.Data;
using E_Commmerce.Models;
using System.ComponentModel.DataAnnotations;

namespace E_Commmerce.ViewModels
{
    public class ProductViewModel
    {

        public int Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; } = 0.0m;

        [MinLength(3), MaxLength(500)]
        [StringLength(500)]
        [NotEqual]
        public string Description { get; set; } = string.Empty;

        public Category? Category { get; set; }

        [Display(Name = "Category")]


        public int? CategoryId { get; set; }

        public UpdateImageViewModel? ImageViewModel { get; set; }

    }
}
