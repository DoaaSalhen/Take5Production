using AutoMapper;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Take5.Models.Models;
using Take5.Models.Models.MasterModels;
using Take5.Services.Contracts;
using Take5.Services.Models;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Implementation
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly UserManager<AspNetUser> _userManager;

        private readonly IRepository<UserNotification, long> _repository;
        private readonly ILogger<UserNotificationService> _logger;
        private readonly IMapper _mapper;

        public UserNotificationService(IRepository<UserNotification, long> repository,
            ILogger<UserNotificationService> logger,
            IMapper mapper,
            UserManager<AspNetUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<UserNotificationModel> CreateUserNotification(long notificationId, string userId)
        {
            try
            {
                UserNotification userNotification = new UserNotification();
                userNotification.NotificationId = notificationId;
                userNotification.userId = userId;
                userNotification.CreatedDate = DateTime.Now;
                userNotification.UpdatedDate = DateTime.Now;
                userNotification.IsDelted = false;
                userNotification.IsVisible = true;
                UserNotification addedUserNotification = _repository.Add(userNotification);
                if(addedUserNotification != null)
                {
                    UserNotificationModel userNotificationModel = _mapper.Map<UserNotificationModel>(addedUserNotification);
                    return userNotificationModel;
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

        public async Task<int> AssignUserNotificationForRoleUsers(long NotificationId, string roleName)
        {
            int addedUserNotificationCount = 0;
            try
            {
                var aspNetUsers = _userManager.GetUsersInRoleAsync(roleName).Result;
                var aspNetUsersAdmin = _userManager.GetUsersInRoleAsync("Admin").Result;
                List<AspNetUser> users = new List<AspNetUser>();
                if (aspNetUsers.Count > 0)
                {
                    users.AddRange(aspNetUsers);
                }
                if (aspNetUsersAdmin.Count > 0)
                {
                    users.AddRange(aspNetUsersAdmin);
                }
                if (users.Count > 0)
                {
                    
                    foreach(var user in users)
                    {
                        UserNotification userNotification= new UserNotification();
                        userNotification.userId = user.Id;
                        userNotification.NotificationId = NotificationId;
                        userNotification.IsVisible = true;
                       
                        if (_repository.Add(userNotification) != null)
                        {
                            addedUserNotificationCount = addedUserNotificationCount + 1;
                        }
                    }
                }
                return addedUserNotificationCount;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public List<UserNotificationModel> GetFiftyUserNotification(string userId)
        {
            throw new NotImplementedException();
        }

        public List<UserNotificationModel> GetUserNotification(string userId)
        {
            try
            {
                List<UserNotificationModel> userNotificationModels = new List<UserNotificationModel>();
                List<UserNotification> userNotifications = _repository.Find(UN => UN.userId == userId && UN.IsVisible == true, false, UN => UN.Notification, UN => UN.Notification.NotificationType).ToList();
                if (userNotifications != null)
                {
                    userNotificationModels = _mapper.Map<List<UserNotificationModel>>(userNotifications);
                    userNotificationModels.ForEach(un => un.NotificationImageUrl = CommanData.NotificationIconFolder + un.Notification.NotificationType.ImgUrl);
                    if(userNotificationModels.Count > 300)
                    {
                        userNotificationModels = userNotificationModels.TakeLast(300).ToList();
                    }
                }
                return userNotificationModels;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public UserNotificationModel GetUserNotificationByUserIdAndNotificationId(string userId, long notificationId)
        {
            throw new NotImplementedException();
        }

        public int GetUserUnseenNotificationCount(string userId)
        {
            try
            {
                var notifications = _repository.Find(un => un.userId == userId && un.Seen == false);
                if (notifications != null)
                {
                    return notifications.Count();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return 0;
        }

        public bool UpdateUserNotification(UserNotificationModel model)
        {
            throw new NotImplementedException();
        }
    }
}
