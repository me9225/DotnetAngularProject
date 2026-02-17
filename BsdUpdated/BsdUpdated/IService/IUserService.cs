using BsdUpdated.DTOs;
using BsdUpdated.Models;

namespace BsdUpdated.IServices
{
    public interface IUserService
    {
        abstract bool VerifyPassword(string hashedPassword, string password);
        Task<(bool Success, string? Token, string? Error)> LoginAsync(LoginDto dto);
        Task<(bool Success, string? Token, string? Error)> UserRegister(CreateUserDto dto);
        // IUserService
        Task<User?> GetUserById(int userId);

    }
}