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
        public static decimal GetWorkHours(this List<DateTime> holidays, DateTime start, DateTime end)
        {
            var period = end - start;
            var days = (int)Math.Floor(period.TotalMinutes / 1440);
            var hours = Math.Min((decimal)((period.TotalMinutes % 1440) / 60), 8m);

            var holidaycount = holidays
                .Where(x => x <= end.Date && x >= start.Date)
                .Select(x => x.Date)
                .Distinct()
                .Count();

            return Math.Max(((days - holidaycount) * 8 + hours), 0);
        }
        public static int ImportHoliday(this HrmDbContext db, List<HolidayDTO> data)
        {
            var holidays = db.Holidays.Select(x => x.Value.Date).ToList();
            var toImport = data.Where(x => x.IsHoliday == "是")
                .Where(x => ! holidays.Contains(x.Date.Date))
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
