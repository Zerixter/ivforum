using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IVForum.API.Models
{
	public class Forum
    {
		[Key]
		public Guid Id { get; set; }
		
		[Required]
		[MaxLength(100)]
		public string Name { get; set; }
		[MaxLength(100)]
		public string Title { get; set; }
		[Required]
		[MaxLength(500)]
		public string Description { get; set; }
		public string Icon { get; set; }
		public string Background { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public Guid OwnerId { get; set; }
        public virtual User Owner { get; set; }
        public Guid WalletId { get; set; }
        public virtual Wallet Wallet { get; set; }
        public virtual List<Project> Projects { get; set; } = new List<Project>();
        public virtual List<User> Participants { get; set; } = new List<User>();
	}
}
