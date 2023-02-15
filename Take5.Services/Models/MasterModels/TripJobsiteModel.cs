using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.MasterModels;

namespace Take5.Services.Models.MasterModels
{
    public class TripJobsiteModel
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

        public long RequestResponsedByNumber { get; set; }
        public string RequestResponsedByName { get; set; }

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

        public long ConvertedJobSiteId { get; set; }

        public List<JobSiteModel> Jobsites { get; set; }

        public string TripDateText { get; set; }

        public string CreatedDateText { get; set; }
        public string StartingDateText { get; set; }

        public string DestinationArrivingDateText { get; set; }

        public string StageOneComplatedTimeText { get; set; }

        public string StageTwoRequestDateText { get; set; }

        public string StageTwoRequestCreatedDateText { get; set; }

        public string StageTwoResponseDateText { get; set; }

        public string StageTwoComplatedTimeText { get; set; }

        public string Take5StatusText { get; set; }

        public string TripStatusText { get; set; }

        public TripCancellationModel TripCancellationModel { get; set; }

        public string TripType { get; set; }

        public bool HasTake5Together { get; set; }

        public string RequestStatusName { get; set; }

        public List<TripJobsiteWarningModel> TripJobsiteWarningModels { get; set; }

    }
}
