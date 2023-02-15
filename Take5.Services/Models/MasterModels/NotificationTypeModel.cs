using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Take5.Services.Models.MasterModels
{
    public class NotificationTypeModel
    {
        public int Id { get; set; }

        [DefaultValue(false)]
        public bool IsDelted { get; set; }

        [DefaultValue(true)]
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; } 

        [Required]
        public string Name { get; set; }

        [Required]
        public string ImgUrl { get; set; }
    }
}
