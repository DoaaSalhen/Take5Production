using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Take5.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Take5.Services.Implementation
{
    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        private readonly FirebaseMessaging messaging;

        public FirebaseNotificationService()
        {
            var app = FirebaseApp.DefaultInstance;
            if (app == null)
            {
                app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("take5app-742d3-firebase-adminsdk-8rzwz-1d93d7737c.json").
                    CreateScoped("https://www.googleapis.com/auth/firebase.messaging")
                });
            }
            messaging = FirebaseMessaging.GetMessaging(app);

        }
        public Message CreateNotification(string title, string notificationBody, string token)
        {
            var message = new Message()
            {
                Token = token,
                //Topic = "news",
                Notification = new Notification()
                {
                    Body = notificationBody,
                    Title = title,
                }
            };



            return message;
        }

        public async Task SendNotification(string token, string title, string body)
        {
            var result = await messaging.SendAsync(CreateNotification(title, body, token));
        }
    }
}