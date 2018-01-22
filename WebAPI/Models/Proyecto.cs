using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Proyecto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual UserModel Owner { get; set; }
        public virtual List<UserModel> Users { get; set; } = new List<UserModel>();
    }
}
