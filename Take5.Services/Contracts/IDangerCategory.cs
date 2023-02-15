using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Contracts
{
    public interface IDangerCategoryService
    {
        List<DangerCategoryModel> GetAllDangerCategories();
        Task<bool> CreateDangerCategory(DangerCategoryModel model);
        Task<bool> UpdateDanger(DangerCategoryModel model);
        bool DeleteDangerCategory(int id);
        DangerCategoryModel GetDanger(int id);
        public DangerCategoryModel GetDangerCategoryByName(string name);
    }
}
