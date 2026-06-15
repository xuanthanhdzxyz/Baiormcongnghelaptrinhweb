using Microsoft.AspNetCore.Mvc;
using Bai6_Validation.Models;
using Bai6_Validation.Repositories;

namespace Bai6_Validation.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;

        // Inject Repository thay vì DbContext
        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // GET: Danh sách sách
        public async Task<IActionResult> Index()
        {
            var books = await _bookRepository.GetAllAsync();
            return View(books);
        }

        // GET: Form thêm sách
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Xử lý thêm sách
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                await _bookRepository.AddAsync(book);
                TempData["SuccessMessage"] = "Thêm sách thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Chi tiết sách
        public async Task<IActionResult> Detail(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound($"Không tìm thấy sách với ID = {id}");
            }
            return View(book);
        }

        // GET: Form chỉnh sửa sách
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Xử lý chỉnh sửa sách
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _bookRepository.UpdateAsync(book);
                    TempData["SuccessMessage"] = "Cập nhật sách thành công!";
                }
                catch (Exception)
                {
                    if (!await _bookRepository.ExistsAsync(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Form xóa sách
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Xử lý xóa sách
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookRepository.DeleteAsync(id);
            TempData["SuccessMessage"] = "Xóa sách thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}