namespace MonsterTradingCardGame_Hoechtl.Models
{
    internal class HttpResponse
    {
        public int StatusCode { get; }
        public string ResponseMessage { get; set; }
        public string Content { get; set; } = "{}";

        public string ContentType { get; set; } = "application/json";
        public int ContentLength => Content?.Length ?? 0;

        public HttpResponse(int statusCode, string message, string content = "")
        {
            StatusCode = statusCode;
            Content = content;
            ResponseMessage = message;
        }

        public void SendOn(StreamWriter writer)
        {
            writer.WriteLine($"HTTP/1.1 {StatusCode} {ResponseMessage}");
            if (ContentLength > 0)
            {
                writer.WriteLine($"Content-Type:{ContentType}");
                writer.WriteLine($"Content-Length:{ContentLength}");
            }

            writer.WriteLine();
            writer.WriteLine(Content);
            writer.Flush();
            writer.Close();
        }

        public static HttpResponse GetSuccessResponse()
        {
            return new HttpResponse(200, "Successfully performed Action.");
        }

        public static HttpResponse GetUnauthorizedResponse()
        {
            return new HttpResponse(401, "Unauthorized", string.Empty);
        }

        public static HttpResponse GetNotFoundResponse()
        {
            return new HttpResponse(404, "Not Found");
        }

        public static HttpResponse GetInternalServerErrorResponse()
        {
            return new HttpResponse(500, "Internal Server Error");
        }
    }
}
