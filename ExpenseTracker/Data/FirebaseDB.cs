using FireSharp.Config;
using FireSharp.Interfaces;
using System;

namespace ExpenseTracker.Data
{
    public class FirebaseDB
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "OzHkf7qdEa6jsvrYcKOyt3vbPAbgIrs8ZPaBJS72",
            BasePath = "https://appexpensetracker-cenfo-default-rtdb.firebaseio.com/"
        };

        public IFirebaseClient Client { get; set; }

        public FirebaseDB()
        {
            Client = new FireSharp.FirebaseClient(config);
        }
    }
}