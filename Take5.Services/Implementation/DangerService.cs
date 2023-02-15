using AutoMapper;
using Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Take5.Models.Models.MasterModels;
using Take5.Services.Contracts;
using Take5.Services.Models;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Implementation
{
    public class DangerService : IDangerService
    {
        private readonly IRepository<Danger, long> _repository;
        private readonly ILogger<DangerService> _logger;
        private readonly IMapper _mapper;
        private readonly IMeasureControlService _measureControlService;

        public DangerService(IRepository<Danger, long> dangerRepository,
          ILogger<DangerService> logger, IMapper mapper,
          IMeasureControlService measureControlService)
        {
            _repository = dangerRepository;
            _logger = logger;
            _mapper = mapper;
            _measureControlService = measureControlService;
        }
        public Task<bool> CreateDanger(DangerModel model)
        {
            try
            {
                model.CreatedDate = DateTime.Now;
                model.UpdatedDate = DateTime.Now;
                model.IsDelted = false;
                model.IsVisible = true;
                var danger = _mapper.Map<Danger>(model);
                var result = _repository.Add(danger);

                if (result != null)
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

        public bool DeleteDanger(int id)
        {
            throw new NotImplementedException();
        }

        public List<DangerModel> GetAllDangers()
        {
            try
            {
                var dangers = _repository.Find(d => d.IsVisible == true, false, d=>d.DangerCategory).ToList();
                var models = new List<DangerModel>();
                if (dangers.Count > 0)
                {
                    models = _mapper.Map<List<DangerModel>>(dangers);
                    foreach(var model in models)
                    {
                       model.measureControlModels = _measureControlService.GetMeasureControlsByDangerId(model.Id).ToList();
                    }
                }
                return models;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        public DangerModel GetDanger(int id)
        {
            try
            {
                Danger danger = _repository.Find(d => d.IsVisible == true && d.Id == id, false, d=>d.DangerCategory).First();
                if(danger != null)
                {
                    DangerModel dangerModel = _mapper.Map<DangerModel>(danger);
                    List<MeasureControlModel> measureControlModels = _measureControlService.GetMeasureControlsByDangerId(dangerModel.Id);
                    dangerModel.measureControlModels = measureControlModels;
                    return dangerModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public DangerModel GetDangerByName(string name)
        {
            try
            {
                Danger danger = _repository.Find(d => d.IsVisible == true && d.Name == name).First();
                DangerModel dangerModel = _mapper.Map<DangerModel>(danger);
                return dangerModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public List<DangerModel> GetDangersByCategory(int categoryId)
        {
            try
            {
                List<Danger> dangers = _repository.Find(d => d.IsVisible == true && d.DangerCategoryId == categoryId, false, d=>d.DangerCategory).ToList();
                List<DangerModel> dangerModels = _mapper.Map<List<DangerModel>>(dangers);
                return dangerModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public Task<bool> UpdateDanger(DangerModel model)
        {
            var danger = _mapper.Map<Danger>(model);

            try
            {
                _repository.Update(danger);

                return Task<bool>.FromResult<bool>(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(false);
        }

        public async Task<string> UploadedImageAsync(IFormFile ImageName, string path)
        {
            try
            {
                string uniqueFileName = null;
                string filePath = null;

                if (ImageName != null)
                {
                    string uploadsFolder = Path.Combine(CommanData.UploadMainFolder, path);
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageName.FileName;
                    uniqueFileName = uniqueFileName.Replace(" ", "");
                    filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageName.CopyToAsync(fileStream);
                    }

                    //using (var dataStream = new MemoryStream())
                    //{
                    //    await ImageName.CopyToAsync(dataStream);
                    //}
                    return uniqueFileName;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }

    }
}
