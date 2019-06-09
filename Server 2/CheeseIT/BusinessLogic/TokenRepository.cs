namespace CheeseIT.BusinessLogic
{
    public class TokenRepository
    {
        static private TokenRepository _instance;
        public string FirebaseToken { get; set; }

        private TokenRepository()
        {
            FirebaseToken = "";
        }

        public static TokenRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TokenRepository();
            }

            return _instance;
        }

    }
}
