namespace WebAPI.Models.Interfaces
{
	public interface ISocial
    {
		string WebsiteUrl { get; set; }
		string FacebookUrl { get; set; }
		string TwitterUrl { get; set; }
		string RepositoryUrl { get; set; }
    }
}
