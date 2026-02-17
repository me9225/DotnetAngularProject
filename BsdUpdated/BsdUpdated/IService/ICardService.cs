//using BsdUpdated.IRepositories;
//using BsdUpdated.IServices;
using BsdUpdated.DTOs;
using BsdUpdated.Models;

namespace BsdUpdated.IServices
{
    public interface ICardService
    {
        Task<IEnumerable<CardDto?>> CreateNewcCards(List<BasketDto> baskets);
        Task<IEnumerable<GiftDtoWithSum?>> GetAllMyCard(int Id);
        Task<Card?> GetCardById(int id);
        Task<IEnumerable<CardWithBuyerDto?>> GetAllPurchasesOrderedByMostPurchasedGift();
        Task<IEnumerable<CardWithBuyerDto>> GetAllCardsWithBuyers();
        Task<IEnumerable<CardWithBuyerDto>> GetAllPurchasesOrderedByCost();
    }
}

 