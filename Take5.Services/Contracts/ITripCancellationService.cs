using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Contracts
{
    public interface ITripCancellationService
    {
        Task<bool> CreateTripCancellation(TripCancellationModel tripCancellationModel);

        Task<TripCancellationModel> GetTripCancellation(long TripId);

    }
}
