using AutoMapper;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Take5.Models.Models;
using Take5.Models.Models.MasterModels;
using Take5.Services.Contracts;
using Take5.Services.Models;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.MasterModels;
using Take5.Services.Models.SpecificModels;

namespace Take5.Services.Implementation
{
    public class TripService : ITripService
    {
        private readonly IRepository<Trip, long> _repository;
        private readonly ILogger<TripService> _logger;
        private readonly IMapper _mapper;
        private readonly ITripJobsiteService _tripJobsiteService;
        private readonly IJobSiteService _jobsiteService;
        private readonly IDriverService _driverService;
        private readonly ITruckService _truckService;
        private readonly ITripCancellationService _tripCancellationService;
        private readonly UserManager<AspNetUser> _userManager;

        public TripService(IRepository<Trip, long> repository,
          ILogger<TripService> logger,
          IMapper mapper,
          ITripJobsiteService tripJobsiteService,
          IJobSiteService jobsiteService,
          IDriverService driverService,
          ITruckService truckService,
          ITripCancellationService tripCancellationService,
          UserManager<AspNetUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _tripJobsiteService = tripJobsiteService;
            _jobsiteService = jobsiteService;
            _driverService = driverService;
            _truckService = truckService;
            _tripCancellationService = tripCancellationService;
            _userManager = userManager;
        }

        public async Task<bool> CreateTripAfterDelete(TripModel model)
        {
            try
            {
                var trip = _mapper.Map<Trip>(model);
                Trip result = _repository.Add(trip);
                if(result != null)
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
                return false;
            }


        }


