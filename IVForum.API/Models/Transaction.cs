using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVForum.API.Models
{
    public class Transaction : Bill
    {
        public Guid ForumId { get; set; }
        public virtual Forum Forum { get; set; }
    }
}
