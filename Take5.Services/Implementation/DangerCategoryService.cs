using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Take5.Models.Models.MasterModels;
using Take5.Services.Contracts;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Implementation
{
    public class DangerCategoryService : IDangerCategoryService
    {
        private readonly IRepository<DangerCategory, long> _repository;
        private readonly ILogger<DangerCategoryService> _logger;
        private readonly IMapper _mapper;

        public DangerCategoryService(IRepository<DangerCategory, long> repository,
         ILogger<DangerCategoryService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public Task<bool> CreateDangerCategory(DangerCategoryModel model)
        {
            var dangerCategory = _mapper.Map<DangerCategory>(model);

            try
            {
               var result = _repository.Add(dangerCategory);
                if(result != null)
                {
                    return Task<bool>.FromResult<bool>(true);
                }
                else
                {
                    return Task<bool>.FromResult<bool>(false);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(false);
        }

        public bool DeleteDangerCategory(int id)
        {
            throw new NotImplementedException();
        }

        public List<DangerCategoryModel> GetAllDangerCategories()
        {
            try
            {
                var dangerCategories = _repository.Find(i => i.IsVisible == true).ToList();
                var models = new List<DangerCategoryModel>();
                models = _mapper.Map<List<DangerCategoryModel>>(dangerCategories);
                return models;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public DangerCategoryModel GetDanger(int id)
        {
            throw new NotImplementedException();
        }

        public DangerCategoryModel GetDangerCategoryByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDanger(DangerCategoryModel model)
        {
            throw new NotImplementedException();
        }
    }
}
