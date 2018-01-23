using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual User Owner { get; set; }
        public virtual List<User> Users { get; set; } = new List<User>();
    }
}
