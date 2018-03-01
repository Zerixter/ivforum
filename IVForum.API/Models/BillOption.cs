using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVForum.API.Models
{
    public class BillOption : Bill
    {
        public Guid WalletId { get; set; }
        public virtual Wallet Wallet { get; set; }
    }
}
