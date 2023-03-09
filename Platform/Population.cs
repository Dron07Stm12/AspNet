using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace Platform.Platform
{
    public class Population
    {
        private RequestDelegate request;

        public Population(RequestDelegate request)
        {
                this.request = request;
        }


        public async Task Invoke(HttpContext http) 
        {
          string[] parts = http.Request.Path.ToString().Split("/",StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 2 && parts[0] == "population")
            {
                string sity = parts[1];
                int? pop = null;

                switch (sity.ToLower())
                {
                    case "london":
                      pop = 8_136_000;
                        break;
                    case "paris":
                      pop = 2_141_000;
                        break;
                    case "monaco":
                      pop = 39_000;
                        break;
                    case "ukr":
                        pop = 50000000;
                        break;
                }

                if (pop.HasValue)
                {
                    await http.Response.WriteAsync($"City: {sity}, Population: {pop}");
                    return;
                }

            }

            if (http != null)
            {
                await request(http);
            }

        }
    }
}
