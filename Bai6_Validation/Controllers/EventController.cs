using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bai6_Validation.Models;
using Bai6_Validation.Repositories;

namespace Bai6_Validation.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventRepository _eventRepository;

        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // GET: Danh sách sự kiện
        public async Task<IActionResult> Index(
            string? searchTerm,
            int? categoryId,
            DateTime? startDateFrom,
            DateTime? startDateTo,
            string? sortBy)
        {
            var events = await _eventRepository.SearchEventsAsync(
                searchTerm, categoryId, startDateFrom, startDateTo, sortBy);

            // Lấy danh sách category cho dropdown filter
            var categories = await _eventRepository.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);

            // Giữ lại giá trị tìm kiếm
            ViewBag.SearchTerm = searchTerm;
            ViewBag.CategoryId = categoryId;
            ViewBag.StartDateFrom = startDateFrom?.ToString("yyyy-MM-dd");
            ViewBag.StartDateTo = startDateTo?.ToString("yyyy-MM-dd");
            ViewBag.SortBy = sortBy;

            return View(events);
        }

        // GET: Chi tiết sự kiện
        public async Task<IActionResult> Detail(int id)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound("Không tìm thấy sự kiện với ID = " + id);
            }
            return View(eventItem);
        }

        // GET: Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _eventRepository.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event_bit240215 eventItem)
        {
            // Kiểm tra EventCategoryId tồn tại
            if (!await _eventRepository.CategoryExistsAsync(eventItem.EventCategoryId))
            {
                ModelState.AddModelError("EventCategoryId", "Loại sự kiện không tồn tại");
            }

            // Kiểm tra EndDate > StartDate
            if (eventItem.EndDate <= eventItem.StartDate)
            {
                ModelState.AddModelError("EndDate", "Ngày kết thúc phải lớn hơn ngày bắt đầu");
            }

            if (ModelState.IsValid)
            {
                await _eventRepository.AddEventAsync(eventItem);
                TempData["SuccessMessage"] = "Thêm sự kiện thành công!";
                return RedirectToAction(nameof(Index));
            }

            var categories = await _eventRepository.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", eventItem.EventCategoryId);
            return View(eventItem);
        }

        // GET: Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            var categories = await _eventRepository.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", eventItem.EventCategoryId);
            return View(eventItem);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event_bit240215 eventItem)
        {
            if (id != eventItem.Id)
            {
                return NotFound();
            }

            // Kiểm tra EventCategoryId tồn tại
            if (!await _eventRepository.CategoryExistsAsync(eventItem.EventCategoryId))
            {
                ModelState.AddModelError("EventCategoryId", "Loại sự kiện không tồn tại");
            }

            // Kiểm tra EndDate > StartDate
            if (eventItem.EndDate <= eventItem.StartDate)
            {
                ModelState.AddModelError("EndDate", "Ngày kết thúc phải lớn hơn ngày bắt đầu");
            }

            if (ModelState.IsValid)
            {
                await _eventRepository.UpdateEventAsync(eventItem);
                TempData["SuccessMessage"] = "Cập nhật sự kiện thành công!";
                return RedirectToAction(nameof(Index));
            }

            var categories = await _eventRepository.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", eventItem.EventCategoryId);
            return View(eventItem);
        }

        // GET: Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            // Kiểm tra nếu sự kiện đang diễn ra
            if (await _eventRepository.IsEventOngoingAsync(id))
            {
                TempData["ErrorMessage"] = "Không thể xóa sự kiện đang diễn ra!";
                return RedirectToAction(nameof(Index));
            }

            return View(eventItem);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Kiểm tra nếu sự kiện đang diễn ra
            if (await _eventRepository.IsEventOngoingAsync(id))
            {
                TempData["ErrorMessage"] = "Không thể xóa sự kiện đang diễn ra!";
                return RedirectToAction(nameof(Index));
            }

            await _eventRepository.DeleteEventAsync(id);
            TempData["SuccessMessage"] = "Xóa sự kiện thành công!";
            return RedirectToAction(nameof(Index));
        }

        // ===== QUẢN LÝ ẢNH =====

        // GET: Thêm ảnh cho sự kiện
        [HttpGet]
        public async Task<IActionResult> AddImage(int eventId)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventItem == null)
            {
                return NotFound();
            }

            ViewBag.EventId = eventId;
            ViewBag.EventName = eventItem.Name;
            return View();
        }

        // POST: Thêm ảnh
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(EventImage_bit240215 image)
        {
            if (ModelState.IsValid)
            {
                await _eventRepository.AddImageAsync(image);
                TempData["SuccessMessage"] = "Thêm ảnh thành công!";
                return RedirectToAction(nameof(Detail), new { id = image.EventId });
            }

            ViewBag.EventId = image.EventId;
            return View(image);
        }

        // POST: Xóa ảnh
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _eventRepository.GetImageByIdAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            var eventId = image.EventId;
            await _eventRepository.DeleteImageAsync(id);
            TempData["SuccessMessage"] = "Xóa ảnh thành công!";
            return RedirectToAction(nameof(Detail), new { id = eventId });
        }

        // POST: Chọn ảnh đại diện
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetThumbnail(int eventId, int imageId)
        {
            await _eventRepository.SetThumbnailAsync(eventId, imageId);
            TempData["SuccessMessage"] = "Đã chọn ảnh đại diện!";
            return RedirectToAction(nameof(Detail), new { id = eventId });
        }
    }
}