using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Take5.Models.Models;
using Take5.Models.Models.MasterModels;
using Take5.Services.Contracts;
using Take5.Services.Implementation;
using Take5.Services.Models;
using Take5.Services.Models.MasterModels;

namespace Take5.Controllers
{
    [RestrictDomain("localhost", "example.com")]
    [Authorize(Roles = "Supervisor, Admin")]
    public class TripController : BaseController
    {
        private readonly ITruckService _truckService;
        private readonly IJobSiteService _jobsiteService;
        private readonly IDriverService _driverService;
        private readonly ITripService _tripService;
        private readonly INotificationService _notificationService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly ISurveyService _surveyService;
        private readonly ITripJobsiteService _tripJobsiteService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IExcelService _excelService;
        private readonly ITripJobsiteWarningService _tripJobsiteWarningService;
        private readonly ITripCancellationService _tripCancellationService;
        private readonly IEmployeeService _employeeService;
        private readonly IFirebaseNotificationService _firebaseNotificationService;

        public TripController(ITruckService truckService, 
                              IJobSiteService jobsiteService,
                              IDriverService driverService,
                              ITripService tripService,
                              INotificationService notificationService,
                              IUserNotificationService userNotificationService,
                              ISurveyService surveyService,
                              ITripJobsiteService tripJobsiteService,
                              UserManager<AspNetUser> userManager,
                              IExcelService excelService,
                              ITripJobsiteWarningService tripJobsiteWarningService,
                              ITripCancellationService tripCancellationService,
                              IEmployeeService employeeService,
                              IFirebaseNotificationService firebaseNotificationService)
        {
            _driverService = driverService;
            _truckService = truckService;
            _jobsiteService = jobsiteService;
            _tripService = tripService;
            _notificationService = notificationService;
            _userNotificationService = userNotificationService;
            _surveyService = surveyService;
            _tripJobsiteService = tripJobsiteService;
            _userManager = userManager;
            _excelService = excelService;
            _tripJobsiteWarningService = tripJobsiteWarningService;
            _tripCancellationService = tripCancellationService;
            _employeeService = employeeService;
            _firebaseNotificationService = firebaseNotificationService;
        }

        public long IsDriverAvaliable(long id)
        {
           TripModel trip = _tripService.GetPendingAndUnCompletedTripForDriver(id);
            if(trip != null)
            {
                return trip.Id;
            }
            else
            {
                return 0;
            }
        }

        public long IsTruckAvaliable(string truckId)
        {
            TripModel trip = _tripService.GetPendingAndUnCompletedTripForTruck(truckId);
            if (trip != null)
            {
                return trip.Id;
            }
            else
            {
                return 0;
            }
        }

