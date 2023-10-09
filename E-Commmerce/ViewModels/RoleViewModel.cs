using System.ComponentModel.DataAnnotations;

namespace E_Commmerce.ViewModels
{
    public class RoleViewModel
    {
        public string? Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
