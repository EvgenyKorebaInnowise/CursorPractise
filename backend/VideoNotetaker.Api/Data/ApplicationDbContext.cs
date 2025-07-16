using Microsoft.EntityFrameworkCore;
using VideoNotetaker.Api.Models;

namespace VideoNotetaker.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Note> Notes => Set<Note>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.Property(n => n.VideoId).IsRequired();
                entity.Property(n => n.TimestampSeconds).IsRequired();
                entity.Property(n => n.Text).IsRequired();
                entity.Property(n => n.CreatedAt).IsRequired();
                entity.HasIndex(n => n.VideoId);
            });
        }
    }
} 