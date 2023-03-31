using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Platform.Platform
{
    public class WeatherMiddleware
    {
        private RequestDelegate request;

        public WeatherMiddleware(RequestDelegate request)
        {
                this.request = request; 
        }

        public async Task Invoke(HttpContext http)
        {
            if (http.Request.Path == "/middleware/class")
            {
                await http.Response.WriteAsync("Middleware Class: It is raining in London");
            }
            else { await request.Invoke(http); }
           
        }


    }
}
