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
        private readonly string _defaultToken = "cwln3Z2MWFo:APA91bHejttas_XngT6GydOXcFYXsywgeYJTJAtv-_7WMBsSMSNEsKu3j3obuiRpXtwADo5i3ViyX76rAPFDJXd3v4P8BA1aW2mvBSxoTGrbBvx4EIdnBCNsauorC6zrTFf0YSSZ3oJg";


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
            if (string.IsNullOrWhiteSpace(token))
            {
                token = _defaultToken;
            }

            var result = await messaging.SendAsync(CreateNotification(title, body, token));
        }

    }
}
