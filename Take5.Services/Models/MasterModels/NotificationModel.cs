using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.MasterModels;

namespace Take5.Services.Models.MasterModels
{
    public class NotificationModel
    {
        [Required]
        public string Message { get; set; }
        [Required]
        public int NotificationTypeId { get; set; }
        [Required]
        public NotificationType NotificationType { get; set; }
        [Required]
        public long Id { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsDelted { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsVisible { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; } 
        [Required]
        public DateTime UpdatedDate { get; set; }

        [Required]
        public Trip Trip { get; set; }

        [Required]
        public long TripId { get; set; }

        [Required]
        public JobSite JobSite { get; set; }

        [Required]
        public long JobSiteId { get; set; }

        public string CreatedDateText { get; set; }
        public string CreatedTimeText { get; set; }

        public string NotificationImageUrl { get; set; }

        public string NotificationTypeName { get; set; }
    }
}
