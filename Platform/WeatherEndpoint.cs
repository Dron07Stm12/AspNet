using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Platform.Platform
{
    public class WeatherEndpoint
    {
        private static int responce;
        public static async Task Endpoint(HttpContext http)
        {
            await http.Response.WriteAsync($"Endpoint Class: It is cloudy in Milan {++responce}");
        }
    }
}
