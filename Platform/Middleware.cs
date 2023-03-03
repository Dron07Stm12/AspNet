using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
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


        public QueryStringMiddleWare()
        {
                
        }

        //Метод Invoke вызывается ASP.NET Core при получении запроса и получении HttpContext объекта,
        //который обеспечивает доступ к запросу и ответу, используя те же классы, которые получает
        //промежуточное ПО лямбда-функции. RequestDelegate возвращает Task, что позволяет ему работать асинхронно.
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Get
            && context.Request.Query["custom"] == "true")
            {

                await context.Response.WriteAsync("Class-based Middleware \n");
            }
            //Одно важное отличие промежуточного ПО на основе классов заключается в том, что объект HttpContext
            //должен использоваться в качестве аргумента, когда вызывая RequestDelete для пересылки запроса, например:
            //await next(context);

            if (next != null)
            {

                await next(context);
            }

        }
    }
}
