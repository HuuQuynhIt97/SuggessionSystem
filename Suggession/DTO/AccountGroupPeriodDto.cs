using Suggession.Models;
using Suggession.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.DTO
{
    public class AccountGroupPeriodDto
    {
        public int Id { get; set; }
        public int AccountGroupId { get; set; }
  
        public int PeriodId { get; set; }
        public AccountGroup AccountGroup { get; set; }


    }
}
