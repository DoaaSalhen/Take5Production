using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Take5.Models.Models.MasterModels;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.MasterModels;
using Take5.Services.Models.SpecificModels;

namespace Take5.Services.Contracts
{
    public interface ISurveyService
    {
        public SurveyStepOneModel LoadSurveyStepOne();

        public SurveyStepTwoModel LoadSurveyStepTwo();

        Task<bool> AddDangersToSurveyStepOne(SurveyStepOneAnswersAPIModel model, long TripId, long JobSiteId);
        TripSurveyModel LoadSurveyQuestionAnswersForTrip(long tripNumber, long jobsiteId);
        List<SurveyQuestionAnswersModel> MapFromTripQuestionToSurveyQuestionAnswersModel(List<TripQuestion> tripQuestions);
        bool DeleteTripQuestion(long tripNumber, long jobsiteId, int step);
        bool DeleteTripDangers(long tripNumber, long jobsiteId);

         ALLSurveyModel getAllSurvey();
        bool DeleteTake5StepOneForTripJobsite(long tripNumber, long jobsiteId);
        Task<bool> AddAnswersToStepNQuestions(SurveyStepOneAnswersAPIModel model, long TripId, long JobSiteId, int step);
        bool DeleteTake5StepTwoForTripJobsite(long tripNumber, long JobsiteId);

        List<SurveyStepOneDangersModel> LoadSurveyDangersForTrip(long tripNumber, long jobsiteId);
    }
}
