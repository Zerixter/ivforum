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
      
        public Guid ForumId { get; set; }
        public virtual Forum Forum { get; set; }
        public virtual List<Bill> Bills { get; set; } = AssignWallet();
        public static List<Bill> AssignWallet()
        {
            return new List<Bill>
            {
                new Bill
                {
                    Id = Guid.NewGuid(),
                    Name = "20",
                    Value = 20
                },
                new Bill
                {
                    Id = Guid.NewGuid(),
                    Name = "50",
                    Value = 50
                },
                new Bill
                {
                    Id = Guid.NewGuid(),
                    Name = "100",
                    Value = 100
                }
            };
        }
    }
}
