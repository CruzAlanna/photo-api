using Microsoft.EntityFrameworkCore;
using PhotoApi.Models;  // Change "PhotoApi" to your actual project name

namespace PhotoApi.Data  // Change "PhotoApi" to your actual project name
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Photo> Photos { get; set; }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Photo && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var photo = (Photo)entityEntry.Entity;
                
                if (entityEntry.State == EntityState.Added)
                {
                    photo.CreatedAt = DateTime.UtcNow;
                }
                
                photo.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}