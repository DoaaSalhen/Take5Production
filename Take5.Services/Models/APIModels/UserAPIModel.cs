using System;
using System.Collections.Generic;
using System.Text;

namespace Take5.Services.Models.APIModels
{
    public class UserAPIModel
    {
        public string UserId { get; set; }
        public long DriverId { get; set; }
        public string DriverName { get; set; }

        public int UserUnSeenNotificationCount { get; set; }
    }
}
