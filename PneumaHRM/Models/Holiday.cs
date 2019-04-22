using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public class Holiday : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Value { get; set; }
        public string Description { get; set; }
    }
}
