﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace IVForum.API.Models
{
	public class Project
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
		
		public string WebsiteUrl { get; set; }
		public string FacebookUrl { get; set; }
		public string TwitterUrl { get; set; }
		public string RepositoryUrl { get; set; }

		public virtual Forum Forum { get; set; } = null;
        public Guid OwnerId { get; set; }
		public virtual User Owner { get; set; }
        public virtual List<Vote> Votes { get; set; } = new List<Vote>();
	}
}
