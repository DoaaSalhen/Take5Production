using AutoMapper;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Take5.Models.Models;
using Take5.Models.Models.MasterModels;
using Take5.Services.Contracts;
using Take5.Services.Models;
using Take5.Services.Models.hub;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Notification, long> _repository;
        private readonly IMapper _mapper;
        private readonly IUserNotificationService _userNotificationService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly IUserConnectionManager _userConnectionManager;
        private readonly ITripJobsiteService _tripJobsiteService;

        public NotificationService(IRepository<Notification, long> repository,
            IMapper mapper,
            IUserNotificationService userNotificationService,
            UserManager<AspNetUser> userManager,
            IUserConnectionManager userConnectionManager,
            IHubContext<NotificationHub> hub,
            ITripJobsiteService tripJobsiteService)
        {
            _repository = repository;
            _mapper = mapper;
            _userNotificationService = userNotificationService;
            _userManager = userManager;
            _userConnectionManager = userConnectionManager;
            _hub = hub;
            _tripJobsiteService = tripJobsiteService;
        }


        public Notification CreateNotification(string message, int notificationTypeId, long tripId, long jobsiteId)
        {
            try
            {
                Notification notification = new Notification();
                notification.Message = message;
                notification.CreatedDate = DateTime.Now;
                notification.UpdatedDate = DateTime.Now;
                notification.IsVisible = true;
                notification.IsDelted = false;
                notification.TripId = tripId;
                notification.JobSiteId = jobsiteId;
                notification.NotificationTypeId = notificationTypeId;
                Notification addedNotification = _repository.Add(notification);
                
                return addedNotification;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public List<NotificationModel> GetAllUnseenNotificationsForUser(string userId)
        {
            throw new NotImplementedException();
        }

        public NotificationModel GetNotification(long id)
        {
            try
            {
               var notification = _repository.Find(n => n.IsVisible == true && n.Id == id, false, n => n.NotificationType).FirstOrDefault();
               NotificationModel notificationModel = _mapper.Map<NotificationModel>(notification);
               return notificationModel;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public List<NotificationModel> GetUnseenNotifications()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateNotification(NotificationModel model)
        {
            throw new NotImplementedException();
        }


        public async Task<bool> PushNotificationToRole(NotificationModel notification, string roleName, DashBoardTripModel dashBoardTripModel)
        {
            try
            {
                notification.CreatedDateText = notification.CreatedDate.ToString("yyyy-MM-dd");
                notification.CreatedTimeText = notification.CreatedDate.ToString("HH-mm-ss");
                notification.NotificationImageUrl = CommanData.NotificationIconFolder + notification.NotificationType.ImgUrl;
                notification.NotificationTypeName = notification.NotificationType.Name;
                notification.NotificationType = null;
                var message = notification.Message;
                notification.Message = "";
                var result = JsonSerializer.Serialize(dashBoardTripModel);
                var notificationModelData = JsonSerializer.Serialize(notification);
                var aspNetUsers = _userManager.GetUsersInRoleAsync(roleName).Result;
                var aspNetUsersAdmin = _userManager.GetUsersInRoleAsync("Admin").Result;
                List<AspNetUser> users = new List<AspNetUser>();
                if(aspNetUsers.Count >0)
                {
                    users.AddRange(aspNetUsers);
                }
                if (aspNetUsersAdmin.Count > 0)
                {
                    users.AddRange(aspNetUsersAdmin);
                }
                if (users.Count > 0)
                {
                    foreach (var user in users)
                    {
                        var connections = _userConnectionManager.GetUserConnections(user.Id);
                        if (connections != null && connections.Count > 0)
                        {
                            foreach (var connectionId in connections)
                            {
                                await _hub.Clients.Client(connectionId).SendAsync("sendToUser", result, message, notificationModelData);
                            }
                        }
                    }
                }
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }


        public async Task<string> HandleNotificationToRole(string notificationMessage , int notificationTypeId, string roleName, long tripId, long jobsiteId)
        {
            int NotificationSendToCount = 0;
            string message = "";
            try
            {
                Notification addedNotification = CreateNotification(notificationMessage, notificationTypeId, tripId, jobsiteId);
                if (addedNotification != null)
                {
                    NotificationSendToCount = _userNotificationService.AssignUserNotificationForRoleUsers(addedNotification.Id, roleName).Result;
                    var users = _userManager.GetUsersInRoleAsync(roleName);
                    if (users.Result.Count == NotificationSendToCount)
                    {
                        message = "Successful Process";
                    }
                    else
                    {
                        message = "Failed Process, There is an error. Message sent to " + NotificationSendToCount + " from " + users.Result.Count;
                    }
                }
                else
                {
                    message = "Failed Process, can not create Notification";
                }
                NotificationModel notificationModel = GetNotification(addedNotification.Id);
                DashBoardTripModel dashBoardTripModel = null;
                bool notifyResult = false;
                TripJobsiteModel tripJobsiteModel = _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteId(tripId, jobsiteId);
                if (tripJobsiteModel != null && (notificationModel.NotificationTypeId >= (int)CommanData.NotificationTypes.TripStarted  && notificationModel.NotificationTypeId <= (int)CommanData.NotificationTypes.StepTwoCompleted))
                {
                    List<TripJobsiteModel> tripJobsiteModels = new List<TripJobsiteModel>();
                    tripJobsiteModels.Add(tripJobsiteModel);
                    dashBoardTripModel = _tripJobsiteService.ConvertFromTripJobsiteModelToDashBoardTripModel(tripJobsiteModels).FirstOrDefault();
                    notifyResult = PushNotificationToRole(notificationModel, roleName, dashBoardTripModel).Result;
                }
                else
                {
                    notifyResult = PushNotificationToRole(notificationModel, roleName, null).Result;
                }
                if (notifyResult == false)
                {
                    message = "Failed Process, can not push notification to supervisor";
                }
                return message;
            }
            catch(Exception e)
            {
                message = "Failed process, contact your system support";
                return message;
            }
        }
    }
}
