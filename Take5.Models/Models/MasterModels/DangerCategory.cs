using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.Entity;

namespace Take5.Models.Models.MasterModels
{
    public class DangerCategory:EntityWithIdentityId<int>
    {
        [Required]
        public string Name { get; set; }
    }
}
