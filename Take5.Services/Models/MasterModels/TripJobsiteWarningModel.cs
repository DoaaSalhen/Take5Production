using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.MasterModels;

namespace Take5.Services.Models.MasterModels
{
    public class TripJobsiteWarningModel
    {
        [Required]
        public int WarningTypeId { get; set; }

        [Required]
        public WarningType WarningType { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
