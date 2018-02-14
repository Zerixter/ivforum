using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVForum.API.Models
{
    public class Token
    {
        [Key]
        [MaxLength(250)]
        public string Id { get; set; }

        [Required]
        public virtual User User { get; set; }
    }
}
