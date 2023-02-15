using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Take5.Models.Models.MasterModels;
using Take5.Services.Contracts;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Implementation
{
    public class TripTake5TogetherService : ITripTake5TogetherService
    {
        private readonly IRepository<TripTake5Together, long> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TripTake5TogetherService> _logger;
        private readonly IDriverService _driverService;
        public TripTake5TogetherService(IRepository<TripTake5Together, long> repository,
            IMapper mapper,
            ILogger<TripTake5TogetherService> logger,
            IDriverService driverService)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _driverService = driverService;
        }
        public bool CreateTripTake5Together(TripTake5Together tripTake5Together)
        {
            try
            {
                var addedTripTake5Together = _repository.Add(tripTake5Together);
                if (addedTripTake5Together != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return false;
            }
        }

        public bool AddTripTake5TogetherForTrip(AllTripSteps allTripSteps)
        {
            try
            {
                bool result = false;
               if(allTripSteps.Take5TogetherAPIModels != null)
                {
                    foreach(var model in allTripSteps.Take5TogetherAPIModels)
                    {
                        var driver  = _driverService.GetDriver(model.ParticipantDriverId);
                        if(driver != null)
                        {
                            TripTake5Together tripTake5Together = new TripTake5Together();
                            tripTake5Together.TripId = allTripSteps.TripId;
                            tripTake5Together.JobSiteId = allTripSteps.JobsiteId;
                            tripTake5Together.DriverId = _driverService.GetDriverByUserId(allTripSteps.UserId).Id;
                            tripTake5Together.ParticipantDriverId = model.ParticipantDriverId;
                            tripTake5Together.WhoStartDriverId = model.WhoStartDriverId;
                            tripTake5Together.Notes = model.Notes;
                            tripTake5Together.IsVisible = true;

                            tripTake5Together.IsDelted = false;

                            tripTake5Together.CreatedDate = DateTime.Now;

                            tripTake5Together.UpdatedDate = DateTime.Now;

                            result = CreateTripTake5Together(tripTake5Together);
                            if (result != true)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return false;
            }
        }


        public List<TripTake5TogetherModel> GetTripTake5TogetherForTrip(long tripNumber, long jobsiteId)
        {
            try
            {
                var take5Togethers = _repository.Find(tg => tg.IsVisible == true && tg.TripId == tripNumber && tg.JobSiteId == jobsiteId, false, tg => tg.Driver).ToList();
                List<TripTake5TogetherModel> tripTake5TogetherModels = _mapper.Map<List<TripTake5TogetherModel>>(take5Togethers);
                if(tripTake5TogetherModels.Count > 0)
                {
                    tripTake5TogetherModels.ForEach(tg => tg.ParticipantDriver = _driverService.GetDriver(tg.ParticipantDriverId));
                    tripTake5TogetherModels.ForEach(tg => tg.WhoStartDriver = _driverService.GetDriver(tg.WhoStartDriverId));
                }
                return tripTake5TogetherModels;
            }
            catch(Exception e)
            {
                return null;
            }
        }


    }
}
