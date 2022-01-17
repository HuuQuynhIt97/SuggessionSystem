using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Suggession.Models;
using Suggession.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Suggession.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountGroup> AccountGroups { get; set; }
        public DbSet<AccountGroupAccount> AccountGroupAccount { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Models.SystemLanguage> systemLanguages { get; set; }

        public DbSet<UploadFile> UploadFiles { get; set; }
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<IdeaHistory> IdeaHistories { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Tab> Tabs { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {


        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<EntityEntry> modified = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
            foreach (EntityEntry item in modified)
            {
                if (item.Entity is IDateTracking changedOrAddedItem)
                {
                    if (item.State == EntityState.Added)
                    {
                        changedOrAddedItem.CreatedTime = changedOrAddedItem.CreatedTime == DateTime.MinValue ? DateTime.Now : changedOrAddedItem.CreatedTime;
                    }
                    else
                    {
                        changedOrAddedItem.ModifiedTime = DateTime.Now;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.LogTo(Console.WriteLine);
    }
}
