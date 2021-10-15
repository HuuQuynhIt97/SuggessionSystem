using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.Models
{
    [Table("OCNew")]
    public class OCNew
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int? ParentId { get; set; }

    }
}
