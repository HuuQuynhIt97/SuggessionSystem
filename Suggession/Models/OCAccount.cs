using Suggession.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.Models
{
    [Table("OCAccounts")]
    public class OCAccount: IDateTracking
    {
        [Key]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int OCId { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }

        [ForeignKey(nameof(OCId))]
        public virtual OC OC { get; set; }
    }
}
