using System;
using System.Collections.Generic;
using System.Text;

namespace Take5.Services.Models.SpecificModels
{
    public class TripStatusStatisticsModel
    {
        public long AllTripsCount { get; set; }

        public long PendingTripsCount { get; set; }

        public long InprogressTripsCount { get; set; }

        public long UnCompletedTake5Count { get; set; }

    }
}
