using System;
using System.Collections.Generic;
using System.Text;

namespace Take5.Services.Models.MasterModels
{
    public class SurveyStepOneModel
    {
        public List<QuestionModel> QuestionModels { get; set; }

        public List<DangerModel> DangerModels { get; set; }

        public List<DangerCategoryModel> DangerCategoryModels { get; set; }

        public List<MeasureControlModel> MeasureControlModels { get; set; }
    }
}
