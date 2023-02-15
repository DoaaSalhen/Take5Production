using System;
using System.Collections.Generic;
using System.Text;
using Take5.Models.Models.MasterModels;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Contracts
{
    public interface ITripTake5TogetherService
    {
        bool CreateTripTake5Together(TripTake5Together tripTake5Together);

        bool AddTripTake5TogetherForTrip(AllTripSteps allTripSteps);

        List<TripTake5TogetherModel> GetTripTake5TogetherForTrip(long tripNumber, long jobsiteId);

    }
}
