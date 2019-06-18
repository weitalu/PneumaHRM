using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public class HrmDbContext : DbContext
    {
        static HrmDbContext()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseSqlite("fake");
            using (var context = new HrmDbContext(builder.Options))
            {
                DataModel = context.Model;
            }
        }

        public static readonly IModel DataModel;

        private string createBy { get; }

        private HrmDbContext(DbContextOptions options) : base(options) { }
        public HrmDbContext(IHttpContextAccessor httpContextAccessor)
        {
            createBy = httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "System";
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveRequestComment> LeaveRequestComments { get; set; }
        public DbSet<RequestBalanceRelation> RequestBalanceRelations { get; set; }

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
                ((Entity)entity.Entity).CreatedOn = DateTime.Now;
            }
            return base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=TestDatabase.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Employee>()
                .HasKey(x => x.ADPrincipalName);

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


            modelBuilder.Entity<LeaveRequestComment>()
                .HasOne(x => x.Request)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RequestBalanceRelation>()
                .HasOne(x => x.Balance)
                .WithMany(x => x.RequestRelations)
                .HasForeignKey(x => x.BalanceId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<RequestBalanceRelation>()
                .HasOne(x => x.Request)
                .WithMany(x => x.BalanceRelations)
                .HasForeignKey(x => x.RequestId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
