using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public abstract class Entity
    {
        public DateTime CreateOn { get; set; } = DateTime.Now;
        public DateTime? UpdateOn { get; set; } = null;
    }
}
