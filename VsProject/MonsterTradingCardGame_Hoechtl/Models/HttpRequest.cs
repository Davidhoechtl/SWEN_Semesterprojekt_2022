using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardGame_Hoechtl.Models
{
    internal class HttpRequest
    {
        public string ProtocolVersion { get; set; }
        public HttpMethod Method { get; }

        public string[] PathData { get; }

        public Dictionary<string, string> Headers = new();
        public string Content { get; set; }
        public string AuthenticationToken => Headers.GetValueOrDefault("Authorization");

        public HttpRequest(StreamReader reader)
        {
            // First Line
            string line = reader.ReadLine();
            string[] firstLineData = line.Split(' ');
            Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), firstLineData[0]);
            PathData = firstLineData[1].Split('/').Skip(1).ToArray();
            ProtocolVersion = firstLineData[2];

            // Headers
            int contentLength = 0;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length == 0)
                {
                    break;
                }

                string[] headerData = line.Split(": ");
                Headers[headerData[0]] = headerData[1];

                if (headerData[0].Equals("Content-Length", StringComparison.OrdinalIgnoreCase))
                {
                    contentLength = int.Parse(headerData[1]);
                }
            }

            // Content
            Content = ReadContent(reader, contentLength);
        }

        private string ReadContent(StreamReader reader, int contentLength)
        {
            if (contentLength > 0 && Headers.ContainsKey("Content-Type"))
            {
                var data = new StringBuilder(200);
                char[] buffer = new char[1024];
                int bytesReadTotal = 0;
                int nextchunk = 0;
                while (bytesReadTotal < contentLength)
                {
                    try
                    {
                        if ((contentLength - bytesReadTotal) < buffer.Length)
                        {
                            nextchunk = (contentLength - bytesReadTotal);

                        }
                        else
                        {
                            nextchunk = buffer.Length;
                        }

                        var bytesRead = reader.Read(buffer, 0, nextchunk);
                        data.Append(buffer, 0, bytesRead);
                        bytesReadTotal += bytesRead;
                        if (bytesRead == 0) break;
                        buffer = new char[1024];
                    }

                    catch (IOException ex)
                    {
                        // IOException can occur when there is a mismatch of the 'Content-Length'
                        // because a different encoding is used
                        // Sending a 'plain/text' payload with special characters (äüö...) is
                        // an example of this
                        break;
                    }
                }
                return data.ToString();
            }

            return string.Empty;
        }

        public override string ToString()
        {
            StringBuilder builder = new();

            builder.AppendLine($"{Method} {ProtocolVersion} {string.Join('/', PathData).ToString()}");
            foreach (KeyValuePair<string, string> pair in Headers)
            {
                builder.AppendLine($"{pair.Key}:{pair.Value}");
            }

            builder.AppendLine(string.Empty);
            builder.Append(Content);

            return builder.ToString();
        }
    }
}
