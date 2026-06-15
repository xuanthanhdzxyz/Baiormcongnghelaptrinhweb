using Microsoft.EntityFrameworkCore;
using Bai6_Validation.Models;

namespace Bai6_Validation.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Ten).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Gia).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TacGia).HasMaxLength(100);
                entity.Property(e => e.NhaXuatBan).HasMaxLength(100);
            });
        }
    }
}