using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Contracts
{
    public interface ITruckService
    {
        List<TruckModel> GetAllTrucks();
        Task<bool> CreateTruck(TruckModel model);
        Task<bool> UpdateTruck(TruckModel model);
        bool DeleteTruck(string id);
        TruckModel GetTruck(string id);
        List<TruckModel> GetAllActiveTrucks();

    }
}
