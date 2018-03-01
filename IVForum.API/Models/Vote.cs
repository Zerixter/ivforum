using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVForum.API.Models
{
    public class Vote : Bill
    {
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
