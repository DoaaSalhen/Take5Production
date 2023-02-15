using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.Entity;

namespace Take5.Models.Models.MasterModels
{
    public class Trip: Entity<long>
    {
        [Required]
        public long DriverId { get; set; }

        [Required]
        public Driver Driver { get; set; }
        [Required]
        public string TruckId { get; set; }
        public Truck Truck { get; set; }
        [Required]
        public DateTime TripDate { get; set; }
        [Required]
        public bool IsConverted { get; set; }
        [Required]
        public int Take5Status { get; set; }
        [Required]
        public int TripStatus { get; set; }

        [Required]
        public bool Cancelled { get; set; }

    }
}
