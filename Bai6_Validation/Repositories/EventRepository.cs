using Microsoft.EntityFrameworkCore;
using Bai6_Validation.Models;
using Bai6_Validation.Data;

namespace Bai6_Validation.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        // ===== EVENT =====
        public async Task<IEnumerable<Event_bit240215>> GetAllEventsAsync()
        {
            return await _context.Events_bit240215
                .Include(e => e.EventCategory)
                .Include(e => e.EventImages)
                .ToListAsync();
        }

        public async Task<Event_bit240215?> GetEventByIdAsync(int id)
        {
            return await _context.Events_bit240215
                .Include(e => e.EventCategory)
                .Include(e => e.EventImages)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddEventAsync(Event_bit240215 eventItem)
        {
            await _context.Events_bit240215.AddAsync(eventItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEventAsync(Event_bit240215 eventItem)
        {
            _context.Events_bit240215.Update(eventItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(int id)
        {
            var eventItem = await GetEventByIdAsync(id);
            if (eventItem != null)
            {
                _context.Events_bit240215.Remove(eventItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> EventExistsAsync(int id)
        {
            return await _context.Events_bit240215.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> IsEventOngoingAsync(int eventId)
        {
            var eventItem = await GetEventByIdAsync(eventId);
            if (eventItem == null) return false;

            var now = DateTime.Now;
            return eventItem.StartDate <= now && eventItem.EndDate >= now;
        }

        // ===== EVENT CATEGORY =====
        public async Task<IEnumerable<EventCategory_bit240215>> GetAllCategoriesAsync()
        {
            return await _context.EventCategories_bit240215.ToListAsync();
        }

        public async Task<EventCategory_bit240215?> GetCategoryByIdAsync(int id)
        {
            return await _context.EventCategories_bit240215.FindAsync(id);
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _context.EventCategories_bit240215.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CategoryHasEventsAsync(int id)
        {
            return await _context.Events_bit240215.AnyAsync(e => e.EventCategoryId == id);
        }

        // ===== EVENT IMAGE =====
        public async Task<IEnumerable<EventImage_bit240215>> GetImagesByEventIdAsync(int eventId)
        {
            return await _context.EventImages_bit240215
                .Where(i => i.EventId == eventId)
                .ToListAsync();
        }

        public async Task<EventImage_bit240215?> GetImageByIdAsync(int id)
        {
            return await _context.EventImages_bit240215.FindAsync(id);
        }

        public async Task AddImageAsync(EventImage_bit240215 image)
        {
            // Nếu là ảnh đại diện, xóa ảnh đại diện cũ
            if (image.IsThumbnail)
            {
                var oldThumbnail = await _context.EventImages_bit240215
                    .FirstOrDefaultAsync(i => i.EventId == image.EventId && i.IsThumbnail);
                if (oldThumbnail != null)
                {
                    oldThumbnail.IsThumbnail = false;
                    _context.EventImages_bit240215.Update(oldThumbnail);
                }
            }

            await _context.EventImages_bit240215.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteImageAsync(int id)
        {
            var image = await GetImageByIdAsync(id);
            if (image != null)
            {
                _context.EventImages_bit240215.Remove(image);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SetThumbnailAsync(int eventId, int imageId)
        {
            // Bỏ chọn tất cả ảnh đại diện của event
            var allImages = await _context.EventImages_bit240215
                .Where(i => i.EventId == eventId)
                .ToListAsync();

            foreach (var img in allImages)
            {
                img.IsThumbnail = false;
            }

            // Chọn ảnh mới làm đại diện
            var newThumbnail = await GetImageByIdAsync(imageId);
            if (newThumbnail != null && newThumbnail.EventId == eventId)
            {
                newThumbnail.IsThumbnail = true;
                await _context.SaveChangesAsync();
            }
        }

        // ===== SEARCH & FILTER =====
        public async Task<IEnumerable<Event_bit240215>> SearchEventsAsync(
            string? searchTerm,
            int? categoryId,
            DateTime? startDateFrom,
            DateTime? startDateTo,
            string? sortBy)
        {
            var query = _context.Events_bit240215
                .Include(e => e.EventCategory)
                .Include(e => e.EventImages)
                .AsQueryable();

            // Tìm kiếm theo tên hoặc địa điểm
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e =>
                    e.Name.Contains(searchTerm) ||
                    e.Location.Contains(searchTerm));
            }

            // Lọc theo loại sự kiện
            if (categoryId.HasValue)
            {
                query = query.Where(e => e.EventCategoryId == categoryId.Value);
            }

            // Lọc theo khoảng ngày bắt đầu
            if (startDateFrom.HasValue)
            {
                query = query.Where(e => e.StartDate >= startDateFrom.Value);
            }
            if (startDateTo.HasValue)
            {
                query = query.Where(e => e.StartDate <= startDateTo.Value);
            }

            // Sắp xếp
            query = sortBy switch
            {
                "startDate_asc" => query.OrderBy(e => e.StartDate),
                "price_asc" => query.OrderBy(e => e.Price),
                "price_desc" => query.OrderByDescending(e => e.Price),
                _ => query.OrderBy(e => e.StartDate) // Mặc định: ngày gần nhất
            };

            return await query.ToListAsync();
        }
    }
}