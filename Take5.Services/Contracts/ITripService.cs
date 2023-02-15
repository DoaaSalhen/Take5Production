using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Take5.Models.Models.MasterModels;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.MasterModels;
using Take5.Services.Models.SpecificModels;

namespace Take5.Services.Contracts
{
    public interface ITripService
    {

        public Task<bool> CreateTripAfterDelete(TripModel model);
        public Task<bool> UpdateJobsiteForTrip(TripModel model);
        List<TripModel> GetAllTrips();
        Task<bool> CreateTrip(TripModel model);
        Task<bool> UpdateTrip(TripModel model);

        Task<bool> UpdateTripStatus(long tripNumber, int tripStatusId, int take5Status = 1);
        TripModel GetTrip(long id);
        Task<TripModel> UpdateTripArrivedDate(long TripNumber, DateTime arrivedDate);
        List<TripModel> GetTripByDate(DateTime tripDate);
        List<TripAPIModel> ConvertFromTripJobsiteModelToTripAPIModel(List<TripJobsiteModel> tripJobsiteModel);
        //Dictionary<string, long> GetTripsCountByStatus();
        List<TripModel> GetTripByStatus(int status);
        //List<TripJobsiteModel> GetHomeTrips();
        //bool DeleteTake5ForTrip(long tripNumber);
        Trip GetTripByDriverIdAndDate(long driverId, DateTime tripDate);
        TripModel GetPendingAndUnCompletedTripForDriver(long driverId);
        TripModel GetPendingAndUnCompletedTripForTruck(string truckId);
        TripJobsiteModel CreateStepTwoRequest(StepTwoStartRequest model, long TripId, long JobsiteId);
        TripModel ResetTrip(long tripNumber);
        SearchTripModel InitiateTripJobsiteSearchModel(SearchTripModel searchTripModel);
        Task<Trip> CancelTrip(long tripNumber, TripCancellationModel tripCancellationModel);

        public Task<bool> DeleteTrip(long id);

    }
}
