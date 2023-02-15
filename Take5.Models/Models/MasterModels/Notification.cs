using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.Entity;

namespace Take5.Models.Models.MasterModels
{
    public class Notification:EntityWithIdentityId<long>
    {
        [Required]
        public string Message { get; set; }
        [Required]
        public int NotificationTypeId { get; set; }
        [Required]
        public NotificationType NotificationType { get; set; }

        [Required]
        public Trip Trip { get; set; }

        [Required]
        public long TripId { get; set; }

        [Required]
        public JobSite JobSite { get; set; }

        [Required]
        public long JobSiteId { get; set; }
    }
}
