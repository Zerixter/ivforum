using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

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
		public string Background { get; set; } = Path.Combine("Assets", "Images", "banner.jpg");
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public int Views { get; set; } = 0;

        public Guid OwnerId { get; set; }
        public virtual User Owner { get; set; }
        public virtual List<Wallet> Wallets { get; set; } = new List<Wallet>();
        public virtual List<Project> Projects { get; set; } = new List<Project>();
        public virtual List<Transaction> Transactions { get; set; } = new List<Transaction>();
	}
}
