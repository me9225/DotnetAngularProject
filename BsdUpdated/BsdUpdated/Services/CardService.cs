//using BsdUpdated.IRepositories;
//using BsdUpdated.IServices;
using BsdUpdated.DTOs;
using BsdUpdated.IServices;
using BsdUpdated.Models;
using BsdUpdated.Repositories;
using BsdUpdated.Data;
using FinalProject.Repositories;
using Microsoft.Extensions.Logging;
using BsdUpdated.IRepositories;


namespace BsdUpdated.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _repository;
        private readonly IGiftService _Gservice;
        private readonly ILogger<CardService> _logger;
        public CardService(ICardRepository cardRepository,ILogger<CardService> logger, SaleContextFactory saleContextFactory, IGiftService giftService)
        {
            _logger = logger;
            _repository =cardRepository;
            _Gservice = giftService;
        }

        public async Task<IEnumerable<GiftDtoWithSum?>> GetAllMyCard(int Id)
        {
            _logger.LogInformation("start Getting all cards for user with ID: {UserId}", Id);
            var cards = await _repository.GetAllMyCard(Id);
            if (cards == null)
            {
                _logger.LogWarning("no cards found for user with ID: {UserId}", Id);
                throw new Exception("no cards found for this user");
            }
            var cardsWithDetails = new List<GiftDtoWithSum>();
            foreach (var card in cards)
            {
                _logger.LogInformation("Fetching gift details for Gift ID: {GiftId}", card.GiftId);
                var gift = await _Gservice.GetGiftById(card.GiftId);
                if (gift == null)
                {
                    _logger.LogError("no gift found for gift id {GiftId}", card.GiftId);
                    throw new Exception($"no gift found for gift id {card.GiftId}");
                }
                var giftWithSum = new GiftDtoWithSum
                {
                    Name = gift.Name,
                    Description = gift.Description,
                    Cost = gift.Cost,
                    Picture = gift.Picture,
                    CategoryId = gift.CategoryId,
                    DonorId = gift.DonorId,
                    WinnerName = gift.WinnerName,
                    Count = card.Count
                };
                cardsWithDetails.Add(giftWithSum);
            }
            return cardsWithDetails;
        }
        public async Task<Card?> GetCardById(int id)
        {
            _logger.LogInformation("Getting card by ID: {CardId}", id);
            return await _repository.GetCardById(id);
        }
        public async Task<IEnumerable<CardDto?>> CreateNewcCards(List<BasketDto> baskets)
        {
            _logger.LogInformation("start Creating new cards from baskets");
            List<Card> cardList = new List<Card>();
            foreach (var card in baskets)
            {
                Card newCard = new Card
                {
                    GiftId = card.GiftId,
                    UserId = card.UserId,
                    BuingDate = DateTime.Now
                };
                cardList.Add(newCard);
            }
            _logger.LogInformation("Creating {CardCount} new cards", cardList.Count);
            var createdCards = await _repository.CreateNewcCards(cardList);
            if (createdCards == null)
            {
                _logger.LogError("Failed to create cards");
                throw new Exception("Failed to create cards");
            }
            IEnumerable<CardDto?> cardDtos = createdCards.Select(c => new CardDto
            {
                Id = c.Id,
                GiftId = c.GiftId,
                UserId = c.UserId,
                BuingDate = c.BuingDate
            });
            return cardDtos;
        }

        public async Task<IEnumerable<CardWithBuyerDto?>> GetAllPurchasesOrderedByMostPurchasedGift()
        {
            return (await _repository.GetAllPurchasesOrderedByMostPurchasedGift());
        }
        public async Task<IEnumerable<CardWithBuyerDto?>> GetAllPurchasesOrderedByCost()
        {
            return (await _repository.GetAllPurchasesOrderedByCost()).ToList();
        }

        public async Task<IEnumerable<CardWithBuyerDto>> GetAllCardsWithBuyers()
        {
            return (await _repository.GetAllCardsWithBuyerNames()).ToList();
        }

    }


}

