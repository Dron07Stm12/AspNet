using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Platform.Services
{
    public class HtmlResponseFormatter : IResponseFormatter
    {
        public static int responseCounter;

        public async Task Format(HttpContext context, string content) 
        {
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync($@"

             <!DOCTYPE html>
             <html lang=""en"">
               <head>

              <link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-beta/css/bootstrap.min.css"" integrity=""sha384-
                               /Y6pD6FV/Vv2HJnA6t+vslU6fwYXjCFtcEpHbNJ0lyAFsXTsjBbfaDjzALeQsN6M"" crossorigin=""anonymous"" />


               <title>Response</title>
               
               

               </head>
                 <body>

                   <div class=""container"">
                   <div class=""jumbotron"">
                     <h2 class=""display-3"">Formatted Response</h2>
                           <span>{content}</span>
                             <span>{++HtmlResponseFormatter.responseCounter}</span>
                     <hr />

                      <a class=""btn btn-primary"" href=""https://www.asp.net/"">Learn more</a>

                   </div>
                 </div>








                   
                </body>
             </html> " 
            );
        
        }

        //public bool RichOutput => true;
    }
}
