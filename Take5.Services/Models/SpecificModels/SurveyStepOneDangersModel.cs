using System;
using System.Collections.Generic;
using System.Text;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Models.SpecificModels
{
    public class SurveyStepOneDangersModel
    {
        public DangerModel DangerModel { get; set; }

        public List<MeasureControlModel> measureControlModels { get; set; }
    }
}
