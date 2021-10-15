using Suggession.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.Models
{
    [Table("TargetYTD")]
    public class TargetYTD: IDateTracking
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public int KPIId { get; set; }

        [ForeignKey(nameof(KPIId))]
        public virtual KPINew KPINew { get; set; }

    }
}
