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
            foreach (var holiday in data.Where(x => x.IsHoliday == "是").OrderBy(x => x.Date))
            {
                var toUpdate = db.Holidays.Where(x => x.Value.Date == holiday.Date.Date).Count();

                if (toUpdate == 0)
                {
                    var desc = $"{holiday.HolidayCategory}. {holiday.Description}".Trim();
                    db.Holidays.Add(new Models.Holiday()
                    {
                        Name = holiday.Name,
                        Description = desc,
                        Value = holiday.Date
                    });
                }
            }
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
