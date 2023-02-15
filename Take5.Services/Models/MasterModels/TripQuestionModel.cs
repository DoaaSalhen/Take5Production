using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Take5.Models.Models.MasterModels;

namespace Take5.Services.Models.MasterModels
{
    public class TripQuestionModel
    {
        public long TripId { get; set; }
        public Trip Trip { get; set; }

        public int QuestionId { get; set; }

        public Question Question { get; set; }
        [Required]
        public bool Answer { get; set; }

        [DefaultValue(false)]
        public bool IsDelted { get; set; }

        [DefaultValue(true)]
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
