using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Contracts
{
    public interface IExcelService
    {
        public MemoryStream ExportTripsToExcel(List<TripJobsiteModel> models);

        public DataTable ReadExcelData(string filePath, string excelConnectionString);

        public MemoryStream ExportDriversDataToExcel(Dictionary<long, string> driverData);
    }
}
