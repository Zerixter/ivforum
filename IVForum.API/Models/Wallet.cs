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

        public Guid ForumId { get; set; }
        public virtual Forum Forum { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual List<BillOption> Bills { get; set; } = new List<BillOption>();
    }
}
