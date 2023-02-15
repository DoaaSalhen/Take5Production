using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Contracts
{
    public interface IDangerService
    {
        List<DangerModel> GetAllDangers();
        Task<bool> CreateDanger(DangerModel model);
        Task<bool> UpdateDanger(DangerModel model);
        bool DeleteDanger(int id);
        DangerModel GetDanger(int id);
        public DangerModel GetDangerByName(string name);
        List<DangerModel> GetDangersByCategory(int categoryId);
        Task<string> UploadedImageAsync(IFormFile ImageName, string path);

    }
}
