using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commmerce.Models
{
    [Table("Products")]
    public class Product
    {
        public int Id { get; set; }


        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]

        public decimal Price { get; set; } = 0.0m;


        public string Description { get; set; } = string.Empty;

        public Category? Category { get; set; } = new Category();
        public int? CategoryId { get; set; }

        public string? ImageName { get; set; }
        public string? ImageContentType { get; set; }



    }
}
