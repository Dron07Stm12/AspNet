using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Platform.Services;



namespace Platform.Platform
{
    public class WeatherEndpointEx
    {
        private IResponseFormatter formatter;

        public WeatherEndpointEx(IResponseFormatter formatter)
        {
            this.formatter = formatter;
        }

       

        public  Task EndpointEx(HttpContext http)
        {

           return this.formatter.Format(http, "public class WeatherEndpointEx");
        }

        public async Task EndpointEx2(HttpContext http)
        {

           await this.formatter.Format(http, "public class WeatherEndpointEx2");
        }

    }


    public class WeatherEndpoints
    {
        private IResponseFormatter formatter;
        public WeatherEndpoints(IResponseFormatter responseFormatter)
        {
            formatter = responseFormatter;
        }

        public async Task Endpoint(HttpContext context)
        {
            await formatter.Format(context, "Endpoint Class: It is cloudy in Milan");
        }
    }



}
