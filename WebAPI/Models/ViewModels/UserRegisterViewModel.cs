using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.ViewModels
{
	public class UserRegisterViewModel
    {
		[Required]
		public string Name { get; set; }
		[Required]
		public string Surname { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
