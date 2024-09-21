using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;


namespace routeone.ViewModels
{
	public class RegisterViewModel
	{
		public string Fname { get; set; }
		public string Lname { get; set; }

		[Required(ErrorMessage ="Email is required")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }


		[Required(ErrorMessage = "ConfirmPassword is required")]
		[Compare("Password", ErrorMessage = "ConfirmPassword does not match password")]
		[DataType(DataType.Password)]

		public string ConfirmPassword { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsAgree { get; set; }
    }
}
