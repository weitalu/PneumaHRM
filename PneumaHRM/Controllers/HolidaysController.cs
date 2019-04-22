using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PneumaHRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HolidaysController : ControllerBase
    {
        private HrmDbContext _db { get; }
        public HolidaysController(HrmDbContext dbContext)
        {
            _db = dbContext;
        }

        [Route("WorkHours")]
        [HttpGet]
        public decimal GetWorkHours(DateTime start, DateTime end)
        {
            return _db.Holidays
                .Select(x => x.Value)
                .ToList()
                .GetWorkHours(start, end);
        }
        [Route("Index")]
        [HttpGet]
        public List<object> GetHolidays(DateTime? start = null, DateTime? end = null)
        {
            return new List<object>();
        }
        [Route("Import")]
        [HttpPost]
        public int ImportHolidays(List<Holiday> data)
        {
            foreach (var holiday in data.Where(x => x.IsHoliday == "是").OrderBy(x => x.Date))
            {
                var toUpdate = _db.Holidays.Where(x => x.Value.Date == holiday.Date.Date).Count();

                if (toUpdate == 0)
                {
                    var desc = $"{holiday.HolidayCategory}. {holiday.Description}".Trim();
                    _db.Holidays.Add(new Models.Holiday()
                    {
                        Name = holiday.Name,
                        Description = desc,
                        Value = holiday.Date
                    });
                }
            }
            return _db.SaveChanges();
        }


        public class Holiday
        {
            public DateTime Date { get; set; }
            public string Name { get; set; }
            public string IsHoliday { get; set; }
            public string HolidayCategory { get; set; }
            public string Description { get; set; }
        }
    }
}
