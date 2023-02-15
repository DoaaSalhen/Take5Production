using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Take5.Models.Models.MasterModels;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Contracts
{
    public interface IDriverService
    {
        List<DriverModel> GetAllDrivers();
        Task<bool> CreateDriver(DriverModel model);
        Task<bool> UpdateDriver(DriverModel model);
        bool DeleteDriver(long id);
        DriverModel GetDriver(long id);
        Driver GetDriverByUserId(string userId);

        List<DriverAPIModel> GetAllDriversForMobile();

        List<DriverModel> GetAllActiveDrivers();

        DriverModel GetDriverModelByUserId(string userId);
        public string CreateRandomPassword(int length);

    }
}
