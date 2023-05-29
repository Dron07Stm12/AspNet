using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Platform.Services
{
    public class GuidService : IResponseFormatter
    {
        private Guid guid = Guid.NewGuid(); 

        public  Task Format(HttpContext http, string content)
        {
            return http.Response.WriteAsync($"Guid: {guid}\n{content}");  
        
        
        }
    }
}
