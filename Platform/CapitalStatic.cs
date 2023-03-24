using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace Platform.Platform
{
    public  class CapitalStatic
    {
        public   async Task Endpoint(HttpContext context) 
        {
            string capital = null;
            // использование данные маршрута, чтобы получить значение переменной сегмента с именем страна через
            //  индексатор(["country"]), определенный классом RouteValuesDictionary.
            string country = context.Request.RouteValues["country"] as string;

            //string coutry2 = "c";
            //context.Request.RouteValues["country"] = coutry2;
            //switch (context.Request.RouteValues["country"])
            //{
            //    case "uk":
            //        capital = "London";
            //        break;
            //    case "france":
            //        capital = "Paris";
            //        break;

            //    case "c":
            //        capital = "Paris";
            //        break;



            //    case "monaco":
            //        context.Response.Redirect($"population/{coutry2}");
            //        return;
            //}


            switch ((country ?? "").ToLower())
            {
                case "uk":
                    capital = "London";
                    break;
                case "france":
                    capital = "Paris";
                    break;
                case "monaco":
                    context.Response.Redirect($"/population/{country}");
                    return;

            }

            if (capital != null)
            {
                await context.Response.WriteAsync($"{capital} is the capital of: {country}");
            }
            else
            {
                 context.Response.StatusCode = StatusCodes.Status200OK;   
            }

        }


    }
}
