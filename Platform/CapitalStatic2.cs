using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;



namespace Platform.Platform
{
    public static class CapitalStatic2
    {

        public static async Task Endpointe(HttpContext context) {


            string capital = null;
            // необходимо обратится к ключю индекса(public object this[string key] { get; set; }) 
            //класса RouteValueDictionary
            string coutry = context.Request.RouteValues["coutry"] as string;
            //или
            //string coutry2 = (string)context.Request.RouteValues["coutry"];


            switch ((coutry ?? "").ToLower())
            {

                case "uk":
                    capital = "London";
                    break;
                case "france":
                    capital = "Paris";
                    break;
                case "monaco":
                    context.Response.Redirect($"/population/{coutry}");
                    return;
            }

            if (capital != null)
            {
                await context.Response.WriteAsync($"{coutry} the it {capital}");
            }

            else
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;  
            }


        }



    }
}
