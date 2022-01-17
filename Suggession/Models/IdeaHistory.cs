using Suggession.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.Models
{
    [Table("IdeaHistory")]
    public class IdeaHistory : IDateTracking
    {
        [Key]
        public int Id { get; set; }
        public int IdeaID { get; set; }
        public int? InsertBy { get; set; }
        public int Status { get; set; }
        public bool Isshow { get; set; }
        public string Comment { get; set; }
        public string CommentZh { get; set; }
        public DateTime CreatedTime { get ; set ; }
        public DateTime? ModifiedTime { get ; set ; }
      

    }
}
