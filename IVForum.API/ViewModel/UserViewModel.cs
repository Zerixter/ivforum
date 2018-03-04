using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVForum.API.ViewModel
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string WebsiteUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string RepositoryUrl { get; set; }
    }
}
