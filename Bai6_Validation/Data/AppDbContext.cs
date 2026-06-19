using Microsoft.EntityFrameworkCore;
using Bai6_Validation.Models;

namespace Bai6_Validation.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet cũ (Book)
        public DbSet<Book> Books { get; set; }

        // 👇 THÊM CÁC DbSet MỚI
        public DbSet<EventCategory_bit240215> EventCategories_bit240215 { get; set; }
        public DbSet<Event_bit240215> Events_bit240215 { get; set; }
        public DbSet<EventImage_bit240215> EventImages_bit240215 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình Book (cũ)
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Ten).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Gia).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TacGia).HasMaxLength(100);
                entity.Property(e => e.NhaXuatBan).HasMaxLength(100);
            });

            // 👇 CẤU HÌNH CHO EVENT

            // 1. Cấu hình EventCategory
            modelBuilder.Entity<EventCategory_bit240215>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            // 2. Cấu hình Event
            modelBuilder.Entity<Event_bit240215>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Location).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);

                // Ràng buộc: không trùng tên và ngày bắt đầu
                entity.HasIndex(e => new { e.Name, e.StartDate })
                      .IsUnique()
                      .HasDatabaseName("IX_Event_Name_StartDate");

                // Quan hệ: EventCategory - Event (1-n)
                entity.HasOne(e => e.EventCategory)
                      .WithMany(c => c.Events)
                      .HasForeignKey(e => e.EventCategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // 3. Cấu hình EventImage
            modelBuilder.Entity<EventImage_bit240215>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);

                // Quan hệ: Event - EventImage (1-n)
                entity.HasOne(ei => ei.Event)
                      .WithMany(e => e.EventImages)
                      .HasForeignKey(ei => ei.EventId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Ràng buộc: mỗi event chỉ có 1 ảnh đại diện
                entity.HasIndex(ei => new { ei.EventId, ei.IsThumbnail })
                      .IsUnique()
                      .HasDatabaseName("IX_EventImage_UniqueThumbnail")
                      .HasFilter("[IsThumbnail] = 1");
            });
        }
    }
}