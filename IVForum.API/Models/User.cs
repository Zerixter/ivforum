using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IVForum.API.Models
{
	public class User
    {
		[Key]
		public Guid Id { get; set; }
		
		[Required]
		public string Name { get; set; }
		[Required]
		public string Surname { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		
		public string WebsiteUrl { get; set; }
		public string FacebookUrl { get; set; }
		public string TwitterUrl { get; set; }
		public string RepositoryUrl { get; set; }

        public virtual Token Token { get; set; } = null;
        public virtual List<Forum> Forums { get; set; } = new List<Forum>();
		public virtual List<Project> Projects { get; set; } = new List<Project>();
	}
}
