using BsdUpdated.DTOs;
using BsdUpdated.IServices;
//using BsdUpdated.IServices;
using BsdUpdated.Models;
using BsdUpdated.Repositories;
using BsdUpdated.Services;
using BsdUpdated.Data;
using FinalProject.Repositories;
using Microsoft.Extensions.Logging;
using BsdUpdated.IRepositories;

namespace FinalProject.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _repository;
        private readonly IGiftService _giftService;
        private readonly ILogger<BasketService> _logger;

        public BasketService(ILogger<BasketService> logger, SaleContextFactory saleContextFactory, IGiftService giftService,IBasketRepository basketRepository)
        {
            _logger = logger;
            _repository =basketRepository;
            _giftService = giftService;
        }

        public async Task<List<BasketDto>> GetAllMyBasket(int userId)
        {
            _logger.LogInformation("Fetching baskets for user with ID: {UserId}", userId);
            var baskets = await _repository.GetAllMyBasket(userId);
            _logger.LogInformation("Retrieved  baskets for user with ID: {UserId}",userId);
            return baskets.Select(b => new BasketDto
            {
                Id = b.Id,
                UserId = b.UserId,
                GiftId = b.GiftId
            }).ToList();
        }
        public async Task<CreateBasketDto> CreateNewBasket(CreateBasketDto basket)
        {
            _logger.LogInformation("Creating new basket for user with ID: {UserId} and Gift ID: {GiftId}", basket.UserId, basket.GiftId);
            try
            {
                var gift = await _giftService.GetGiftById(basket.GiftId);
                _logger.LogInformation("Fetched gift with ID: {GiftId} for basket creation", basket.GiftId);
                if (gift == null)
                {
                    _logger.LogWarning("Gift with ID: {GiftId} not found", basket.GiftId);
                    throw new Exception("Gift not found");
                }

                Basket b = new();
                b.UserId = basket.UserId;
                b.GiftId = basket.GiftId;


                var B = await _repository.CreateNewBasket(b);
                return basket;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating basket for user with ID: {UserId} and Gift ID: {GiftId}", basket.UserId, basket.GiftId);
                throw new ApplicationException("Failed to create basket", ex);
            }
        }
        public async Task<BasketDto> DeleteOneBasket(int id)
        {
            _logger.LogInformation("start Deleting basket with ID: {BasketId}", id);
            try
            {
                var basket = await _repository.DeleteOneBasket(id);
                if (basket == null)
                {
                    _logger.LogWarning("Basket with ID: {BasketId} not found for deletion", id);
                    throw new Exception("Basket not found");
                }
                var gift = await _giftService.GetGiftById(basket.GiftId);
                if (gift == null)
                {
                    _logger.LogWarning("Gift with ID: {GiftId} not found during basket deletion", basket.GiftId);
                    throw new Exception("Gift not found");
                }
                BasketDto bd = new();
                bd.Id = basket.Id;
                bd.UserId = basket.UserId;
                bd.GiftId = basket.GiftId;
                return bd;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting basket with ID: {BasketId}", id);
                throw new ApplicationException("Failed to delete basket", ex);
            }
        }
        public async Task DeleteAllBasket(int userId)
        {
            _logger.LogInformation("start Deleting all baskets for user with ID: {UserId}", userId);
            try
            {
                var deletedBaskets = await _repository.DeleteAllBasket(userId);

                if (!deletedBaskets.Any())
                {
                    _logger.LogWarning("No baskets found to delete for user with ID: {UserId}", userId);
                    throw new Exception("No baskets found to delete");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting all baskets for user with ID: {UserId}", userId);
                throw new ApplicationException("Failed to delete all baskets", ex);
            }
        }

    }
}

