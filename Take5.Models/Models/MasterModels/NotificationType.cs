using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.Entity;

namespace Take5.Models.Models.MasterModels
{
    public class NotificationType:Entity<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ImgUrl { get; set; }
    }
}
