using System;
using System.Collections.Generic;
using System.Text;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Models.SpecificModels
{
    public class HomeModel
    {
        public Dictionary<string, long> TripStatuscounts { get; set; }

        public NotificationModel NotificationModel { get; set; }

        public List<DashBoardTripModel> StartedTrips { get; set; }

        public List<DashBoardTripModel> DestinationArrivedTrips { get; set; }

        public List<DashBoardTripModel> StepOneCompletedTrips { get; set; }

        public List<StepTwoRequestModel> StepTwoRequestModels { get; set; }

        public List<DashBoardTripModel> StepTwoCompletedTrips { get; set; }



    }
}
