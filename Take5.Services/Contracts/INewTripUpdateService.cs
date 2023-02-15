using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Take5.Services.Models.APIModels;

namespace Take5.Services.Contracts
{
    public interface INewTripUpdateService
    {
        public Task<string> StartTrip(TripStartingModel tripStartingModel);

        public Task<string> TripDestinationArrived(TripDestinationArrivedModel tripDestinationArrivedModel, long tripId, long jobsiteId);

        public Task<string> AddStepOneAnswers(SurveyStepOneAnswersAPIModel model, string userId);

        


    }
}
