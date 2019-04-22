using System;
using Xunit;
using PneumaHRM.Models;
using System.Collections.Generic;

namespace PneumaHRMTests
{
    public class GetWorkHoursTests
    {
        [Fact]
        public void Test1()
        {
            var start = new DateTime(2019, 3, 1, 9, 0, 0);
            var end = new DateTime(2019, 3, 1, 18, 0, 0);
            var holidays = new List<Holiday>();

            var hours = holidays.GetWorkHours(start, end);

            Assert.Equal(8, hours);
        }

        [Theory]
        [InlineData("2019-03-01T08:00:00", "2019-03-01T11:00:00", 3)]
        [InlineData("2019-03-01T12:00:00", "2019-03-01T16:30:00", 4.5)]
        [InlineData("2019-03-01T12:00:00", "2019-03-02T16:30:00", 12.5)]
        [InlineData("2019-03-01T09:00:00", "2019-03-02T16:30:00", 15.5)]
        [InlineData("2019-03-01T00:00:00", "2019-03-03T00:00:00", 16)]
        [InlineData("2019-03-01T08:00:00", "2019-03-05T17:00:00", 32)]
        [InlineData("2019-03-01T07:30:00", "2019-03-05T19:00:00", 32)]
        [InlineData("2019-03-01T07:30:00", "2019-03-06T07:30:00", 32)]
        [InlineData("2019-03-01T07:30:00", "2019-03-06T09:30:00", 34)]
        [InlineData("2019-03-01T09:30:00", "2019-03-06T18:30:00", 40)]
        [InlineData("2019-02-28T07:30:00", "2019-03-07T20:30:00", 40)]
        public void Theory(string start, string end, decimal expectHour)
        {
            var holidays = new List<Holiday>()
            {
                new Holiday(){ Value = new DateTime(2019,2,28) },
                new Holiday(){ Value = new DateTime(2019,3,4) },
                new Holiday(){ Value = new DateTime(2019,3,7) }
            };

            var hours = holidays.GetWorkHours(DateTime.Parse(start), DateTime.Parse(end));

            Assert.Equal(expectHour, hours);
        }
    }
}
