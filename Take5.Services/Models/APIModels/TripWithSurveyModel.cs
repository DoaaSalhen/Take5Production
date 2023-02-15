using System;
using System.Collections.Generic;
using System.Text;

namespace Take5.Services.Models.APIModels
{
    public class TripWithSurveyModel
    {
        public ALLSurveyModel ALLSurveyModel { get; set; }

        public TripAPIModel TripAPIModel { get; set; }
        public List<DriverAPIModel> Drivers { get; set; }

    }
}
