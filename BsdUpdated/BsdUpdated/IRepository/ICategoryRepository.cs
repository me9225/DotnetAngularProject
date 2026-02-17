using BsdUpdated.Models;

namespace BsdUpdated.IRepositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category?> GetCategoryById(int id);
    }
}