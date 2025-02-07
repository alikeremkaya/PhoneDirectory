using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PhoneDirectory.Domain.Core.Base;
using PhoneDirectory.Domain.Entities;
using PhoneDirectory.Domain.Enums;
using PhoneDirectory.Infrastructure.Configurations;
using System.Security.Claims;

namespace PhoneDirectory.Infrastructure.AppContext
{
    public class AppDbContext:DbContext
    {
        public const string DevConnectionString = "AppConnectionDev";
     

      

       
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<CommunicationInfo> CommunicationInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
           
            builder.ApplyConfigurationsFromAssembly(typeof(IEntityConfiguration).Assembly);
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            SetBaseProperties();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetBaseProperties();
            return base.SaveChangesAsync(cancellationToken);
        }

       
        private void SetBaseProperties()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
           
            var userId = /*_httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)*/"Kullanıcı bulunamadı";

            foreach (var entry in entries)
            {
                SetIfAdded(entry, userId);
                SetIfModified(entry, userId);
                SetIfDeleted(entry, userId);
            }
        }

        private void SetIfAdded(EntityEntry<BaseEntity> entry, string userId)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.Status = Status.Created;
                entry.Entity.CreatedBy = userId;
                entry.Entity.CreatedDate = DateTime.Now;
            }
        }

        private void SetIfModified(EntityEntry<BaseEntity> entry, string userId)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.Status = Status.Updated;
                entry.Entity.UpdatedBy = userId;
                entry.Entity.UpdatedDate = DateTime.Now;
            }
        }

        private void SetIfDeleted(EntityEntry<BaseEntity> entry, string userId)
        {
            if (entry.State != EntityState.Deleted)
            {
                return;
            }

            // Process only auditable entities.
            if (entry.Entity is not AuditableEntity entity)
            {
                return;
            }

            // Soft-delete: mark entity as Deleted instead of removing it
            entry.State = EntityState.Modified;
            entry.Entity.Status = Status.Deleted;
            entity.DeletedDate = DateTime.Now;
            entity.DeletedBy = userId;
        }
    }
}

