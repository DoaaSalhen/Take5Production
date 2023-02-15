using System;
using System.Collections.Generic;
using System.Text;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Models.APIModels
{
    public class ALLSurveyModel
    {
        public List<QuestionAnswerModel> StepOneQuestions { get; set; }

        public List<QuestionAnswerModel> StepTwoQuestions { get; set; }

        public List<DangerControlsWithCategoryAPIModel> DangerWithCategoryAPIModels { get; set; }

    }
}
