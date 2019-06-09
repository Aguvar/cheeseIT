using CheeseIT.BusinessLogic.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;

namespace CheeseIT.BusinessLogic
{
    public class MobileMessagingService : IMobileMessagingService
    {
        private readonly FirebaseMessaging messaging;

        public MobileMessagingService()
        {
            FirebaseApp app = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("cheeseit-firebase-adminsdk.json").CreateScoped("https://www.googleapis.com/auth/firebase.messaging"),
            });

            messaging = FirebaseMessaging.GetMessaging(app);
        }

        private Message CreateNotification(string title, string notificationBody, string token)
        {
            return new Message()
            {
                Token = token,
                Notification = new Notification()
                {
                    Body = notificationBody,
                    Title = title
                }
            };
        }

        public async Task SendNotification(string token, string title, string body)
        {
            var result = await messaging.SendAsync(CreateNotification(title, body, token));
            //do something with result
        }

    }
}
