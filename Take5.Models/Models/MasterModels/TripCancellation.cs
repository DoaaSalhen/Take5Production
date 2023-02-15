using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.MasterModels;

namespace Take5.Models.Models.MasterModels
{
    public class TripCancellation
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long TripId { get; set; }

        public Trip Trip { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Reason { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}