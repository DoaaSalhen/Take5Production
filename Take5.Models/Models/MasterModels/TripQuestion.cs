using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.Entity;

namespace Take5.Models.Models.MasterModels
{
    public class TripQuestion : EntityWithIdentityId<long>
    {
        public long TripId { get; set; }

        [Required]
        public Trip Trip { get; set; }


        [Required]
        public long JobSiteId { get; set; }

        public JobSite JobSite { get; set; }

        public int QuestionId { get; set; }

        public Question Question { get; set; }
        [Required]
        public bool Answer { get; set; }
    }

}
