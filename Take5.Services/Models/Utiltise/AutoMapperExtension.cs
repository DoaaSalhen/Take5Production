using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Take5.Models.Auth;
using Take5.Models.Models;
using Take5.Models.Models.MasterModels;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.MasterModels;

namespace MoreForYou.Services.Models.Utilities.Mapping
{
    public static class AutoMapperExtension
    {
        public static void ConfigAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AssembleType));
        }

        public class AutoMapperProfiles : Profile
        {
            public AutoMapperProfiles()
            {

                CreateMap<Question, QuestionModel>();
                CreateMap<QuestionModel, Question>();
                CreateMap<DangerCategoryModel, DangerCategory>();
                CreateMap<DangerCategory, DangerCategoryModel>();
                CreateMap<Danger, DangerModel>();
                CreateMap<DangerModel, Danger>();
                CreateMap<Truck, TruckModel>();
                CreateMap<TruckModel, Truck>();
                CreateMap<Driver, DriverModel>();
                CreateMap<DriverModel, Driver>();
                CreateMap<JobSite, JobSiteModel>();
                CreateMap<JobSiteModel, JobSite>();
                CreateMap<Trip, TripModel>();
                CreateMap<TripModel, Trip>();
                CreateMap<AspNetRole, RoleModel>();
                CreateMap<RoleModel, AspNetRole>();
                CreateMap<AspNetUser, UserModel>();
                CreateMap<UserModel, AspNetUser>();
                CreateMap<MeasureControl, MeasureControlModel>();
                CreateMap<MeasureControlModel, MeasureControl>();
                CreateMap<Employee, EmployeeModel>();
                CreateMap<EmployeeModel, Employee>();
                CreateMap<Notification, NotificationModel>();
                CreateMap<NotificationModel, Notification>();
                CreateMap<UserNotification, UserNotificationModel>();
                CreateMap<UserNotificationModel, UserNotification>();
                CreateMap<TripJobsiteModel, TripJobsite>();
                CreateMap<TripJobsite, TripJobsiteModel>();
                CreateMap<SurveyStepTwoModel,  SurveyStepOneModel> ();
                CreateMap<StepTwoRequestModel, TripJobsite>();
                CreateMap<TripJobsite, StepTwoRequestModel>();
                CreateMap<SurveyStepOneAnswersAPIModel, SurveyStepTwoAnswersAPIModel > ();
                CreateMap<TripCancellation, TripCancellationModel>();
                CreateMap<TripCancellationModel, TripCancellation>();
                CreateMap<Trip, TripCancellation>();
                CreateMap<DriverAPIModel, Driver>();
                CreateMap<Driver, DriverAPIModel>();

                CreateMap<TripDanger, TripDangerModel>();
                CreateMap<TripDangerModel, TripDanger>();

                CreateMap<TripTake5Together, TripTake5TogetherModel>();
                CreateMap<TripTake5TogetherModel, TripTake5Together>();

                CreateMap<TripJobsiteWarning, TripJobsiteWarningModel>();
                CreateMap<TripJobsiteWarningModel, TripJobsiteWarning>();

            }
        }
    }
    public class AssembleType
    {
    }
}