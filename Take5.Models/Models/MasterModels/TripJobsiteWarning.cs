using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.Entity;

namespace Take5.Models.Models.MasterModels
{
    public class TripJobsiteWarning:EntityWithIdentityId<long>
    {
        [Required]
        public TripJobsite TripJobsite { get; set; }

        [Required]
        public long TripJobsiteTripId { get; set; }

        [Required]
        public long TripJobsiteJobSiteId { get; set; }

        [Required]
        public int WarningTypeId { get; set; }

        [Required]
        public WarningType WarningType { get; set; }

        [Required]
        public string  Message { get; set; }
    }
}
