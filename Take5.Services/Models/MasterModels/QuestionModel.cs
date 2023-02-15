using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Take5.Services.Models.MasterModels
{
    public class QuestionModel
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public int Step { get; set; }
        [Required]
        public int Id { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool IsDelted { get; set; }

        [DefaultValue(true)]
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; } 
        public DateTime UpdatedDate { get; set; } 
    }
}
