using System;
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
		public string Title { get; set; }
		[Required]
		[MaxLength(1000)]
		public string Description { get; set; }
		public string Icon { get; set; }
		public string Background { get; set; } = @"http://localhost/assets/images/banner.jpg";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public int TotalMoney { get; set; } = 0;
        public int Views { get; set; } = 0;
		
		public string WebsiteUrl { get; set; }
		public string RepositoryUrl { get; set; }

		public virtual Forum Forum { get; set; } = null;
		public virtual User Owner { get; set; }
        public virtual List<Vote> Votes { get; set; } = new List<Vote>();

        public static List<object> ValidateProject(Project project)
        {
            List<object> Errors = new List<object>();
            if (project.Description is null)
            {
                Errors.Add(new { Message = "S'ha deixat el camp de descripció buit." });
            }
            if (project.Title is null)
            {
                Errors.Add(new { Message = "S'ha deixat el camp de títol buit." });
            }
            if (project.Description.Length > 1000)
            {
                Errors.Add(new { Message = "La llargada de la descripció no pot ser més llarga de 1000 carácters." });
            }
            if (project.Title.Length > 100)
            {
                Errors.Add(new { Message = "La llargada de del títol no pot ser més llarga de 100 carácters." });
            }
            return Errors;
        }
    }
}
