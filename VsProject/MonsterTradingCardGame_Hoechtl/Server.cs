


namespace MonsterTradingCardGame_Hoechtl
{
    using MonsterTradingCardGame_Hoechtl.Handler;
    using MonsterTradingCardGame_Hoechtl.Infrastructure;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    internal class Server
    {
        public IEnumerable<IHandler> Handlers { get; private set; }

        private const string address = "127.0.0.1";
        private const int port = 8000;
        private int requestCount = 0;

        public Server(IEnumerable<IHandler> handlerModules)
        {
            this.Handlers = handlerModules;
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

                HttpResponse response = null;
                IHandler handler = GetHandlerByName(request.RequestPathData[0]);
                if(handler != null)
                {
                    response = handler.HandlerAction(request.RequestPathData.Skip(1).ToString());
                }
                else
                {
                    response = new HttpResponse(404, "Not Found", string.Empty);
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
    }
}
