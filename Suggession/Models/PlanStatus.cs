using Suggession.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.Models
{
    [Table("PlanStatus")]
    public class PlanStatus
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

    }
}
