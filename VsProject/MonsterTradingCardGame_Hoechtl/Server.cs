


namespace MonsterTradingCardGame_Hoechtl
{
    using MonsterTradingCardGame_Hoechtl.Handler;
    using MonsterTradingCardGame_Hoechtl.Models;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    internal class Server
    {
        public IEnumerable<IHandler> Handlers { get; private set; }

        private const string address = "127.0.0.1";
        private const int port = 8000;
        private readonly HandlerMethodResolver handlerMethodResolver;
        private int requestCount = 0;

        public Server(IEnumerable<IHandler> handlerModules, HandlerMethodResolver handlerMethodResolver)
        {
            this.Handlers = handlerModules;
            this.handlerMethodResolver = handlerMethodResolver;
        }

        public void Start()
        {
            TcpListener httpServer = new TcpListener(IPAddress.Any, port);
            httpServer.Start();

            Console.WriteLine($"Server listens on {address}:{port}...");
            while (true)
            {
                TcpClient client = httpServer.AcceptTcpClient();

                requestCount++;

                // reader.close would lead also to writer.close
                StreamReader reader = new StreamReader(client.GetStream());
                StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = false };

                HttpRequest request = new HttpRequest(reader);
                Console.WriteLine(request);

                string handlerName = GetHandlerNameFromRequest(request);
                IHandler handler = GetHandlerByName(handlerName);

                HttpResponse response = new HttpResponse(404, "Not Found", string.Empty);
                if (handler != null)
                {
                    response = handlerMethodResolver.InvokeHandlerMethod(handler, request.Method, request.PathData, request.Content, request.AuthenticationToken);
                }

                response.SendOn(writer);
            }
        }

        private IHandler GetHandlerByName(string name)
        {
            foreach(IHandler handler in Handlers)
            {
                if(handler.ModuleName.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return handler;
                }
            }

            return null;
        }

        private string GetHandlerNameFromRequest(HttpRequest request)
        {
            return request.PathData[0];
        }
    }
}
