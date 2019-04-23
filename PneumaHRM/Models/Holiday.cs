using GraphQL.Types;
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

    public class HolidayDTO
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string IsHoliday { get; set; }
        public string HolidayCategory { get; set; }
        public string Description { get; set; }
    }

    public class HolidayType : ObjectGraphType<Holiday>
    {
        public HolidayType()
        {
            Field(x => x.Name, true);
            Field(x => x.Value);
        }
    }
}
