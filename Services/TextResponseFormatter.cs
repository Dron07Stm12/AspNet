using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;



namespace Platform.Services
{
   


    public class TextResponseFormatter: IResponseFormatter
    {
        private int responseCounter;    
        public static TextResponseFormatter shared;
       
        //private static int responseCounter2;

        //Класс TextResponseFormatter реализует интерфейс и записывает содержимое
        //в ответ в виде простой строки с префикс, чтобы было очевидно, когда используется класс.
        public async Task Format(HttpContext context,string content)
        {
            await context.Response.WriteAsync($"Responce {++responseCounter}: \n{content}");
            //await request(context);
        }

        public async Task Format2(HttpContext context, string content,int i)
        {
            await context.Response.WriteAsync($"Responce {++responseCounter}: \n{content}, \n{i}");
            //await request(context);
        }


        public static TextResponseFormatter Singleton {
            
            get 
            {
                if (shared == null)
                {
                    //cоздадим обьект
                    //Хотя в классе TextResponseFormatter
                    //обращаемся к статическому полю  shared, но в него мы помещаем обьект 
                    TextResponseFormatter.shared = new TextResponseFormatter();

                }
                //по сути через чтения свойства Singleton мы получим обьект: new TextResponseFormatter();
                // что потом даст нам возможность обратится через этот обьект к методу Format
                return shared;  
            } 
        
        
        }

        //public static async Task Format_Static(HttpContext context, string content)
        //{

        //    await context.Response.WriteAsync($"Responce {--responseCounter2}: \n{context}");

        //}
    }
}
