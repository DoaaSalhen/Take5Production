using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.Entity;

namespace Take5.Models.Models.MasterModels
{
    public class TripTake5Together: EntityWithIdentityId<long>
    {

        [Required]
        public Trip Trip { get; set; }

        [Required]
        public long TripId { get; set; }

        [Required]
        public JobSite JobSite { get; set; }

        [Required]
        public long JobSiteId { get; set; }

        [Required]
        public Driver Driver { get; set; }

        [Required]
        public long DriverId { get; set; }

        [Required]
        public long ParticipantDriverId { get; set; }

        [Required]
        public long  WhoStartDriverId { get; set; }

        [Required]
        public string Notes { get; set; }

    }
}
