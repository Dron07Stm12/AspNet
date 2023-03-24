using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Platform.Platform
{
    public class CountryRouteConstraint : IRouteConstraint  
    {
        private static string[] countries = {"uk",/*"france",*/"monaco","shtat" };

        //Параметры метода Match предоставляют объект HttpContext для запроса, маршрут,
        //имя сегмента, переменные сегмента, извлеченные из URL-адреса
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            string segmentValue = values[routeKey] as string ?? "";
            return Array.IndexOf(countries, segmentValue.ToLower()) > -1;
        }
    }
}
