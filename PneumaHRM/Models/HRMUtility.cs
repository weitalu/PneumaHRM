using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public static class HRMUtility
    {
        public static decimal GetWorkHours(this List<Holiday> holidays, DateTime start, DateTime end)
        {
            var period = end - start;
            var days = (int)Math.Floor(period.TotalMinutes / 1440);
            var hours = Math.Min((decimal)((period.TotalMinutes % 1440) / 60), 8m);

            var holidaycount = holidays.Where(x => x.Value <= end.Date && x.Value >= start.Date).Count();

            return Math.Max(((days - holidaycount) * 8 + hours), 0);
        }
    }
}
