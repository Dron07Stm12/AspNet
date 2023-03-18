using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;



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

                    //URL-адреса генерируются с помощью класса LinkGenerator.
                    // он должен быть получен используя функцию внедрения зависимостей
                case "monaco":
                    //context.Response.Redirect($"/population/{coutry}");

                    // изменил конечную точку Capital, убрав прямую зависимость от URL-адреса /population и полагаясь на
                    //функции маршрутизации для создания URL - адреса.

                    //Класс LinkGenerator предоставляет метод GetPathByRouteValues,
                    //который используется для создания URL-адреса, который будет использоваться в перенаправление

                    //Аргументами метода GetPathByRouteValues ​​являются объект HttpContext конечной точки, имя маршрута, который будет
                    //используется для создания ссылки, и объект, который используется для предоставления значений для переменных сегмента. Метод GetPathByRouteValues
                    //возвращает URL-адрес, который будет перенаправлен в конечную точку Population
                    //Запрос будет перенаправлен на конечную точку Capital, которая сгенерирует URL-адрес.
                    LinkGenerator generator = context.RequestServices.GetService<LinkGenerator>();
                    string url = generator.GetPathByRouteValues(context, "population", new  { city = coutry });
                    context.Response.Redirect(url);
                    return;

                case "shtat":
                    LinkGenerator generator2 = context.RequestServices.GetService<LinkGenerator>();
                    string url2 = generator2.GetPathByRouteValues(context,"population", new  {city = coutry });
                    context.Response.Redirect(url2);    
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
