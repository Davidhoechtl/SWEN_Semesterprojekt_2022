
namespace MonsterTradingCardGame_Hoechtl.Infrastructure
{
    using MonsterTradingCardGame_Hoechtl.Models;
    using System;

    internal class SessionService
    {
        private const string adminKey = "adminKey";
        private Dictionary<int, SessionKey> sessionKeys = new();

        public SessionService()
        {
            // Add admin token
            sessionKeys.Add(1, new SessionKey(adminKey, DateTime.MaxValue, Permission.Admin));
        }

        public Guid CreateNewSessionKey(int user_Id)
        {
            Guid guid = Guid.NewGuid();
            sessionKeys.Add(user_Id, new SessionKey(guid.ToString(), DateTime.Now.AddHours(3), Permission.User));
            return guid;
        }

        public Permission GetPermissonOfKey(string sessionKey)
        {
            SessionKey found = GetValidSessionKeyById(sessionKey);

            return found?.Permission ?? Permission.None;
        }

        //public bool HasPermission(string sessionId, PermissionAttribute permissionAttribute)
        //{
        //    SessionKey found = GetValidSessionKeyById(sessionId);

        //    switch (permissionAttribute?.RequiredPermission)
        //    {
        //        case Permission.None: return true;
        //        case Permission.Admin:
        //            if (found?.Permission == Permission.Admin)
        //            {
        //                return true;
        //            }
        //            return false;
        //        default:
        //            if (found?.Permission == Permission.User || found?.Permission == Permission.Admin)
        //            {
        //                return true;
        //            }
        //            return false;
        //    }
        //}

        public SessionKey GetValidSessionKeyById(string sessionKey)
        {
            SessionKey found = sessionKeys
                .Select(pair => pair.Value)
                .FirstOrDefault(key => key.Id.Equals(sessionKey));

            if (found != null &&
                found.ExpirationDate >= DateTime.Now)
            {
                return found;
            }

            return null;
        }

        public bool IsValidUsersOrAdminKey(string sessionKey, int userId)
        {
            if( sessionKeys.TryGetValue(userId, out SessionKey userKey))
            {
                if (userKey != null)
                {
                    return userKey.Id.Equals(sessionKey);
                }
            }

            return adminKey.Equals(sessionKey);
        }

        public SessionContext CreateSessionContext(string sessionKey)
        {
            return new SessionContext() {
                SessionKey = GetValidSessionKeyById(sessionKey), 
                UserId = GetUserIdBySessionKey(sessionKey) 
            };
        }

        private int? GetUserIdBySessionKey( string sessionKey )
        {
            return sessionKeys.FirstOrDefault(pair => pair.Value.Id.Equals(sessionKey)).Key;
        }
    }
}
