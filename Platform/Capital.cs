using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Platform.Platform
{
    public class Capital
    {
        private RequestDelegate next;
        public Capital() { }
        public Capital(RequestDelegate nextDelegate)
        {
            next = nextDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            string[] parts = context.Request.Path.ToString()
            .Split("/", StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2 && parts[0] == "capital")
            {
                string capital = null;
                string country = parts[1];
                switch (country.ToLower())
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

                    //case "ukr":
                    //    context.Response.Redirect($"/population/{country}");
                    //    return;
                }
                if (capital != null)
                {
                    await context.Response
                    .WriteAsync($"{capital} is the capital of {country}");
                    return;
                }
            }
            if (next != null)
            {
                await next(context);
            }
        }

    }
}


//if (context.Request.Path == "/s/stash")
//{
//    string[] array2 = context.Request.Path.ToString().Split("/", StringSplitOptions.RemoveEmptyEntries);

//    if (array2.Length == 2 && array2[0] == "s")
//    {
//        string sity = array2[1];
//        int? pop = null;

//        switch (sity.ToLower())
//        {
//            case "stash":
//                pop = 100000;
//                break;

//            case "sharkiv":
//                pop = 1000000;
//                break;

//        }

//        if (pop.HasValue)
//        {
//            await context.Response.WriteAsync($"Sity:{sity}, Pop: {pop}");
//            return;
//        }

//    }

//}
//if (request != null)
//{
//    await request(context);
//}
