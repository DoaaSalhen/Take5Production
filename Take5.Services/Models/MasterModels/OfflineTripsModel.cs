using System;
using System.Collections.Generic;
using System.Text;

namespace Take5.Services.Models.MasterModels
{
    public class OfflineTripsModel
    {
        public List<TripJobsiteModel> tripJobsiteModels { get; set; }

        public long TripNumber { get; set; }
        public DateTime TripDate { get; set; }

        public long JobsiteNumber { get; set; }

        public long DriverNumber { get; set; }
    }
}
