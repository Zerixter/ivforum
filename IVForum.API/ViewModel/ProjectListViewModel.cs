using IVForum.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVForum.API.ViewModel
{
    public class ProjectListViewModel : ProjectViewModel
    {
        public string Background { get; set; }
        public DateTime CreationDate { get; set; }
        public int TotalMoney { get; set; }
        public string RepositoryUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public Forum Forum { get; set; }
        public UserViewModel Owner { get; set; }
    }
}
