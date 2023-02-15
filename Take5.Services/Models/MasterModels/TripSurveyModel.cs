using System;
using System.Collections.Generic;
using System.Text;
using Take5.Models.Models.MasterModels;
using Take5.Services.Models.SpecificModels;

namespace Take5.Services.Models.MasterModels
{
    public class TripSurveyModel
    {
        public TripJobsiteModel TripJobsiteModel { get; set; }
        public List<SurveyQuestionAnswersModel> SurveyStepOneAnswersModels { get; set; }

        public List<SurveyStepOneDangersModel> SurveyStepOneDangersModels { get; set; }

        public List<SurveyQuestionAnswersModel> SurveyStepTwoAnswersModels { get; set; }

        public List<TripTake5TogetherModel> tripTake5TogetherModels { get; set; }

    }
}
