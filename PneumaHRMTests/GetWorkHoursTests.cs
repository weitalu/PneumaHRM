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
            var holidays = new List<DateTime>();

            var hours = holidays.GetWorkHours(start, end);

            Assert.Equal(8, hours);
        }

        [Theory]
        [InlineData("2019-02-28T08:00:00", "2019-02-28T11:00:00", 0)]
        [InlineData("2019-03-01T11:00:00", "2019-03-01T14:00:00", 2)]
        [InlineData("2019-02-27T08:00:00", "2019-02-28T11:00:00", 8)]
        [InlineData("2019-02-27T00:00:00", "2019-02-28T00:00:00", 8)]
        [InlineData("2019-03-01T12:00:00", "2019-03-01T20:30:00", 5)]
        [InlineData("2019-03-01T12:00:00", "2019-03-02T16:30:00", 11.5)]
        [InlineData("2019-03-01T09:00:00", "2019-03-02T16:30:00", 14.5)]
        [InlineData("2019-03-01T00:00:00", "2019-03-03T00:00:00", 16)]
        [InlineData("2019-03-01T09:00:00", "2019-03-05T17:00:00", 31)]
        [InlineData("2019-03-01T07:30:00", "2019-03-05T19:00:00", 32)]
        [InlineData("2019-03-01T07:30:00", "2019-03-06T07:30:00", 32)]
        [InlineData("2019-03-01T07:30:00", "2019-03-06T09:30:00", 32.5)]
        [InlineData("2019-03-01T09:30:00", "2019-03-06T18:30:00", 39.5)]
        [InlineData("2019-02-28T07:30:00", "2019-03-07T20:30:00", 40)]
        public void Theory(string start, string end, decimal expectHour)
        {
            var holidays = new List<DateTime>()
            {
                 new DateTime(2019,2,28),
                 new DateTime(2019,3,4),
                 new DateTime(2019,3,7)
            };

            var hours = holidays.GetWorkHours(DateTime.Parse(start), DateTime.Parse(end));

            Assert.Equal(expectHour, hours);
        }
    }
}
