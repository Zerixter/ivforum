namespace WebAPI.Models.Interfaces
{
	interface IUser
    {
		string Name { get; set; }
		string Surname { get; set; }
		string Email { get; set; }
		string Password { get; set; }
    }
}
