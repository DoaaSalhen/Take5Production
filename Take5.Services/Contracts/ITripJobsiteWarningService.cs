using System;
using System.Collections.Generic;
using System.Text;
using Take5.Models.Models.MasterModels;
using Take5.Services.Models;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Contracts
{
    public interface ITripJobsiteWarningService
    {
        public bool CreateTripJobsiteWarning(TripJobsiteWarning tripJobsiteWarning);

        public List<TripJobsiteWarningModel> GetTripJobsiteWarning(long tripNumber, long jobsiteId);

        public bool GetAllTripWarning();

        public double CalculateDifference(DateTime time1, DateTime time2);
        public bool AddNewTripJobsiteWarning(long tripNumber, long jobsiteId, int warningType, double diff);



    }
}
