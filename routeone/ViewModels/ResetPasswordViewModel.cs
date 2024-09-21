using System.ComponentModel.DataAnnotations;

namespace routeone.ViewModels
{
    public class ResetPasswordViewModel
    {

        [Required(ErrorMessage = "New Password is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "ConfirmPassword is required")]
        [Compare("NewPassword", ErrorMessage = "ConfirmPassword does not match password")]
        [DataType(DataType.Password)]

        public string ConfirmPassword { get; set; }

    }
}
