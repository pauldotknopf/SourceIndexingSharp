using System;

namespace SourceIndexingSharp.Indexing.Stash
{
    public class StashCredentials
    {
        public StashCredentials(string userName, string password)
        {
            if(string.IsNullOrEmpty(userName))
                throw new ArgumentOutOfRangeException("userName");

            if(string.IsNullOrEmpty(password))
                throw new ArgumentOutOfRangeException("password");

            UserName = userName;
            Password = password;
        }

        public string UserName { get; private set; }

        public string Password { get; private set; }
    }
}
