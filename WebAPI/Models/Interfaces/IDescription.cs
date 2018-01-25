namespace WebAPI.Models.Interfaces
{
	interface IDescription
    {
		string Name { get; set; }
		string Title { get; set; }
		string Description { get; set; }
		string Icon { get; set; }
		string Background { get; set; }
    }
}
