using Suggession.Models.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suggession.Models
{
    [Table("Accounts")]
    public class Account: IDateTracking
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Username { get; set; }
        [MaxLength(255)]
        public string FullName { get; set; }
        [MaxLength(255)]
        public string Password { get; set; }
        [MaxLength(255)]
        public string Email { get; set; }
        public bool IsLock { get; set; }
        public int? AccountGroupId { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedTime { get ; set ; }
        public DateTime? ModifiedTime { get ; set ; }

        public virtual ICollection<AccountGroupAccount> AccountGroupAccount { get; set; }


    }
}
