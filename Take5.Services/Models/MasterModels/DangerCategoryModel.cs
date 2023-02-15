using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Take5.Services.Models.MasterModels
{
    public class DangerCategoryModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Id { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool IsDelted { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsVisible { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; } 
        [Required]
        public DateTime UpdatedDate { get; set; } 

        public List<DangerModel> DangerModels { get; set; }
    }
}
