using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Contracts
{
    public interface IUserNotificationService
    {
        Task<UserNotificationModel> CreateUserNotification(long notificationId, string userId);

        List<UserNotificationModel> GetUserNotification(string userId);

        List<UserNotificationModel> GetFiftyUserNotification(string userId);

        bool UpdateUserNotification(UserNotificationModel model);

        int GetUserUnseenNotificationCount(string userId);
        UserNotificationModel GetUserNotificationByUserIdAndNotificationId(string userId, long notificationId);
        Task<int> AssignUserNotificationForRoleUsers(long NotificationId, string RoleName);

    }
}
