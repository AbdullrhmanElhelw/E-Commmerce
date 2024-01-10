using E_Commmerce.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace E_Commmerce.ViewModels.User
{
    public class CreateUserViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 3)]

        public string LastName { get; set; } = null!;

        [Required]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public UpdateImageViewModel? ImageViewModel { get; set; }
        public bool IsAdmin { get; set; }



    }
}
