using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public abstract class Entity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; } = null;
        
        
    }
}
