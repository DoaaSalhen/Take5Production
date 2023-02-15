using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Take5.Services.Models.APIModels
{
    public class AllTripSteps
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public long TripId { get; set; }

        [Required]
        public long JobsiteId { get; set; }

        [Required]
        public string TruckNumber { get; set; }
        public TripDestinationArrivedModel TripDestinationArrivedModel { get; set; }

        public SurveyStepOneAnswersAPIModel SurveyStepOneAnswersAPIModel { get; set; }

        public StepTwoStartRequest Take5StepTwoRequestAPIModel { get; set; }

        public SurveyStepOneAnswersAPIModel SurveyStepTwoAnswersAPIModel { get; set; }

        public List<Take5TogetherAPIModel> Take5TogetherAPIModels { get; set; }

        public string EndStatus { get; set; }
    }
}
