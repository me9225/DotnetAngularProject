using BsdUpdated.DTOs;
using BsdUpdated.Models;
using BsdUpdated.Services;
using BsdUpdated.Data;
using FinalProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BsdUpdated.IServices;

namespace BsdUpdated.Controllers
{
    [Authorize(Roles = "Manager")]
    [ApiController]
    [Route("api/[controller]")]
    public class WinnersController : ControllerBase
    {
        private readonly SaleContext _context;
        private readonly IWinnerService _WinnerService;
        private readonly IGiftService _giftService;
        private readonly ILogger<WinnersController> _logger;
        private readonly IUserService _userService;

        public WinnersController(IWinnerService winnerService, SaleContext context, ILogger<WinnersController> logger,IGiftService giftService, IUserService userService)
        {
            _WinnerService = winnerService;
            _context = context;
            _logger = logger;
            _giftService = giftService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WinnerDto>>> GetAllWinners()
        {
            try
            {
                var winners = await _WinnerService.GetAllWinners();
                return Ok(winners);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all winners.");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<WinnerDto>> AddWinner([FromQuery] int giftId)
        {
            try
            {
                var winner = await _WinnerService.CreateNewWinner(giftId);
                var gift = await _giftService.GetGiftById(winner.IdGift);
                var user = await _userService.GetUserById(winner.IdUser);

                gift.WinnerName = user.EMail;
                await _giftService.UpdateGift(gift);

                var updatedGift = await _giftService.GetGiftById(gift.Id);
                if (updatedGift.WinnerName != gift.WinnerName)
                {
                    _logger.LogWarning("Failed to update WinnerName for giftId: {GiftId}", gift.Id);
                }
                // אם אין רוכשים, החזר NotFound (סטטוס 404)
                if (winner == null)
                {
                    return NotFound(new { message = "אין רוכשים." });
                }

                return Ok(winner);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid argument provided while adding a new winner.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new winner.");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]     
        public async Task<ActionResult<bool>> DeleteAllWinners()
        {
            try
            {
                var winners = await _WinnerService.GetAllWinners(); // או הקריאה המתאימה להוציא את כל הזוכים
                if (winners == null || !winners.Any()) // אם המערך ריק
                {
                    return NotFound(new { message = "אין זוכים למחוק." });
                }
                foreach (var winner in winners)
                {
                    var gift = await _giftService.GetGiftById(winner.IdGift);
                    if (gift != null)
                    {
                        gift.WinnerName = " ";
                        await _giftService.UpdateGift(gift);
                    }
                }
                var sucsses = await _WinnerService.DeleteAllWinners();
                return Ok(sucsses);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting all winners.");
                return BadRequest(new { message = ex.Message });
            }
        }
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<WinnerDto>>> GetAll()
        //{
        //    var list = await _context.Winner
        //        .Select(w => new WinnerDto {
        //            Id = w.Id,
        //            IdUser = w.IdUser,
        //            IdGift = w.IdGift
        //        })
        //        .ToListAsync();
        //    return Ok(list);
        //}

        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<WinnerDto>> GetById(int id)
        //{
        //    var w = await _context.Winner.FindAsync(id);
        //    if (w == null) return NotFound();
        //    return Ok(new WinnerDto { Id = w.Id, IdUser = w.IdUser, IdGift = w.IdGift });
        //}

        //[HttpPost]
        //public async Task<ActionResult<WinnerDto>> Create(CreateWinnerDto create)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);
        //    var winner = new Winner { IdUser = create.IdUser, IdGift = create.IdGift };
        //    _context.Winner.Add(winner);
        //    await _context.SaveChangesAsync();
        //    return CreatedAtAction(nameof(GetById), new { id = winner.Id }, new WinnerDto { Id = winner.Id, IdUser = winner.IdUser, IdGift = winner.IdGift });
        //}

        //[HttpPut("{id:int}")]
        //public async Task<IActionResult> Update(int id, CreateWinnerDto update)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);
        //    var winner = await _context.Winner.FindAsync(id);
        //    if (winner == null) return NotFound();
        //    winner.IdUser = update.IdUser;
        //    winner.IdGift = update.IdGift;
        //    await _context.SaveChangesAsync();
        //    return NoContent();
        //}

        //[HttpDelete("{id:int}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var winner = await _context.Winner.FindAsync(id);
        //    if (winner == null) return NotFound();
        //    _context.Winner.Remove(winner);
        //    await _context.SaveChangesAsync();
        //    return NoContent();
        //}
    }
}