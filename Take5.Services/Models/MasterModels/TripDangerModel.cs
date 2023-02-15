using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.MasterModels;

namespace Take5.Services.Models.MasterModels
{
    public class TripDangerModel
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


        [Required]
        public bool IsDelted { get; set; }

        [Required]

        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        public DangerModel DangerModel { get; set; }
    }
}
