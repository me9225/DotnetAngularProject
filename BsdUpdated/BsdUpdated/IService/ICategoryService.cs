//using BsdUpdated.IRepositories;
//using BsdUpdated.IServices;

//using BsdUpdated.IRepositories;
//using BsdUpdated.IServices;
using BsdUpdated.DTOs;

namespace BsdUpdated.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task<CategoryDto?> GetCategoryById(int id);
    }
}