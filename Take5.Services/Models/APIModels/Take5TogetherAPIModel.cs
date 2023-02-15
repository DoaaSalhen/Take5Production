using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Take5.Services.Models.APIModels
{
    public class Take5TogetherAPIModel
    {

        [Required]
        public long ParticipantDriverId { get; set; }

        [Required]
        public long WhoStartDriverId { get; set; }

        [Required]
        public string Notes { get; set; }
    }
}