        public bool CheckTripNumberIdentity(long tripId)
        {
            TripModel tripModel = _tripService.GetTrip(tripId);
            if(tripModel != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public string getTripDetails(long tripId, long jobsiteId)
        {
            try
            {
               TripJobsiteModel tripJobsiteModel = _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteId(tripId, jobsiteId);
                if(tripJobsiteModel != null)
                {
                    if (tripJobsiteModel.Trip.Cancelled == true)
                    {
                        tripJobsiteModel.TripCancellationModel = _tripCancellationService.GetTripCancellation(tripJobsiteModel.TripId).Result;
                    }
                    if (tripJobsiteModel.RequestStatus == (int)CommanData.RequestStatus.Approved )
                    {
                        if(tripJobsiteModel.RequestResponsedBy != "System")
                        {
                            Employee employee = _employeeService.GetEmployeeByUserId(tripJobsiteModel.RequestResponsedBy);
                            tripJobsiteModel.RequestResponsedByName = employee.EmployeeName;
                            tripJobsiteModel.RequestResponsedByNumber = employee.EmployeeNumber;
                        }
                        else
                        {
                            tripJobsiteModel.RequestResponsedByName = "System";
                            tripJobsiteModel.RequestResponsedByNumber = 0;
                        }
                    }

                    if(tripJobsiteModel.TripStatus >= (int)CommanData.TripStatus.StepTwoRequested && tripJobsiteModel.TripStatus <= (int)CommanData.TripStatus.TripCompleted)
                    {
                       var warnings = _tripJobsiteWarningService.GetTripJobsiteWarning(tripId, jobsiteId);
                        if(warnings.Count > 0)
                        {
                            tripJobsiteModel.TripJobsiteWarningModels = warnings;
                        }
                    }
                    tripJobsiteModel = _tripJobsiteService.SetTripStatuesAndDatesAsText(tripJobsiteModel);
                    return JsonSerializer.Serialize(tripJobsiteModel);
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                return null;
            }
           
        }
        // GET: TripController
        public ActionResult Index()
        {
            var trips = _tripJobsiteService.GetAllJobsiteTrips();
            ViewBag.Message = TempData["Message"];
            return View(trips);
        }

        // GET: TripController/Details/5
        public ActionResult Details(long tripId, long jobsiteId)
        {
            try
            {
                TripJobsiteModel tripJobsiteModel = _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteId(tripId, jobsiteId);
                if (tripJobsiteModel != null)
                {
                    tripJobsiteModel = _tripJobsiteService.SetTripStatuesAndDatesAsText(tripJobsiteModel);
                    return View(tripJobsiteModel);
                }
                return View();
            }
            catch(Exception e)
            {
                return View();
            }
           
        }

        // GET: TripController/Create
        public ActionResult Create()
        {
            try
            {
                TripModel model = new TripModel();
                model.Trucks = _truckService.GetAllActiveTrucks().ToList();
                //model.Trucks.Insert(0, new TruckModel { Id = "select Truck" });
                model.JobSites = _jobsiteService.GetAllActiveJobsites().ToList();
                var drivers = _driverService.GetAllActiveDrivers().ToList();
                model.Drivers = drivers;
                model.TripDate = DateTime.Now;
                return View(model);
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // POST: TripController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TripModel model)
        {
            try
            {
                model.Take5Status = (int)CommanData.Take5Status.NotStarted;
                model.TripStatus = (int)CommanData.TripStatus.Pending;
                bool isTripAdded = _tripService.CreateTrip(model).Result;
                
                if (isTripAdded == true)
                {
                    TempData["Message"] = "Success Process! trip has been added!";
                    return RedirectToAction("Create");
                }
                else
                {
                    TempData["Error"] = "Failed Process, Can not create trip";
                    return RedirectToAction("Create");
                }
            }
            catch
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: TripController/Edit/5

        public ActionResult Edit(long TripNumber)
        {
            try
            {
                TripModel model = _tripService.GetTrip(TripNumber);
                model.Trucks = _truckService.GetAllActiveTrucks().ToList();
                model.Trucks.Insert(0, new TruckModel { Id = "" });
                model.JobSites = _jobsiteService.GetAllActiveJobsites().ToList();
                var drivers = _driverService.GetAllActiveDrivers().ToList();
                var tripJobsite = _tripJobsiteService.GetTripJobsiteModelByTripNumber(TripNumber).Last();
                if(tripJobsite != null)
                {
                    model.JobSiteId = tripJobsite.JobSiteId;
                }
                model.Drivers = drivers;
                return View(model);
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // POST: TripController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TripModel model)
        {
            try
            {
                bool result = false;
                string message = "";
                TripModel trip = _tripService.GetTrip(model.Id);
                trip.UpdatedDate = DateTime.Now;
                trip.DriverId = model.DriverId;
                trip.TruckId = model.TruckId;
                trip.TripDate = model.TripDate;
                
                bool updateResult = _tripService.UpdateTrip(trip).Result;
                if (updateResult == true)
                {
                   var tripJobsite = _tripJobsiteService.GetTripJobsiteModelByTripNumber(trip.Id).Last();
                    //if (model.JobSiteId != tripJobsite.JobSiteId)
                    //{
                        result = true;//_tripService.UpdateJobsiteForTrip(model).Result;
                        if(result == true)
                        {
                            if (model.DriverId == trip.DriverId)
                            {
                                message = "تم تعديل الرحلة " + model.Id + " الخاصة بك";
                                Notification notification = _notificationService.CreateNotification(message, (int)CommanData.NotificationTypes.TripEditing, model.Id, model.JobSiteId);
                                UserNotificationModel userNotificationModel = _userNotificationService.CreateUserNotification(notification.Id, trip.Driver.UserId).Result;
                            }
                            else if (model.DriverId != trip.DriverId)
                            {
                                message = "تم الغاء الرحلة " + model.Id + "الخاصة بك";

                                Notification notification = _notificationService.CreateNotification(message, (int)CommanData.NotificationTypes.TripCancelling, model.Id, model.JobSiteId);
                                UserNotificationModel userNotificationModel = _userNotificationService.CreateUserNotification(notification.Id, trip.Driver.UserId).Result;

                                TripModel UpdatedTrip = _tripService.GetTrip(model.Id);
                                message = "تم اضافة رحلة جديدة لك رقم " + model.Id;
                                notification = _notificationService.CreateNotification(message, (int)CommanData.NotificationTypes.NewTripAssigned, model.Id, model.JobSiteId);
                                userNotificationModel = _userNotificationService.CreateUserNotification(notification.Id, UpdatedTrip.Driver.UserId).Result;
                            }

                            TempData["Message"] = "Trip are updated Successfully";
                            return RedirectToAction(nameof(SearchTrip));
                        }
                        //else
                        //{
                        //  ViewBag.Error = "Faield to edit jobsite for this trip";
                        //}
                    //}
                    //else
                    //{
                    //    TempData["Message"] = "Trip are updated Successfully";
                    //    return RedirectToAction(nameof(SearchTrip));
                    //}
                }
                else
                {
                    TempData["Error"] = "Trip are updated Successfully";
                }
                model.Trucks = _truckService.GetAllTrucks().ToList();
                model.Trucks.Insert(0, new TruckModel { Id = "" });
                model.JobSites = _jobsiteService.GetAllJobsites().ToList();
                model.Drivers = _driverService.GetAllDrivers().ToList();
                return View(model);
            }
            catch
            {
                return RedirectToAction("ERROR404");
            }
        }


        public bool Delete(long  id)
        {
            try
            {
               bool result = _tripService.DeleteTrip(id).Result;

                return result;
            }
            catch
            {
                return false;
            }
        }
        public IActionResult ConvertTrip(long TripNumber, long jobsiteId)
        {
            try
            {
                TripJobsiteModel model = _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteId(TripNumber, jobsiteId);
                if(model != null)
                {
                    model.ConvertedJobSiteId = model.JobSiteId;
                    model.Jobsites = _jobsiteService.GetAllJobsites().ToList();
                    return View(model);
                }
                else
                {
                    TempData["Error"] = "Failed process, trip doesn't exist";
                    return RedirectToAction("SearchTrip");
                }

            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConvertTrip(TripJobsiteModel model)
        {
            try
            {
                bool result = false;
                string message = "";
                TripJobsiteModel tripJobsiteModel =  _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteId(model.TripId, model.ConvertedJobSiteId);
                bool IsTripConverted = await _tripJobsiteService.ConvertTrip(model.TripId, tripJobsiteModel.JobSiteId);
                if(IsTripConverted == true)
                {
                    if(tripJobsiteModel.TripStatus >= (int) CommanData.TripStatus.SurveyStepOneCompleted)
                    {
                        result = _surveyService.DeleteTake5StepOneForTripJobsite(model.TripId, tripJobsiteModel.JobSiteId);
                    }
                    else
                    {
                        result = true;
                    }
                    if (result == true)
                    {
                        result = _tripJobsiteService.CreateTripJobsite(model.TripId, model.JobSiteId).Result;
                        if (result == true)
                        {
                            TripModel tripModelAfterReset = _tripService.ResetTrip(model.TripId);
                            JobSiteModel jobSiteModel = _jobsiteService.GetJobsite(model.JobSiteId);
                            message = "تم تغيير موقع الرحلة رقم " + model.TripId + "الي " + jobSiteModel.Name;
                            Notification notification = _notificationService.CreateNotification(message, (int)CommanData.TripStatus.TripConverted, model.TripId, model.JobSiteId);
                            if (notification != null)
                            {
                                DriverModel driverModel = _driverService.GetDriver(tripModelAfterReset.DriverId);
                                UserNotificationModel addedUserNotificationModel = _userNotificationService.CreateUserNotification(notification.Id, driverModel.UserId).Result;
                                if (addedUserNotificationModel != null)
                                {
                                    TempData["Message"] = "Trip is converted successfully";
                                    return RedirectToAction("SearchTrip");
                                }
                                else
                                {
                                    TempData["Message"] = "Trip is converted successfully, but can Send notification for driver";
                                    return RedirectToAction("SearchTrip");
                                }
                            }
                            else
                            {
                                TempData["Message"] = "Trip is converted successfully, but can Send notification";
                                return RedirectToAction("SearchTrip");
                            }
                        }
                        else
                        {
                            TempData["Error"] = "failed add new jobsite for trip";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Trip doesn't updated, failed to reset Take5 for trip";
                    }
                }
                else
                {
                    TempData["Error"] = "failed convert trip";
                }
                model.Jobsites = _jobsiteService.GetAllJobsites().ToList();
                return RedirectToAction("ConvertTrip");
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }


        public ActionResult GetTrips()
        {
            var trips = _tripService.GetTripByDate(DateTime.Today);
            if(trips != null)
            {
                return View();
            }
            else
            {
                return View();
            }
        }

        public async Task<JsonResult> ApproveStepTwoRequest(long tripNumber, long jobsiteNumber)
        {
            try
            {
              TripJobsiteModel tripJobsiteModel =  _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteId(tripNumber, jobsiteNumber);
                if(tripJobsiteModel != null)
                {
                    if (tripJobsiteModel.TripStatus == (int)CommanData.TripStatus.StepTwoRequested)
                    {
                            bool result = false;
                        
                            //double TimeDifferenceBtweenStepOneAndStepTwoResponse = (DateTime.Now - (DateTime)tripJobsiteModel.StageOneComplatedTime).TotalMinutes;
                            //if (TimeDifferenceBtweenStepOneAndStepTwoResponse < CommanData.NormalDifferenceBetweenStep1AndStep2)
                            //{
                            //    TripJobsiteWarning tripJobsiteWarning = new TripJobsiteWarning();
                            //    tripJobsiteWarning.Message = "Warning! step2 request responsed after " + TimeDifferenceBtweenStepOneAndStepTwoResponse + "Minutes" +
                            //        "from Step1 completion";
                            //    tripJobsiteWarning.WarningTypeId = (int)CommanData.WarningTypes.StepTwoCompletionWarning;
                            //    result = _tripJobsiteWarningService.CreateTripJobsiteWarning(tripJobsiteWarning);
                            //}
                            //else
                            //{
                            //    result = true;
                            //}
                                tripJobsiteModel.RequestResponsedBy = _userManager.GetUserAsync(User).Result.Id;
                                result = await _tripJobsiteService.ApproveStepTwoRequest(tripJobsiteModel);
                                TripModel tripModel =  _tripService.GetTrip(tripNumber);
                                if(result == true)
                                {
                                        tripModel.TripStatus = (int)CommanData.TripStatus.StepTwoResponsed;
                                        tripModel.UpdatedDate = DateTime.Now;
                                        result = await _tripService.UpdateTrip(tripModel);
                                        if (result == true)
                                        {
                                            double TimeDifferenceBtweenStepOneAndTwoRequest = _tripJobsiteWarningService.CalculateDifference((DateTime)tripJobsiteModel.StageTwoRequestDate, (DateTime)DateTime.Now);
                                            if (TimeDifferenceBtweenStepOneAndTwoRequest != 0)
                                            {
                                                _tripJobsiteWarningService.AddNewTripJobsiteWarning(tripJobsiteModel.TripId, tripJobsiteModel.JobSiteId, (int)CommanData.WarningTypes.StepTwoResponseWarning, TimeDifferenceBtweenStepOneAndTwoRequest);
                                            }
                                            //string token = _driverService.GetDriver(tripModel.DriverId).UserToken;
                                            //await _firebaseNotificationService.SendNotification(token, "طلب المرحلة الثانية", "تم الموافقة علي طلب بدء المرحلة الثانية");
                                            return Json("Request Approved Successfully");
                                        }
                                        else
                                        {
                                            return Json("Failed Process, Problem in trip Data");
                                        }
                                }
                                else
                                {
                                     return Json("Failed Process, Request failed to Approve");
                                }
                            
                    }
                    else if(tripJobsiteModel.TripStatus > (int)CommanData.TripStatus.StepTwoRequested)
                    {
                        return Json("Failed Process, Request already Approved, trip at " + Enum.GetName(typeof(CommanData.TripStatus), tripJobsiteModel.TripStatus) + " status");
                    }
                    else
                    {
                        return Json("Failed Process, Failed to Approve Request, trip at " + Enum.GetName(typeof(CommanData.TripStatus), tripJobsiteModel.TripStatus) + " status");

                    }
                }
                else
                {
                    return Json("Failed Process, wrong trip");
                }
            }
            catch(Exception e)
            {
                return Json("Failed Process, Failed to Approve request. Contact your support team");
            }
        }

        public IActionResult SearchTrip()
        {
            try
            {
                SearchTripModel searchTripModel = new SearchTripModel();
                searchTripModel = _tripService.InitiateTripJobsiteSearchModel(searchTripModel);
                if(searchTripModel != null)
                {
                   List<TripJobsiteModel> todayTripJobsiteModels = _tripJobsiteService.GetTodayTrips();
                    if(todayTripJobsiteModels != null)
                    {
                        todayTripJobsiteModels.ForEach(t => t.TripStatusText = Enum.GetName(typeof(CommanData.TripStatus), t.TripStatus).ToString());
                        searchTripModel.tripJobsiteModels = todayTripJobsiteModels;
                    }
                    return View(searchTripModel);
                }
                else
                {
                    TempData["Error"] = "Failed process, error in search data";
                    return RedirectToAction("ERROR404");
                }


            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchTrip(SearchTripModel searchTripModel)
        {
            try
            {
                if(searchTripModel != null)
                {
                   List<TripJobsiteModel> tripJobsiteModels = _tripJobsiteService.SearchForTrip(searchTripModel);
                    if(tripJobsiteModels != null)
                    {
                        tripJobsiteModels.ForEach(t => t.TripStatusText = Enum.GetName(typeof(CommanData.TripStatus), t.TripStatus).ToString());
                    }
                    searchTripModel.tripJobsiteModels = tripJobsiteModels;
                    searchTripModel = _tripService.InitiateTripJobsiteSearchModel(searchTripModel);
                }
                else
                {
                    ViewBag.Error = "Failed process, error in search data";
                    return RedirectToAction("SearchTrip");
                }
                if(TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"];
                }
                return View(searchTripModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        public IActionResult ExportTrip()
        {
            try
            {
                SearchTripModel searchTripModel = new SearchTripModel();
                searchTripModel = _tripService.InitiateTripJobsiteSearchModel(searchTripModel);
                if (searchTripModel != null)
                {
                    return View(searchTripModel);
                }
                else
                {
                    TempData["Error"] = "Failed process, error in search data";
                    return RedirectToAction("ERROR404");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExportTrip(SearchTripModel searchTripModel)
        {
            try
            {
                if (searchTripModel != null)
                {
                    List<TripJobsiteModel> tripJobsiteModels = _tripJobsiteService.SearchForTrip(searchTripModel);
                    if(tripJobsiteModels != null)
                    {
                       MemoryStream memoryStream =  _excelService.ExportTripsToExcel(tripJobsiteModels);
                        return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportTrips.xlsx");
                    }
                    else
                    {
                        TempData["Error"] = "Error in file exporting";
                    }
                }
                else
                {
                    TempData["Error"] = "Error in file exporting";
                }
                searchTripModel = _tripService.InitiateTripJobsiteSearchModel(searchTripModel);
                return View(searchTripModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        public IActionResult ShowStepTwoRequests()
        {
            try
            {
                List<StepTwoRequestModel> stepTwoRequestModels = _tripJobsiteService.GetStepTwoRequests();

                return View("ShowRequests", stepTwoRequestModels);
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        public IActionResult GetDriverTrips()
        {
            try
            {
                List<StepTwoRequestModel> stepTwoRequestModels = _tripJobsiteService.GetStepTwoRequests();

                return View("ShowRequests", stepTwoRequestModels);
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        public IActionResult ShowOfflineTrips()
        {
            try
            {
                List<TripJobsiteModel> tripJobsiteModels = _tripJobsiteService.GetOfflineTrips();
                List<TripJobsiteModel> todayOfflineTrips = new List<TripJobsiteModel>();
                if (tripJobsiteModels.Count > 0)
                {
                    todayOfflineTrips = tripJobsiteModels.Where(tj => tj.Trip.TripDate.Date == DateTime.Today).ToList();
                }
                return View(todayOfflineTrips);
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShowOfflineTrips(OfflineTripsModel offlineTripsModel)
        {
            try
            {
                List<TripJobsiteModel> tripJobsiteModels = _tripJobsiteService.GetOfflineTrips();
                List<TripJobsiteModel> todayOfflineTrips = new List<TripJobsiteModel>();
                if (tripJobsiteModels.Count > 0)
                {
                    todayOfflineTrips = tripJobsiteModels.Where(tj => tj.Trip.TripDate.Date == DateTime.Today).ToList();
                }
                return View(todayOfflineTrips);
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }


        public IActionResult CancelTrip()
        {
            try
            {
                return View();
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public bool CancelTripById(TripCancellationModel Canceldata)
        {
            try
            {
               AspNetUser aspNetUser = _userManager.GetUserAsync(User).Result;
                TripCancellationModel tripCancellationModel = new TripCancellationModel();
                tripCancellationModel.TripId = Canceldata.TripId;
                tripCancellationModel.UserId = aspNetUser.Id;
                tripCancellationModel.Reason = Canceldata.Reason;
                tripCancellationModel.CreatedDate = DateTime.Now;
               var cancelledTrip = _tripService.CancelTrip(Canceldata.TripId, tripCancellationModel);
                if(cancelledTrip != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public IActionResult SearchTripById(long tripId)
        {
            try
            {
                List<TripJobsiteModel> tripJobsiteModels = _tripJobsiteService.GetTripJobsiteModelByTripNumber(tripId);
                if (tripJobsiteModels.Count > 0)
                {
                    CancellationModel cancellationModel = new CancellationModel();
                    cancellationModel.TripJobsiteModels = tripJobsiteModels;
                    cancellationModel.TripNumber = tripId;
                    int lastTripJobsiteIndex = tripJobsiteModels.Count -1;
                    if(tripJobsiteModels[0].TripStatus == (int)CommanData.TripStatus.TripCompleted || tripJobsiteModels[0].Trip.Cancelled == true)
                    {
                        cancellationModel.CanCancel = false;
                        //cancellationModel.TripJobsiteModels.ForEach(tj => tj.TripStatusText = Enum.GetName(typeof(CommanData.TripStatus), tj.TripStatus));
                    }
                    else
                    {
                        cancellationModel.CanCancel = true;
                    }
                    if (tripJobsiteModels.Count > 1)
                    {
                        cancellationModel.ConvertedMessage = "Trip Converted from jobsite #" + tripJobsiteModels[lastTripJobsiteIndex - 1].JobSite.Name + " to Jobsite #" + tripJobsiteModels[lastTripJobsiteIndex];
                    }
                    return View("CancelTrip", cancellationModel);
                }
                else
                {
                    TempData["Error"] = "wrong trip number";
                    return View("CancelTrip", null);
                }

            }
            catch(Exception e)
            {
                return null;
            }
        }

    }
}
