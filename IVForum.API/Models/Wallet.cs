using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVForum.API.Models
{
    public class Wallet
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public virtual User Owner { get; set; }
        [Required]
        public virtual Forum Forum { get; set; }

        public virtual List<Bill> Bills { get; set; } = new List<Bill>();
    }
}
