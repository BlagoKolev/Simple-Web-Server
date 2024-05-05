using HttpServer.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.HTTP
{
    public class HttpRequest
    {
        public HttpRequest()
        {
            this.Headers = new List<Header>();
            this.Cookies = new List<Cookie>();
        }

        public string? MethodType { get; set; }
        public string? Path { get; set; }
        public ICollection<Header>? Headers { get; set; }
        public ICollection<Cookie>? Cookies { get; set; }
        public string? Body { get; set; }

        public static HttpRequest Create(string input)
        {
            var request = new HttpRequest();

            var lines = input.Split(HttpConstants.NEW_LINE, StringSplitOptions.None);

            var firstLine = lines[0].Split(" ");
            request.MethodType = firstLine[0];
            request.Path = firstLine[1];

            var index = 1;
            var isInHeaders = true;
            StringBuilder sb = new StringBuilder();

            while (index < lines.Length)
            {
                var line = lines[index];
                index++;

                if (string.IsNullOrWhiteSpace(line))
                {
                    isInHeaders = false;
                    continue;
                }

                if (isInHeaders)
                {
                    var header = new Header(line);
                    request.Headers.Add(header);
                }
                else
                {
                    sb.AppendLine(line).ToString();
                }
            }

            if (request.Headers.Any(x => x.Key == HttpConstants.COOKIE))
            {
                GetCookiesFromHeaders(request);
            }
            request.Body = sb.ToString();

            return request;
        }

        private static void GetCookiesFromHeaders(HttpRequest request)
        {
            var cookiesAsString = request?.Headers?.FirstOrDefault(x => x.Key == HttpConstants.COOKIE)?.Value;
            var cookiesList = cookiesAsString?.Split("; ", StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in cookiesList)
            {            
                request?.Cookies?.Add(Cookie.Parse(item));
            }
        }
    }
}
