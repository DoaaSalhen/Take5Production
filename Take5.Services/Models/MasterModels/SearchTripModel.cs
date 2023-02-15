using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.MasterModels;

namespace Take5.Services.Models.MasterModels
{
    public class SearchTripModel
    {
        public long DriverId { get; set; }
        public Driver Driver { get; set; }
        public string TruckId { get; set; }
        public Truck Truck { get; set; }

        public long JobSiteId { get; set; }

        public JobSite JobSite { get; set; }

        public long TripId { get; set; }
        public Trip Trip { get; set; }

        public DateTime? TripDateFrom { get; set; }

        public DateTime? TripDateTo { get; set; }

        public bool IsTripConverted { get; set; }

        public bool IsTripOffline { get; set; }

        public bool IsTripCancelled { get; set; }


        public bool IsTripTake5Completed { get; set; }

        public bool IsTripTake5UnCompleted { get; set; }


        public int Take5Status { get; set; }

        public int TripStatus { get; set; }

        public List<TruckModel> Trucks { get; set; }

        public List<DriverModel> Drivers { get; set; }

        public List<JobSiteModel> JobSites { get; set; }
        public List<TripJobsiteModel> tripJobsiteModels { get; set; }

        public List<TripStatusModel> tripStatusModels { get; set; }

        [BindProperty]
        public List<int> SelectedSatuses { get; set; }

        public bool HasTake5Together { get; set; }

    }
}
