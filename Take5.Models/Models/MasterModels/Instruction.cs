using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.Entity;

namespace Take5.Models.Models.MasterModels
{
    public class Instruction:Entity<int>
    {
        [Required]
        public string text { get; set; }

        public int Step { get; set; }
    }
}
