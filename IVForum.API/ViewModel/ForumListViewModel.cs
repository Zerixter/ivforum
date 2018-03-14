using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVForum.API.ViewModel
{
    public class ForumListViewModel : ForumViewModel
    {
        public string Icon { get; set; }
        public string Background { get; set; }
        public DateTime CreationDate { get; set; }
        public int Views { get; set; }
        public UserViewModel Owner { get; set; }
    }
}
