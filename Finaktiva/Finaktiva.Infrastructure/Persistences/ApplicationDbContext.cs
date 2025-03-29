using Finaktiva.Domain.Common;
using Finaktiva.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finaktiva.Infrastructure.Persistences
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<EventLog> EventLogs { get; set; }
        public DbSet<ExcepcionLog> ExcepcionLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<EventType>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<EventLog>();
            modelBuilder.Entity<ExcepcionLog>();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Entity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreateById = entry.Entity.CreateById is null ? Guid.Empty : entry.Entity.CreateById;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = entry.Entity.LastModifiedBy is null ? Guid.Empty : entry.Entity.LastModifiedBy;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<Entity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreateById = entry.Entity.CreateById is null ? Guid.Empty : entry.Entity.CreateById;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = entry.Entity.LastModifiedBy is null ? Guid.Empty : entry.Entity.LastModifiedBy;
                        break;
                }
            }
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
