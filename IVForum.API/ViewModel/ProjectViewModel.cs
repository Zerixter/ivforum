    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IVForum.API.ViewModel
{
    public class ProjectViewModel
    {
        public string Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
        public string Icon { get; set; }
        public string Background { get; set; }
        public int TotalMoney { get; set; } = 0;
        public int Views { get; set; } = 0;
        public string WebsiteUrl { get; set; }
        public string RepositoryUrl { get; set; }
    }
}
