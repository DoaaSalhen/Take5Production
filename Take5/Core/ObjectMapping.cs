using Take5.Models.Models.MasterModels;
using Take5.Services.Models.APIModels;

namespace Take5.Core
{
    public class ObjectMapping
    {
        public static bool VaildateAllTripSteps(AllTripSteps allTripSteps)
        {
            if (allTripSteps != null)
            {
                if (!StringExtensions.IsValid(allTripSteps.TruckNumber))
                    return false;
                if (!StringExtensions.IsValid(allTripSteps.EndStatus))
                    return false;
                return true;
            }
            return false;
        }
     
    }
}
