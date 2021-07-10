using FireSharp.Config;
using FireSharp.Interfaces;
using System;

namespace ExpenseTracker.Data
{
    public class FirebaseDB
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = Environment.GetEnvironmentVariable("Firebase:Auth"),
            BasePath = Environment.GetEnvironmentVariable("Firebase:Base")
        };

        public IFirebaseClient Client { get; set; }

        public FirebaseDB()
        {
            Client = new FireSharp.FirebaseClient(config);
        }
    }
}