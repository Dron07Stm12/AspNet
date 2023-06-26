using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Platform.Services;
using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Builder;
using Platform.Platform;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using System.Net.NetworkInformation;

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
            IResponseFormatter formatter2 = provider.GetService<IResponseFormatter>();
           // app.Map(s, async delegate (HttpContext context) { await formatter1.Format(context, "MapUse"); });
            app.MapGet(s, delegate (HttpContext context) { return WeatherEndpoint.Endpoint_format(context,"new_format",formatter2); });
        }


        public static void MapUser(this IEndpointRouteBuilder app, string s) 
        {
            Type type = typeof(IResponseFormatter);
            //Downcasting -  нисходящее от базового к производному(чтоб иметь функционал интерфейса IResponseFormatte)
            IResponseFormatter formatter = (IResponseFormatter)app.ServiceProvider.GetService(type);
            app.Map(s, async delegate (HttpContext context) { await formatter.Format(context, "MapUse"); }) ;
               
        }


        //метод расширения принемающий параметр уневерсального типа <T>
        //указывающий класс конечной точки(typeof(RequestDelegate))
        // также путь(string path) для маршрута
        // и имя метода(string methodName = "EndpointEx") класса конечной точки, обрабатывающего запросы
        public static void MapEndpoints<T>(this IEndpointRouteBuilder app, string path, string methodName = "EndpointEx2")
        {

            MethodInfo methodInfo = typeof(T).GetMethod(methodName);
            if (methodInfo == null || methodInfo.ReturnType != typeof(Task))
            {
                throw new Exception("Method cannot be used");
            }

            T endpointInstance = ActivatorUtilities.CreateInstance<T>(app.ServiceProvider);
            //получение информации о типе
            Type type = typeof(RequestDelegate);
            app.MapGet(path, (RequestDelegate)methodInfo.CreateDelegate(type, endpointInstance));

        }

        
        public static void MapEndpoints2<T>(this IEndpointRouteBuilder app, string path, string methodName = "EndpointEx")
        {
            MethodInfo info = typeof(T).GetMethod(methodName);
            Type type = typeof(Task);
            if (info == null || info.ReturnType != type)
            {
                throw new Exception("no method by used");
            }
            //Класс ActivatorUtilities, определенный в пространстве имен Microsoft.Extensions.DependencyInjection, предоставляет
            //методы для создания экземпляров классов, у которых есть зависимости, объявленные через их конструктор
            T endpointClassInstance = ActivatorUtilities.CreateInstance<T>(app.ServiceProvider);
            app.MapGet(path, (RequestDelegate)info.CreateDelegate(typeof(RequestDelegate),endpointClassInstance));

        }

        public static void MapEndpoints3(this IEndpointRouteBuilder app, string path, string methodName = "EndpointEx")
        {
            MethodInfo info = typeof(WeatherEndpointEx).GetMethod(methodName);
            Type type = typeof(Task);
            if (info == null || info.ReturnType != type)
            {
                throw new Exception("no method by used");
            }
            //Класс ActivatorUtilities, определенный в пространстве имен Microsoft.Extensions.DependencyInjection, предоставляет
            //методы для создания экземпляров классов, у которых есть зависимости, объявленные через их конструктор
            Type type2 = typeof(WeatherEndpointEx);     
            object endpointClassInstance = ActivatorUtilities.CreateInstance(app.ServiceProvider,type2);
            app.MapGet(path, (RequestDelegate)info.CreateDelegate(typeof(RequestDelegate), endpointClassInstance));

        }


        public static void MapEndpoint<T>(this IEndpointRouteBuilder app,string path, string methodName = "Endpoint")
        {
            MethodInfo methodInfo = typeof(T).GetMethod(methodName);
            if (methodInfo == null || methodInfo.ReturnType != typeof(Task))
            {
                throw new System.Exception("Method cannot be used");
            }
            T endpointInstance =
            ActivatorUtilities.CreateInstance<T>(app.ServiceProvider);
            


            app.MapGet(path, (RequestDelegate)methodInfo
            .CreateDelegate(typeof(RequestDelegate), endpointInstance));
        }


        public static void MapEndpoint4<T>(this IEndpointRouteBuilder app, string path, string methodName = "Endpoint")
        {
            MethodInfo methodInfo = typeof(T).GetMethod(methodName);
            if (methodInfo == null || methodInfo.ReturnType != typeof(Task))
            {
                throw new System.Exception("Method cannot be used");
            }
            T endpointInstance =
            ActivatorUtilities.CreateInstance<T>(app.ServiceProvider);
            ParameterInfo[] parameters = methodInfo.GetParameters();
            //public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector);
           // Func<ParameterInfo, HttpContext> func = delegate (ParameterInfo info) { return info.ParameterType == typeof(HttpContext) ?  ; };

            //app.MapGet(path, delegate (HttpContext context)
            //{
            //    return (Task)methodInfo.Invoke(endpointInstance,
            //    parameters.Select(p => p.ParameterType == typeof(HttpContext) ? context : app.ServiceProvider.GetService(p.ParameterType)).ToArray());
            //});

            app.MapGet(path, delegate (HttpContext context) { return (Task)methodInfo.Invoke(endpointInstance, parameters.
                Select(delegate (ParameterInfo info) { return info.ParameterType == typeof(HttpContext) ?
                    context : app.ServiceProvider.GetService(info.ParameterType); }).ToArray()
                ); 
            });
         
            //parameters.Select(p => p.ParameterType == typeof(HttpContext) ? context : app.ServiceProvider.GetService(p.ParameterType)).ToArray());
            //app.MapGet(path, (RequestDelegate)methodInfo
            //.CreateDelegate(typeof(RequestDelegate), endpointInstance));
        }
         

        public static void MapEndpoint5<T>(this IEndpointRouteBuilder builder, string path, string methodName = "Endpoint") 
        {
            //Получение типа через оператор  typeof
            Type type = typeof(T);
            //Выполняет поиск открытого метода с заданным именем
            MethodInfo info = type.GetMethod(methodName);
            //Получение типа через оператор  typeof
            Type type2 = typeof(Task); 
            if (info == null || info.ReflectedType != type2)
            {
                throw new Exception("error");
            }
           

            ParameterInfo[] methodParams = info.GetParameters();
            builder.MapGet(path, delegate (HttpContext http)
            {
                T end = ActivatorUtilities.CreateInstance<T>(http.RequestServices);
                T endpointInstance =
                            ActivatorUtilities.CreateInstance<T>(http.RequestServices);
                return (Task)info.Invoke(endpointInstance, methodParams.Select(delegate (ParameterInfo parameter)
                {
                    return parameter.ParameterType == typeof(HttpContext) ? http : http.RequestServices.GetService(parameter.ParameterType);
                }).ToArray());
            });

            //builder.MapGet(path,delegate(HttpContext http) { return (Task)info.Invoke(end,methodParams.Select(delegate(ParameterInfo parameter) {
            //    return parameter.ParameterType == typeof(HttpContext) ? http : http.RequestServices.GetService(parameter.ParameterType);}).ToArray()); });


            //builder.MapGet(path, delegate (HttpContext http) {
            //    return (Task)info.Invoke(end, methodParams.Select(delegate (ParameterInfo parameter) {
            //        return  http.RequestServices.GetService(parameter.ParameterType);
            //    }).ToArray());
            //});
        }


        public static void MapEndpoint6<T>(this IEndpointRouteBuilder app,string path, string methodName = "Endpoint")
        {

            Type type= typeof(T);
            MethodInfo info2 = type.GetMethod(methodName);
            Type type2 = typeof(Task);  
            if (info2 == null || info2.ReturnType != type2)
            {
                throw new System.Exception("Method cannot be used");
            }
         
            ParameterInfo[] methodParams = info2.GetParameters();
            //app.MapGet(path, delegate (HttpContext context)
            //{
            //    T e = ActivatorUtilities.CreateInstance<T>(context.RequestServices);
            //    return (Task)info2.Invoke(e, methodParams.Select(delegate (ParameterInfo info) {
            //        return info.ParameterType == typeof(HttpContext) ? context :
            //        context.RequestServices.GetService(info.ParameterType);
            //    }).ToArray());
            //});


            app.MapGet(path, delegate (HttpContext context)
            {
                T e = ActivatorUtilities.CreateInstance<T>(context.RequestServices);
                return (Task)info2.Invoke(e, methodParams.Select(delegate (ParameterInfo info) {
                   

                    if (info.ParameterType == typeof(HttpContext))
                    {
                        return context;
                    }

                    else 
                    {
                        return context.RequestServices.GetService(info.ParameterType);
                    }




                }).ToArray());
            });






        }


    }
}
