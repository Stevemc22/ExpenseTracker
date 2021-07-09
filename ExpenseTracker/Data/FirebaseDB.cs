using FireSharp.Config;
using FireSharp.Interfaces;

namespace ExpenseTracker.Data
{
    public class FirebaseDB
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "",
            BasePath = ""
        };

        public IFirebaseClient Client { get; set; }

        public FirebaseDB()
        {
            Client = new FireSharp.FirebaseClient(config);
        }
    }
}