using CheeseIT.BusinessLogic.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

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

    }
}
