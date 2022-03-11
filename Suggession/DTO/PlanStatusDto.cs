using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.DTO
{

    public class PlanStatusDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
    
}
