using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Platform.Services;
using System;

namespace Platform.Platform
{
    public class WeatherEndpoint
    {
        private static int responce;
        public static async Task Endpoint(HttpContext http)
        {
            await http.Response.WriteAsync($"Endpoint Class: It is cloudy in Milan {++responce}");
        }

        //Свойство HttpContext.RequestServices возвращает объект, реализующий интерфейсы IServiceProvider, которые
        // предоставляет доступ к службам, настроенным в методе Start.ConfigureServices приложения.Microsoft.
        //Пространство имен Extensions.DependencyInjection,
        public static async Task Endpoint_service(HttpContext http) {
            //GetRequiredService<IResponseFormatter>() -    метод возвращает службу, указанную параметром универсального типа. 
            //служба которая находится в  Startape
            IResponseFormatter formatter = http.RequestServices.GetRequiredService<IResponseFormatter>();
            await formatter.Format(http, "Static_Endpoint_service");
        }

        public static async Task Endpoint_format(HttpContext http, string s, IResponseFormatter response)
        {
            await response.Format(http, s);

        }


        public static async Task Endpoint_format2(HttpContext context,IResponseFormatter formatter) 
        {
            await formatter.Format(context, "Endpoint_format2");
        }


    }
}
