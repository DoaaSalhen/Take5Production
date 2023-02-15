using System;
using System.Collections.Generic;
using System.Text;

namespace Take5.Services.Models.APIModels
{
    public class CheckRequestStatusModel
    {
        public string UserId { get; set; }
        public long TripId { get; set; }
        public long JobsiteId { get; set; }
        public string TruckNumber { get; set; }
    }


    public class CheckTripStatusModel
    {
        public string UserId { get; set; }
        public long TripId { get; set; }
        public long JobsiteId { get; set; }

        public string TruckNumber { get; set; }
    }
}
