using BsdUpdated.DTOs;
using BsdUpdated.Models;

namespace BsdUpdated.IRepositories
{
    public interface ICardRepository
    {
        Task<IEnumerable<Card?>> CreateNewcCards(List<Card> cards);
        Task<IEnumerable<GroupedCardDto?>> GetAllMyCard(int Id);
        Task<Card?> GetCardById(int id);
        Task<IEnumerable<CardWithBuyerDto>> GetAllPurchasesOrderedByMostPurchasedGift();
        Task<IEnumerable<CardWithBuyerDto>> GetAllCardsWithBuyerNames();
        Task<IEnumerable<CardWithBuyerDto>> GetAllPurchasesOrderedByCost();


    }
}