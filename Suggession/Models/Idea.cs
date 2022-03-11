using Suggession.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.Models
{
    [Table("Idea")]
    public class Idea : IDateTracking
    {
        [Key]
        public int Id { get; set; }
        public int? ReceiveID { get; set; }
        public int? SendID { get; set; }
        public string Title { get; set; }
        public string Issue { get; set; }
        public string Suggession { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public bool Isshow { get; set; }
        public bool IsAnnouncement { get; set; }
        public bool IsShowApproveTab { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedTime { get ; set ; }
        public DateTime? ModifiedTime { get ; set ; }
      

    }
}
