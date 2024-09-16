using bossabox_backend_challange.Models;
using Microsoft.EntityFrameworkCore;

namespace bossabox_backend_challange.Data;

public class VUTTRDbContext : DbContext
{
    public VUTTRDbContext(DbContextOptions<VUTTRDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Tool> Tools { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>()
            .HasMany(x => x.Tools)
            .WithOne(x => x.User);

        modelBuilder.Entity<User>()
            .HasIndex(x => x.Name).IsUnique(true);

        modelBuilder.Entity<User>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Tool>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Tag>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
         
    }
}
