using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.MasterModels;

namespace Take5.Services.Models.MasterModels
{
    public class UserNotificationModel
    {
        [Required]
        public string userId { get; set; }

        [Required]
        public long NotificationId { get; set; }
        [Required]
        public Notification Notification { get; set; }

        [DefaultValue(false)]
        public bool IsDelted { get; set; }

        [DefaultValue(true)]
        public bool IsVisible { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        [Required]
        public bool Seen { get; set; }
        public string NotificationImageUrl { get; set; }

    }
}
