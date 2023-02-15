using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Take5.Services.Contracts;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Implementation
{
    public class StepTwoRequestService : IStepTwoRequestService
    {
        public Task<bool> CreateStepTwoRequest(StepTwoRequestModel model)
        {
            throw new NotImplementedException();
        }

        public bool DeleteStepTwoRequest(int id)
        {
            throw new NotImplementedException();
        }

        public List<StepTwoRequestModel> GetAllStepTwoRequests()
        {
            throw new NotImplementedException();
        }

        public StepTwoRequestModel GetStepTwoRequest(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStepTwoRequest(StepTwoRequestModel model)
        {
            throw new NotImplementedException();
        }
    }
}
