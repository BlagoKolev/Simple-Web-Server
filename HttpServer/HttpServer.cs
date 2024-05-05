using HttpServer.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebServer.HTTP.Interfaces;

namespace WebServer.HTTP
{
    public class HttpServer : IHttpServer
    {
        IDictionary<string, Func<HttpRequest, HttpResponse>> routeTable = new Dictionary<string, Func<HttpRequest, HttpResponse>>();

        public void AddRoute(string path, Func<HttpRequest, HttpResponse> action)
        {
            if (routeTable.ContainsKey(path))
            {
                routeTable[path] = action;
            }
            else
            {
                routeTable.Add(path, action);
            }
        }

        public async Task StartAsync(int port)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, port);
            tcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                ProcessClient(tcpClient);
            }
        }

        private async Task ProcessClient(TcpClient tcpClient)
        {
            try
            {
                using NetworkStream stream = tcpClient.GetStream();
                var data = new List<byte>();
                var buffer = new byte[HttpConstants.DEFAULT_BUFFER_SIZE];
                var position = 0;

                while (true)
                {
                    var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    position += bytesRead;

                    if (bytesRead < buffer.Length)
                    {
                        var partialBuffer = new byte[bytesRead];
                        Array.Copy(buffer, partialBuffer, bytesRead);
                        data.AddRange(partialBuffer);
                        break;
                    }
                    else
                    {
                        data.AddRange(buffer);
                    }
                }

                var result = Encoding.UTF8.GetString(data.ToArray());
                Console.WriteLine(result);
                var request = HttpRequest.Create(result);
                var responseHtml = "<h1>Welcome!</h1>";
                var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);

                var responceHeaders = "HTTP/2.0 200 OK" + HttpConstants.NEW_LINE
                    + "ContentType: text/html" + HttpConstants.NEW_LINE
                    + "ContentLength: " + responseHtml.Length
                    + HttpConstants.NEW_LINE
                    + HttpConstants.NEW_LINE;
                var responseHeaderBytes = Encoding.UTF8.GetBytes(responceHeaders);

                await stream.WriteAsync(responseBodyBytes, 0, responseBodyBytes.Length);
                await stream.WriteAsync(responseHeaderBytes, 0, responseHeaderBytes.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}
