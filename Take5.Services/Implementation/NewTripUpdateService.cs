using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Take5.Models.Models.MasterModels;
using Take5.Services.Contracts;
using Take5.Services.Models;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Implementation
{
    public class NewTripUpdateService : INewTripUpdateService
    {
        private readonly ITripJobsiteService _tripJobsiteService;
        private readonly ITripService _tripService;
        private readonly ISurveyService _surveyService;
        private readonly INotificationService _notificationService;
        private readonly IDriverService _driverService;


        public NewTripUpdateService(ITripJobsiteService tripJobsiteService,
            ITripService tripService,
            ISurveyService surveyService,
            INotificationService notificationService,
            IDriverService driverService)
        {
            _tripJobsiteService = tripJobsiteService;
            _tripService = tripService;
            _surveyService = surveyService;
            _notificationService = notificationService;
            _driverService = driverService;

        }
        public async Task<string> AddStepOneAnswers(SurveyStepOneAnswersAPIModel model, string userId)
        {
            //try
            //{
            //    if (model.QuestionAnswerModels != null)
            //    {
            //        bool addResult = _surveyService.AddAnswersToStepNQuestions(model).Result;
            //        if (addResult == true)
            //        {
            //            addResult = _tripJobsiteService.UpdateStepOneCompletion(model).Result;
            //            if (addResult == true)
            //            {
            //                addResult = _tripService.UpdateTripStatus(model.TripId, (int)CommanData.TripStatus.SurveyStepOneCompleted, (int)CommanData.Take5Status.StepOneCompletedOnly).Result;
            //                if (addResult == true)
            //                {
            //                    Driver driver = _driverService.GetDriverByUserId(userId);
            //                    NotificationModel notificationModel = new NotificationModel();
            //                    int notificationTypeId = (int)CommanData.NotificationTypes.SurveyStepOneCompleted;
            //                    string notificationMessage = driver.FullName + " completed survey step one for trip number " + model.TripId + " at " + model.CreatedDate.ToString("yyy-mm-dd") + ", " + model.CreatedDate.ToString("hh:MM:ss");
            //                    string message = _notificationService.HandleNotificationToRole(notificationMessage, notificationTypeId, "Supervisor").Result;

            //                    return message;
            //                }
            //                else
            //                {
            //                    return "Failed Process, update trip status";
            //                }
            //            }
            //            else
            //            {
            //                return "Failed Process, update trip status";
            //            }
            //        }
            //        else
            //        {
            //            return "Failed Process, Can not complete Take 5";
            //        }
            //    }
            //    else
            //    {
            //        return "Wrong Answers";

            //    }
            //}
            //catch(Exception e)
            //{
            //    return null;
            //}
            return null;
        }

        public async Task<string> StartTrip(TripStartingModel tripStartingModel)
        {
            bool updateResult = false;
            try
            {
                updateResult = _tripJobsiteService.SatrtTripJobsite(tripStartingModel).Result;
                if (updateResult == true)
                {
                    TripModel tripModel = _tripService.GetTrip(tripStartingModel.TripId);
                    if (tripModel != null)
                    {
                        updateResult = _tripService.UpdateTripStatus(tripStartingModel.TripId, (int)CommanData.TripStatus.Started).Result;
                        if (updateResult == true)
                        {
                           
                            return "Successful Process";
                        }
                        else
                        {
                            return "Failed Process, can not update trip status";
                        }
                    }
                    else
                    {
                        return "Failed Process, cinvalid trip";
                    }
                }
                else
                {
                    return "Failed Process, can not update trip status";
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Task<string> TripDestinationArrived(TripDestinationArrivedModel tripDestinationArrivedModel, long tripId, long jobsiteId)
        {
            bool updateResult = false;
            try
            {
                updateResult = _tripJobsiteService.ArrivingTripJobsite(tripDestinationArrivedModel, tripId, jobsiteId).Result;
                if (updateResult == true)
                {
                    TripJobsiteModel tripJobsiteModel = _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteId(tripId, jobsiteId);
                    TripModel tripModel = _tripService.GetTrip(tripId);
                    if (tripModel != null)
                    {
                        updateResult = _tripService.UpdateTripStatus(tripId, (int)CommanData.TripStatus.DestinationArrived).Result;

                        if (updateResult == true)
                        {
                            NotificationModel notificationModel = new NotificationModel();
                            int notificationTypeId = (int)CommanData.NotificationTypes.DestinationArrived;
                            string notificationMessage = tripModel.Driver.FullName + " with Trip Number " + tripModel.Id + " arrived his Destination at" + tripJobsiteModel.JobSite.Name + " at " + tripDestinationArrivedModel.DestinationArrivedDate.ToString("yyyy-MM-dd") + ", " + tripDestinationArrivedModel.DestinationArrivedDate.ToString("HH:mm:ss");
                            string message = _notificationService.HandleNotificationToRole(notificationMessage, notificationTypeId, "SuperVisor", tripId, jobsiteId).Result;
                            return Task<string>.FromResult("Successful Process");
                        }
                        else
                        {
                            return Task<string>.FromResult("Failed Process, falied to update trip");
                        }
                    }
                    else
                    {
                        return Task<string>.FromResult("Failed Process, wrong Trip");
                    }
                }
                else
                {
                    return Task<string>.FromResult("Failed Process, falied to update trip");
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }



    }
}
