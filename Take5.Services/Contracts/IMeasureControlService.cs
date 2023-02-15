using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Take5.Models.Models.MasterModels;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Contracts
{
    public interface IMeasureControlService
    {
        List<MeasureControlModel> GetAllMeasureControls();
        Task<bool> CreateMeasureControl(MeasureControlModel model);
        Task<bool> UpdateMeasureControl(MeasureControlModel model);
        bool DeleteMeasureControl(int id);
        List<MeasureControlModel> GetMeasureControlsByDangerId(long dangerId);
        bool DeleteMeasureControlFordanger(int id);

        List<MeasureControlAPI> MapMeasureControlModelToMeasureControlAPI(List<MeasureControlModel> measureControls);

    }
}
