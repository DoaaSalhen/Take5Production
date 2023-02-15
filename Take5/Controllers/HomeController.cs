using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Take5.Models;
using Take5.Models.Models;
using Take5.Services.Contracts;
using Take5.Services.Models;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.MasterModels;
using Take5.Services.Models.SpecificModels;

namespace Take5.Controllers
{
    [Authorize(Roles = "Supervisor, Admin")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ITripService _tripService;
        private readonly ITripJobsiteService _tripJobsiteService;
        private readonly ITripCancellationService _tripCancellationService;
        private readonly ITripJobsiteWarningService _tripJobsiteWarningService;

        private readonly UserManager<AspNetUser> _usermanager;

        private readonly INotificationService _notificationService;

        public HomeController(ILogger<HomeController> logger,
            ITripService tripService,
            UserManager<AspNetUser> usermanager,
            INotificationService notificationService,
            ITripJobsiteService tripJobsiteService,
            ITripCancellationService tripCancellationService,
            ITripJobsiteWarningService tripJobsiteWarningService)
        {
            _logger = logger;
            _tripService = tripService;
            _usermanager = usermanager;
            _notificationService = notificationService;
            _tripJobsiteService = tripJobsiteService;
            _tripCancellationService = tripCancellationService;
            _tripJobsiteWarningService = tripJobsiteWarningService;
        }

        public IActionResult Index()
        {
            try
            {

                HomeModel homeModel = new HomeModel();
                homeModel.TripStatuscounts = _tripJobsiteService.GetTripsCountByStatus();
                List<TripJobsiteModel> inProgressTrips = _tripJobsiteService.GetHomeTrips();
                if (inProgressTrips.Count > 0)
                {
                    //foreach(var trip in inProgressTrips)
                    //{
                    //    if(trip.Trip.Cancelled == true)
                    //    {
                    //        trip.TripCancellationModel = _tripCancellationService.GetTripCancellation(trip.TripId).Result;
                    //    }
                    //    if (trip.TripStatus >= (int)CommanData.TripStatus.StepTwoRequested && trip.TripStatus <= (int)CommanData.TripStatus.TripCompleted)
                    //    {
                    //        var warnings = _tripJobsiteWarningService.GetTripJobsiteWarning(trip.TripId, trip.JobSiteId);
                    //        if (warnings.Count > 0)
                    //        {
                    //            trip.TripJobsiteWarningModels = warnings;
                    //        }
                    //    }
                    //}
                    List<TripJobsiteModel> StartedTrips = inProgressTrips.Where(t => t.TripStatus == (int)CommanData.TripStatus.Started).ToList();
                    List<TripJobsiteModel> DestinationArrivedTrips = inProgressTrips.Where(t => t.TripStatus == (int)CommanData.TripStatus.DestinationArrived).ToList();
                    List<TripJobsiteModel> StepOneCompletedTrips = inProgressTrips.Where(t => t.TripStatus == (int)CommanData.TripStatus.SurveyStepOneCompleted).ToList();
                    List<TripJobsiteModel> StepTwoRequestedTrips = inProgressTrips.Where(t => t.TripStatus == (int)CommanData.TripStatus.StepTwoRequested || t.TripStatus == (int)CommanData.TripStatus.StepTwoResponsed).ToList();
                    List<TripJobsiteModel> StepTwoCompletedTrips = inProgressTrips.Where(t => t.TripStatus == (int)CommanData.TripStatus.SurveyStepTwoCompleted).ToList();
                    if (StartedTrips.Count > 0)
                    {
                        List<DashBoardTripModel> StartedTrips2 = _tripJobsiteService.ConvertFromTripJobsiteModelToDashBoardTripModel(StartedTrips);
                        if (StartedTrips2 != null)
                        {
                            homeModel.StartedTrips = StartedTrips2;
                        }
                    }
                    if(DestinationArrivedTrips.Count >0)
                    {
                        List<DashBoardTripModel> DestinationArrivedTrips2 = _tripJobsiteService.ConvertFromTripJobsiteModelToDashBoardTripModel(DestinationArrivedTrips);
                        if (DestinationArrivedTrips2 != null)
                        {
                            homeModel.DestinationArrivedTrips = DestinationArrivedTrips2;
                        }
                    }
                    if (StepOneCompletedTrips.Count > 0)
                    {
                        List<DashBoardTripModel> StepOneCompletedTrips2 = _tripJobsiteService.ConvertFromTripJobsiteModelToDashBoardTripModel(StepOneCompletedTrips);
                        if (StepOneCompletedTrips2 != null)
                        {
                            homeModel.StepOneCompletedTrips = StepOneCompletedTrips2;
                        }
                    }
                    if (StepTwoRequestedTrips.Count > 0)
                    {

                        homeModel.StepTwoRequestModels = _tripJobsiteService.MapFromTripJobsiteModelToStepTwoRequestModel(StepTwoRequestedTrips); ;
                    }
                    if (StepTwoCompletedTrips.Count > 0)
                    {
                        List<DashBoardTripModel> StepTwoCompletedTrips2 = _tripJobsiteService.ConvertFromTripJobsiteModelToDashBoardTripModel(StepTwoCompletedTrips);
                        if (StepTwoCompletedTrips2 != null)
                        {
                            homeModel.StepTwoCompletedTrips = StepTwoCompletedTrips2;
                        }
                    }
                }

                return View(homeModel);            
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        
    }
}
