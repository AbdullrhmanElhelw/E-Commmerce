using System.ComponentModel.DataAnnotations;

namespace E_Commmerce.ViewModels.User
{
    public class UserLoginViewModel
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public UpdateImageViewModel? ImageViewModel { get; set; }

        [Required, DataType(DataType.Password)]

        public string Password { get; set; }

    }
}
