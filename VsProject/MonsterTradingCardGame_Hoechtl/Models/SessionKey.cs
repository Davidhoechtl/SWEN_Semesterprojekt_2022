
namespace MonsterTradingCardGame_Hoechtl.Models
{
    internal class SessionKey
    {
        public string Id { get; set; }
        public DateTime ExpirationDate { get; set; }

        public Permission Permission { get; set; }

        public SessionKey(string id, DateTime expirationDate, Permission premission)
        {
            Id = id;
            ExpirationDate = expirationDate;
            Permission = premission;
        }
    }
}
