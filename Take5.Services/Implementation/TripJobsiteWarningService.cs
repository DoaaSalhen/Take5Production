using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Take5.Models.Models.MasterModels;
using Take5.Services.Contracts;
using Take5.Services.Models;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Implementation
{
    public class TripJobsiteWarningService : ITripJobsiteWarningService
    {
        private readonly IRepository<TripJobsiteWarning, long> _repository;
        private readonly ILogger<TripJobsiteWarningService> _logger;
        private readonly IMapper _mapper;

        public TripJobsiteWarningService(IRepository<TripJobsiteWarning, long> repository,
            ILogger<TripJobsiteWarningService> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public bool CreateTripJobsiteWarning(TripJobsiteWarning tripJobsiteWarning)
        {
            try
            {
                tripJobsiteWarning.CreatedDate = DateTime.Now;
                tripJobsiteWarning.UpdatedDate = DateTime.Now;
                tripJobsiteWarning.IsDelted = false;
                tripJobsiteWarning.IsVisible = true;
                var addedTripJobsiteWarning = _repository.Add(tripJobsiteWarning);
                if(addedTripJobsiteWarning != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool GetAllTripWarning()
        {
            throw new NotImplementedException();
        }

        public double CalculateDifference(DateTime time1, DateTime time2)
        {
            try
            {
                double diff = (time1 - time2).TotalMinutes;
                if(diff < CommanData.NormalDifferenceBetweenStep1AndStep2)
                {
                    return Math.Floor(diff);
                }
                else
                {
                    return 0;
                }
            }
            catch(Exception e)
            {
                return 0;
            }
        }

        public List<TripJobsiteWarningModel> GetTripJobsiteWarning(long tripNumber, long jobsiteId)
        {
            try
            {
                List<TripJobsiteWarningModel> tripJobsiteWarningModels = new List<TripJobsiteWarningModel>();
               var warnings = _repository.Find(tw => tw.IsVisible == true && tw.TripJobsiteTripId == tripNumber && tw.TripJobsiteJobSiteId == jobsiteId, false, tw=>tw.WarningType);
                if (warnings != null)
                {
                    tripJobsiteWarningModels = _mapper.Map<List<TripJobsiteWarningModel>>(warnings);
                }
                    return tripJobsiteWarningModels;

            }
            catch(Exception e)
            {

            }
            return null;
        }

        public bool AddNewTripJobsiteWarning(long tripNumber, long jobsiteId, int warningType, double diff)
        {
            try
            {
                string message = "";
                bool result = false;
                if(warningType == (int)CommanData.WarningTypes.StepTwoRequestWarning)
                {
                    message = "Step2 request sent after " + diff + " minutes from step one complation";
                }
                else if(warningType == (int)CommanData.WarningTypes.StepTwoResponseWarning)
                {
                    message = "Step2 request approved after " + diff + " minutes from step one complation";
                }
                else if (warningType == (int)CommanData.WarningTypes.StepTwoCompletionWarning)
                {
                    message = "Step2 completed after " + diff + " minutes from step one complation";
                }

                TripJobsiteWarning tripJobsiteWarning = new TripJobsiteWarning();
                tripJobsiteWarning.TripJobsiteJobSiteId = jobsiteId;
                tripJobsiteWarning.TripJobsiteTripId = tripNumber;
                tripJobsiteWarning.WarningTypeId = warningType;
                tripJobsiteWarning.Message = message;
                result = CreateTripJobsiteWarning(tripJobsiteWarning);
                return result;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
