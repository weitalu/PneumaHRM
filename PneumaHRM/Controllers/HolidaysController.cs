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
        public int ImportHolidays(List<HolidayDTO> data)
        {
            return _db.ImportHoliday(data);
        }


    }
}
