using CommunityHub.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Infrastructure.Services.User
{
    public interface IFamilyPictureService
    {
        Task<FamilyPicture> CreateFamilyPictureAsync(FamilyPicture familyPicture);
        Task<FamilyPicture> UpdateFamilyPictureAsync(FamilyPicture familyPicture);
        Task<FamilyPicture> DeleteFamilyPictureAsync(FamilyPicture familyPicture);
        Task<FamilyPicture> GetFamilyPictureByIdAsync(int id);
        Task<FamilyPicture> GetFamilyPictureByPathAsync(string path);

        Task<List<FamilyPicture>> GetAllFamilyPicturesAsync();
    }
}
