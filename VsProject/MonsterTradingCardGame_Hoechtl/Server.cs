using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame_Hoechtl
{
    internal class Server
    {
        public void Open()
        {
            TcpListener httpServer = new TcpListener(IPAddress.Loopback, 8000);
            httpServer.Start();
            while (true)
            {
                TcpClient clientSocket = httpServer.AcceptTcpClient();
                Task.Factory.StartNew(() =>
                {
                    var writer = new StreamWriter(clientSocket.GetStream()) { AutoFlush = false };
                    var reader = new StreamReader(clientSocket.GetStream());

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                        if (line.Length == 0)
                        {
                            break;
                        }
                    }

                    writer.WriteLine("HTTP/1.1 200 Ok");
                    writer.WriteLine();
                    writer.WriteLine("<html><body>Hello World!</body></html>");

                    Thread.Sleep(10000);

                    writer.Flush();
                    writer.Close();
                });
            }
        }
    }
}
