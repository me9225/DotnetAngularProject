//using BsdUpdated.IRepositories;
//using BsdUpdated.IServices;
using BsdUpdated.DTOs;
using BsdUpdated.IServices;
using BsdUpdated.Models;
using BsdUpdated.Repositories;

using BsdUpdated.Data;
namespace BsdUpdated.Services
{
    public class DonorService : IDonorService
    {
        private readonly DonorRepository _repository;
        private readonly ILogger<DonorService> _logger;
        public DonorService(ILogger<DonorService> logger,SaleContextFactory saleContextFactory)
        {
            _logger = logger;
            _repository = new DonorRepository(saleContextFactory);
        }

        public async Task<DonorDto?> GetDonorById(int id)
        {
            _logger.LogInformation($"Fetching donor with id {id}");
            var d = await _repository.GetDonorById(id);
            if (d == null)
            {
                _logger.LogWarning($"Donor with id {id} not found.");
                throw new Exception($"Donor with id {id} not found.");
            }
            return new DonorDto
            {
                Id = d.Id,
                Name = d.Name,
                Email = d.EMail,
            };
        }


        public async Task<DonorDto?> CreateNewDonor(CreateDonorDto donorDto)
        {
            _logger.LogInformation($"Creating new donor with email {donorDto.Email}");
            var donor = new Donor
            {
                Name = donorDto.Name,
                EMail = donorDto.Email,
                GiftsList = new List<Gift>()
            };
            var exist = await _repository.GetDonorByEmail(donor.EMail);
            _logger.LogInformation($"Checking if donor with email {donorDto.Email} already exists");
            if (exist != null)
            {
                _logger.LogWarning($"Donor with email {donorDto.Email} already exists.");   
                throw new Exception("Donor with this email already exists.");
            }
            var d = await _repository.CreateNewDonor(donor);
            if (d == null)
            {
                _logger.LogError("Donor could not be created.");
                throw new Exception("Donor could not be created.");
            }
            return new DonorDto
            {
                Id = d.Id,
                Name = d.Name,
                Email = d.EMail
            };
        }

        public async Task<DonorDto?> UpdateDonor(DonorDto donorDto)
        {
            _logger.LogInformation($"start Updating donor with id {donorDto.Id}");
            Donor donor = new Donor
            {
                Id = donorDto.Id,
                Name = donorDto.Name,
                EMail = donorDto.Email,
            };
            _logger.LogInformation($"try Updating donor with id {donorDto.Id}");
            var d = await _repository.UpdateDonor(donor);
            
            if (d == null)
            {
                _logger.LogError("Donor could not be updated.");
                throw new Exception("Donor could not be updated.");
            }
            return new DonorDto
            {
                Id = d.Id,
                Name = d.Name,
                Email = d.EMail
            };
        }

        public async Task<IEnumerable<DonorDto>> GetAllDonors()
        {
            var donors = await _repository.GetAllDonors();
            return donors.Select(d => new DonorDto
            {
                Id = d.Id,
                Name = d.Name,
                Email = d.EMail,
            }).ToList();
        }

        public async Task<DonorDto?> DeleteDonor(int id)
        {
            _logger.LogInformation($"start Deleting donor with id {id}");
            var d = await _repository.DeleteDonor(id);
            if (d == null)
            {
                _logger.LogError($"Donor with id {id} not found.");
                throw new Exception($"Donor with id {id} not found.");
            }
            return new DonorDto
            {
                Id = d.Id,
                Name = d.Name,
                Email = d.EMail,
            };
        }
        public async Task<IEnumerable<GiftDto>> GetDonorGiftList(int id)
        {
            _logger.LogInformation($"Fetching gift list for donor with id {id}");
            var gifts = await _repository.GetDonorGiftList(id);
            _logger.LogInformation($"Found {gifts.Count()} gifts for donor with id {id}");
            return gifts.Select(g => new GiftDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                Cost = g.Cost,
                Picture = g.Picture,
                CategoryId = g.CategoryId,
                DonorId = g.DonorId,
                WinnerName = g.WinnerName
            }).ToList();
        }
    }
}