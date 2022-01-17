using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.Models
{
    [Table("Tab")]
    public class Tab
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string NameZh { get; set; }
        public string Type { get; set; }

    }
}
