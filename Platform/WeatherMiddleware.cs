using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Platform.Services;

namespace Platform.Platform
{
    public class WeatherMiddleware
    {
        private RequestDelegate request;
        private IResponseFormatter formatter;
        private static int responseCounter;

        public WeatherMiddleware(RequestDelegate request, IResponseFormatter formatter)
        {
            this.request = request;
            this.formatter = formatter;
        }

        public WeatherMiddleware()
        {

        }


        //public async Task Invoke(HttpContext http)
        //{
        //    if (http.Request.Path == "/middleware/class")
        //    {
        //        //await formatter.Format(http, "services_middleware_class");
        //        await http.Response.WriteAsync("Middleware Class: It is raining in London");

        //    }
        //    else
        //    {                
        //     await request.Invoke(http);
        //    }

        //}

        public async Task Invoke(HttpContext http)
        {

            await http.Response.WriteAsync("Middleware Class");
        

        }


        public async static Task Format(HttpContext context, string content)
        {
            await context.Response.WriteAsync($"Responce {++responseCounter}: \n{content}");
            //await request(context);
        }



        public async Task Invoke2(HttpContext http,string s)
        {
            //if (http.Request.Query["bool"] == "true")
            //{
            await http.Response.WriteAsync("Middleware Class: It is raining in London");
            //}

            await request(http);
            //await http.Response.WriteAsync("Middleware Class: It is raining in London");

        }



    }
}
