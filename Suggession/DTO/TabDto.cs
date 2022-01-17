using Suggession.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.DTO
{
    public class TabDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string NameZh { get; set; }
        public string Type { get; set; }
        public bool Statues { get; set; }
    }
}