        public Task<bool> CreateTrip(TripModel model)
        {
            try
            {
                model.IsVisible = true;
                model.IsDelted = false;
                model.CreatedDate = DateTime.Now;
                model.UpdatedDate = DateTime.Now;
                model.TripDate = model.TripDate;
                var trip = _mapper.Map<Trip>(model);
                Trip result = _repository.Add(trip);
                if (result != null)
                {
                    var addedTripJobsit = _tripJobsiteService.CreateTripJobsite(result.Id, model.JobSiteId);
                    if (addedTripJobsit != null)
                    {
                        return Task<bool>.FromResult<bool>(true);
                    }
                    else
                    {
                        bool deleteResult = _repository.Delete(result);
                        if (deleteResult == true)
                        {
                            return Task<bool>.FromResult<bool>(true);
                        }
                        else
                        {
                            return Task<bool>.FromResult<bool>(false); ;
                        }
                    }
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

        public async Task<bool> DeleteTrip(long id)
        {
            try
            {
               bool result = _repository.DeleteById(id);
                return result;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public List<TripModel> GetAllTrips()
        {

            try
            {
                var trips = _repository.Find(d => d.IsVisible == true, false, d => d.Truck, d => d.Driver).ToList();
                var models = new List<TripModel>();
                models = _mapper.Map<List<TripModel>>(trips);
                return models;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }
        public TripModel GetTrip(long id)
        {
            try
            {
                var trip = _repository.Find(t => t.Id == id && t.IsVisible == true, false, trip => trip.Driver).FirstOrDefault();
                if (trip != null)
                {
                    TripModel tripModel = _mapper.Map<TripModel>(trip);
                    return tripModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public TripModel GetPendingAndUnCompletedTripForDriver(long driverId)
        {
            try
            {
                var trip = _repository.Find(t => t.DriverId == driverId && t.IsVisible == true && t.Cancelled == false && (t.TripStatus != (int)CommanData.TripStatus.TripCompleted)).FirstOrDefault();
                if (trip != null)
                {
                    TripModel tripModel = _mapper.Map<TripModel>(trip);
                    return tripModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public TripModel GetPendingAndUnCompletedTripForTruck(string truckId)
        {
            try
            {
                var trip = _repository.Find(t => t.TruckId == truckId && t.IsVisible == true && t.Cancelled == false && (t.TripStatus == (int)CommanData.TripStatus.Pending || t.TripStatus != (int)CommanData.TripStatus.TripCompleted)).FirstOrDefault();
                if (trip != null)
                {
                    TripModel tripModel = _mapper.Map<TripModel>(trip);
                    return tripModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public List<TripModel> GetTripByDate(DateTime tripDate)
        {
            try
            {
                var trips = _repository.Find(t => t.TripDate.Date == tripDate && t.IsVisible == true, false, trip => trip.Driver, t => t.Truck);
                if (trips.Any() == true)
                {
                    List<TripModel> tripModels = _mapper.Map<List<TripModel>>(trips);
                    return tripModels;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Trip GetTripByDriverIdAndDate(long driverId, DateTime tripDate)
        {
            try
            {
                Trip trip = _repository.Find(t => t.IsVisible == true && t.DriverId == driverId && t.TripDate.Date == tripDate.Date).FirstOrDefault();
                return trip;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<bool> UpdateJobsiteForTrip(TripModel model)
        {
            var ExistingTripJobsite = _tripJobsiteService.GetTripByTripNumber(model.Id);
            bool result =  _tripJobsiteService.DeleteTripJobsite(model.Id).Result;
            if(result == true)
            {
               var trip = GetTrip(model.Id);
               var deleteResult = DeleteTrip(model.Id).Result;
                if(deleteResult == true)
                {
                   var tripModel = _mapper.Map<TripModel>(trip);
                   var createTripResult = CreateTripAfterDelete(tripModel).Result;
                    if(createTripResult == true)
                    {
                        ExistingTripJobsite.JobSiteId = model.JobSiteId;
                        var tripJobsiteModel = _mapper.Map<TripJobsite>(ExistingTripJobsite);
                       var createTripJobsiteResult = _tripJobsiteService.CreateTripJobsiteAfterDelete(tripJobsiteModel).Result;
                        return createTripJobsiteResult;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }



        public async Task<bool> UpdateTrip(TripModel model)
        {
            bool updateResult = false;
            try
            {
                Trip trip = _mapper.Map<Trip>(model);
                updateResult = _repository.Update(trip);
                return updateResult;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<TripModel> UpdateTripArrivedDate(long TripNumber, DateTime arrivedDate)
        {
            bool updateResult = false;
            TripModel tripModel = null;
            try
            {
                Trip trip = _repository.Find(t => t.Id == TripNumber && t.IsVisible == true).FirstOrDefault();
                if (trip != null)
                {
                    //trip.ArrivedDate = arrivedDate;
                    trip.TripStatus = (int)CommanData.TripStatus.Started;
                    updateResult = _repository.Update(trip);
                }
                if (updateResult == true)
                {
                    tripModel = _mapper.Map<TripModel>(trip);
                }
                return tripModel;
            }
            catch (Exception e)
            {
                return tripModel;
            }
        }

        public List<TripModel> GetTripByStatus(int status)
        {
            try
            {
                var trips = _repository.Find(t => t.TripStatus == status && t.IsVisible == true, false, trip => trip.Driver, t => t.Truck);
                if (trips.Any() == true)
                {
                    List<TripModel> tripModels = _mapper.Map<List<TripModel>>(trips);
                    return tripModels;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<TripAPIModel> ConvertFromTripJobsiteModelToTripAPIModel(List<TripJobsiteModel> tripJobsiteModels)
        {
            try
            {
                List<TripAPIModel> tripAPIModels = new List<TripAPIModel>();
                foreach (var tripJobsiteModel in tripJobsiteModels)
                {
                    TripAPIModel tripAPIModel = new TripAPIModel();
                    tripAPIModel.TripNumber = tripJobsiteModel.TripId;
                    tripAPIModel.TripDate = tripJobsiteModel.Trip.TripDate;
                    tripAPIModel.TruckNumber = tripJobsiteModel.Trip.TruckId;
                    tripAPIModel.JobsiteName = tripJobsiteModel.JobSite.Name;
                    tripAPIModel.JobsiteNumber = tripJobsiteModel.JobSite.Id;
                    tripAPIModel.Latituide = tripJobsiteModel.JobSite.Latitude;
                    tripAPIModel.Longitude = tripJobsiteModel.JobSite.Longitude;
                    tripAPIModel.DriverNumber = tripJobsiteModel.Trip.Driver.Id;
                    tripAPIModel.DriverName = tripJobsiteModel.Trip.Driver.FullName;
                    tripAPIModel.TripStatus = Enum.GetName(typeof(CommanData.TripStatus), tripJobsiteModel.TripStatus);
                    tripAPIModel.IsTripConverted = tripJobsiteModel.Trip.IsConverted;
                    tripAPIModel.Take5Status = Enum.GetName(typeof(CommanData.Take5Status), tripJobsiteModel.Trip.Take5Status);
                    if(tripJobsiteModel.StartingDate.HasValue == true)
                    {
                        tripAPIModel.StatrtingDate = tripJobsiteModel.StartingDate.Value.ToString("yyyy-MM-dd, HH:mm:ss");
                    }
                    if (tripJobsiteModel.DestinationArrivingDate.HasValue)
                    {
                        tripAPIModel.ArrivingDate = tripJobsiteModel.DestinationArrivingDate.Value.ToString("yyyy-MM-dd, HH:mm:ss");
                    }
                    tripAPIModel.JobsiteHasNetworkCoverage = tripJobsiteModel.JobSite.HasNetworkCoverage;
                    tripAPIModels.Add(tripAPIModel);
                }
                return tripAPIModels;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public TripJobsiteModel CreateStepTwoRequest(StepTwoStartRequest model, long TripId, long JobsiteId)
        {
            try
            {
                TripJobsiteModel tripJobsiteModel = _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteId(TripId, JobsiteId);
                tripJobsiteModel.StageTwoRequestDate = model.RequestDate;
                tripJobsiteModel.StageTwoRequestCreatedDate = DateTime.Now;
                tripJobsiteModel.UpdatedDate = DateTime.Now;
                tripJobsiteModel.TripStatus = (int)CommanData.TripStatus.StepTwoRequested;
                tripJobsiteModel.Take5Status = (int)CommanData.Take5Status.StepTwoRequested;
                tripJobsiteModel.RequestStatus = (int)CommanData.RequestStatus.Pending;
                bool result = _tripJobsiteService.UpdateTripJobsite(tripJobsiteModel).Result;
                if (result == true)
                {
                    TripModel tripModel = GetTrip(TripId);
                    if (tripModel != null)
                    {
                        tripModel.TripStatus = (int)CommanData.TripStatus.StepTwoRequested;
                        tripModel.Take5Status = (int)CommanData.Take5Status.StepTwoRequested;
                        tripModel.UpdatedDate = DateTime.Now;
                        Trip trip = _mapper.Map<Trip>(tripModel);
                        result = _repository.Update(trip);
                        if (result)
                        {
                            return tripJobsiteModel;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public TripModel ResetTrip(long tripNumber)
        {
            try
            {
                bool result = false;
                TripModel tripModel = GetTrip(tripNumber);
                tripModel.Take5Status = (int)CommanData.Take5Status.NotStarted;
                tripModel.TripStatus = (int)CommanData.TripStatus.Pending;
                tripModel.IsConverted = true;
                tripModel.UpdatedDate = DateTime.Now;
                result = UpdateTrip(tripModel).Result;
                if(result == true)
                {
                    return tripModel;
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

        public SearchTripModel InitiateTripJobsiteSearchModel(SearchTripModel searchTripModel)
        {
            try
            {
                searchTripModel.Drivers = _driverService.GetAllDrivers();
                searchTripModel.JobSites = _jobsiteService.GetAllJobsites();
                searchTripModel.Trucks = _truckService.GetAllTrucks();
                if (searchTripModel.Drivers != null)
                {
                  searchTripModel.Drivers.Insert(0, new DriverModel { Id = -1, FullName = "Select Driver" });
                    searchTripModel.DriverId = -1;
                }
                if (searchTripModel.JobSites != null)
                {
                    searchTripModel.JobSites.Insert(0, new JobSiteModel { Id = -1, Name = "Select Jobsite" });
                    searchTripModel.JobSiteId = -1;
                }
                if (searchTripModel.Trucks != null)
                {
                    searchTripModel.TruckId = "-1";
                }

                //List<TripStatusModel> tripStatusModels = new List<TripStatusModel>();
                //var tripStatusNames = Enum.GetValues(typeof(CommanData.TripStatus));
                //if(tripStatusNames != null)
                //{
                //    foreach (var name in tripStatusNames)
                //    {
                //        tripStatusModels.Add(new TripStatusModel { Id = (int)Enum.Parse(typeof(CommanData.TripStatus), name.ToString()), Status = name.ToString() });
                //    }
                //    searchTripModel.tripStatusModels = tripStatusModels;
                //}

                List<TripStatusModel> tripStatusModels = new List<TripStatusModel>
                {
                    new TripStatusModel {Id=(int)CommanData.TripStatus.Pending, Status="Pending"},
                    new TripStatusModel {Id=(int)CommanData.TripStatus.Started, Status="Starting"},
                    new TripStatusModel {Id=(int)CommanData.TripStatus.DestinationArrived, Status="Destination Arriving"},
                    new TripStatusModel {Id=(int)CommanData.TripStatus.SurveyStepOneCompleted, Status="Step One Completed"},
                    new TripStatusModel {Id=(int)CommanData.TripStatus.StepTwoRequested, Status="Request for step2"},
                    new TripStatusModel {Id=(int)CommanData.TripStatus.StepTwoResponsed, Status="Step2 request Approved"},
                    new TripStatusModel {Id=(int)CommanData.TripStatus.SurveyStepTwoCompleted, Status="Step Two Completed"},
                    new TripStatusModel {Id=(int)CommanData.TripStatus.TripCompleted, Status="Completed Trips"},
                };
                searchTripModel.tripStatusModels = tripStatusModels;
                searchTripModel.TripDateFrom = null;
                searchTripModel.TripDateTo = null;
                return searchTripModel;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public async Task<bool> UpdateTripStatus(long tripNumber, int tripStatusId, int take5Status = 1)
        {
            bool updateResult = false;
            try
            {
                TripModel tripModel = GetTrip(tripNumber);
                tripModel.TripStatus = tripStatusId;
                tripModel.Take5Status = take5Status;
                tripModel.UpdatedDate = DateTime.Now;
                updateResult = await UpdateTrip(tripModel);
                return updateResult;
            }
            catch (Exception e)
            {
                return updateResult;
            }
        }

        public SearchDriverTripModel InitiateSearchDriverTripModel()
        {
            try
            {
                SearchDriverTripModel searchDriverTripModel = new SearchDriverTripModel();
                searchDriverTripModel.JobSites = _jobsiteService.GetAllJobsites();
            }
            catch (Exception e)
            {

            }
            return null;
        }
        public async Task<Trip> CancelTrip(long tripNumber, TripCancellationModel tripCancellationModel)
        {
            try
            {
                bool updateResult = false;
                updateResult =  _tripCancellationService.CreateTripCancellation(tripCancellationModel).Result;
                if(updateResult == true)
                {
                    Trip trip = _repository.Find(t => t.Id == tripNumber && t.IsVisible == true && t.Cancelled == false).FirstOrDefault();
                    if (trip != null)
                    {
                        trip.Cancelled = true;
                        updateResult = _repository.Update(trip);
                        if (updateResult == true)
                        {
                            return trip;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }


        }

    }
}
