using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public class HrmContext
    {
        public HrmContext(HrmDbContext db, ClaimsPrincipal user,List<DateTime> holidays)
        {
            DbContext = db;
            UserContext = user;
            Holidays = holidays; 
        }
        public HrmDbContext DbContext { get; }
        public ClaimsPrincipal UserContext { get; }

        public List<DateTime> Holidays { get; }

    }
}
