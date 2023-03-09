using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Platform.Platform;

namespace Platform
{

    //Классы промежуточного ПО получают RequestDelegate в качестве параметра конструктора,
    //который используется для пересылки запроса следующему
    //компонент в трубопроводе.Метод Invoke вызывается ASP.NET Core при получении запроса и получении HttpContext.
    //объект, который обеспечивает доступ к запросу и ответу, используя те же классы, которые получает промежуточное 
    //ПО лямбда-функции.RequestDelegate возвращает Task, что позволяет ему работать асинхронно.
    //Одно важное отличие промежуточного ПО на основе классов заключается в том, что объект HttpContext должен
    //использоваться в качестве аргумента, когда вызывая RequestDelete для пересылки запроса


    public class QueryStringMiddleWare
    {
        private RequestDelegate next;

        //Классы промежуточного ПО получают RequestDelegate в качестве параметра конструктора,
        //который используется для пересылки запроса следующему компонент в трубопроводе.
        public QueryStringMiddleWare(RequestDelegate nextDelegate)
        {
            next = nextDelegate;

        }
        RequestDelegate requestDelegate = async (cont) => { await cont.Response.WriteAsync("rt\n"); };
        RequestDelegate requestDelegate2 = async (cont) => { await cont.Response.WriteAsync("rt2\n"); };
        RequestDelegate requestDelegate3 = async (cont) => { await cont.Response.WriteAsync("rt3\n"); };

        public QueryStringMiddleWare()
        {

        }

        //Метод Invoke вызывается ASP.NET Core при получении запроса и получении HttpContext объекта,
        //который обеспечивает доступ к запросу и ответу, используя те же классы, которые получает
        //промежуточное ПО лямбда-функции. RequestDelegate возвращает Task, что позволяет ему работать асинхронно.
        //public delegate Task RequestDelegate(HttpContext context) - сигнатура
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Get
            /*&& context.Request.Query["custom"] == "true"*/)
            {

                await context.Response.WriteAsync("Class-based Middleware \n");
            }
            //Одно важное отличие промежуточного ПО на основе классов заключается в том, что объект HttpContext
            //должен использоваться в качестве аргумента, когда вызывая RequestDelete для пересылки запроса, например:
            //await next(context);

            if (next != null)
            {
                await requestDelegate(context);
                await requestDelegate2(context);
                await next(context);
            }

           

        }

        public async Task Invoke2(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Get
           /* && context.Request.Query["custom"] == "true"*/)
            {

                await context.Response.WriteAsync("Class-based Middleware \n");
            }
            //Одно важное отличие промежуточного ПО на основе классов заключается в том, что объект HttpContext
            //должен использоваться в качестве аргумента, когда вызывая RequestDelete для пересылки запроса, например:
            //await next(context);

            //if (next != null)
            //{

            //    await next(context);
            //}
            await requestDelegate(context);
            await requestDelegate2(context);
            await requestDelegate(context);
            await requestDelegate2(context);
            await next(context);

        }


        //public static async Task Invoke3(HttpContext context)
        //{
        //    if (context.Request.Method == HttpMethods.Get
        //    && context.Request.Query["custom"] == "true")
        //    {

        //        await context.Response.WriteAsync("Class-based Middleware \n");
        //    }
        //    //Одно важное отличие промежуточного ПО на основе классов заключается в том, что объект HttpContext
        //    //должен использоваться в качестве аргумента, когда вызывая RequestDelete для пересылки запроса, например:
        //    //await next(context);

        //    //if (next != null)
        //    //{

        //    //    await next(context);
        //    //}

        //}



    }


    public class LocationMiddleware {

        private RequestDelegate request;
        private MessageOptions message;

        public LocationMiddleware(RequestDelegate requ, IOptions<MessageOptions> options)
        {
            request = requ;
            message = options?.Value;
                
        }

        

        public async Task Invoke(HttpContext context) 
        {
            if (context.Request.Path == "/location")
            {
                await context.Response.WriteAsync($"{message?.CountryName} , {message?.CityName} \n");
            }

            else
            { await request(context); }
        }


        public async Task Invoke2(HttpContext context)
        {
            
            await context.Response.WriteAsync($"{message?.CountryName} , {message?.CityName} \n");

            if (request != null)
            {
                await request(context);
            }

           
            
        }


    }



}
