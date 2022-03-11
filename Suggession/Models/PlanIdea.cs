using Suggession.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.Models
{
    [Table("PlanIdea")]
    public class PlanIdea
    {
        public PlanIdea()
        {
            //CreatedTime = DateTime.Now;
            IsDisplay = true;
        } 
        [Key]
        public int ID { get; set; }
        public int IdeaID { get; set; }
        public string Plan { get; set; }
        public string Description { get; set; }
        public int StatusPlanID { get; set; }
        public int CreatedBy { get; set; }
        public bool IsDisplay { get; set; }
        public DateTime? CreatedTime { get ; set ; }
        public DateTime? DeleteTime { get ; set ; }

    }
}
