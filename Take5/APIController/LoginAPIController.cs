using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Take5.Models.Models;
using Take5.Models.Models.MasterModels;
using Take5.Services.Contracts;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.MasterModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Take5.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginAPIController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly SignInManager<AspNetUser> _signInManager;

        private readonly UserManager<AspNetUser> _userManager;

        private readonly ITripService _tripService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly ITripJobsiteService _tripJobsiteService;

        public LoginAPIController(IDriverService driverService,
            SignInManager<AspNetUser> signInManager,
            UserManager<AspNetUser> userManager,
            ITripService tripService,
            IUserNotificationService userNotificationService,
            ITripJobsiteService tripJobsiteService)
        {
            _driverService = driverService;
            _signInManager = signInManager;
            _userManager = userManager;
            _tripService = tripService;
            _userNotificationService = userNotificationService;
            _tripJobsiteService = tripJobsiteService;
        }

        [HttpPost("userLogin")]
        public async Task<ActionResult> UserLogin(LoginModel loginModel)
        {
            DriverModel driver = _driverService.GetDriver(loginModel.DriverNumber);
            if (driver != null)
            {
                AspNetUser aspNetUser = await _userManager.FindByIdAsync(driver.UserId);
                if (aspNetUser != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(aspNetUser.Email, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        HomeAPIAPIModel homeAPIAPIModel = new HomeAPIAPIModel();
                        UserAPIModel UserAPIModel = new UserAPIModel();
                        UserAPIModel.DriverId = driver.Id;
                        UserAPIModel.DriverName = driver.FullName;
                        UserAPIModel.UserId = aspNetUser.Id;
                        UserAPIModel.UserUnSeenNotificationCount = _userNotificationService.GetUserUnseenNotificationCount(aspNetUser.Id);
                        homeAPIAPIModel.UserAPIModel = UserAPIModel;
                        //TripJobsiteModel tripJobsiteModel = _tripJobsiteService.GetCurrentJobsiteTripForDriver(driver.Id);
                        //if (tripJobsiteModel != null)
                        //{
                        //    List<TripJobsiteModel> tripJobsiteModels = new List<TripJobsiteModel>();
                        //    tripJobsiteModels.Add(tripJobsiteModel);
                        //    List<TripAPIModel> tripAPIModels = _tripService.ConvertFromTripJobsiteModelToTripAPIModel(tripJobsiteModels);
                        //    if (tripAPIModels != null)
                        //    {
                        //       // homeAPIAPIModel.PendingTrip = tripAPIModels.First();
                        //    }
                        //}
                        return Ok(new { Data = homeAPIAPIModel, Message = "Successful Process" });
                    }
                }
            }
            return BadRequest(new { Data = 0, Message = "رقم المستخدم أو كلمة السر خطأ" });
        }

        [HttpPost("GetPendingTrip")]
        public async Task<ActionResult> GetPendingJobsiteTripForDriver(string userId)
        {
            AspNetUser user = await _userManager.FindByIdAsync(userId);
            if(user != null)
            {
               Driver driver = _driverService.GetDriverByUserId(user.Id);
                if(driver != null)
                {
                    TripJobsiteModel tripJobsiteModel = _tripJobsiteService.GetJobsitePendingTripForDriver(driver.Id);
                    if (tripJobsiteModel != null)
                    {
                        List<TripJobsiteModel> tripJobsiteModels = new List<TripJobsiteModel>();
                        tripJobsiteModels.Add(tripJobsiteModel);
                        List<TripAPIModel> tripAPIModels = _tripService.ConvertFromTripJobsiteModelToTripAPIModel(tripJobsiteModels);
                        if (tripAPIModels != null)
                        { 
                            return Ok(new { Data = tripAPIModels.First(), Message = "Successful Process" });
                        }
                        else
                        {
                            return Ok(new { Data = 0, Message = "Failed Process, error in the system" });
                        }
                    }
                    else
                    {
                        return Ok(new { Data = 0, Message = "Failed Process, there is no pending Trips" });
                    }
                }
                else
                {
                    return BadRequest(new { Data = 0, Message = "Failed Process, Invalid Data" });
                }
            }
            else
            {
                return BadRequest(new { Data = 0, Message = "Failed Process, Invalid Data" });
            }
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult> UpdateToken(string userId, string newToken)
        {
            DriverModel driverModel = _driverService.GetDriverModelByUserId(userId);
            bool result = false;
            if (driverModel != null)
            {
                driverModel.UserToken = newToken;

                result = _driverService.UpdateDriver(driverModel).Result;
                if (result == true)
                {
                    return Ok(new { Message = "sucess process", Data = true });
                }
                else
                {
                    return BadRequest(new { Message = "Failed Process", Data = false });
                }
            }
            else
            {
                return BadRequest(new { Message = "Failed Process, invalid Driver Data", Data = false });
            }
        }

        //[HttpPost("GetPendingTrip")]
        //public async Task<ActionResult> SendRequestToCompleteTake5StepTwo()
        //{
        //    AspNetUser user = await _userManager.FindByIdAsync(userId);
        //    if (user != null)
        //    {
        //        Driver driver = _driverService.GetDriverByUserId(user.Id);
        //        if (driver != null)
        //        {
        //            TripJobsiteModel tripJobsiteModel = _tripService.GetJobsitePendingTripForDriver(driver.Id);
        //            if (tripJobsiteModel != null)
        //            {
        //                TripAPIModel tripAPIModel = _tripService.ConvertFromTripJobsiteModelToTripAPIModel(tripJobsiteModel);
        //                if (tripAPIModel != null)
        //                {
        //                    return Ok(new { Data = tripAPIModel, Message = "Successful Process" });
        //                }
        //                else
        //                {
        //                    return Ok(new { Data = 0, Message = "Failed Process, error in the system" });
        //                }
        //            }
        //            else
        //            {
        //                return Ok(new { Data = 0, Message = "Failed Process, there is no pending Trips" });
        //            }
        //        }
        //        else
        //        {
        //            return BadRequest(new { Data = 0, Message = "Failed Process, Invalid Data" });
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest(new { Data = 0, Message = "Failed Process, Invalid Data" });
        //    }
        //}

        // GET: api/<LoginAPIController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<LoginAPIController>/5

    }
}
