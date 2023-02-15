using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.Entity;

namespace Take5.Models.Models.MasterModels
{
    public class Driver:Entity<long>
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string UserId { get; set; }

        [Required]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        public string UserToken { get; set; }

    }
}
