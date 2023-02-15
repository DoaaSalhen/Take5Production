using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.MasterModels;

namespace Take5.Services.Models.MasterModels
{
    public class TripCancellationModel
    {
        [Required]
        public string UserId { get; set; }

        public string Reason { get; set; }

        [Required]
        public long TripId { get; set; }

        [Required]
        public Trip Trip { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public string EmployeeName { get; set; }

        public long EmployeeNumber { get; set; }

    }
}
