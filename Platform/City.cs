using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;



namespace Platform.Platform
{
    public class City
    {

        public async Task Endpoints(HttpContext context) 
        {
            //// использование данных маршрута, чтобы получить значение переменной сегмента через
            //  индексатор([key"] - ключ и переменная url пути шаблона), определенный классом RouteValuesDictionary.

            string s = context.Request.RouteValues["key"] as string;
            //или так
            // string s = (string)context.Request.RouteValues["key"];
            // или так
            //string s3 = "key";
            //string s = context.Request.RouteValues[s3] as string;

            int? i = null;

            switch ((s ?? "").ToLower())
            {
                case "dron":
                    i = 1;
                    break;
                case "lero":
                    i = 2;  
                    break;
                case "nata":
                    i = 3;
                    break;

                case "":
                    i = 4;  
                    break;  

            }

            if (i.HasValue)
            {
                await context.Response.WriteAsync($"pipel: {s}, it {i}");
            }

            else
            {
                //await context.Response.WriteAsync("no fiaund");
                //int status = context.Response.StatusCode = StatusCodes.Status400BadRequest;
                //await context.Response.WriteAsync($"{status}");
                // или
                context.Response.StatusCode = StatusCodes.Status404NotFound;                   
            }

        }

    }
}
