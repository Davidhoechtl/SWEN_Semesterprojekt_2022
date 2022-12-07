
namespace MonsterTradingCardGame_Hoechtl.Infrastructure
{
    internal class HttpResponse
    {
        public int StatusCode { get; }
        public string ResponseMessage { get; set; }
        public string Content { get; set; } = "{}";

        public string ContentType { get; set; } = "application/json";
        public int ContentLength => Content?.Length ?? 0;

        public HttpResponse(int statusCode, string message, string content)
        {
            StatusCode = statusCode;
            Content = content;
            ResponseMessage = message;
        }

        public void SendOn(StreamWriter writer)
        {
            writer.WriteLine($"HTTP/1.1 {StatusCode} {ResponseMessage}");
            if(ContentLength > 0)
            {
                writer.WriteLine($"Content-Type:{ContentType}");
                writer.WriteLine($"Content-Length:{ContentLength}");
            }

            writer.WriteLine();
            writer.WriteLine(Content);
            writer.Flush();
            writer.Close();
        }
    }
}
