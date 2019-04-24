using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public class HrmDbContext : DbContext
    {
        private string createBy { get; }
        public HrmDbContext(IHttpContextAccessor httpContextAccessor)
        {
            createBy = httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "System";
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveRequestApprove> LeaveRequestApproves { get; set; }
        public DbSet<LeaveRequestComment> LeaveRequestComments { get; set; }
        public DbSet<LeaveRequestDeputy> LeaveRequestDeputies { get; set; }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries().ToList();
            var updated = entries
                .Where(x => x.State == EntityState.Modified);
            var added = entries
                .Where(x => x.State == EntityState.Added);
            foreach (var entity in updated)
            {
                ((Entity)entity.Entity).ModifiedBy = createBy;
                ((Entity)entity.Entity).ModifiedOn = DateTime.Now;
            }
            foreach (var entity in added)
            {
                ((Entity)entity.Entity).CreatedBy = createBy;
                ((Entity)entity.Entity).CreateOn = DateTime.Now;
            }
            return base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=PneumaHRM;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<LeaveBalance>()
                .HasOne(x => x.Owner)
                .WithMany(x => x.Balances)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(x => x.RequestIssuer)
                .WithMany(x => x.Leaves)
                .HasForeignKey(x => x.RequestIssuerId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            modelBuilder.Entity<LeaveRequestDeputy>()
                .HasOne(x => x.Request)
                .WithMany(x => x.Deputies)
                .HasForeignKey(x => x.RequestId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<LeaveRequestApprove>()
                .HasOne(x => x.Request)
                .WithMany(x => x.Approves)
                .HasForeignKey(x => x.RequestId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<LeaveRequestComment>()
                .HasOne(x => x.Request)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
