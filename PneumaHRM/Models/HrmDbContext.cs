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
        public DbSet<LeaveBalance> Leaves { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveRequestApprove> LeaveRequestApproves { get; set; }
        public DbSet<LeaveRequestComment> LeaveRequestComments { get; set; }
        public DbSet<LeaveRequestDeputy> LeaveRequestDeputies { get; set; }

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
                .HasOne(x => x.DeputyBy)
                .WithMany(x => x.Deputies)
                .HasForeignKey(x => x.DeputyById)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<LeaveRequestDeputy>()
                .HasOne(x => x.Request)
                .WithMany(x => x.Deputies)
                .HasForeignKey(x => x.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LeaveRequestApprove>()
                .HasOne(x => x.ApproveBy)
                .WithMany(x => x.Approves)
                .HasForeignKey(x => x.ApproveById)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<LeaveRequestApprove>()
                .HasOne(x => x.Request)
                .WithMany(x => x.Approves)
                .HasForeignKey(x => x.RequestId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<LeaveRequestComment>()
                .HasOne(x => x.CommentBy)
                .WithMany(x => x.RequestComments)
                .HasForeignKey(x => x.CommentById)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<LeaveRequestComment>()
                .HasOne(x => x.Request)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
