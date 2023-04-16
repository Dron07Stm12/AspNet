using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Platform.Services;
using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Builder;
using Platform.Platform;


namespace Platform.Services
{
    public static class EndPointExtensions
    {
        //Cоздаем  метод расширения для интерфейса IEndpointRouterBuilder
        public static void MapWeather(this IEndpointRouteBuilder app, string path) {
          
            //получение информации о типе
            Type type = typeof(IResponseFormatter);

            //GetService(type) -возвращаем службу для указанного типа и так 
            IResponseFormatter formatter = (IResponseFormatter)app.ServiceProvider.GetService(type);
            app.MapGet(path, delegate (HttpContext http) { return WeatherEndpoint.Endpoint_format2(http,formatter); });
        
        }
        //Cоздаем новый метод расширения для интерфейса IEndpointRouterBuilder
        public static void MapUserr(this IEndpointRouteBuilder app, string s)
        {
            IServiceProvider provider = app.ServiceProvider;
            //получение информации о типе
            Type type = typeof(IResponseFormatter);

            //явное приведение типа object метода GetService(Type) к типу интерфейса(IResponseFormatter) для возвращения службы указанного типа
            IResponseFormatter formatter1 = (IResponseFormatter)provider.GetService(type);

            //или  метод GetService<IResponseFormatter>() возвращает службу для типа, указанного параметром универсального типа
            formatter1 = provider.GetService<IResponseFormatter>();
           // app.Map(s, async delegate (HttpContext context) { await formatter1.Format(context, "MapUse"); });
            app.MapGet(s, delegate (HttpContext context) { return WeatherEndpoint.Endpoint_format(context,"new_format",formatter1); });
        }


        public static void MapUser(this IEndpointRouteBuilder app, string s) 
        {
            Type type = typeof(IResponseFormatter);
            //Downcasting -  нисходящее от базового к производному(чтоб иметь функционал интерфейса IResponseFormatte)
            IResponseFormatter formatter = (IResponseFormatter)app.ServiceProvider.GetService(type);
            app.Map(s, async delegate (HttpContext context) { await formatter.Format(context, "MapUse"); }) ;
               
        }


    }
}
