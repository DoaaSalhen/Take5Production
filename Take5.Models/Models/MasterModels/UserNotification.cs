﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Take5.Models.Models.MasterModels
{
    public class UserNotification
    {
        [Required]
        public string userId { get; set; }

        [Required]
        public long NotificationId { get; set; }
        [Required]
        public Notification Notification { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool IsDelted { get; set; }
        [Required]
        [DefaultValue(true)]
        public bool IsVisible { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        [Required]
        public bool Seen { get; set; }
    }
}
