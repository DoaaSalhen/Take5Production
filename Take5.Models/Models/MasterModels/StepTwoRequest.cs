using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Take5.Models.Models.MasterModels
{
    public class StepTwoRequest
    {
        [Required]
        public long Id { get; set; }

        [DefaultValue(false)]
        public bool IsDelted { get; set; }

        [DefaultValue(true)]
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        [Required]
        public long TripId { get; set; }
        [Required]
        public int Status { get; set; }
    }
}
