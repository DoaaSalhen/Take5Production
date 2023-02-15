using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.MasterModels;

namespace Take5.Services.Models.MasterModels
{
    public class StepTwoRequestModel
    {
        [Required]
        public long TripId { get; set; }

        [Required]
        public long JobsiteId { get; set; }

        public string JobsiteName { get; set; }


        [Required]
        public string DriverName { get; set; }
        public long DriverId { get; set; }


        [Required]
        public DateTime DestinationArrivingDate { get; set; }

        [Required]
        public DateTime StageOneComplatedTime { get; set; }

        [Required]
        public DateTime StageTwoRequestDate { get; set; }

        [Required]
        public DateTime StageTwoRequestCreatedDate { get; set; }


        [Required]
        public DateTime StageTwoResponseDate { get; set; }

        [Required]
        public int RequestStatus { get; set; }

        public string RequestStatusName { get; set; }

        public string RequestResponsedBy { get; set; }

        [Required]
        public int Take5Status { get; set; }

        [Required]
        public int TripStatus { get; set; }

        public string TripStatusName { get; set; }

        public string WarningMessage { get; set; }

    }
}
