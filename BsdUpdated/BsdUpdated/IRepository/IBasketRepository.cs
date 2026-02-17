//using AutoMapper;

//using AutoMapper;
using BsdUpdated.Models;

namespace BsdUpdated.IRepositories
{
    public interface IBasketRepository
    {
        Task<Basket> CreateNewBasket(Basket basket);
        Task<List<Basket>> DeleteAllBasket(int id);
        Task<Basket> DeleteOneBasket(int id);
        Task<IEnumerable<Basket>> GetAllMyBasket(int Id);
    }
}