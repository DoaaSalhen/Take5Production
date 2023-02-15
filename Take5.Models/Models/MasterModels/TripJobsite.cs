using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.Entity;

namespace Take5.Models.Models.MasterModels
{
    public class TripJobsite
    {
        [Required]
        public long TripId { get; set; }
        public Trip Trip { get; set; }

        [Required]
        public long JobSiteId { get; set; }

        public JobSite JobSite { get; set; }

        public DateTime? StartingDate { get; set; }

        public DateTime? DestinationArrivingDate { get; set; }

        public DateTime? StageOneComplatedTime { get; set; }

        public DateTime? StageTwoRequestDate { get; set; }

        public DateTime? StageTwoRequestCreatedDate { get; set; }

        public DateTime? StageTwoResponseDate { get; set; }

        public int? RequestStatus { get; set; }

        public string RequestResponsedBy { get; set; }

        public DateTime? StageTwoComplatedTime { get; set; }

        [Required]
        public int Take5Status { get; set; }

        [Required]
        public int TripStatus { get; set; }

        [Required]
        public bool IsDelted { get; set; }

        [Required]
        public bool IsVisible { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }

        [Required]
        public bool Converted { get; set; }

    }
}
