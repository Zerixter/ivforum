using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using WebAPI.Models.Interfaces;

namespace WebAPI.Models
{
	public class Forum : IDescription
    {
		[Key]
		public Guid Id { get; set; }

		// IDescription Interface
		[Required]
		public string Name { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Icon { get; set; }
		public string Background { get; set; }

		[Required]
		public virtual User Owner { get; set; }
		public virtual List<Project> Projects { get; set; } = new List<Project>();

	}
}
