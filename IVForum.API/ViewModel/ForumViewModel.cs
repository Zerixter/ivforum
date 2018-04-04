using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVForum.API.ViewModel
{
    public class ForumViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateBeginsVote { get; set; } = DateTime.Parse("10/10/1999").Date;
        public DateTime DateEndsVote { get; set; } = DateTime.Parse("10/10/1999").Date;
    }
}
