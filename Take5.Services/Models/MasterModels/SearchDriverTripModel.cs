using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Take5.Models.Models.MasterModels;

namespace Take5.Services.Models.MasterModels
{
    public class SearchDriverTripModel
    {
        public TripJobsiteModel TripJobsiteModel { get; set; }

        public DateTime? TripDateFrom { get; set; }

        public DateTime? TripDateTo { get; set; }

        public long DriverId { get; set; }
        public Driver Driver { get; set; }

        public List<JobSiteModel> JobSites { get; set; }

        public List<TripJobsiteModel> tripJobsiteModels { get; set; }

        public List<TripStatusModel> tripStatusModels { get; set; }

        [BindProperty]
        public List<int> SelectedSatuses { get; set; }
    }
}
