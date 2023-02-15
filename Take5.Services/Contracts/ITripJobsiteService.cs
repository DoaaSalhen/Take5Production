using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Take5.Models.Models.MasterModels;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Contracts
{
    public interface ITripJobsiteService
    {
        public Task<bool> CreateTripJobsiteAfterDelete(TripJobsite tripJobsite);
        public Task<bool> DeleteTripJobsite(long tripnumber);
        Task<bool> UpdateTripJobsite(TripJobsiteModel model);
        TripJobsiteModel GetTripJobsiteModelByTripNumberAndJobsiteId(long tripNumber, long jobsiteId);

        List<TripJobsiteModel> GetTripJobsiteModelByTripNumber(long tripNumber);
        List<TripJobsiteModel> GetAllJobsiteTrips();
        Task<bool> CreateTripJobsite(long tripId, long jobsiteId);
        TripJobsiteModel GetJobsitePendingTripForDriver(long driverId);
        List<TripJobsiteModel> GetHomeTrips();
        Dictionary<string, long> GetTripsCountByStatus();

        //Task<TripJobsite> CancelJobSiteTrip(long tripNumber);
        List<StepTwoRequestModel> GetStepTwoRequests();
        List<StepTwoRequestModel> MapFromTripJobsiteModelToStepTwoRequestModel(List<TripJobsiteModel> tripJobsiteModels);
        Task<bool> ApproveStepTwoRequest(TripJobsiteModel tripJobsiteModel);

        TripJobsite GetTripByTripNumber(long tripNumber);
        bool UpdateJobsiteForTrip(long tripNumber, long jobsiteId);

        List<TripJobsiteModel> SearchForTrip(SearchTripModel searchTripModel);

        List<TripJobsiteModel> GetTodayTrips();
        Task<bool> SatrtTripJobsite(TripStartingModel tripStartingModel);
        Task<bool> ArrivingTripJobsite(TripDestinationArrivedModel TripDestinationArrivedModel, long TripId, long JobsiteId);
        Task<bool> UpdateStepOneCompletion(SurveyStepOneAnswersAPIModel model,long TripId, long JobSiteId);
        Task<bool> UpdateStepTwoCompletion(SurveyStepOneAnswersAPIModel model, long TripId, long JobSiteId);

        List<TripJobsiteModel> SearchDriverTrips(SearchDriverTripModel searchDriverTripModel);

        SearchDriverTripModel InitiateSearchDriverTripModel();
        Task<TripJobsite> GetTripJobsiteByTripNumberAndJobsiteId(long tripNumber, long jobsiteId);
        bool AddStepTwoRequestForOfflineTrip(StepTwoStartRequest model, long TripId, long JobsiteId);
        Task<string> AddUpdatesToOfflineTrip(AllTripSteps model);
        List<TripJobsiteModel> GetOfflineTrips();
        List<StepTwoRequestModel> GetPendingStepTwoRequests();

        TripJobsiteModel SetTripStatuesAndDatesAsText(TripJobsiteModel tripJobsiteModel);

        TripJobsiteModel GetCurrentJobsiteTripForDriver(long driverId);

        Task<bool> ConvertTrip(long tripId, long JobsiteId);

        Task<bool> UpdateTripJobsiteStatus(long TripId, long JobsiteId, string tripStatus);

        Task<bool> UpdateTripJobsiteTake5Status(long TripId, long JobsiteId, string take5Status);
        bool AddOfflineResponseforStepTwoRequestForTrip(StepTwoStartRequest model, long TripId, long JobsiteId);
        List<DashBoardTripModel> ConvertFromTripJobsiteModelToDashBoardTripModel(List<TripJobsiteModel> tripJobsiteModels);
        Task<bool> UpdateDashboard(long tripId, long jobsiteId, string roleName, string tripStatus);
        public TripJobsiteModel GetTripJobsiteModelByTripNumberAndJobsiteIdForAPI(long tripNumber, long jobsiteId);
    }
}
