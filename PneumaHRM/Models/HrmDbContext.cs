using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public class HrmDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveRequestApprove> LeaveRequestApproves { get; set; }
        public DbSet<LeaveRequestComment> LeaveRequestComments { get; set; }
        public DbSet<LeaveRequestDeputy> LeaveRequestDeputies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=PneumaHRM;Integrated Security=True");
        }
    }
}
