using Bai6_Validation.Models;

namespace Bai6_Validation.Repositories
{
    public interface IEventRepository
    {
        // Event
        Task<IEnumerable<Event_bit240215>> GetAllEventsAsync();
        Task<Event_bit240215?> GetEventByIdAsync(int id);
        Task AddEventAsync(Event_bit240215 eventItem);
        Task UpdateEventAsync(Event_bit240215 eventItem);
        Task DeleteEventAsync(int id);
        Task<bool> EventExistsAsync(int id);

        // Event Category
        Task<IEnumerable<EventCategory_bit240215>> GetAllCategoriesAsync();
        Task<EventCategory_bit240215?> GetCategoryByIdAsync(int id);
        Task<bool> CategoryExistsAsync(int id);
        Task<bool> CategoryHasEventsAsync(int id);

        // Event Image
        Task<IEnumerable<EventImage_bit240215>> GetImagesByEventIdAsync(int eventId);
        Task<EventImage_bit240215?> GetImageByIdAsync(int id);
        Task AddImageAsync(EventImage_bit240215 image);
        Task DeleteImageAsync(int id);
        Task SetThumbnailAsync(int eventId, int imageId);

        // Search & Filter
        Task<IEnumerable<Event_bit240215>> SearchEventsAsync(
            string? searchTerm,
            int? categoryId,
            DateTime? startDateFrom,
            DateTime? startDateTo,
            string? sortBy);

        // Check event status
        Task<bool> IsEventOngoingAsync(int eventId);
    }
}