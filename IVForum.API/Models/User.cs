using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace IVForum.API.Models
{
	public class User
    {
		[Key]
        public Guid Id { get; set; }
        
        [MaxLength(1000)]
        public string Description { get; set; }
        public string Avatar { get; set; } = @"http://localhost/assets/images/avatar.png";
		public string WebsiteUrl { get; set; }
		public string FacebookUrl { get; set; }
		public string TwitterUrl { get; set; }
		public string RepositoryUrl { get; set; }

        public string IdentityId { get; set; }
        public virtual UserModel Identity { get; set; }
        public virtual List<Forum> Forums { get; set; } = new List<Forum>();
		public virtual List<Project> Projects { get; set; } = new List<Project>();
        public virtual List<Wallet> Wallets { get; set; } = new List<Wallet>();
    }
}
