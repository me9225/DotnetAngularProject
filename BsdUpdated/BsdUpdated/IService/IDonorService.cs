//using BsdUpdated.IRepositories;
//using BsdUpdated.IServices;

//using BsdUpdated.IRepositories;
//using BsdUpdated.IServices;
using BsdUpdated.DTOs;

namespace BsdUpdated.IServices
{
    public interface IDonorService
    {
        Task<DonorDto?> CreateNewDonor(CreateDonorDto donorDto);
        Task<DonorDto?> DeleteDonor(int id);
        Task<IEnumerable<DonorDto>> GetAllDonors();
        Task<DonorDto?> GetDonorById(int id);
        Task<IEnumerable<GiftDto>> GetDonorGiftList(int id);
        Task<DonorDto?> UpdateDonor(DonorDto donorDto);
    }
}