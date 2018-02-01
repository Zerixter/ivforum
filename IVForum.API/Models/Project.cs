using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IVForum.API.Models
{
	public class Project
    {
		[Key]
		public Guid Id { get; set; }
		
		[Required]
		public string Name { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Icon { get; set; }
		public string Background { get; set; }
		
		public string WebsiteUrl { get; set; }
		public string FacebookUrl { get; set; }
		public string TwitterUrl { get; set; }
		public string RepositoryUrl { get; set; }

		public virtual Forum Forum { get; set; } = null;
		public virtual User Owner { get; set; }
		public virtual List<User> Users { get; set; } = new List<User>();
	}
}
