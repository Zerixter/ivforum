using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using WebAPI.Models.Interfaces;

namespace WebAPI.Models
{
	public class User : IUser, ISocial
	{
        public Guid Id { get; set; }

		// IUser Interface
		[Required]
		public string Name { get; set; }
		[Required]
		public string Surname { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }

		// ISocial Interface
		public string WebsiteUrl { get; set; }
		public string FacebookUrl { get; set; }
		public string TwitterUrl { get; set; }
		public string RepositoryUrl { get; set; }

		public virtual List<Forum> Forums { get; set; } = new List<Forum>();
		public virtual List<Project> Projects { get; set; } = new List<Project>();
	}
}
