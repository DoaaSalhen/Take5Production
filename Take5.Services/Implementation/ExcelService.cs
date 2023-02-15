using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using Take5.Services.Contracts;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Implementation
{
    public class ExcelService:IExcelService
    {
        private readonly ILogger<ExcelService> _logger;

        public ExcelService(ILogger<ExcelService> logger)
        {
            _logger = logger;
        }

        public MemoryStream ExportTripsToExcel(List<TripJobsiteModel> models)
        {
            try
            {
                string StartingDate = "";
                string StartingTime = "";
                string ArrivalDate = "";
                string ArrivalTime = "";
                string StepOneDate = "";
                string StepOneTime = "";
                string StepTwoRequestDate = "";
                string StepTwoRequestTime = "";
                string StepTwoResponseDate = "";
                string StepTwoResponseTime = "";
                string StepTwoCompletionDate = "";
                string StepTwoCompletionTime = "";
                string CancelledDate = "";
                string CancelledByName = "";
                string CancelledByNumber = "";
                string CancellationReason = "";
                bool TripCompleted;
                DataTable dt = new DataTable("Grid");
                dt.Columns.AddRange(new DataColumn[29] {
                                            new DataColumn("Trip Date"),
                                            new DataColumn("Number"),
                                            new DataColumn("Jobsite Number"),
                                            new DataColumn("Jobsite Name"),
                                            new DataColumn("Driver Number"),
                                            new DataColumn("Driver Name"),
                                            new DataColumn("Driver Phone Number"),
                                            new DataColumn("Truck"),
                                            new DataColumn("Starting Date"),
                                            new DataColumn("Starting Time"),
                                            new DataColumn("Arriving Date"),
                                            new DataColumn("Arriving Time"),
                                            new DataColumn("Take5 step1 Date"),
                                            new DataColumn("Take5 step1 Time"),
                                            new DataColumn("Take5 step2 Request Date"),
                                            new DataColumn("Take5 step2 Request Time"),
                                            new DataColumn("Take5 step2 Response Date"),
                                            new DataColumn("Take5 step2 Response Time"),
                                            new DataColumn("Who Responsed"),
                                            new DataColumn("Take5 step2 Date"),
                                            new DataColumn("Take5 step2 Time"),
                                            new DataColumn("Trip Status"),
                                            new DataColumn("Take5 Status"),
                                            new DataColumn("Converted"),
                                            new DataColumn("Type"),
                                            new DataColumn("CancellationDate"),
                                            new DataColumn("CancelledByNumber"),
                                            new DataColumn("CancelledByName"),
                                            new DataColumn("CancellationReason"),});


                foreach (var trip in models)
                {

                    if (trip.StartingDate.HasValue)
                    {
                        StartingDate = trip.StartingDate.Value.ToString("yyyy-MM-dd");
                        StartingTime = trip.StartingDate.Value.ToString("HH-mm-ss");
                    }
                    else
                    {
                        StartingDate = StartingTime = null;
                    }
                    if (trip.DestinationArrivingDate.HasValue)
                    {
                        ArrivalDate = trip.DestinationArrivingDate.Value.ToString("yyyy-MM-dd");
                        ArrivalTime = trip.DestinationArrivingDate.Value.ToString("HH-mm-ss");
                    }
                    else
                    {
                        ArrivalDate = ArrivalTime = null;
                    }

                    if (trip.StageOneComplatedTime.HasValue)
                    {
                        StepOneDate = trip.StageOneComplatedTime.Value.ToString("yyyy-MM-dd");
                        StepOneTime = trip.StageOneComplatedTime.Value.ToString("HH-mm-ss");
                    }
                    else
                    {
                        StepOneDate = StepOneTime = null;
                    }

                    if (trip.StageTwoRequestDate.HasValue)
                    {
                        StepTwoRequestDate = trip.StageTwoRequestDate.Value.ToString("yyyy-MM-dd");
                        StepTwoRequestTime = trip.StageTwoRequestDate.Value.ToString("HH-mm-ss");
                    }
                    else
                    {
                        StepTwoRequestDate = StepTwoRequestTime = null;
                    }

                    if (trip.StageTwoResponseDate.HasValue)
                    {
                        StepTwoResponseDate = trip.StageTwoResponseDate.Value.ToString("yyyy-MM-dd");
                        StepTwoResponseTime = trip.StageTwoResponseDate.Value.ToString("HH-mm-ss");
                    }
                    else
                    {
                        StepTwoResponseDate = StepTwoResponseTime = null;
                    }

                    if (trip.StageTwoComplatedTime.HasValue)
                    {
                        StepTwoCompletionDate = trip.StageTwoComplatedTime.Value.ToString("yyyy-MM-dd");
                        StepTwoCompletionTime = trip.StageTwoComplatedTime.Value.ToString("HH-mm-ss");
                    }
                    else
                    {
                        StepTwoCompletionDate = StepTwoCompletionTime = null;
                    }
                    if(trip.JobSite.HasNetworkCoverage == true)
                    {
                        trip.TripType = "Online";
                    }
                    else
                    {
                        trip.TripType = "Offline";
                    }

                    if(trip.Trip.Cancelled != true)
                    {
                        dt.Rows.Add(trip.Trip.TripDate.Date, trip.TripId, trip.JobSiteId, trip.JobSite.Name, trip.Trip.Driver.Id, trip.Trip.Driver.FullName,
                         trip.Trip.Driver.PhoneNumber, trip.Trip.TruckId, StartingDate, StartingTime,
                          ArrivalDate, ArrivalTime, StepOneDate, StepOneTime, StepTwoRequestDate, StepTwoRequestTime, StepTwoResponseDate,
                          StepTwoResponseTime, trip.RequestResponsedBy, StepTwoCompletionDate, StepTwoCompletionTime, trip.TripStatus, trip.Take5Status,
                          trip.Trip.IsConverted, trip.TripType);
                    }
                    else
                    {
                        dt.Rows.Add(trip.Trip.TripDate.Date, trip.TripId, trip.JobSiteId, trip.JobSite.Name, trip.Trip.Driver.Id, trip.Trip.Driver.FullName,
                        trip.Trip.Driver.PhoneNumber, trip.Trip.TruckId, StartingDate, StartingTime,
                         ArrivalDate, ArrivalTime, StepOneDate, StepOneTime, StepTwoRequestDate, StepTwoRequestTime, StepTwoResponseDate,
                         StepTwoResponseTime, trip.RequestResponsedBy, StepTwoCompletionDate, StepTwoCompletionTime, trip.TripStatus, trip.Take5Status,
                         trip.Trip.IsConverted, trip.TripType, trip.TripCancellationModel.CreatedDate, 
                         trip.TripCancellationModel.EmployeeNumber, trip.TripCancellationModel.EmployeeName,
                         trip.TripCancellationModel.Reason);
                    }

                }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return stream;
                    }
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }


        public DataTable ReadExcelData(string filePath, string excelConnectionString)
        {
            try
            {
                DataTable dt = new DataTable();
                excelConnectionString = string.Format(excelConnectionString, filePath);

                using (OleDbConnection connExcel = new OleDbConnection(excelConnectionString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();
                        }
                    }
                }
                return dt;
            }
            catch (Exception e)
            {

            }
            return null;
        }


        public MemoryStream ExportDriversDataToExcel(Dictionary<long, string> driverData)
        {
            try
            {
                DataTable dt = new DataTable("DriverData");
                dt.Columns.AddRange(new DataColumn[2] {
                                            new DataColumn("Driver Number"),
                                            new DataColumn("Password"),});


                foreach (var driver in driverData)
                {
                    dt.Rows.Add(driver.Key, driverData[driver.Key]);
                }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return stream;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

    }
}
