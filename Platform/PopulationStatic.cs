using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Routing;


namespace Platform.Platform
{
    public  class PopulationStatic
    {
        public static async Task Endpointe(HttpContext context) 
        {
         string city = context.Request.RouteValues["city"] as string;
            int? pop = null;

            switch ((city ?? "").ToLower())
            {
                case "london":
                    pop = 8000;
                    break;

                case "paris":
                    pop = 2_141_000;
                    break;

                case "monaco":
                    pop = 500;  
                    break;
                case "shtat":
                    pop = 600;  
                    break;  

            }

            if (pop.HasValue)
            {
                await context.Response.WriteAsync($"City: {city}, pop: {pop}");
            }
            else {  context.Response.StatusCode = StatusCodes.Status404NotFound; }


        }

    }
}
