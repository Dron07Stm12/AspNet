using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Platform.Services;

namespace Platform.Platform
{
    public class WeatherMiddleware2
    {
        private RequestDelegate request;
        private IResponseFormatter formatter;


        public WeatherMiddleware2(RequestDelegate requestDelegate, IResponseFormatter responseFormatter)
        {
                request= requestDelegate;
                formatter= responseFormatter;   
        }

        public async Task Invoke(HttpContext context) 
        {
            if (context.Request.Path == "/middleware/class")
            {
                await formatter.Format(context, "Middleware Class: It is raining in London");
            }

            else { await request(context); }
        }


    }

    public class WeatherMiddleware3
    {
        private RequestDelegate request;
      //  private IResponseFormatter formatter;


        public WeatherMiddleware3(RequestDelegate requestDelegate)
        {
            request = requestDelegate;
          //  formatter = responseFormatter;
        }

        public WeatherMiddleware3()
        {
                
        }



        public async Task Invoke(HttpContext context, IResponseFormatter formatter)
        {
            if (context.Request.Path == "/middleware/class")
            {
                await formatter.Format(context, "Middleware Class: It is raining in London3");
            }

            else { await request(context); }
        }

        public async Task Invoke2(HttpContext context, IResponseFormatter formatter)
        {           
                await formatter.Format(context, "Middleware Class: Invoke");
                     
        }

        public static async Task Statik(HttpContext context) 
        {
            IResponseFormatter formatter = context.RequestServices.GetRequiredService<IResponseFormatter>();
            await formatter.Format(context,"st"); 
        }


    }


    public class WeatherMiddleware4
    {
        private RequestDelegate request;
        //  private IResponseFormatter formatter;


        public WeatherMiddleware4(RequestDelegate requestDelegate)
        {
            request = requestDelegate;
            //  formatter = responseFormatter;
        }


        public async Task Invoke2(HttpContext context, IResponseFormatter formatter, IResponseFormatter formatter2, IResponseFormatter formatter3)
        {
            if (context.Request.Path == "/middleware4/class")
            {
                await formatter.Format(context, "\nMiddleware Class: It is raining in London3");
                await formatter2.Format(context, "\nMiddleware Class: It is raining in London3");
                await formatter3.Format(context, "\nMiddleware Class: It is raining in London3");
            }

            else { await request(context); }
        }

        public  Task Invoke(HttpContext context, IResponseFormatter formatter, IResponseFormatter formatter2, IResponseFormatter formatter3)
        {
            if (context.Request.Path == "/middleware4/class")
            {
                Task task  = formatter.Format(context, "Middleware Class: It is raining in London\n");
                
                Task task2 = formatter2.Format(context, "Middleware Class: It is raining in London2\n");
                Task task3 = formatter3.Format(context, "Middleware Class: It is raining in London3\n");
                
                return Task.WhenAll(task,task2,task3);
               
                
            }

            else { return request(context); }
        }


    }



}
