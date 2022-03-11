using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.DTO
{

    public class PlanIdeaDto
    {
        public int ID { get; set; }
        public int IdeaID { get; set; }
        public string Plan { get; set; }
        public int Index { get; set; }
        public string Description { get; set; }
        public int StatusPlanID { get; set; }
        public int CreatedBy { get; set; }
        public bool IsDisplay { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string PlanStatusName { get; set; }
        public string DeleteTime { get; set; }
    }
    
}
