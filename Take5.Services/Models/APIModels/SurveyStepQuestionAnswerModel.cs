using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Take5.Services.Models.APIModels
{
    public class SurveyStepQuestionAnswerModel
    {
        //[Required]
        //public long TripId { get; set; }
        //[Required]
        //public long JobSiteId { get; set; }

        [Required]
        public List<QuestionAnswerModel> QuestionAnswerModels { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
        //[Required]
        //public string UserId { get; set; }
    }
}
