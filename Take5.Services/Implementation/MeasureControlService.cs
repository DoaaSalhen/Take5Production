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
using Take5.Services.Models.APIModels;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Implementation
{
    public class MeasureControlService : IMeasureControlService
    {
        private readonly IRepository<MeasureControl, long> _repository;
        private readonly ILogger<MeasureControlService> _logger;
        private readonly IMapper _mapper;

        public MeasureControlService(IRepository<MeasureControl, long> repository,
            ILogger<MeasureControlService> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public Task<bool> CreateMeasureControl(MeasureControlModel model)
        {
            throw new NotImplementedException();
        }

        public bool DeleteMeasureControl(int id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteMeasureControlFordanger(int id)
        {
            try
            {
                List<MeasureControlModel> measureControlModels = GetMeasureControlsByDangerId(id);
                if (measureControlModels != null && measureControlModels.Count > 0)
                {
                    foreach (var control in measureControlModels)
                    {
                        control.IsVisible = false;
                        control.IsDelted = true;
                        MeasureControl measureControl = _mapper.Map<MeasureControl>(control);
                        _repository.Update(measureControl);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<MeasureControlModel> GetAllMeasureControls()
        {
            try
            {
                List<MeasureControl> measureControls = _repository.Find(c => c.IsVisible == true, false, c=>c.Danger).ToList();
                List<MeasureControlModel> measureControlModels = _mapper.Map<List<MeasureControlModel>>(measureControls);
                return measureControlModels;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public Task<bool> UpdateMeasureControl(MeasureControlModel model)
        {
            throw new NotImplementedException();
        }

        public List<MeasureControlModel> GetMeasureControlsByDangerId(long dangerId)
        {
            try
            {
                List<MeasureControl> measureControls = _repository.Find(c => c.IsVisible == true && c.DangerId == dangerId ).ToList();
                List<MeasureControlModel> measureControlModels = _mapper.Map<List<MeasureControlModel>>(measureControls);
                return measureControlModels;
            }
            catch (Exception e)
            {
                return null;
            }
        }



        public List<MeasureControlAPI> MapMeasureControlModelToMeasureControlAPI(List<MeasureControlModel> measureControls)
        {
            try
            {
                List<MeasureControlAPI> measureControlAPIs = new List<MeasureControlAPI>();
               foreach(var control in measureControls)
                {
                    MeasureControlAPI measureControlAPI = new MeasureControlAPI();
                    measureControlAPI.MeasureControlId = control.Id;
                    measureControlAPI.MeasureControlName = control.Name;
                    measureControlAPIs.Add(measureControlAPI);
                }
                return measureControlAPIs;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
