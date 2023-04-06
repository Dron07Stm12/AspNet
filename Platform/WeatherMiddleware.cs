using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Platform.Platform
{
    public class WeatherMiddleware
    {
        private RequestDelegate request;
        private static int responseCounter;

        public WeatherMiddleware(RequestDelegate request)
        {
            this.request = request;
        }

        public WeatherMiddleware()
        {

        }


        //public async Task Invoke(HttpContext http)
        //{
        //    if (http.Request.Path == "/middleware/class")
        //    {
        //        await http.Response.WriteAsync("Middleware Class: It is raining in London");
        //    }
        //    else { await request.Invoke(http); }

        //}


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
