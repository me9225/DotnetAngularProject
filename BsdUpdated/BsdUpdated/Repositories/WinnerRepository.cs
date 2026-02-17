using BsdUpdated.Data;
using BsdUpdated.IRepositories;
using BsdUpdated.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BsdUpdated.Repositories
{
    public class WinnerRepository : IWinnerRepository
    {
        private readonly SaleContextFactory _saleContextFactory;
        private Lazy<SaleContext> _lazyContext;
        private readonly IGiftRepository _giftRepository;


        public WinnerRepository(SaleContextFactory saleContextFactory, IGiftRepository giftRepository)
        {
            _saleContextFactory = saleContextFactory;
            _lazyContext = new Lazy<SaleContext>(() => _saleContextFactory.CreateContext());
            _giftRepository = giftRepository;
        }


        private SaleContext _context => _lazyContext.Value;
        public async Task<Winner?> CreateNewWinner(Winner winner)
        {
            _context.Winner.Add(winner);
            await _context.SaveChangesAsync();
            //var gift=_context.Gift.
            return winner == null ? null : winner;
        }

        public async Task<IEnumerable<Winner?>> GetAllWinners()
        {
            return await _context.Winner.ToListAsync();
        }
        public async Task<Winner?> GetWinnerByGiftId(int giftid)
        {
            return await _context.Winner.FirstOrDefaultAsync(w => w.IdGift == giftid);
        }

        public async Task<IEnumerable<Winner?>> DeleteAllWinners()
        {
            var winners = await _context.Winner.ToListAsync();
            if (winners == null || winners.Count == 0) return null;
            _context.Winner.RemoveRange(winners);
            await _context.SaveChangesAsync();
            return winners;
        }

        public async Task<IEnumerable<int?>> GetUsersIdForGift(int Giftid)//זה id של winner
        {
            var gift = await _context.Gift.FindAsync(Giftid);

            // אם אין מתנה כזאת או אם CardsList הוא null, החזר רשימה ריקה
            if (gift  == null)
            {
                return new List<int?>();  // רשימה ריקה
            }

            List<int?> userIds = new List<int?>();

            var allCards = await _context.Card.Where(c => c.GiftId == Giftid).ToListAsync();

            foreach (var card in allCards)
            {
                userIds.Add(card.UserId);
            }

            // אם לא נמצאו משתמשים, החזר רשימה ריקה
            return userIds.Count == 0 ? new List<int?>() : userIds;
        }
    }
}