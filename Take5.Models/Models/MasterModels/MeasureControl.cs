using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.Entity;

namespace Take5.Models.Models.MasterModels
{
    public class MeasureControl: Entity<long>
    {
        [Required]
        public string Name { get; set; }
        public int DangerId { get; set; }
        [Required]
        public Danger Danger { get; set; }


    }
}
