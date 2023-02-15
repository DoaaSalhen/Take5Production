using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.MasterModels;

namespace Take5.Services.Models.MasterModels
{
    public class TripModel
    {
        [Required]
        public long DriverId { get; set; }
        public Driver Driver { get; set; }
        [Required]
        public string TruckId { get; set; }
        public Truck Truck { get; set; }
        [Required]
        public long JobSiteId { get; set; }
        public JobSite JobSite { get; set; }
        public long Id { get; set; }

        [DefaultValue(false)]
        public bool IsDelted { get; set; }

        [DefaultValue(true)]
        public bool IsVisible { get; set; }

        [Required]
        public bool IsConverted { get; set; }

        public DateTime CreatedDate { get; set; } 
        public DateTime UpdatedDate { get; set; } 

        public List<TruckModel> Trucks { get; set; }

        public List<DriverModel> Drivers { get; set; }

        public List<JobSiteModel> JobSites { get; set; }

        [Required]
        public DateTime TripDate { get; set; }

        [Required]
        public DateTime ArrivedDate { get; set; }
        [Required]
        public DateTime departureDate { get; set; }

        [Required]
        public int Take5Status { get; set; }
        [Required]
        public int TripStatus { get; set; }
        [Required]
        public DateTime StageOneComplatedTime { get; set; }
        [Required]
        public DateTime StageTwoComplatedTime { get; set; }

    }
}
