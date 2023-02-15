using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.Entity;

namespace Take5.Models.Models.MasterModels
{
    public class JobSite: Entity<long>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public string Desc { get; set; }
        [Required]
        public bool HasNetworkCoverage { get; set; }
    }
}
