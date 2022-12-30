
namespace MonsterTradingCardGame_Hoechtl.Infrastructure
{
    using MonsterTradingCardGame_Hoechtl.Handler.PremissionAttributes;
    using MonsterTradingCardGame_Hoechtl.Models;

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

        public bool HasPermission(string sessionId, PermissionAttribute permissionAttribute)
        {
            SessionKey found = GetValidSessionKeyById(sessionId);

            switch (permissionAttribute?.RequiredPermission)
            {
                case Permission.None: return true;
                case Permission.Admin:
                    if (found?.Premission == Permission.Admin)
                    {
                        return true;
                    }
                    return false;
                default:
                    if (found?.Premission == Permission.User || found?.Premission == Permission.Admin)
                    {
                        return true;
                    }
                    return false;
            }
        }

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

        public bool IsUsersKeyOrAdmin(string sessionKey, int userId)
        {
            if(sessionKeys.TryGetValue(userId, out SessionKey key))
            {
                return key.Id.Equals(sessionKey);
            }

            return adminKey.Equals(sessionKey);
        }

        public SessionContext CreateSessionContext(string sessionKey)
        {
            return new SessionContext() { SessionKey = GetValidSessionKeyById(sessionKey) };
        }
    }
}
