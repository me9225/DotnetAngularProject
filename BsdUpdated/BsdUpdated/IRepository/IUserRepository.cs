using BsdUpdated.Models;

namespace BsdUpdated.IRepositories
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);
        Task<User?> GetByEmail(string email);
        // IUserRepository
        Task<User?> GetUserById(int userId);

    }
}