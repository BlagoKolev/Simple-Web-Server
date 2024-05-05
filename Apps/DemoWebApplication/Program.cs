using WebServer.HTTP;
using WebServer.HTTP.Interfaces;

namespace DemoWebApplication
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            IHttpServer server = new WebServer.HTTP.HttpServer();

            server.AddRoute("/", HomePage);
            server.AddRoute("/about",About);

            await server.StartAsync(80);
     
        }
        public static HttpResponse HomePage(HttpRequest request)
        {
            throw new NotImplementedException();
        }
        public static HttpResponse About(HttpRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
