using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVForum.API.Models
{
    public class Bill
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public string ImgUri { get; set; }
    }
}
