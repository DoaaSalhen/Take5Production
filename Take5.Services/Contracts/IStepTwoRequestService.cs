using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Contracts
{
    public interface IStepTwoRequestService
    {
        List<StepTwoRequestModel> GetAllStepTwoRequests();
        Task<bool> CreateStepTwoRequest(StepTwoRequestModel model);
        Task<bool> UpdateStepTwoRequest(StepTwoRequestModel model);
        bool DeleteStepTwoRequest(int id);
        StepTwoRequestModel GetStepTwoRequest(int id);
    }
}
