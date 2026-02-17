//using BsdUpdated.IRepositories;
//using BsdUpdated.IServices;

//using BsdUpdated.IRepositories;
//using BsdUpdated.IServices;
using BsdUpdated.DTOs;

namespace BsdUpdated.IServices
{
    public interface IWinnerService
    {
        Task<WinnerDto?> CreateNewWinner(int giftId);
        Task<bool> DeleteAllWinners();
        Task<IEnumerable<WinnerDto?>> GetAllWinners();
    }
}