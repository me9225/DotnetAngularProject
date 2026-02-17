//using BsdUpdated.IRepositories;
//using BsdUpdated.IServices;
using BsdUpdated.DTOs;
using BsdUpdated.IServices;
using BsdUpdated.Models;
using BsdUpdated.Repositories;
using BsdUpdated.Data;

namespace BsdUpdated.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly CategoryRepository _repository;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(ILogger<CategoryService> logger,SaleContextFactory saleContextFactory)
        {
            _logger = logger;
            _repository = new CategoryRepository(saleContextFactory);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {   
            _logger.LogInformation("Fetching all categories");
            var categories = await _repository.GetAllCategories();
                if (categories == null) return Enumerable.Empty<CategoryDto>();
                return categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList();
        }
        

        public async Task<CategoryDto?> GetCategoryById(int id)
        {
            _logger.LogInformation("Fetching category with ID: {CategoryId}", id);
            var c = await _repository.GetCategoryById(id);
            if (c == null)
            {
                _logger.LogWarning("Category with ID: {CategoryId} not found", id);
                throw new Exception($"Category with id {id} not found.");
            }
            return new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
            };
        }
    }
}