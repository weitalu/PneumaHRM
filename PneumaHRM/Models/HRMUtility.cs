using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public static class HRMUtility
    {
        public static (bool, string) CanDeputyBy(this LeaveRequest target, string deputy)
        {
            if (target == null) return (false, "target not exist");
            // if (target.RequestIssuerId == deputy) return (false, "can't deputy yourself");
            if (target.Deputies.Select(x => x.DeputyBy).Contains(deputy)) return (false, "already deputy");
            return (true, "");
        }
        public static bool CanDelete(this LeaveRequest target)
        {
            return target != null && (target.State == LeaveRequestState.New || target.State == LeaveRequestState.Approved);
        }
        public static decimal GetWorkHours(this List<DateTime> holidays, DateTime start, DateTime end)
        {
            decimal result = 0m;
            for (var current = start; current < end; current = current.AddMinutes(30))
            {
                var isWorkHour = !holidays.Contains(current.Date) && current.Hour >= 9 && current.Hour < 18 && current.Hour != 12;
                if (isWorkHour) result += 0.5m;
            }
            return result;
        }
        public static int ImportHoliday(this HrmDbContext db, List<HolidayDTO> data)
        {
            var holidays = db.Holidays.Select(x => x.Value.Date).ToList();
            var toImport = data.Where(x => x.IsHoliday == "是")
                .Where(x => !holidays.Contains(x.Date.Date))
                .Select(x => new Holiday()
                {
                    Name = x.Name,
                    Description = $"{x.HolidayCategory}. {x.Description}".Trim(),
                    Value = x.Date
                }).ToList();

            db.Holidays.AddRange(toImport);
            return db.SaveChanges();
        }
        public static void SeedData(this HrmDbContext db)
        {
            db.Database.EnsureCreated();
            if (db.Holidays.Count() == 0)
            {
                var json = File.ReadAllText("InitData/Holidays.json");
                var data = JArray.Parse(json).ToObject<List<HolidayDTO>>();
                db.ImportHoliday(data);
            }
        }
    }
}
