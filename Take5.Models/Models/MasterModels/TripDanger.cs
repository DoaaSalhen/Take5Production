using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.Entity;

namespace Take5.Models.Models.MasterModels
{
    public class TripDanger:EntityWithIdentityId<long>
    {
        [Required]
        public long TripId { get; set; }
        [Required]
        public Trip Trip { get; set; }

        [Required]
        public long JobSiteId { get; set; }
        [Required]
        public JobSite JobSite { get; set; }

        [Required]
        public long MeasureControlId { get; set; }
        [Required]
        public MeasureControl MeasureControl { get; set; }

        [Required]
        public int DangerId { get; set; }



    }
}
