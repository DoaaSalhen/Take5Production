using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Take5.Services.Models.APIModels
{
    public class SurveyStepOneAnswersAPIModel: SurveyStepQuestionAnswerModel
    {
        
        public List<DangerAPI> DangerAPIs { get; set; }

    }



}
