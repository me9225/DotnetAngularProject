//using BsdUpdated.IRepositories;
//using BsdUpdated.IServices;

//using BsdUpdated.IRepositories;
//using BsdUpdated.IServices;
using BsdUpdated.DTOs;

namespace BsdUpdated.IServices
{
    public interface IGiftService
    {
        Task<GiftDto> CreateNewGift(GiftDto giftDto);
        Task<bool> DeleteGift(int id);
        Task<List<GiftDto>> GetAllGifts();
        Task<GiftDto?> GetGiftById(int id);
        Task<List<GiftDto>> GetGiftsByCategoryId(int categoryId);
        Task<List<GiftDto>> GetGiftsByCost(int price1, int price2);
        Task<GiftDto?> UpdateGift(GiftDto giftDto);
        Task<List<CardDto>> GetCardsByGiftId(int giftId);
    }
}