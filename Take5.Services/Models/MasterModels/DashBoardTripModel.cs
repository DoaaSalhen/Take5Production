using System;
using System.Collections.Generic;
using System.Text;

namespace Take5.Services.Models.MasterModels
{
    public class DashBoardTripModel
    {
        public long TripNumber { get; set; }

        public string JobsiteName { get; set; }

        public long JobsiteNumber { get; set; }

        public bool JobsiteHasNetworkCoverage { get; set; }
        public double Longitude { get; set; }
        public double Latituide { get; set; }

        public string DriverName { get; set; }

        public long DriverNumber { get; set; }

        public string TruckNumber { get; set; }

        public string TripStatus { get; set; }

        public string Take5Status { get; set; }

        public bool IsTripConverted { get; set; }

        public DateTime TripDate { get; set; }

        public string StatrtingDate { get; set; }

        public DateTime StatrtingDate2 { get; set; }

        public string ArrivingDate { get; set; }
        public string StepOneCompletedDate { get; set; }

        public string RequestDate { get; set; }

        public string StepTwoCompletedDate { get; set; }

    }
}
