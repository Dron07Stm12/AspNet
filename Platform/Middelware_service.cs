using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Platform.Services;

namespace Platform.Platform
{
    public class Middelware_service
    {
      private  RequestDelegate request;
      private IResponseFormatter formatter;

        public Middelware_service(RequestDelegate request, IResponseFormatter formatter)
        {
            this.request = request;   
            this.formatter = formatter; 
        }

        public Middelware_service()
        {
                
        }



        public async Task Invoke(HttpContext http)
        {
            if (http.Request.Path == "/class_middlware_service")
            {
                await formatter.Format(http, "class_middlware_service");
            }

            else { await request(http); }
        }


        public async Task Invoke2(HttpContext context) 
        {           
                await context.Response.WriteAsync("method run\n");                
        }


    }
}
