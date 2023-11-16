using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Take5.Core;
using Take5.Models;
using Take5.Models.Models;
using Take5.Models.Models.MasterModels;
using Take5.Services;
using Take5.Services.Contracts;
using Take5.Services.Models;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.hub;
using Take5.Services.Models.MasterModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Take5.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripAPIController : ControllerBase
    {
        private readonly ITripService _tripService;
        private readonly INotificationService _notificationService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IQuestionService _questionService;
        private readonly IDriverService _driverService;
        private readonly IStepTwoRequestService _stepTwoRequestService;
        private readonly IMapper _mapper;
        private readonly ISurveyService _surveyService;
        private readonly ITripJobsiteService _tripJobsiteService;
        private readonly ITripJobsiteWarningService _tripJobsiteWarningService;
        private readonly ITripTake5TogetherService _tripTake5TogetherService;

        public TripAPIController(ITripService tripService,
            INotificationService notificationService,
            IUserNotificationService userNotificationService,
            UserManager<AspNetUser> userManager,
            IQuestionService questionService,
            IDriverService driverService,
            IStepTwoRequestService stepTwoRequestService,
            ISurveyService surveyService,
            IMapper mapper,
            ITripJobsiteService tripJobsiteService,
            ITripTake5TogetherService tripTake5TogetherService,
            ITripJobsiteWarningService tripJobsiteWarningService)
        {
            _tripService = tripService;
            _notificationService = notificationService;
            _userNotificationService = userNotificationService;
            _userManager = userManager;
            _questionService = questionService;
            _driverService = driverService;
            _stepTwoRequestService = stepTwoRequestService;
            _surveyService = surveyService;
            _mapper = mapper;
            _tripJobsiteService = tripJobsiteService;
            _tripTake5TogetherService = tripTake5TogetherService;
            _tripJobsiteWarningService = tripJobsiteWarningService;
        }

        [HttpPost("TripStarting")]
        public IActionResult TripStarting(TripStartingModel tripStartingModel)
        {
            try
            {
                bool updateResult = false;
                if(tripStartingModel != null)
                {
                  AspNetUser aspNetUser =  _userManager.FindByIdAsync(tripStartingModel.UserId).Result;
                    if(aspNetUser != null)
                    {
                      TripJobsiteModel tripJobsiteModel = _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteId(tripStartingModel.TripId, tripStartingModel.JobsiteId);
                       if(tripJobsiteModel != null)
                        {
                            if (tripJobsiteModel.Trip.Driver.UserId == tripStartingModel.UserId && tripJobsiteModel.Trip.TruckId == tripStartingModel.TruckNumber)
                            {
                                if (tripJobsiteModel.Trip.TripStatus == (int)CommanData.TripStatus.Pending)
                                {
                                    updateResult = _tripJobsiteService.SatrtTripJobsite(tripStartingModel).Result;
                                    if (updateResult == true)
                                    {
                                        Driver driver = tripJobsiteModel.Trip.Driver;
                                        NotificationModel notificationModel = new NotificationModel();
                                        int notificationTypeId = (int)CommanData.NotificationTypes.TripStarted;
                                        string notificationMessage = driver.FullName + " Started trip number " + tripStartingModel.TripId;
                                        string message = _notificationService.HandleNotificationToRole(notificationMessage, notificationTypeId, "Supervisor", tripStartingModel.TripId, tripStartingModel.JobsiteId).Result;
                                        //_tripJobsiteService.UpdateDashboard(tripJobsiteModel.TripId, tripJobsiteModel.JobSiteId, "Supervisor", "Started");
                                        return Ok(new { Message = "Success Process", Data = "Done" });
                                    }
                                    else
                                    {
                                        return BadRequest(new { Message = "عملية فاشلة لايمكن تعديل بيانات الرحلة", Data = false });
                                    }
                                }
                                else
                                {
                                    return BadRequest(new { Message = "لايمكن بدء الرحلة ,الرحلة في مرحلة" + Enum.GetName(typeof(CommanData.TripStatus), tripJobsiteModel.Trip.TripStatus), Data = false });
                                }
                            }
                            else
                            {
                                return Ok(new { Message = "عملية فاشلة الرحلة غير مسجلة", Data = "Cancelled" });
                            }
                        }
                        else
                        {
                            return Ok(new { Message = "عملية فاشلة الرحلة غير مسجلة", Data = "Cancelled" });

                        }
                    }
                    else
                    {
                        return BadRequest(new { Message = "عملية فاشلة خطأ في بيانات المستخدم", Data = false });
                    }
                }
                else
                {
                    return BadRequest(new { Message = "البيانات خاطئة  ", Data = false });
                }
            }
            catch(Exception e)
            {
                return BadRequest(new { Message = "عملية فاشلة  ", Data = false });
            }
        }

        // GET: api/<TripAPIController>
        [HttpPost("DestinationArrived")]
        public IActionResult DrvierArrivedTripDestination(AllTripSteps allTripSteps)
        {
            bool updateResult = false;
            string message = "";
            int NotificationSendToCount = 0;
            try
            {
                AspNetUser aspNetUser = _userManager.FindByIdAsync(allTripSteps.UserId).Result;
                if(allTripSteps.TripDestinationArrivedModel != null)
                {
                    if (aspNetUser != null)
                    {
                        TripJobsiteModel tripJobsiteModel = _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteId(allTripSteps.TripId, allTripSteps.JobsiteId);
                        if (tripJobsiteModel != null)
                        {
                            if (tripJobsiteModel.Trip.TripStatus == (int)CommanData.TripStatus.Started)
                            {
                                updateResult = _tripJobsiteService.ArrivingTripJobsite(allTripSteps.TripDestinationArrivedModel, allTripSteps.TripId, allTripSteps.JobsiteId).Result;
                                if (updateResult == true)
                                {
                                        TripModel tripModel = _tripService.GetTrip(allTripSteps.TripId);
                                        if (updateResult == true)
                                        {
                                            NotificationModel notificationModel = new NotificationModel();
                                            int notificationTypeId = (int)CommanData.NotificationTypes.DestinationArrived;
                                            string notificationMessage = tripModel.Driver.FullName + " with Trip Number " + tripModel.Id + " arrived his Destination at" + tripJobsiteModel.JobSite.Name + " at " + allTripSteps.TripDestinationArrivedModel.DestinationArrivedDate.ToString("yyyy-MM-dd") + ", " + allTripSteps.TripDestinationArrivedModel.DestinationArrivedDate.ToString("HH:mm:ss");
                                            message = _notificationService.HandleNotificationToRole(notificationMessage, notificationTypeId, "SuperVisor", tripModel.Id, tripJobsiteModel.JobSiteId).Result;
                                            return Ok(new { Message = message, Data = true });
                                        }
                                        else
                                        {
                                            return BadRequest(new { Message = "Failed Process, falied to update trip", Data = false });
                                        }
                                }
                                else
                                {
                                    return BadRequest(new { Message = "Failed Process, falied to update trip", Data = false });
                                }
                            }
                            else
                            {
                                return BadRequest(new { Message = "Failed Process, can not add desination arrival trip in " + Enum.GetName(typeof(CommanData.TripStatus), tripJobsiteModel.TripStatus), Data = false });
                            }
                        }
                        else
                        {
                            return BadRequest(new { Message = "Failed Process, Wrong Trip Data", Data = false });
                        }
                    }
                    else
                    {
                        return BadRequest(new { Message = "Failed Process, Ivalid User Data", Data = false });
                    }
                }
                else
                {
                    return BadRequest(new { Message = "Failed Process, empty object", Data = false });
                }

            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "failed Process", Data = false });
            }
        }

        // GET api/<TripAPIController>/5
        [HttpPost("AddStepOneAnswers")]
        public IActionResult GetSurveyStepOneAnswers(AllTripSteps allTripSteps)
        {
            string message = "";
            try
            {
                AspNetUser user = _userManager.FindByIdAsync(allTripSteps.UserId).Result;
                if(user != null)
                {
                    TripJobsiteModel tripJobsiteModel = _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteId(allTripSteps.TripId, allTripSteps.JobsiteId);
                    if (tripJobsiteModel != null)
                    {
                        if (tripJobsiteModel.Trip.TripStatus == (int)CommanData.TripStatus.DestinationArrived)
                        {
                            if (allTripSteps.SurveyStepOneAnswersAPIModel.QuestionAnswerModels != null)
                            {
                                bool addResult = _surveyService.AddAnswersToStepNQuestions(allTripSteps.SurveyStepOneAnswersAPIModel, allTripSteps.TripId, allTripSteps.JobsiteId, (int)CommanData.SurveySteps.StepOne).Result;
                                if (addResult == true)
                                {
                                    if(allTripSteps.SurveyStepOneAnswersAPIModel.DangerAPIs != null)
                                    {
                                        _surveyService.AddDangersToSurveyStepOne(allTripSteps.SurveyStepOneAnswersAPIModel, allTripSteps.TripId, allTripSteps.JobsiteId);
                                    }
                                    addResult = _tripJobsiteService.UpdateStepOneCompletion(allTripSteps.SurveyStepOneAnswersAPIModel, allTripSteps.TripId, allTripSteps.JobsiteId).Result;
                                    if(addResult == true)
                                    {
                                            Driver driver = _driverService.GetDriverByUserId(user.Id);
                                            NotificationModel notificationModel = new NotificationModel();
                                            int notificationTypeId = (int)CommanData.NotificationTypes.StepOneCompleted;
                                            string notificationMessage = driver.FullName + " completed survey step one for trip number " + allTripSteps.TripId + " at " + allTripSteps.SurveyStepOneAnswersAPIModel.CreatedDate.ToString("yyy-mm-dd") + ", " + allTripSteps.SurveyStepOneAnswersAPIModel.CreatedDate.ToString("HH:MM:ss");
                                            message = _notificationService.HandleNotificationToRole(notificationMessage, notificationTypeId, "Supervisor", allTripSteps.TripId, allTripSteps.JobsiteId).Result;
                                            
                                            return Ok(new { Message = message, Data = true });                                       
                                    }
                                    else
                                    {
                                        return BadRequest(new { Message = "Failed Process, update trip status", Data = false });
                                    }
                                }
                                else
                                {
                                    return BadRequest(new { Message = "Failed Process, Can not complete Take 5", Data = false });
                                }
                            }
                            else
                            {
                                return BadRequest(new { Message = "Wrong Answers", Data = false });

                            }
                        }
                        else
                        {
                            return BadRequest(new { Message = "Failed Process, Can not complete take 5 survey now, trip in "+ Enum.GetName(typeof(CommanData.TripStatus), tripJobsiteModel.TripStatus) +" status", Data = false });
                        }
                    }
                    else
                    {
                        return BadRequest(new { Message = "Failed Process, Wrong Trip Data", Data = false });
                    }
                }
                else
                {
                    return BadRequest(new { Message = "Failed Process, Invalid User Data", Data = false });

                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "failed Process", Data = false });
            }
        }

        //POST api/<TripAPIController>
        [HttpPost("SendStepTwoRequest")]
        public IActionResult SendRequestForStepTwo(AllTripSteps allTripSteps)
        {
            try
            {
                string message;
                AspNetUser user = _userManager.FindByIdAsync(allTripSteps.UserId).Result;
                if (user != null)
                {
                    TripJobsiteModel tripJobsiteModel = _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteId(allTripSteps.TripId, allTripSteps.JobsiteId);
                    if (tripJobsiteModel != null)
                    {
                        if (tripJobsiteModel.TripStatus == (int)CommanData.TripStatus.SurveyStepOneCompleted)
                        {
                            Driver driver = _driverService.GetDriverByUserId(user.Id);
                            double TimeDifferenceBtweenStepOneAndTwoRequest = (allTripSteps.Take5StepTwoRequestAPIModel.RequestDate - (DateTime)tripJobsiteModel.StageOneComplatedTime).TotalHours;
                            tripJobsiteModel = _tripService.CreateStepTwoRequest(allTripSteps.Take5StepTwoRequestAPIModel, allTripSteps.TripId, allTripSteps.JobsiteId);
                            //bool addRequestResult = _tripService.CreateStepTwoRequest(requestModel).Result;
                            if (tripJobsiteModel != null)
                            {
                                int notificationTypeId = (int)CommanData.NotificationTypes.StepTwoRequest;
                                string notificationMessage = driver.FullName + " Requested to complete step two for survey with trip number " + tripJobsiteModel.TripId + ". survey step one has completed for " + TimeDifferenceBtweenStepOneAndTwoRequest + "ago at" + tripJobsiteModel.StageOneComplatedTime.Value.ToString("yyy-mm-dd") + ", " + tripJobsiteModel.StageOneComplatedTime.Value.ToString("HH:MM:ss");
                                message = _notificationService.HandleNotificationToRole(notificationMessage, notificationTypeId, "Supervisor", allTripSteps.TripId, allTripSteps.JobsiteId).Result;
                                return Ok(new { Message = message, Data = true });
                            }
                            else
                            {
                                return BadRequest(new { Message = "Failed Process, Can not add request to survey step two", Data = false });
                            }
                        }
                        else
                        {
                            return BadRequest(new { Message = "Failed Process, Can not Send Request now trip at "+ Enum.GetName(typeof(CommanData.TripStatus), tripJobsiteModel.TripStatus)+ " status", Data = false });
                        }
                    }
                    else
                    {
                        return BadRequest(new { Message = "Failed Process, Wrong Trip Data", Data = false });
                    }
                }
                else
                {
                    return BadRequest(new { Message = "Failed Process, Invalid User data", Data = false });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Failed Process, Can not add request to survey step two", Data = false });
            }
        }

        [HttpPost("IsRequestApproved")]
        public IActionResult IsRequestApproved(CheckRequestStatusModel model)
        {
            try
            {
                string message;
                bool result;
                string requestStatus = "";
                if (StringExtensions.IsValid(model.TruckNumber))
                {
                    AspNetUser user = _userManager.FindByIdAsync(model.UserId).Result;
                    if (user != null)
                    {
                        TripJobsiteModel tripJobsiteModel = _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteIdForAPI(model.TripId, model.JobsiteId);
                        if (tripJobsiteModel != null)
                        {
                            if ((tripJobsiteModel.Trip.Driver.UserId == model.UserId && tripJobsiteModel.Trip.TruckId == model.TruckNumber)
                                && (tripJobsiteModel.Trip.Cancelled != true) &&
                                (tripJobsiteModel.Converted != true))
                            {
                                if (tripJobsiteModel.Trip.TripStatus >= (int)CommanData.TripStatus.StepTwoRequested)
                                {
                                    string status = Enum.GetName(typeof(CommanData.RequestStatus), tripJobsiteModel.RequestStatus);
                                    if (status == "Pending")
                                    {
                                        message = "Request not Apprroved yet";
                                        result = false;
                                        requestStatus = "Pending";
                                    }
                                    else
                                    {
                                        message = "Your Request is Apprroved";
                                        result = true;
                                        requestStatus = "Apprroved";
                                    }
                                    return Ok(new { Message = message, Data = requestStatus });
                                }
                                else
                                {
                                    return BadRequest(new { Message = "Failed Process, No Request Added yet", Data = false });
                                }
                            }
                            else
                            {
                                return Ok(new { Message = "Trip has been Cancelled", Data = "Cancelled" });
                            }
                        }
                        else
                        {
                            return BadRequest(new { Message = "Failed Process, Wrong Trip Data", Data = false });
                        }
                    }
                    else
                    {
                        return BadRequest(new { Message = "Failed Process, Invalid User data", Data = false });
                    }
                }
                else
                {
                    return BadRequest(new {Message= "Failed Process, Invalid Truck Number", Data = false});
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Failed Process, Can not add request to survey step two", Data = false });
            }
        }


        [HttpPost("AddSurveyStepTwoAnswers")]
        public IActionResult GetSurveyStepTwoAnswers(AllTripSteps allTripSteps)
        {
            string message = "";
            try
            {
                AspNetUser user = _userManager.FindByIdAsync(allTripSteps.UserId).Result;
                bool result = false;
                if (user != null)
                {
                    TripJobsiteModel tripJobsiteModel = _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteId(allTripSteps.TripId, allTripSteps.JobsiteId);
                    if (tripJobsiteModel != null)
                    {
                        if (tripJobsiteModel.RequestStatus == (int)CommanData.RequestStatus.Approved)
                        {
                            double TimeDifferenceBtweenStepOneAndStepTwoCompletion = ((DateTime) allTripSteps.SurveyStepTwoAnswersAPIModel.CreatedDate- (DateTime)tripJobsiteModel.StageOneComplatedTime).TotalMinutes;
                            if(TimeDifferenceBtweenStepOneAndStepTwoCompletion < CommanData.NormalDifferenceBetweenStep1AndStep2)
                            {
                                TripJobsiteWarning tripJobsiteWarning = new TripJobsiteWarning();
                                tripJobsiteWarning.Message = "Warning! step2 completed after "+TimeDifferenceBtweenStepOneAndStepTwoCompletion +"Minutes"+ 
                                    "from Step1 completion";
                                tripJobsiteWarning.WarningTypeId = (int) CommanData.WarningTypes.StepTwoCompletionWarning;
                                result = _tripJobsiteWarningService.CreateTripJobsiteWarning(tripJobsiteWarning);
                            }
                            else
                            {
                                result = true;
                            }
                            if(result == true)
                            {
                                if (allTripSteps.SurveyStepTwoAnswersAPIModel.QuestionAnswerModels != null)
                                {
                                    SurveyStepOneAnswersAPIModel surveyStepOneAnswersAPIModel = _mapper.Map<SurveyStepOneAnswersAPIModel>(allTripSteps.SurveyStepTwoAnswersAPIModel);
                                    result = _surveyService.AddAnswersToStepNQuestions(surveyStepOneAnswersAPIModel, allTripSteps.TripId, allTripSteps.JobsiteId, (int)CommanData.SurveySteps.StepOne).Result;
                                    if (result == true)
                                    {
                                        result = _tripJobsiteService.UpdateStepTwoCompletion(surveyStepOneAnswersAPIModel, allTripSteps.TripId, allTripSteps.JobsiteId).Result;
                                        if (result == true)
                                        {
                                            TripModel tripModel = _tripService.GetTrip(allTripSteps.TripId);      
                                            var updateResult = _tripService.UpdateTripStatus(allTripSteps.TripId, (int)CommanData.TripStatus.SurveyStepTwoCompleted, (int)CommanData.Take5Status.Completed).Result;
                                            if (updateResult == true)
                                            {
                                                Driver driver = _driverService.GetDriverByUserId(user.Id);
                                                NotificationModel notificationModel = new NotificationModel();
                                                int notificationTypeId = (int)CommanData.NotificationTypes.StepOneCompleted;
                                                string notificationMessage = driver.FullName + " Completed Survey Step Two for trip number " + allTripSteps.TripId + " at " + allTripSteps.SurveyStepTwoAnswersAPIModel.CreatedDate.ToString("yyy-mm-dd") + ", " + allTripSteps.SurveyStepTwoAnswersAPIModel.CreatedDate.ToString("HH:MM:ss");
                                                message = _notificationService.HandleNotificationToRole(notificationMessage, notificationTypeId, "Supervisor", allTripSteps.TripId, allTripSteps.JobsiteId).Result;
                                                return Ok(new { Message = message, Data = true });
                                            }
                                            else
                                            {
                                                return BadRequest(new { Message = "Failed Process, Can not update trip status", Data = false });

                                            }

                                        }
                                        else
                                        {
                                            return BadRequest(new { Message = "Failed Process, Can not update trip status", Data = false });
                                        }

                                    }
                                    else
                                    {
                                        return BadRequest(new { Message = "Failed Process, Can not complete Take 5", Data = false });
                                    }
                                }
                                else
                                {
                                    return BadRequest(new { Message = "Wrong Answers", Data = false });

                                }
                            }
                            else
                            {
                                return BadRequest(new { Message = "Failed Process, Contact your support team", Data = false });
                            }

                        }
                        else
                        {
                            return BadRequest(new { Message = "Failed Process, Can not complete take 5 survey before Destination arrival", Data = false });
                        }
                    }
                    else
                    {
                        return BadRequest(new { Message = "Failed Process, Wrong Trip Data", Data = false });
                    }
                }
                else
                {
                    return BadRequest(new { Message = "Failed Process, Invalid User Data", Data = false });

                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "failed Process", Data = false });
            }
        }


        //[HttpPost("SendLastTripUpdate")]
        //public IActionResult SendLastTripUpdate(AllTripSteps model)
        //{
        //    string message = "";
        //    try
        //    {
        //        AspNetUser user = _userManager.FindByIdAsync(model.UserId).Result;
        //        bool result = false;
        //        if (user != null)
        //        {
        //            TripJobsiteModel tripJobsiteModel = _tripJobsiteService.GetTripByTripNumberAndJobsiteId(model.TripId, model.JobsiteId);
        //            if (tripJobsiteModel != null)
        //            {
        //                string lastTripStatus = Enum.GetName(typeof(CommanData.TripStatus), tripJobsiteModel.TripStatus);
        //                string tripCurrentStatus = Enum.GetName(typeof(CommanData.TripStatus), tripJobsiteModel.TripStatus + 1);
        //                if (model.CurrentStatus == tripCurrentStatus)
        //                {
        //                    if (tripJobsiteModel.TripStatus == (int)CommanData.TripStatus.Pending)
        //                    {
        //                        string startResult = _newTripUpdateService.StartTrip(model.TripStartingModel).Result;
        //                        if (startResult != null)
        //                        {
        //                            if (startResult.Contains("Successful Process"))
        //                            {
        //                                ALLSurveyModel aLLSurveyModel = _surveyService.getAllSurvey();
        //                                return Ok(new { Message = "Success Process", Data = aLLSurveyModel });
        //                            }
        //                            else
        //                            {
        //                                return BadRequest(new { Message = message, Data = 0 });
        //                            }
        //                        }
        //                        else
        //                        {
        //                            return BadRequest(new { Message = "Failed Process", Data = 0 });
        //                        }
        //                    }
        //                    else if ()
        //                    {

        //                    }
        //                }
        //                else
        //                {
        //                    return BadRequest(new { Message = "Current Status is different trip status is " + lastTripStatus, Data = 0 });
        //                }
        //            }
        //            else
        //            {
        //                return BadRequest(new { Message = "عملية فاشلة الرحلة غير مسجلة", Data = 0 });
        //            }
        //        }
        //        else
        //        {
        //            return BadRequest(new { Message = "Failed Process, Invalid User Data", Data = false });

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(new { Message = "failed Process", Data = false });
        //    }
        //}


        [HttpPost("SendOfflineTripAllSteps")]
        public IActionResult SendOfflineTripAllSteps(AllTripSteps model)
        {
            string message;
            try
            {
               if(model != null)
                {
                    AspNetUser user = _userManager.FindByIdAsync(model.UserId).Result;
                    bool result = false;
                    if (user != null)
                    {
                        TripJobsiteModel tripJobsiteModel = _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteId(model.TripId, model.JobsiteId);
                        if (tripJobsiteModel != null)
                        {
                            if (tripJobsiteModel.Trip.Driver.UserId == model.UserId && tripJobsiteModel.Trip.TruckId == model.TruckNumber)
                            {
                                if (tripJobsiteModel.JobSite.HasNetworkCoverage == false)
                                {
                                    if (tripJobsiteModel.Trip.Cancelled != true)
                                    {
                                        if (tripJobsiteModel.Converted != true)
                                        {
                                            if(model.EndStatus == "TripCompleted")
                                            {
                                                string CurrentTripStatus = Enum.GetName(typeof(CommanData.TripStatus), tripJobsiteModel.TripStatus);
                                                //if (model.StartStatus == CurrentTripStatus)
                                                //{
                                                if (tripJobsiteModel.TripStatus == (int)CommanData.TripStatus.Started)
                                                {
                                                    string updateResult = _tripJobsiteService.AddUpdatesToOfflineTrip(model).Result;
                                                    if (updateResult.Contains("Success"))
                                                    {
                                                        Driver driver = tripJobsiteModel.Trip.Driver;
                                                        NotificationModel notificationModel = new NotificationModel();
                                                        int notificationTypeId = (int)CommanData.NotificationTypes.TripCompleted;
                                                        string notificationMessage = driver.FullName + " Completed trip number " + model.TripId;
                                                        message = _notificationService.HandleNotificationToRole(notificationMessage, notificationTypeId, "Supervisor", model.TripId, model.JobsiteId).Result;

                                                        return Ok(new { Message = "Success Process, All Trip steps added successfully", Data = "Done" });
                                                    }
                                                    else
                                                    {
                                                        return BadRequest(new { Message = updateResult, Data = false });
                                                    }
                                                }
                                                else
                                                {
                                                    return BadRequest(new { Message = "Failed Process, Trip at started status", Data = false });
                                                }
                                                //}
                                                //else
                                                //{
                                                //    return BadRequest(new { Message = "unmatched start status", Data = false });
                                                //}
                                            }
                                            else
                                            {
                                                return BadRequest(new { Message = "Failed Process, wrong data", Data = false });
                                            }
                                        }
                                        else
                                        {
                                            return Ok(new { Message = "SuccessFul Process", Data = "Converted" });
                                        }
                                    }
                                    else
                                    {
                                        return Ok(new { Message = "SuccessFul Process", Data = "Cancelled" });
                                    }
                                }
                                else
                                {
                                    return BadRequest(new { Message = "Failed Process, invalid trip type", Data = false });
                                }
                            }
                            else
                            {
                                return Ok(new { Message = "SuccessFul Process", Data = "Cancelled" });
                            }

                        }
                        else
                        {
                            return BadRequest(new { Message = "Failed Process, invalid trip", Data = false });
                        }
                    }
                    else
                    {
                        return BadRequest(new { Message = "Failed Process, invalid user data", Data = false });
                    }
                }
                else
                {
                    return BadRequest(new { Message = "Failed Process, invalid data, main object is empty", Data = false });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "failed Process", Data = false });
            }
        }

        [HttpPost("SendTripUpdate")]
        public async Task<IActionResult> SendTripUpdate(AllTripSteps model)
        {
            bool result = true;
            bool invalidObject = false;
            string message = "";
            string response = "Done";
            try
            {
                if (ObjectMapping.VaildateAllTripSteps(model))
                {
                    AspNetUser user = _userManager.FindByIdAsync(model.UserId).Result;
                    if (user != null)
                    {
                        TripJobsiteModel tripJobsiteModel = _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteIdForAPI(model.TripId, model.JobsiteId);
                        if (tripJobsiteModel != null)
                        {
                            if (tripJobsiteModel.JobSite.HasNetworkCoverage == true)
                            {
                                if (tripJobsiteModel.Trip.Driver.UserId == model.UserId && tripJobsiteModel.Trip.TruckId == model.TruckNumber)
                                {
                                    if (tripJobsiteModel.Trip.Cancelled != true)
                                    {
                                        if (tripJobsiteModel.Converted != true)
                                        {
                                            string LastTripStatus = Enum.GetName(typeof(CommanData.TripStatus), tripJobsiteModel.TripStatus);
                                            int LastTripStatusNum = tripJobsiteModel.TripStatus;
                                            int EndStatusNum = (int)(CommanData.TripStatus)Enum.Parse(typeof(CommanData.TripStatus), model.EndStatus);
                                            if ((LastTripStatusNum < EndStatusNum && result != false && LastTripStatus != "TripCompleted"))
                                            {
                                                while ((LastTripStatusNum < EndStatusNum && result != false && LastTripStatus != "TripCompleted"))
                                                {
                                                    if (LastTripStatus == "Started")
                                                    {
                                                        if (model.TripDestinationArrivedModel != null)
                                                        {
                                                            result = await _tripJobsiteService.ArrivingTripJobsite(model.TripDestinationArrivedModel, model.TripId, model.JobsiteId);
                                                            if (result != true)
                                                            {
                                                                message = "Failed Process, Can not update trip status at destination arrival status";
                                                            }
                                                            else
                                                            {
                                                                int notificationTypeId = (int)CommanData.NotificationTypes.DestinationArrived;
                                                                string notificationMessage = tripJobsiteModel.Trip.Driver.FullName + " arrived his destination trip number " + tripJobsiteModel.TripId + "at" + model.TripDestinationArrivedModel.DestinationArrivedDate.ToString("yyy-MM-dd, HH:mm:ss");
                                                                message = _notificationService.HandleNotificationToRole(notificationMessage, notificationTypeId, "Supervisor", model.TripId, model.JobsiteId).Result;
                                                                //_tripJobsiteService.UpdateDashboard(tripJobsiteModel.TripId, tripJobsiteModel.JobSiteId, "Supervisor", "DestinationArrived");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            result = false;
                                                            invalidObject = true;
                                                        }
                                                    }
                                                    else if (LastTripStatus == "DestinationArrived")
                                                    {
                                                        if (model.SurveyStepOneAnswersAPIModel != null)
                                                        {
                                                            result = _surveyService.AddAnswersToStepNQuestions(model.SurveyStepOneAnswersAPIModel, model.TripId, model.JobsiteId, (int)CommanData.SurveySteps.StepOne).Result;
                                                            if (result == true)
                                                            {
                                                                if (model.SurveyStepOneAnswersAPIModel.DangerAPIs.Count > 0)
                                                                {
                                                                    result = _surveyService.AddDangersToSurveyStepOne(model.SurveyStepOneAnswersAPIModel, model.TripId, model.JobsiteId).Result;
                                                                }
                                                                result = await _tripJobsiteService.UpdateStepOneCompletion(model.SurveyStepOneAnswersAPIModel, model.TripId, model.JobsiteId);
                                                                if (result != true)
                                                                {
                                                                    result = _surveyService.DeleteTake5StepOneForTripJobsite(model.TripId, model.JobsiteId);
                                                                    message = "Failed Process, Can not add Stage one of survey";
                                                                }
                                                                else
                                                                {
                                                                    int notificationTypeId = (int)CommanData.NotificationTypes.StepOneCompleted;
                                                                    string notificationMessage = tripJobsiteModel.Trip.Driver.FullName + " completed step one of Take5 Survey for trip number " + tripJobsiteModel.TripId + "at" + model.SurveyStepOneAnswersAPIModel.CreatedDate.ToString("yyy-MM-dd, HH:mm:ss");
                                                                    message = _notificationService.HandleNotificationToRole(notificationMessage, notificationTypeId, "Supervisor", model.TripId, model.JobsiteId).Result;
                                                                    //await _tripJobsiteService.UpdateDashboard(tripJobsiteModel.TripId, tripJobsiteModel.JobSiteId, "Supervisor", "SurveyStepOneCompleted");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                message = "Failed Process, Can not add Stage one of survey";
                                                            }

                                                            //if (result == true)
                                                            //{
                                                            //    result = _surveyService.AddAnswersToStepNQuestions(model.SurveyStepOneAnswersAPIModel, model.TripId, model.JobsiteId).Result;
                                                            //    if(result == false)
                                                            //    {
                                                            //        // set trip to previous status
                                                            //         await _tripJobsiteService.UpdateTripJobsiteStatus(model.TripId, model.JobsiteId, "DestinationArrived");
                                                            //         await _tripJobsiteService.UpdateTripJobsiteTake5Status(model.TripId, model.JobsiteId, "NotStarted");

                                                            //    }
                                                            //}
                                                            //else
                                                            //{ 
                                                            //if (result != true)
                                                            //{
                                                            //    message = "Failed Process, Can not add Stage one of survey";
                                                            //}
                                                            //}
                                                        }
                                                        else
                                                        {
                                                            result = false;
                                                            invalidObject = true;
                                                        }
                                                    }
                                                    else if (LastTripStatus == "SurveyStepOneCompleted")
                                                    {
                                                        if (model.EndStatus == "StepTwoRequested")
                                                        {
                                                            if (model.Take5StepTwoRequestAPIModel != null)
                                                            {
                                                                tripJobsiteModel = _tripService.CreateStepTwoRequest(model.Take5StepTwoRequestAPIModel, model.TripId, model.JobsiteId);
                                                                if (tripJobsiteModel != null)
                                                                {
                                                                    result = true;
                                                                    double TimeDifferenceBtweenStepOneAndTwoRequest = _tripJobsiteWarningService.CalculateDifference((DateTime)tripJobsiteModel.StageTwoRequestDate, (DateTime)tripJobsiteModel.StageOneComplatedTime);
                                                                    if (TimeDifferenceBtweenStepOneAndTwoRequest != 0)
                                                                    {
                                                                        _tripJobsiteWarningService.AddNewTripJobsiteWarning(model.TripId, model.JobsiteId, (int)CommanData.WarningTypes.StepTwoRequestWarning, TimeDifferenceBtweenStepOneAndTwoRequest);
                                                                    }
                                                                    int notificationTypeId = (int)CommanData.NotificationTypes.StepTwoRequest;
                                                                    double diff = ((DateTime)tripJobsiteModel.StageTwoRequestDate - (DateTime)tripJobsiteModel.StageOneComplatedTime).TotalMinutes;
                                                                    string notificationMessage = tripJobsiteModel.Trip.Driver.FullName + " Requested to complete step two for survey with trip number " + tripJobsiteModel.TripId + ". survey step one has completed for " +
                                                                        diff + " minutes ago at " + tripJobsiteModel.StageOneComplatedTime.Value.ToString("yyy-MM-dd") + ", " + tripJobsiteModel.StageOneComplatedTime.Value.ToString("HH:mm:ss");
                                                                    message = _notificationService.HandleNotificationToRole(notificationMessage, notificationTypeId, "Supervisor", model.TripId, model.JobsiteId).Result;
                                                                    //await _tripJobsiteService.UpdateDashboard(tripJobsiteModel.TripId, tripJobsiteModel.JobSiteId, "Supervisor", "StepTwoRequested");
                                                                }
                                                                else
                                                                {
                                                                    result = false;
                                                                    message = "Failed Process, Can not add request for stage 2";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                result = false;
                                                                invalidObject = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (model.Take5StepTwoRequestAPIModel != null)
                                                            {
                                                                result = _tripJobsiteService.AddStepTwoRequestForOfflineTrip(model.Take5StepTwoRequestAPIModel, model.TripId, model.JobsiteId);
                                                                if (result != true)
                                                                {
                                                                    message = "Failed Process, Can not request for stage 2";
                                                                }
                                                                else
                                                                {
                                                                    double TimeDifferenceBtweenStepOneAndTwoRequest = _tripJobsiteWarningService.CalculateDifference((DateTime)tripJobsiteModel.StageTwoRequestDate, (DateTime)tripJobsiteModel.StageOneComplatedTime);
                                                                    if (TimeDifferenceBtweenStepOneAndTwoRequest != 0)
                                                                    {
                                                                        _tripJobsiteWarningService.AddNewTripJobsiteWarning(model.TripId, model.JobsiteId, (int)CommanData.WarningTypes.StepTwoRequestWarning, TimeDifferenceBtweenStepOneAndTwoRequest);
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                result = false;
                                                                invalidObject = true;
                                                            }
                                                        }
                                                    }
                                                    else if (LastTripStatus == "StepTwoRequested")
                                                    {
                                                        if (model.Take5StepTwoRequestAPIModel != null)
                                                        {
                                                            if (model.Take5StepTwoRequestAPIModel.ResponseDate != null)
                                                            {
                                                                result = _tripJobsiteService.AddOfflineResponseforStepTwoRequestForTrip(model.Take5StepTwoRequestAPIModel, model.TripId, model.JobsiteId);
                                                                if (result != true)
                                                                {
                                                                    message = "Failed Process, error in step two request";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                result = false;
                                                                message = "Failed Process, request not approved yet";
                                                            }

                                                        }
                                                        else
                                                        {
                                                            result = false;
                                                            invalidObject = true;
                                                        }
                                                    }

                                                    else if (LastTripStatus == "StepTwoResponsed")
                                                    {
                                                        if (model.SurveyStepTwoAnswersAPIModel != null)
                                                        {
                                                            SurveyStepOneAnswersAPIModel surveyStepOneAnswersAPIModel = _mapper.Map<SurveyStepOneAnswersAPIModel>(model.SurveyStepTwoAnswersAPIModel);
                                                            result = await _surveyService.AddAnswersToStepNQuestions(surveyStepOneAnswersAPIModel, model.TripId, model.JobsiteId, (int)CommanData.SurveySteps.StepTwo);
                                                            if (result == true)
                                                            {
                                                                result = _tripJobsiteService.UpdateStepTwoCompletion(surveyStepOneAnswersAPIModel, model.TripId, model.JobsiteId).Result;
                                                                if (result != true)
                                                                {
                                                                    result = _surveyService.DeleteTake5StepTwoForTripJobsite(model.TripId, model.JobsiteId);
                                                                    message = "Failed Process, Can not add Stage two of survey";
                                                                }
                                                                else
                                                                {
                                                                    TripJobsiteModel tripJobsiteModel1 = _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteId(model.TripId, model.JobsiteId);
                                                                    double TimeDifferenceBtweenStepOneAndTwoRequest = _tripJobsiteWarningService.CalculateDifference((DateTime)model.SurveyStepTwoAnswersAPIModel.CreatedDate, (DateTime)tripJobsiteModel1.StageOneComplatedTime);
                                                                    if (TimeDifferenceBtweenStepOneAndTwoRequest != 0)
                                                                    {
                                                                        _tripJobsiteWarningService.AddNewTripJobsiteWarning(model.TripId, model.JobsiteId, (int)CommanData.WarningTypes.StepTwoRequestWarning, TimeDifferenceBtweenStepOneAndTwoRequest);
                                                                    }
                                                                    int notificationTypeId = (int)CommanData.NotificationTypes.StepTwoCompleted;
                                                                    double diff = ((DateTime)model.SurveyStepTwoAnswersAPIModel.CreatedDate - (DateTime)tripJobsiteModel1.StageOneComplatedTime).TotalMinutes;

                                                                    string notificationMessage = tripJobsiteModel.Trip.Driver.FullName + " completed Step two of Take5 Survey for trip number " + tripJobsiteModel.TripId + "at" +
                                                                     model.SurveyStepTwoAnswersAPIModel.CreatedDate.ToString("yyy-MM-dd, HH:mm:ss") + " after " + diff + " minutes from step one completion";
                                                                    message = _notificationService.HandleNotificationToRole(notificationMessage, notificationTypeId, "Supervisor", model.TripId, model.JobsiteId).Result;
                                                                    //await _tripJobsiteService.UpdateDashboard(tripJobsiteModel.TripId, tripJobsiteModel.JobSiteId, "Supervisor", "SurveyStepTwoCompleted");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                message = "Failed Process, Can not add Stage two of survey";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            result = false;
                                                            invalidObject = true;
                                                        }
                                                    }
                                                    else if (LastTripStatus == "SurveyStepTwoCompleted")
                                                    {
                                                        if (model.Take5TogetherAPIModels != null)
                                                        {
                                                            result = _tripTake5TogetherService.AddTripTake5TogetherForTrip(model);
                                                        }
                                                        else
                                                        {
                                                            result = true;
                                                        }
                                                        if (result == true)
                                                        {
                                                            result = _tripJobsiteService.UpdateTripJobsiteStatus(model.TripId, model.JobsiteId, "TripCompleted").Result;
                                                            if (result != true)
                                                            {
                                                                message = "Failed Process, Can not complete trip";
                                                            }
                                                            else
                                                            {
                                                                int notificationTypeId = (int)CommanData.NotificationTypes.TripCompleted;
                                                                string notificationMessage = tripJobsiteModel.Trip.Driver.FullName + " completed trip number " + tripJobsiteModel.TripId;
                                                                message = _notificationService.HandleNotificationToRole(notificationMessage, notificationTypeId, "Supervisor", model.TripId, model.JobsiteId).Result;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            message = "Failed Process, Can not add Take5 Together data";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        result = false;
                                                        message = "invalid Data";
                                                    }
                                                    if (result == true)
                                                    {
                                                        tripJobsiteModel = _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteId(model.TripId, model.JobsiteId);
                                                        LastTripStatus = Enum.GetName(typeof(CommanData.TripStatus), tripJobsiteModel.TripStatus);
                                                        LastTripStatusNum = tripJobsiteModel.TripStatus;
                                                    }
                                                    else
                                                    {
                                                        result = false;
                                                        if (invalidObject == true)
                                                        {
                                                            message = "Failed Process; Invalid data, empty object";
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                message = "Trip at " + LastTripStatus + " Status";
                                                result = true;
                                            }

                                            // loop End
                                        }
                                        else
                                        {
                                            result = true;
                                            response = "Converted";
                                            message = "Failed Process, trip already converted";
                                        }
                                    }
                                    else
                                    {
                                        result = true;
                                        response = "Cancelled";
                                        message = "Failed Process, trip already cancelled";
                                    }
                                }
                                else
                                {
                                    result = true;
                                    response = "Cancelled";
                                    message = "Failed Process, trip already Cancelled";
                                }
                            }
                            else
                            {
                                result = false;
                                message = "invalid trip type";
                            }
                        }
                        else
                        {
                            result = true;
                            response = "Cancelled";
                            message = "invalid trip data";
                        }
                    }
                    else
                    {
                        result = false;
                        message = "invalid user data";
                    }
                }
                else
                {
                    result = false;
                    message = "invalid truck number";
                }
                if (result == true)
                {
                    return Ok(new { Message = "SuccessFul Process", Data = response });
                }
                else
                {
                    return BadRequest(new { Message = message, Data = false });
                }

            }
            catch (Exception e)
            {
                return BadRequest(new { Message = message, Data = false });
            }
                     
        }

        [HttpPost("IsTripCancelled")]
        public async Task<IActionResult> IsTripCancelled(CheckTripStatusModel model)
        {
            try
            {
                AspNetUser user = _userManager.FindByIdAsync(model.UserId).Result;
                if(user != null)
                {
                    TripJobsite tripJobsite =  await _tripJobsiteService.GetTripJobsiteByTripNumberAndJobsiteId(model.TripId, model.JobsiteId);
                    if(tripJobsite != null)
                    {
                        if(tripJobsite.Trip.Cancelled)
                        {
                            return Ok(new { Message = "Successful Process", Data = "Cancelled" });
                        }
                        else
                        {
                           return Ok(new { Message = "Successful Process", Data = "Done" });
                        }
                    }
                    else
                    {
                        return BadRequest(new { Message = "Failed Process, wrong trip data", Data = false });
                    }
                }
                else
                {
                    return BadRequest(new { Message = "Failed Process, wrong user data", Data = false });
                }
            }
            catch(Exception e)
            {
                return BadRequest(new { Message = "Failed Process", Data = false });
            }
        }

        [HttpPost("IsTripConverted")]
        public async Task<IActionResult> IsTripConverted(CheckTripStatusModel model)
        {
            try
            {
                AspNetUser user = _userManager.FindByIdAsync(model.UserId).Result;
                if (user != null)
                {
                    TripJobsite tripJobsite = await _tripJobsiteService.GetTripJobsiteByTripNumberAndJobsiteId(model.TripId, model.JobsiteId);
                    if (tripJobsite != null)
                    {
                        if (tripJobsite.Converted)
                        {
                            return Ok(new { Message = "Successful Process", Data = "Converted" });
                        }
                        else
                        {
                            return Ok(new { Message = "Successful Process", Data = "Done" });
                        }
                    }
                    else
                    {
                        return BadRequest(new { Message = "Failed Process, wrong trip data", Data = false });
                    }
                }
                else
                {
                    return BadRequest(new { Message = "Failed Process, wrong user data", Data = false });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Failed Process", Data = false });
            }
        }

        [HttpPost("CheckTripStatus")]
        public async Task<IActionResult> GetTripStatus(CheckTripStatusModel model)
        {
            try
            {
                AspNetUser user = _userManager.FindByIdAsync(model.UserId).Result;
                if (user != null)
                {
                    TripJobsiteModel tripJobsite =  _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteIdForAPI(model.TripId, model.JobsiteId);
                    if (tripJobsite != null)
                    {
                        if (tripJobsite.Trip.Driver.UserId == model.UserId && tripJobsite.Trip.TruckId == model.TruckNumber)
                        {
                            if (tripJobsite.Trip.Cancelled)
                            {
                                return Ok(new { Message = "Successful Process", Data = "Cancelled" });
                            }
                            else if (tripJobsite.Converted)
                            {
                                return Ok(new { Message = "Successful Process", Data = "Converted" });
                            }
                            else
                            {
                                return Ok(new { Message = "Successful Process", Data = "Done" });
                            }
                        }
                        else
                        {
                            return Ok(new { Message = "Successful Process", Data = "Cancelled" });
                        }
                    }
                    else
                    {
                        return BadRequest(new { Message = "Failed Process, wrong trip data", Data = false });
                    }
                }
                else
                {
                    return BadRequest(new { Message = "Failed Process, wrong user data", Data = false });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "Failed Process", Data = false });
            }
        }

        [HttpPost("GetCurrentTripForDriver")]
        public async Task<ActionResult> GetCurrentJobsiteTripForDriver(string userId)
        {
            TripWithSurveyModel tripWithSurveyModel = new TripWithSurveyModel();
            try
            {
                AspNetUser user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    Driver driver = _driverService.GetDriverByUserId(user.Id);
                    if (driver != null)
                    {
                        TripJobsiteModel tripJobsiteModel = _tripJobsiteService.GetCurrentJobsiteTripForDriver(driver.Id);
                        if (tripJobsiteModel != null)
                        {
                            List<TripJobsiteModel> tripJobsiteModels = new List<TripJobsiteModel>();
                            tripJobsiteModels.Add(tripJobsiteModel);
                            List<TripAPIModel> tripAPIModels = _tripService.ConvertFromTripJobsiteModelToTripAPIModel(tripJobsiteModels);
                            if (tripAPIModels != null)
                            {
                                tripWithSurveyModel.TripAPIModel = tripAPIModels.First();
                                ALLSurveyModel aLLSurveyModel = _surveyService.getAllSurvey();
                                List<DriverAPIModel> drivers = _driverService.GetAllDriversForMobile();
                                if (aLLSurveyModel != null)
                                {
                                    tripWithSurveyModel.ALLSurveyModel = aLLSurveyModel;
                                }
                                if (drivers.Count > 0)
                                {
                                    tripWithSurveyModel.Drivers = drivers;
                                }
                                return Ok(new { Data = tripWithSurveyModel, Message = "Successful Process" });
                            }
                            else
                            {
                                tripWithSurveyModel = null;
                                return Ok(new { Data = tripWithSurveyModel, Message = "Failed Process, error in the system" });
                            }
                        }
                        else
                        {
                            tripWithSurveyModel = null;
                            return Ok(new { Data = tripWithSurveyModel, Message = "ليس لديك رحالات حالية" });
                        }
                    }
                    else
                    {
                        tripWithSurveyModel = null;
                        return BadRequest(new { Data = tripWithSurveyModel, Message = "Failed Process, Invalid Data" });
                    }
                }
                else
                {
                    tripWithSurveyModel = null;
                    return BadRequest(new { Data = tripWithSurveyModel, Message = "Failed Process, Invalid Data" });
                }
            }
            catch (Exception e)
            {
                tripWithSurveyModel = null;
                return BadRequest(new { Data = tripWithSurveyModel, Message = "Failed Process, contact your technical support" });
            }
        }
    }
}
