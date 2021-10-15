using Suggession.Models.Interface;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suggession.Models
{
    [Table("Comments")]
    public class Comment : IDateTracking
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        [MaxLength(50)]

        public int CreatedBy { get; set; }
        public int AccountId { get; set; }
        public int IdeaId { get; set; }
        public int? ModifiedBy { get; set; }
        [MaxLength(100)]
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }
        
    }
}
