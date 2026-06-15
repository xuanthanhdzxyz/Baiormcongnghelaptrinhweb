using Bai6_Validation.Models;

namespace Bai6_Validation.Repositories
{
    public interface IBookRepository
    {
        // Lấy tất cả sách
        Task<IEnumerable<Book>> GetAllAsync();

        // Lấy sách theo ID
        Task<Book?> GetByIdAsync(int id);

        // Thêm sách mới
        Task AddAsync(Book book);

        // Cập nhật sách
        Task UpdateAsync(Book book);

        // Xóa sách
        Task DeleteAsync(int id);

        // Kiểm tra sách tồn tại
        Task<bool> ExistsAsync(int id);
    }
}