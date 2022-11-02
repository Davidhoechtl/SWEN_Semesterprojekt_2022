
namespace MonsterTradingCardGame_Hoechtl.Infrastructure
{
    internal class ResponseMessage
    {
        public int StatusCode { get; }
        public string Message { get; }

        public string Content { get; set; }

        public ResponseMessage(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
