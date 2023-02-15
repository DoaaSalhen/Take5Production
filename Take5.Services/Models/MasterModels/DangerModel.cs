using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.MasterModels;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Take5.Services.Models.MasterModels
{
    public class DangerModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int DangerCategoryId { get; set; }
        public DangerCategory DangerCategory { get; set; }
        public string Icon { get; set; }

        [Required]
        [NotMapped]
        public IFormFile Iconfile { get; set; }
       // public int step { get; set; }
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

        public List<DangerCategoryModel> DangerCategoryModels { get; set; }

        public List<MeasureControlModel> measureControlModels { get; set; }

        public List<long> SelectedMeasureControlIds { get; set; }
    }
}
