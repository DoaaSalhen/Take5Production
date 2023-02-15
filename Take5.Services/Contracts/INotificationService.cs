using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Take5.Models.Models.MasterModels;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Contracts
{
    public interface INotificationService
    {
        Notification CreateNotification(string message, int notificationTypeId, long tripId, long jobsiteId);

        NotificationModel GetNotification(long id);

        Task<bool> UpdateNotification(NotificationModel model);

        List<NotificationModel> GetUnseenNotifications();
        List<NotificationModel> GetAllUnseenNotificationsForUser(string userId);
        Task<bool> PushNotificationToRole(NotificationModel notification, string roleName, DashBoardTripModel dashBoardTripModel);
        Task<string> HandleNotificationToRole(string notificationMessage, int notificationTypeId, string roleName, long tripId, long jobsiteId);

    }
}
