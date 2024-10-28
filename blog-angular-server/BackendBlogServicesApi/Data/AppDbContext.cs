using Microsoft.EntityFrameworkCore;
using BackendBlogServicesApi.Entries;
using BackendBlogServicesApi.Entity;

namespace BackendBlogServicesApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EntriesBlog> EntriesBlog { get; set; }
        public DbSet<EntriesBlogCategory> EntriesBlogCategory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la entidad Users
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<User>()
                .Property(u => u.Estado)
                .HasDefaultValue(true);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.email)
                .IsUnique();

            // Configuración de la entidad EntriesBlog
            modelBuilder.Entity<EntriesBlog>()
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<EntriesBlog>()
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<EntriesBlog>()
                .Property(e => e.Estado)
                .HasDefaultValue(true);

            modelBuilder.Entity<EntriesBlog>()
                .HasIndex(e => e.Title)
                .IsUnique();


            // Configuración de la entidad Categories
            modelBuilder.Entity<Categories>()
                    .Property(c => c.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Categories>()
                .Property(c => c.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Categories>()
                .Property(c => c.Estado)
                .HasDefaultValue(true);

            modelBuilder.Entity<Categories>()
                .HasIndex(c => c.Name)
                .IsUnique();


            // Configuración de la entidad EntriesBlogCategories (relación entre EntriesBlog y Categories)
            modelBuilder.Entity<EntriesBlogCategory>()
                    .Property(ec => ec.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<EntriesBlogCategory>()
                .Property(ec => ec.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<EntriesBlogCategory>()
                .Property(ec => ec.Estado)
                .HasDefaultValue(true);

            modelBuilder.Entity<EntriesBlogCategory>()
                .HasOne(ec => ec.EntriesBlog)
                .WithMany()
                .HasForeignKey(ec => ec.IdEntriesBlog)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EntriesBlogCategory>()
                .HasOne(ec => ec.Category)
                .WithMany()
                .HasForeignKey(ec => ec.IdCategories)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
