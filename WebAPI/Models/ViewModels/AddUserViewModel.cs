using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.ViewModels
{
	public class AddUserViewModel
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
