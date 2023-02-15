using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.Entity;

namespace Take5.Models.Models.MasterModels
{
    public class Danger:EntityWithIdentityId<int>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int DangerCategoryId { get; set; }

        public DangerCategory DangerCategory { get; set; }
        [Required]
        public string Icon { get; set; }

      //  public int step { get; set; }
    }
}
