using Suggession.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.Models
{
    [Table("TimeLine")]
    public class TimeLine : IDateTracking
    {
        [Key]
        public int ID { get; set; }
        public int IdeaID { get; set; }
        public int CreatedBy { get; set; }
        public string StatusName { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedTime { get ; set ; }
        public DateTime? ModifiedTime { get ; set ; }
      

    }
}
