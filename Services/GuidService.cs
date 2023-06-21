using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Platform.Services
{
    public class GuidService : IResponseFormatter
    {
        private Guid guid = Guid.NewGuid(); 
        public async  Task Format(HttpContext http, string content)
        {
         //  await http.Response.WriteAsync($"Guid: {guid}\n");  // return http.Response.WriteAsync($"Guid: {guid}\t{content}");
         //  await http.Response.WriteAsync(content);
            //  await http.Response.WriteAsync($"\t{http}");
          await http.Response.WriteAsync($"Guid: {guid}\t{content}");
        }
    }
}
