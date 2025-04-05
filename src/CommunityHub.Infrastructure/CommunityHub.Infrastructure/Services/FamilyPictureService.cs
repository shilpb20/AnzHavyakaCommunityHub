using AppComponents.Repository.Abstraction;
using AppComponents.Repository.EFCore;
using CommunityHub.Infrastructure.Data;
using CommunityHub.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Infrastructure.Services.User
{
    public class FamilyPictureService : IFamilyPictureService
    {
        private readonly ILogger<FamilyPictureService> _logger;
        private readonly IRepository<FamilyPicture, ApplicationDbContext> _repository;

        public FamilyPictureService(ILogger<FamilyPictureService> logger,
            IRepository<FamilyPicture, ApplicationDbContext> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<FamilyPicture> CreateFamilyPictureAsync(FamilyPicture familyPicture)
        {
            return await _repository.AddAsync(familyPicture);
        }

        public async Task<FamilyPicture> DeleteFamilyPictureAsync(FamilyPicture familyPicture)
        {
            return await _repository.DeleteAsync(familyPicture);
        }

        public async Task<List<FamilyPicture>> GetAllFamilyPicturesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<FamilyPicture> GetFamilyPictureByIdAsync(int id)
        {
            return await _repository.GetAsync(x => x.Id == id);
        }

        public async Task<FamilyPicture> GetFamilyPictureByPathAsync(string path)
        {
            return await _repository.GetAsync(x => x.ImageUrl == path);
        }

        public async Task<FamilyPicture> UpdateFamilyPictureAsync(FamilyPicture familyPicture)
        {
            return await _repository.UpdateAsync(familyPicture);
        }
    }
}
