using AutoMapper;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Take5.Models.Models;
using Take5.Models.Models.MasterModels;
using Take5.Services.Contracts;
using Take5.Services.Models;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.hub;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Implementation
{
    public class TripJobsiteService : ITripJobsiteService
    {
        private readonly IRepository<TripJobsite, Trip> _repository;
        private readonly ILogger<TripJobsiteService> _logger;
        private readonly IMapper _mapper;
        private readonly ISurveyService _surveyService;
        private readonly ITripTake5TogetherService _tripTake5TogetherService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly IHubContext<DashBoardHub> _dashBoardHub;

        private readonly IUserConnectionManager _userConnectionManager;
        private readonly ITripCancellationService _tripCancellationService;

        private readonly IEmployeeService _employeeService;

        public TripJobsiteService(IRepository<TripJobsite, Trip> repository,
          ILogger<TripJobsiteService> logger,
          IMapper mapper,
          ISurveyService surveyService,
          ITripTake5TogetherService tripTake5TogetherService,
          UserManager<AspNetUser> userManager,
          IHubContext<NotificationHub> hub,
          IUserConnectionManager userConnectionManager,
          IHubContext<DashBoardHub> dashBoardHub,
          ITripCancellationService tripCancellationService,
          IEmployeeService employeeService)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _surveyService = surveyService;
            _tripTake5TogetherService = tripTake5TogetherService;
            _userManager = userManager;
            _userConnectionManager = userConnectionManager;
            _hub = hub;
            _dashBoardHub = dashBoardHub;
            _tripCancellationService = tripCancellationService;
            _employeeService = employeeService;
        }

        public Task<bool> CreateTripJobsite(long tripId, long jobsiteId)
        {
            try
            {
                TripJobsite tripJobsite = new TripJobsite();
                tripJobsite.TripId = tripId;
                tripJobsite.JobSiteId = jobsiteId;
                tripJobsite.TripStatus = (int)CommanData.TripStatus.Pending;
                tripJobsite.Take5Status = (int)CommanData.Take5Status.NotStarted;
                tripJobsite.IsVisible = true;
                tripJobsite.IsDelted = false;
                tripJobsite.CreatedDate = DateTime.Now;
                tripJobsite.UpdatedDate = DateTime.Now;
                //tripJobsite.Cancelled = false;
                var result = _repository.Add(tripJobsite);

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
                return Task<bool>.FromResult<bool>(false);

            }
        }


        public Task<bool> CreateTripJobsiteAfterDelete(TripJobsite tripJobsite)
        {
            try
            {
                
                var result = _repository.Add(tripJobsite);

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
                return Task<bool>.FromResult<bool>(false);

            }
        }


        public async Task<bool> UpdateTripJobsiteStatus(long TripId, long JobsiteId, string tripStatus)
        {
            bool updateResult = false;
            try
            {
                int tripStatusNumber = (int)(CommanData.TripStatus)Enum.Parse(typeof(CommanData.TripStatus), tripStatus);
                TripJobsite tripJobsite =  GetTripJobsiteByTripNumberAndJobsiteId(TripId, JobsiteId).Result;
                tripJobsite.TripStatus = tripStatusNumber;
                tripJobsite.Take5Status = (int)CommanData.Take5Status.Completed;
                tripJobsite.UpdatedDate = DateTime.Now;
                Trip trip = tripJobsite.Trip;
                trip.TripStatus = tripStatusNumber;
                trip.Take5Status = (int)CommanData.Take5Status.Completed;
                trip.UpdatedDate = DateTime.Now;
                updateResult = _repository.UpdateTwoEntities(tripJobsite, trip, false);
                return updateResult;
            }
            catch (Exception e)
            {
                return updateResult;
            }
        }

        public async Task<bool> UpdateTripJobsiteTake5Status(long TripId, long JobsiteId, string take5Status)
        {
            bool updateResult = false;
            try
            {
                int take5StatusNumber = (int)(CommanData.Take5Status)Enum.Parse(typeof(CommanData.Take5Status), take5Status);
                TripJobsite tripJobsite = GetTripJobsiteByTripNumberAndJobsiteId(TripId, JobsiteId).Result;
                tripJobsite.Take5Status = take5StatusNumber;
                tripJobsite.UpdatedDate = DateTime.Now;
                Trip trip = tripJobsite.Trip;
                trip.Take5Status = take5StatusNumber;
                trip.UpdatedDate = DateTime.Now;
                updateResult = _repository.UpdateTwoEntities(tripJobsite, trip, false);
                return updateResult;
            }
            catch (Exception e)
            {
                return updateResult;
            }
        }



        public async Task<bool> UpdateTripJobsite(TripJobsiteModel model)
        {
            bool updateResult = false;
            try
            {
                TripJobsite trip = _mapper.Map<TripJobsite>(model);
                updateResult = _repository.Update(trip);

                return updateResult;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> SatrtTripJobsite(TripStartingModel tripStartingModel)
        {
            bool updateResult = false;
            try
            {
                TripJobsite tripJobsite = GetTripJobsiteByTripNumberAndJobsiteId(tripStartingModel.TripId, tripStartingModel.JobsiteId).Result;
                tripJobsite.StartingDate = tripStartingModel.StartingDate;
                tripJobsite.TripStatus = (int)CommanData.TripStatus.Started;
                tripJobsite.UpdatedDate = DateTime.Now;
                Trip trip = tripJobsite.Trip;
                trip.TripStatus = (int)CommanData.TripStatus.Started;
                trip.UpdatedDate = DateTime.Now;
                updateResult = _repository.UpdateTwoEntities(tripJobsite, trip, false);
                return updateResult;
            }
            catch (Exception e)
            {
                return updateResult;
            }
        }

        public async Task<bool> ArrivingTripJobsite(TripDestinationArrivedModel TripDestinationArrivedModel, long TripId, long JobsiteId)
        {
            bool updateResult = false;
            try
            {
                TripJobsite tripJobsite = GetTripJobsiteByTripNumberAndJobsiteId(TripId, JobsiteId).Result;
                tripJobsite.DestinationArrivingDate = TripDestinationArrivedModel.DestinationArrivedDate;
                tripJobsite.TripStatus = (int)CommanData.TripStatus.DestinationArrived;
                tripJobsite.Take5Status = (int)CommanData.Take5Status.NotStarted;
                tripJobsite.UpdatedDate = DateTime.Now;
                Trip trip = tripJobsite.Trip;
                trip.TripStatus = (int)CommanData.TripStatus.DestinationArrived;
                trip.Take5Status = (int)CommanData.Take5Status.NotStarted;
                trip.UpdatedDate = DateTime.Now;
                updateResult = _repository.UpdateTwoEntities(tripJobsite,trip, false);
                return updateResult;
            }
            catch (Exception e)
            {
                return updateResult;
            }
        }

        public async Task<bool> UpdateStepOneCompletion(SurveyStepOneAnswersAPIModel model, long TripId, long JobSiteId)
        {
            bool updateResult = false;
            try
            {
                TripJobsite tripJobsite = GetTripJobsiteByTripNumberAndJobsiteId(TripId, JobSiteId).Result;
                tripJobsite.Take5Status = (int)CommanData.Take5Status.StepOneCompletedOnly;
                tripJobsite.TripStatus = (int)CommanData.TripStatus.SurveyStepOneCompleted;
                tripJobsite.StageOneComplatedTime = model.CreatedDate;
                tripJobsite.UpdatedDate = DateTime.Now;
                Trip trip = tripJobsite.Trip;
                trip.TripStatus = (int)CommanData.TripStatus.SurveyStepOneCompleted;
                trip.Take5Status = (int)CommanData.Take5Status.StepOneCompletedOnly;
                trip.UpdatedDate = DateTime.Now;
                updateResult = _repository.UpdateTwoEntities(tripJobsite, trip);
                
                
                return updateResult;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> UpdateStepTwoCompletion(SurveyStepOneAnswersAPIModel model, long TripId, long JobSiteId)
        {
            bool updateResult = false;
            try
            {
                TripJobsite tripJobsite = GetTripJobsiteByTripNumberAndJobsiteId(TripId, JobSiteId).Result;
                tripJobsite.Take5Status = (int)CommanData.Take5Status.StepTwoCompleted;
                tripJobsite.TripStatus = (int)CommanData.TripStatus.SurveyStepTwoCompleted;
                tripJobsite.StageTwoComplatedTime = model.CreatedDate;
                tripJobsite.UpdatedDate = DateTime.Now;
                Trip trip = tripJobsite.Trip;
                trip.TripStatus = (int)CommanData.TripStatus.SurveyStepTwoCompleted;
                trip.Take5Status = (int)CommanData.Take5Status.StepTwoCompleted;
                trip.UpdatedDate = DateTime.Now;
                updateResult = _repository.UpdateTwoEntities(tripJobsite, trip);


                return updateResult;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public List<TripJobsiteModel> GetAllJobsiteTrips()
        {
            try
            {
                var trips = _repository.Find(d => d.IsVisible == true, false, d => d.JobSite, d => d.Trip, d => d.Trip.Truck, d => d.Trip.Driver).ToList();
                var models = new List<TripJobsiteModel>();
                models = _mapper.Map<List<TripJobsiteModel>>(trips);
                return models;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public TripJobsiteModel GetTripJobsiteModelByTripNumberAndJobsiteId(long tripNumber, long jobsiteId)
        {
            TripJobsite tripJobsite = _repository.Find(tj => tj.IsVisible == true && tj.TripId == tripNumber&& tj.JobSiteId == jobsiteId, false, tj => tj.Trip, tj => tj.Trip.Driver, tj => tj.JobSite).FirstOrDefault();
            TripJobsiteModel tripJobsiteModel = _mapper.Map<TripJobsiteModel>(tripJobsite);
            return tripJobsiteModel;
        }

        public TripJobsiteModel GetTripJobsiteModelByTripNumberAndJobsiteIdForAPI(long tripNumber, long jobsiteId)
        {
            TripJobsite tripJobsite = _repository.Find(tj =>  tj.TripId == tripNumber && tj.JobSiteId == jobsiteId, false, tj => tj.Trip, tj => tj.Trip.Driver, tj => tj.JobSite).FirstOrDefault();
            TripJobsiteModel tripJobsiteModel = _mapper.Map<TripJobsiteModel>(tripJobsite);
            return tripJobsiteModel;
        }

        public async Task<TripJobsite> GetTripJobsiteByTripNumberAndJobsiteId(long tripNumber, long jobsiteId)
        {
            try
            {
                TripJobsite tripJobsite = _repository.Find(tj => tj.IsVisible == true && tj.TripId == tripNumber && tj.JobSiteId == jobsiteId, false, tj => tj.Trip, tj => tj.Trip.Driver, tj => tj.JobSite).FirstOrDefault();
                return tripJobsite;
            }
            catch(Exception e)
            {
                return null;
            }

        }

        public TripJobsite GetTripByTripNumber(long tripNumber)
        {
            TripJobsite tripJobsite = _repository.Find(tj => tj.IsVisible == true && tj.TripId == tripNumber).FirstOrDefault();
            return tripJobsite;
        }

        public TripJobsiteModel GetJobsitePendingTripForDriver(long driverId)
        {
            try
            {
                TripJobsite trip = _repository.Find(tj => tj.Trip.DriverId == driverId && tj.Trip.TripStatus == (int)CommanData.TripStatus.Pending && tj.Trip.IsVisible == true && tj.IsVisible == true, false, tj => tj.JobSite, tj => tj.Trip, tj => tj.Trip.Driver).FirstOrDefault();
                if (trip != null)
                {
                    TripJobsiteModel tripModel = _mapper.Map<TripJobsiteModel>(trip);
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

        public TripJobsiteModel GetCurrentJobsiteTripForDriver(long driverId)
        {
            try
            {
                TripJobsite trip = _repository.Find(tj => tj.Trip.DriverId == driverId && (tj.Trip.TripStatus != (int)CommanData.TripStatus.TripCompleted) && tj.Trip.Cancelled == false && tj.Trip.IsVisible == true && tj.IsVisible == true, false, tj => tj.JobSite, tj => tj.Trip, tj => tj.Trip.Driver).FirstOrDefault();
                if (trip != null)
                {
                    TripJobsiteModel tripModel = _mapper.Map<TripJobsiteModel>(trip);
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

        public List<TripJobsiteModel> GetHomeTrips()
        {
            try
            {
                List<TripJobsiteModel> tripJobsiteModels = new List<TripJobsiteModel>();
                var trips = _repository.Find(t => t.IsVisible == true && (t.TripStatus != (int)CommanData.TripStatus.TripCompleted && t.TripStatus != (int)CommanData.TripStatus.Pending) && t.Trip.Cancelled != true && t.Converted != true, false, t => t.Trip, t => t.JobSite, t => t.Trip.Driver);
                if (trips.Any() == true)
                {
                    tripJobsiteModels = _mapper.Map<List<TripJobsiteModel>>(trips);
                }
                return tripJobsiteModels;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Dictionary<string, long> GetTripsCountByStatus()
        {
            Dictionary<string, long> tripStatusStatisticsModel = new Dictionary<string, long>();
            try
            {
                var trips = _repository.Find(t => t.IsVisible == true);
                if (trips.Any() == true)
                {
                    tripStatusStatisticsModel["Pending"] = trips.Where(t => t.TripStatus == (int)CommanData.TripStatus.Pending).Count();
                    tripStatusStatisticsModel["InProgress"] = trips.Where((t => (t.TripStatus != (int)CommanData.TripStatus.Pending && t.TripStatus != (int)CommanData.TripStatus.SurveyStepTwoCompleted) && t.Trip.Cancelled == false)).Count();
                    tripStatusStatisticsModel["CompletedTake5"] = trips.Where(t => t.Take5Status == (int)CommanData.Take5Status.Completed).Count();
                    tripStatusStatisticsModel["All"] = trips.Count();
                }
                else
                {
                    tripStatusStatisticsModel["Pending"] = 0;
                    tripStatusStatisticsModel["InProgress"] = 0;
                    tripStatusStatisticsModel["CompletedTake5"] = 0;
                    tripStatusStatisticsModel["All"] = 0;
                }
                return tripStatusStatisticsModel;

            }
            catch (Exception e)
            {
                return tripStatusStatisticsModel;
            }
        }


        public List<StepTwoRequestModel> GetStepTwoRequests()
        {
            try
            {
                List<StepTwoRequestModel> stepTwoRequestModels = new List<StepTwoRequestModel>();
                List<TripJobsite> tripJobsitesStepTwoRequests = _repository.Find(tj => tj.IsVisible == true && (tj.TripStatus == (int)CommanData.TripStatus.StepTwoRequested || tj.Trip.TripStatus == (int)CommanData.TripStatus.StepTwoResponsed), false, tj=>tj.JobSite,tj => tj.Trip.Driver).ToList();
                if(tripJobsitesStepTwoRequests.Count > 0)
                {
                    List<TripJobsiteModel> tripJobsiteModels = _mapper.Map<List<TripJobsiteModel>>(tripJobsitesStepTwoRequests);
                    stepTwoRequestModels = MapFromTripJobsiteModelToStepTwoRequestModel(tripJobsiteModels);

                }
                return stepTwoRequestModels;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<StepTwoRequestModel> GetPendingStepTwoRequests()
        {
            try
            {
                List<StepTwoRequestModel> stepTwoRequestModels = new List<StepTwoRequestModel>();
                List<TripJobsite> tripJobsitesStepTwoRequests = _repository.Find(tj => tj.IsVisible == true && (tj.TripStatus == (int)CommanData.TripStatus.StepTwoRequested), false, tj => tj.JobSite, tj => tj.Trip.Driver).ToList();
                if (tripJobsitesStepTwoRequests.Count > 0)
                {
                    List<TripJobsiteModel> tripJobsiteModels = _mapper.Map<List<TripJobsiteModel>>(tripJobsitesStepTwoRequests);
                    stepTwoRequestModels = MapFromTripJobsiteModelToStepTwoRequestModel(tripJobsiteModels);

                }
                return stepTwoRequestModels;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public List<StepTwoRequestModel> MapFromTripJobsiteModelToStepTwoRequestModel(List<TripJobsiteModel> tripJobsiteModels)
        {
            try
            {
                List<StepTwoRequestModel> stepTwoRequestModels = new List<StepTwoRequestModel>();
                if (tripJobsiteModels != null)
                {
                    foreach (var tripJobsite in tripJobsiteModels)
                    {
                        double TimeDifferenceBtweenStepOneAndTwoRequest = ((DateTime)tripJobsite.StageTwoRequestDate - (DateTime)tripJobsite.StageOneComplatedTime).TotalHours;
                        StepTwoRequestModel request = new StepTwoRequestModel();
                        request.TripId = tripJobsite.TripId;
                        request.JobsiteId = tripJobsite.JobSiteId;
                        request.JobsiteName = tripJobsite.JobSite.Name;
                        request.DriverId = tripJobsite.Trip.Driver.Id;
                        request.DriverName = tripJobsite.Trip.Driver.FullName;
                        request.DestinationArrivingDate = (DateTime)tripJobsite.DestinationArrivingDate;
                        request.StageOneComplatedTime = (DateTime)tripJobsite.StageOneComplatedTime;
                        request.StageTwoRequestDate = (DateTime)tripJobsite.StageTwoRequestDate;
                        request.RequestStatusName = Enum.GetName(typeof(CommanData.RequestStatus), (int)tripJobsite.RequestStatus);
                        if (TimeDifferenceBtweenStepOneAndTwoRequest < CommanData.unPackgingTime)
                        {
                            request.WarningMessage = "Step one completed before " + TimeDifferenceBtweenStepOneAndTwoRequest +" hours";
                        }
                        stepTwoRequestModels.Add(request);
                    }
                }
                return stepTwoRequestModels;
            }
            catch(Exception e)
            {
                return null;
            }
        }
        
        public async Task<bool> ApproveStepTwoRequest(TripJobsiteModel tripJobsiteModel)
        {
            try
            {
                tripJobsiteModel.RequestStatus = (int)CommanData.RequestStatus.Approved;
                tripJobsiteModel.StageTwoResponseDate = DateTime.Now;
                tripJobsiteModel.TripStatus = (int)CommanData.TripStatus.StepTwoResponsed;
                tripJobsiteModel.UpdatedDate = DateTime.Now;
                TripJobsite tripJobsite = _mapper.Map<TripJobsite>(tripJobsiteModel);
               bool result = _repository.Update(tripJobsite);
                return result;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool UpdateJobsiteForTrip(long tripNumber, long jobsiteId)
        {
            try
            {
                TripJobsite tripJobsite = _repository.Find(tj => tj.IsVisible == true && tj.TripId == tripNumber).Last();
                if (tripJobsite != null)
                {
                    tripJobsite.JobSiteId = jobsiteId;
                    tripJobsite.UpdatedDate = DateTime.Now;
                    bool result = _repository.Update(tripJobsite);
                    return result;
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

        public List<TripJobsiteModel> SearchForTrip(SearchTripModel searchTripModel)
        {
            try
            {
              var tripsJobsite = _repository.Find(tj => tj.IsVisible == true, false, tj=>tj.Trip, tj => tj.JobSite, tj => tj.Trip.Driver, tj => tj.Trip.Truck);
                if (tripsJobsite != null)
                {
                    if (searchTripModel.TripDateFrom.HasValue)
                    {
                        tripsJobsite = tripsJobsite.Where(tj => tj.Trip.TripDate.Date >= searchTripModel.TripDateFrom.Value.Date);
                    }
                    if (searchTripModel.TripDateTo.HasValue)
                    {
                        tripsJobsite = tripsJobsite.Where(tj => tj.Trip.TripDate.Date <= searchTripModel.TripDateTo.Value.Date);
                    }
                    if (searchTripModel.TripId != 0)
                    {
                        tripsJobsite = tripsJobsite.Where(tj => tj.TripId == searchTripModel.TripId);
                    }
                    if (searchTripModel.DriverId != -1)
                    {
                        tripsJobsite = tripsJobsite.Where(tj => tj.Trip.DriverId == searchTripModel.DriverId);
                    }
                    if (searchTripModel.JobSiteId != -1)
                    {
                        tripsJobsite = tripsJobsite.Where(tj => tj.JobSiteId == searchTripModel.JobSiteId);
                    }
                    if (searchTripModel.TruckId != "-1")
                    {
                        tripsJobsite = tripsJobsite.Where(tj => tj.Trip.TruckId == searchTripModel.TruckId);
                    }
                    
                    if (searchTripModel.IsTripConverted == true)
                    {
                        tripsJobsite = tripsJobsite.Where(tj => tj.Trip.IsConverted == true);
                    }
                    if (searchTripModel.IsTripOffline == true)
                    {
                        tripsJobsite = tripsJobsite.Where(tj => tj.JobSite.HasNetworkCoverage == false);
                    }
                    //if (searchTripModel.IsTripTake5Completed == true && searchTripModel.IsTripTake5UnCompleted == false)
                    //{
                    //    tripsJobsite = tripsJobsite.Where(tj => tj.Take5Status == (int)CommanData.Take5Status.Completed);
                    //}
                    //if (searchTripModel.IsTripTake5Completed == false && searchTripModel.IsTripTake5UnCompleted == true)
                    //{
                    //    tripsJobsite = tripsJobsite.Where(tj => tj.Take5Status != (int)CommanData.Take5Status.Completed && tj.TripStatus != (int)CommanData.TripStatus.Pending);
                    //}
                    if (searchTripModel.SelectedSatuses != null)
                    {
                        tripsJobsite = tripsJobsite.Where(tj=>searchTripModel.SelectedSatuses.Contains(tj.TripStatus));
                    }
                    List<TripJobsiteModel> tripJobsiteModels = _mapper.Map<List<TripJobsiteModel>>(tripsJobsite.ToList());
                    foreach(var tripJobsite in tripJobsiteModels)
                    {
                        if(tripJobsite.Trip.Cancelled == true)
                        {
                            tripJobsite.TripCancellationModel = _tripCancellationService.GetTripCancellation(tripJobsite.TripId).Result;
                        }
                        if(tripJobsite.RequestStatus == (int)CommanData.RequestStatus.Approved && tripJobsite.RequestResponsedBy != "System")
                        {
                            Employee employee = _employeeService.GetEmployeeByUserId(tripJobsite.RequestResponsedBy);
                            tripJobsite.RequestResponsedByName = employee.EmployeeName;
                            tripJobsite.RequestResponsedByNumber = employee.EmployeeNumber;
                        }
                        else
                        {
                            tripJobsite.RequestResponsedByName = "System";
                            tripJobsite.RequestResponsedByNumber = 0;
                        }
                    }
                    return tripJobsiteModels;
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

        public List<TripJobsiteModel> GetTodayTrips()
        {
            try
            {
               var tripsJobsite = _repository.Find(tj => tj.IsVisible == true && tj.Trip.TripDate.Date.CompareTo(DateTime.Today) == 0, false, tj=>tj.Trip, tj => tj.JobSite, tj => tj.Trip.Driver, tj => tj.Trip.Truck);
                if (tripsJobsite != null)
                {
                    List<TripJobsiteModel> tripJobsiteModels = _mapper.Map<List<TripJobsiteModel>>(tripsJobsite);
                    return tripJobsiteModels;
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

        public List<TripJobsiteModel> SearchDriverTrips(SearchDriverTripModel searchDriverTripModel)
        {
            //try
            //{
            //    _repository.Find(tj => tj.Trip.DriverId == searchDriverTripModel.DriverId && tj.IsVisible == true, false, tj => tj.Trip, tj => tj.JobSite);
            //}
            //catch(Exception e)
            //{

            //}
            return null;
        }

        public SearchDriverTripModel InitiateSearchDriverTripModel()
        {
            throw new NotImplementedException();
        }

        public bool AddStepTwoRequestForOfflineTrip(StepTwoStartRequest model, long TripId, long JobsiteId)
        {
            try
            {
                TripJobsite tripJobsite = GetTripJobsiteByTripNumberAndJobsiteId(TripId, JobsiteId).Result;
                tripJobsite.StageTwoRequestDate = model.RequestDate;
                tripJobsite.StageTwoResponseDate = model.RequestDate;
                tripJobsite.StageTwoRequestCreatedDate = DateTime.Now;
                tripJobsite.UpdatedDate = DateTime.Now;
                tripJobsite.TripStatus = (int)CommanData.TripStatus.StepTwoResponsed;
                tripJobsite.Take5Status = (int)CommanData.Take5Status.StepTwoRequested;
                tripJobsite.RequestStatus = (int)CommanData.RequestStatus.Approved;
                tripJobsite.RequestResponsedBy = "System";
                Trip trip = tripJobsite.Trip;
                trip.TripStatus = (int)CommanData.TripStatus.StepTwoResponsed;
                trip.Take5Status = (int)CommanData.Take5Status.StepTwoRequested;
                trip.UpdatedDate = DateTime.Now;
                bool result = _repository.UpdateTwoEntities(tripJobsite, trip);
                return result;

            }
            catch (Exception e)
            {
                return false;
            }
        }


        public async Task<string> AddUpdatesToOfflineTrip(AllTripSteps model)
        {
            string returnMessage = "";
            try
            {
                TripJobsite tripJobsite = GetTripJobsiteByTripNumberAndJobsiteId(model.TripId, model.JobsiteId).Result;
                if (model.TripDestinationArrivedModel != null)
                {
                    bool arrivedResult = await ArrivingTripJobsite(model.TripDestinationArrivedModel, model.TripId, model.JobsiteId);
                    if (arrivedResult == true)
                    {
                        if (model.SurveyStepOneAnswersAPIModel != null)
                        {
                            bool addStepOneResult = UpdateStepOneCompletion(model.SurveyStepOneAnswersAPIModel, model.TripId, model.JobsiteId).Result;

                            if (addStepOneResult == true)
                            {
                                addStepOneResult = _surveyService.AddAnswersToStepNQuestions(model.SurveyStepOneAnswersAPIModel, model.TripId, model.JobsiteId, (int)CommanData.SurveySteps.StepOne).Result;
                                if (addStepOneResult == true)
                                {
                                    if (model.Take5StepTwoRequestAPIModel != null)
                                    {
                                        bool addRequestResult = AddStepTwoRequestForOfflineTrip(model.Take5StepTwoRequestAPIModel, model.TripId, model.JobsiteId);
                                        if (addRequestResult == true)
                                        {
                                            if (model.SurveyStepTwoAnswersAPIModel != null)
                                            {
                                                SurveyStepOneAnswersAPIModel surveyStepOneAnswersAPIModel = _mapper.Map<SurveyStepOneAnswersAPIModel>(model.SurveyStepTwoAnswersAPIModel);
                                                bool addStepTwoResult = UpdateStepTwoCompletion(surveyStepOneAnswersAPIModel, model.TripId, model.JobsiteId).Result;
                                                if (addStepTwoResult == true)
                                                {
                                                    addStepTwoResult = _surveyService.AddAnswersToStepNQuestions(surveyStepOneAnswersAPIModel, model.TripId, model.JobsiteId, (int) CommanData.SurveySteps.StepTwo).Result;
                                                    if (addStepTwoResult == true)
                                                    {
                                                        bool addTake5TogetherResult = false;
                                                        if (model.Take5TogetherAPIModels != null)
                                                        {
                                                           addTake5TogetherResult = _tripTake5TogetherService.AddTripTake5TogetherForTrip(model);
                                                        }
                                                        else
                                                        {
                                                            addTake5TogetherResult = true;
                                                        }
                                                        if (addTake5TogetherResult == true)
                                                        {
                                                            addTake5TogetherResult = UpdateTripJobsiteStatus(model.TripId, model.JobsiteId, "TripCompleted").Result;
                                                            if (addTake5TogetherResult != true)
                                                            {
                                                                returnMessage = "Failed Process, Can not complete trip";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            returnMessage = "Failed Process, Can not add Take5 Together data";
                                                        }
                                                        returnMessage = "Success Process, trip updated successfully";
                                                    }
                                                    else
                                                    {
                                                        addRequestResult = AddStepTwoRequestForOfflineTrip(model.Take5StepTwoRequestAPIModel, model.TripId, model.JobsiteId);
                                                        if (addRequestResult == true)
                                                        {
                                                            returnMessage = "Failed Process, Failed to add Step Two of survey";
                                                        }
                                                        else
                                                        {
                                                            returnMessage = "Failed Process, Failed to add Step Two of survey, while trip status is updated, contact your technical support";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    returnMessage = "Failed Process, Failed to add Step Two of survey";
                                                }
                                            }
                                            else
                                            {
                                                returnMessage = "Failed Process, Step Two of survey is empty";
                                            }
                                        }
                                        else
                                        {
                                            returnMessage = "Failed Process, Failed to add Step Two Request";
                                        }
                                    }
                                    else
                                    {
                                        returnMessage = "Failed Process, Step Two Request is empty";
                                    }
                                }
                                else
                                {
                                    arrivedResult = await ArrivingTripJobsite(model.TripDestinationArrivedModel, model.TripId, model.JobsiteId);
                                    if (arrivedResult == true)
                                    {
                                        returnMessage = "Failed Process, Failed to add Step one of survey";
                                    }
                                    else
                                    {
                                        returnMessage = "Failed Process, Failed to add Step one of survey, while trip status is updated, contact your technical support";
                                    }
                                }
                                
                            }
                            else
                            {
                                returnMessage = "Failed Process, Failed to add Step One of survey";
                            }
                        }
                        else
                        {
                            returnMessage = "Failed Process, Step one of survey is empty";
                        }
                    }
                    else
                    {
                        returnMessage = "Failed Process, Failed to update trip status for destination arrival";
                    }
                }
                else
                {
                    returnMessage = "Failed Process, Destination arrived  is empty";
                }
                return returnMessage;
            }
            catch (Exception e)
            {
                return "Failed Process, contact your support";
            }
        }

        public List<TripJobsiteModel> GetOfflineTrips()
        {
            try
            {
                List<TripJobsiteModel> tripJobsiteModels = new List<TripJobsiteModel>();
                List<TripJobsite> TripJobsites = _repository.Find(tj => tj.IsVisible == true && tj.JobSite.HasNetworkCoverage == false, false, tj => tj.Trip.Driver, tj => tj.JobSite).ToList();
                if (TripJobsites.Count > 0)
                {
                    tripJobsiteModels = _mapper.Map<List<TripJobsiteModel>>(TripJobsites);
                }
                return tripJobsiteModels;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public TripJobsiteModel SetTripStatuesAndDatesAsText(TripJobsiteModel tripJobsiteModel)
        {
            try
            {
                tripJobsiteModel.Take5StatusText = Enum.GetName(typeof(CommanData.Take5Status), tripJobsiteModel.Take5Status);
                tripJobsiteModel.TripStatusText = Enum.GetName(typeof(CommanData.TripStatus), tripJobsiteModel.TripStatus);
                if(tripJobsiteModel.TripStatus == (int) CommanData.TripStatus.StepTwoRequested)
                {
                    tripJobsiteModel.RequestStatusName = Enum.GetName(typeof(CommanData.RequestStatus), tripJobsiteModel.RequestStatus).ToString();
                }
                else if(tripJobsiteModel.TripStatus >= (int)CommanData.TripStatus.StepTwoRequested)
                {
                    tripJobsiteModel.RequestStatusName = Enum.GetName(typeof(CommanData.RequestStatus), tripJobsiteModel.RequestStatus).ToString();
                    if(tripJobsiteModel.RequestResponsedBy == "System")
                    {
                        tripJobsiteModel.RequestResponsedByName = "System";
                        tripJobsiteModel.RequestResponsedByNumber = 0;
                    }
                    else
                    {
                        Employee employee = _employeeService.GetEmployeeByUserId(tripJobsiteModel.RequestResponsedBy);
                        tripJobsiteModel.RequestResponsedByName = employee.EmployeeName;
                        tripJobsiteModel.RequestResponsedByNumber = employee.EmployeeNumber;
                    }
                }
                if (tripJobsiteModel.Trip.TripDate != null)
                {
                    tripJobsiteModel.TripDateText = tripJobsiteModel.Trip.TripDate.ToString("yyyy-MM-dd, HH:mm:ss");
                }

                if (tripJobsiteModel.Trip.CreatedDate != null)
                {
                    tripJobsiteModel.CreatedDateText = tripJobsiteModel.Trip.CreatedDate.ToString("yyyy-MM-dd, HH:mm:ss");
                }

                if (tripJobsiteModel.StartingDate.HasValue == true)
                {
                    tripJobsiteModel.StartingDateText = tripJobsiteModel.StartingDate.Value.ToString("yyyy-MM-dd, HH:mm:ss");
                }
                if (tripJobsiteModel.DestinationArrivingDate.HasValue == true)
                {
                    tripJobsiteModel.DestinationArrivingDateText = tripJobsiteModel.DestinationArrivingDate.Value.ToString("yyyy-MM-dd, HH:mm:ss");
                }
                if (tripJobsiteModel.StageOneComplatedTime.HasValue == true)
                {
                    tripJobsiteModel.StageOneComplatedTimeText = tripJobsiteModel.StageOneComplatedTime.Value.ToString("yyyy-MM-dd, HH:mm:ss");
                }
                if (tripJobsiteModel.StageTwoRequestDate.HasValue == true)
                {
                    tripJobsiteModel.StageTwoRequestDateText = tripJobsiteModel.StageTwoRequestDate.Value.ToString("yyyy-MM-dd, HH:mm:ss");
                }
                if (tripJobsiteModel.StageTwoResponseDate.HasValue == true)
                {
                    tripJobsiteModel.StageTwoResponseDateText = tripJobsiteModel.StageTwoResponseDate.Value.ToString("yyyy-MM-dd, HH:mm:ss");
                }
                //if (tripJobsiteModel.StageTwoRequestCreatedDate.HasValue == true)
                //{
                //    tripJobsiteModel.StageTwoRequestCreatedDateText = tripJobsiteModel.StageTwoRequestCreatedDate.Value.ToString("yyyy-MM-dd, HH:mm:ss");
                //}
                if (tripJobsiteModel.StageTwoComplatedTime.HasValue == true)
                {
                    tripJobsiteModel.StageTwoComplatedTimeText = tripJobsiteModel.StageTwoComplatedTime.Value.ToString("yyyy-MM-dd, HH:mm:ss");
                }

                return tripJobsiteModel;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<TripJobsite> ConvertTripJobsite(long tripNumber, long jobsiteId)
        {
            try
            {
                bool updateResult = false;
                TripJobsite tripJobsite = _repository.Find(t => t.TripId == tripNumber && t.JobSiteId == jobsiteId && t.IsVisible == true && t.Trip.Cancelled == false).FirstOrDefault();
                if (tripJobsite != null)
                {
                    tripJobsite.Converted = true;
                    tripJobsite.IsVisible = false;
                    updateResult = _repository.Update(tripJobsite);
                    if (updateResult == true)
                    {
                        return tripJobsite;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }


        }

        public List<TripJobsiteModel> GetTripJobsiteModelByTripNumber(long tripNumber)
        {
            try
            {
                var tripJobsites = _repository.Find(tj => tj.TripId == tripNumber && tj.IsVisible == true, false, tj => tj.Trip, tj => tj.Trip.Driver, tj => tj.JobSite);
                List<TripJobsiteModel> tripJobsiteModels = _mapper.Map<List<TripJobsiteModel>>(tripJobsites);
                return tripJobsiteModels;
            }
            catch(Exception e)
            {
                return null;
            }
        }


        public async Task<bool> ConvertTrip(long tripId, long JobsiteId)
        {
            try
            {
               bool updateResult = false;
               TripJobsite tripJobsite = GetTripJobsiteByTripNumberAndJobsiteId(tripId, JobsiteId).Result;
                if(tripJobsite != null)
                {
                    tripJobsite.IsVisible = false;
                    tripJobsite.Converted = true;
                    updateResult = _repository.Update(tripJobsite);
                    return updateResult;
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

        public bool AddOfflineResponseforStepTwoRequestForTrip(StepTwoStartRequest model, long TripId, long JobsiteId)
        {
            try
            {
                TripJobsite tripJobsite = GetTripJobsiteByTripNumberAndJobsiteId(TripId, JobsiteId).Result;
                tripJobsite.StageTwoResponseDate = model.ResponseDate;
                tripJobsite.UpdatedDate = DateTime.Now;
                tripJobsite.TripStatus = (int)CommanData.TripStatus.StepTwoResponsed;
                tripJobsite.RequestStatus = (int)CommanData.RequestStatus.Approved;
                tripJobsite.RequestResponsedBy = "System";
                Trip trip = tripJobsite.Trip;
                trip.TripStatus = (int)CommanData.TripStatus.StepTwoResponsed;
                trip.UpdatedDate = DateTime.Now;
                bool result = _repository.UpdateTwoEntities(tripJobsite, trip);
                return result;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        async Task<bool> SendTripToDashboard(DashBoardTripModel dashBoardTripModel, string roleName, string tripStatus)
        {
            try
            {
                var aspNetUsers = _userManager.GetUsersInRoleAsync(roleName);
                if (aspNetUsers.Result != null)
                {
                   var result = JsonSerializer.Serialize(dashBoardTripModel);
                    foreach (var user in aspNetUsers.Result)
                    {
                        var connections = _userConnectionManager.GetUserConnections(user.Id);
                        if (connections != null && connections.Count > 0)
                        {
                            foreach (var connectionId in connections)
                            {
                                await _hub.Clients.Client(connectionId).SendAsync("updateDashboard", result, tripStatus);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> UpdateDashboard(long tripId, long jobsiteId, string roleName, string tripStatus)
        {
            try
            {
               TripJobsiteModel tripJobsiteModel = GetTripJobsiteModelByTripNumberAndJobsiteId(tripId, jobsiteId);
                if(tripJobsiteModel != null)
                {
                    List<TripJobsiteModel> tripJobsiteModels = new List<TripJobsiteModel>();
                    tripJobsiteModels.Add(tripJobsiteModel);
                    DashBoardTripModel dashBoardTripModel = ConvertFromTripJobsiteModelToDashBoardTripModel(tripJobsiteModels).FirstOrDefault();
                    bool result = SendTripToDashboard(dashBoardTripModel, roleName, tripStatus).Result;
                    return result;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<DashBoardTripModel> ConvertFromTripJobsiteModelToDashBoardTripModel(List<TripJobsiteModel> tripJobsiteModels)
        {
            try
            {
                List<DashBoardTripModel> dashBoardTripModels = new List<DashBoardTripModel>();
                foreach (var tripJobsiteModel in tripJobsiteModels)
                {
                    DashBoardTripModel dashBoardTripModel = new DashBoardTripModel();
                    dashBoardTripModel.TripNumber = tripJobsiteModel.TripId;
                    dashBoardTripModel.TripDate = tripJobsiteModel.Trip.TripDate;
                    dashBoardTripModel.TruckNumber = tripJobsiteModel.Trip.TruckId;
                    dashBoardTripModel.JobsiteName = tripJobsiteModel.JobSite.Name;
                    dashBoardTripModel.JobsiteNumber = tripJobsiteModel.JobSite.Id;
                    dashBoardTripModel.Latituide = tripJobsiteModel.JobSite.Latitude;
                    dashBoardTripModel.Longitude = tripJobsiteModel.JobSite.Longitude;
                    dashBoardTripModel.DriverNumber = tripJobsiteModel.Trip.Driver.Id;
                    dashBoardTripModel.DriverName = tripJobsiteModel.Trip.Driver.FullName;
                    dashBoardTripModel.TripStatus = Enum.GetName(typeof(CommanData.TripStatus), tripJobsiteModel.TripStatus);
                    dashBoardTripModel.IsTripConverted = tripJobsiteModel.Trip.IsConverted;
                    dashBoardTripModel.Take5Status = Enum.GetName(typeof(CommanData.Take5Status), tripJobsiteModel.Trip.Take5Status);
                    if (tripJobsiteModel.StartingDate.HasValue == true)
                    {
                        dashBoardTripModel.StatrtingDate = tripJobsiteModel.StartingDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (tripJobsiteModel.DestinationArrivingDate.HasValue)
                    {
                        dashBoardTripModel.ArrivingDate = tripJobsiteModel.DestinationArrivingDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (tripJobsiteModel.StageOneComplatedTime.HasValue)
                    {
                        dashBoardTripModel.StepOneCompletedDate = tripJobsiteModel.StageOneComplatedTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (tripJobsiteModel.StageTwoRequestCreatedDate.HasValue)
                    {
                        dashBoardTripModel.RequestDate = tripJobsiteModel.StageTwoRequestCreatedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (tripJobsiteModel.StageTwoComplatedTime.HasValue)
                    {
                        dashBoardTripModel.StepTwoCompletedDate = tripJobsiteModel.StageTwoComplatedTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    dashBoardTripModels.Add(dashBoardTripModel);
                }

                return dashBoardTripModels;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public async Task<bool> DeleteTripJobsite(long tripnumber)
        {
            try
            {
                TripJobsite tripJobsite = GetTripByTripNumber(tripnumber);
                if(tripJobsite != null)
                {
                    bool result = _repository.Delete(tripJobsite);
                    return result;
                }
                else
                {
                    return false;
                }
                
                
            }
            catch (Exception e)
            {
                return false;
            }
        }


    }
}
