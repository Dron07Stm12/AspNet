using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static System.Net.WebRequestMethods;
using Microsoft.Extensions.Options;
using Platform.Platform;
using Microsoft.AspNetCore.Builder.Extensions;
using Platform;
using Microsoft.AspNetCore.Routing;

namespace Platform
{
    
   
    public class Startup
    {
      
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Настройка промежуточного программного обеспечения - общий шаблон
            //services.Configure<MessageOptions>(options => { options.CityName = "Albany"; });

            Action<MessageOptions> action = delegate (MessageOptions messageOptions) { messageOptions.CityName = "Japan"; };
            services.Configure<MessageOptions>(action);
            //services.Configure(action);

        }

        // также добавление  промежуточного программного обеспечения - общего шаблона(IOptions<MessageOptions> msgOptions)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<MessageOptions> msgOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //метод UseMiddleware регистрирует и выполняет класс где находится  компонент ПО промежуточного слоя
            //app.UseMiddleware<QueryStringMiddleWare>();

            //app.UseMiddleware<Population>();
            //app.UseMiddleware<Capital>();




            //Добавление ПО в конвеер запросов 
            app.UseRouting();

            //Метод UseEndpoints с помощью делегатов
            RequestDelegate request = async delegate (HttpContext http) { await http.Response.WriteAsync("Routed"); };
            Action<IEndpointRouteBuilder> action = delegate(IEndpointRouteBuilder endpoint) { endpoint.MapGet("route",request); };
            app.UseEndpoints(action);



            //Метод UseEndpoints используется для определения маршрутов, которые сопоставляют URL-адреса с конечными точками
            //URL-адреса сопоставляются с использованием шаблонов("routing"), которые сравниваются с путем URL-адресов запроса,
            //и каждый маршрут создает отношение между одним шаблоном URL и одной конечной точкой
            //Конечные точки определяются с помощью RequestDelegate, который является тем же делегатом, который используется
            //обычным промежуточным программным обеспечением. Поэтому конечные точки — это асинхронные методы,
            //которые получают объект HttpContext и используют его для создания ответа.


            



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("routing", async context => await context.Response.WriteAsync("Request Was Routed"));

            });

            app.Use(async(cont,next) => { await cont.Response.WriteAsync("Path"); });

        }


    }
}

















//Метод Use регистрирует компонент промежуточного слоя, который обычно выражается в виде лямбда-функции, которая получает каждый запрос.
//по мере прохождения по конвейеру(есть еще один метод, используемый для классов, как описано в следующем разделе).
//Аргументы лямбда-функции — это объект HttpContext и функция, которая вызывается, чтобы указать ASP.NET Core передать
//запрос к следующему компоненту промежуточного программного обеспечения в конвейере.
//Объект HttpContext описывает HTTP-запрос и HTTP-ответ, а также предоставляет дополнительный контекст, включая сведения.
//пользователя, связанного с запросом.

//метод Use регистрирует компонент промежуточного слоя(ПО) в методе Configure
//для обработки его(компонента) через конвеер запросов
//app.Use(async (context, next) =>
//{
//    if (context.Request.Method == HttpMethods.Get
//    && context.Request.Query["custom"] == "true")
//    {
//        await context.Response.WriteAsync("Custom Middleware \n");

//    }

//    await next();

//});







////В данном случае мы используем перегрузку метода Use, которая в качестве
////параметров принимает контекст запроса -объект HttpContext и делегат Func<Task>,
////который представляет собой ссылку на следующий в конвейере компонент middleware.
//app.Use(async (func2, nex) =>
//{
//    await func2.Response.WriteAsync("Dron \t");
//    await nex();
//});



//Func<HttpContext, Func<Task>, Task> func = async delegate (HttpContext context, Func<Task> task)
//{
//    if (context.Request.Method == HttpMethods.Get && context.Request.Query["myRequest"] == "true")
//    {
//        await context.Response.WriteAsync("myRequest\n");
//    }
//    await task();
//};

//app.Use(func);






//Func<HttpContext, Func<Task>, Task> func2 = async (http, con) => { await http.Response.WriteAsync("Nata\t");
//    await con.Invoke();
//};

////string date2 = default;
//Func<HttpContext, Func<Task>, Task> func = async delegate (HttpContext context, Func<Task> task)
//{
//    if (context.Response.StatusCode == 200)
//    {
//        await context.Response.WriteAsync($"status code: {context.Response.StatusCode}\t");
//        await task();
//    }

//    //date2 = DateTime.Now.ToShortDateString();
//    //await context.Response.WriteAsync("Lero\t");              
//};

//app.Use(func);






//app.Use((x, d) => x.Response.WriteAsync("next"));

//Func<HttpContext, Func<Task>, Task> func3 = delegate (HttpContext context, Func<Task> task)
//{
//    return context.Response.WriteAsync("Lero\t");

//    return task();

//};

//app.Run(async (context) => await context.Response.WriteAsync($"Date: {date2}"));
//string date = default;

//app.Use(async (context, next) =>
//{


//    date = DateTime.Now.ToShortDateString();
//    await next.Invoke();                 // вызываем middleware из app.Run

//});

//app.Run(async (context) => await context.Response.WriteAsync($"Date: {date}"));

//app.UseEndpoints(end => { end.MapGet("/", async c =>  await c.Response.WriteAsync("Kampot")); });






//ветвь
//app.Map("/branch11", branch =>
//{
//    branch.Use(async (cont, next) =>
//    {

//        if (cont.Request.Method == HttpMethods.Get && cont.Request.Query["my"] == "true")
//        {
//            await cont.Response.WriteAsync("Dron\n");
//        }


//        await next();
//    });

//    //    //или регистрируем ПО(мiddleware) через класс
//    //    branch.UseMiddleware<QueryStringMiddleWare>();

//    RequestDelegate handler = async delegate (HttpContext http) { await http.Response.WriteAsync("delegate RequestDelegate in methods Run"); };
//    branch.Run(handler);
//    //branch.Run(async(context) => { await context.Response.WriteAsync("methods Run"); });
//});





////Непосредственное использование делегатов Func,Action и метода Map
//Func<HttpContext, Func<Task>, Task> func = async delegate (HttpContext context, Func<Task> task)
//{
//    await context.Response.WriteAsync("func");
//    await task();
//};

//Action<IApplicationBuilder> value = delegate (IApplicationBuilder builder)
//{
//    builder.Use(func);
//};

//app.Map("/branch", value);










//Использование ветви branch метода Map для обслуживания другой линии в конвеере запросов
//а  конец  запросов служит метод Run, не пуская дальше в конвеер следующий компонент 
//app.Map("/branch", branch =>
//{
//    branch.UseMiddleware<QueryStringMiddleWare>();


//    branch.Use(async (cont, next) =>
//    {

//        await cont.Response.WriteAsync("dron\n");
//        await next();

//    });

//    // метод Run() отмечает конец конвеера линии запросов
//    branch.Run(async(context) => await context.Response.WriteAsync("game over middleware pipeline"));

//    branch.Use(async (context, next) =>
//    {

//        await context.Response.WriteAsync($"Branch Middleware");


//    });
//});




//Func<HttpContext, bool> predicate2 = delegate (HttpContext context)
//{
//    if (context.Request.Query.ContainsKey("pre"))
//    {
//        return true;
//    }
//    else
//    {
//        return false;
//    }
//};

//RequestDelegate request = async delegate (HttpContext http) { await http.Response.WriteAsync("delegate RequestDelegate2"); };
//app.MapWhen(predicate2, brench =>
//{
//    brench.UseMiddleware<QueryStringMiddleWare>();
//    brench.UseMiddleware<QueryStringMiddleWare>();
//    //QueryStringMiddleWare query = new QueryStringMiddleWare();
//    //brench.Run(query.Invoke);
//    brench.Run(request);
//    brench.UseMiddleware<QueryStringMiddleWare>();
//    brench.Use(async (cont, next) => { await cont.Response.WriteAsync("dron"); });

//});






//RequestDelegate request = async delegate (HttpContext http) { await http.Response.WriteAsync("delegate RequestDelegate2"); };
//////или с помощью делегата, безимянного блока кода
//Func<HttpContext, bool> predicate = delegate (HttpContext context)
//{
//    if (context.Request.Query.ContainsKey("pre"))
//    {
//        return true;
//    }
//    else
//    {
//        return false;
//    }
//};
//// условие ввода предиката http://localhost:1981/?pre
//app.MapWhen(predicate,
//          branch =>
//          {
//              branch.Use(async (context, next) =>
//              {
//                  await context.Response.WriteAsync("Custom Middleware2 \n");
//                  await next();
//              });

//              branch.Run(new QueryStringMiddleWare(request).Invoke);


//          });












//странная работа app.MapWhen(pred, action)
//Func<HttpContext, bool> pred = delegate (HttpContext http)
//{

//    if (http.Request.Query.ContainsKey("pre"))
//    {
//        return true;
//    }
//    else
//    {
//        return false;
//    }

//};

//Action<IApplicationBuilder> action = delegate
//{
//    app.Use(async (context, next) =>
//    {
//        if (context.Request.Method == HttpMethods.Get
//       /* && context.Request.Query["custom"] == "true"*/)
//        {
//            await context.Response.WriteAsync("Custom Middleware \n");

//        }

//        await next();

//    });

//    app.UseMiddleware<QueryStringMiddleWare>();
//    app.UseMiddleware<QueryStringMiddleWare>();
//    app.UseMiddleware<QueryStringMiddleWare>();
//    app.UseMiddleware<QueryStringMiddleWare>();

//};

//app.MapWhen(pred, action);





/////////////////////////////////////////Всякие методы


//QueryStringMiddleWare ware = new QueryStringMiddleWare();
//app.Map("/branch2", branch2 => { branch2.Run(ware.Invoke2);});
//app.Map("/branch3", branch3 => branch3.Run(new QueryStringMiddleWare().Invoke));

//RequestDelegate requestDelegate = async (cont) => { await cont.Response.WriteAsync("rt"); };
//RequestDelegate request = async delegate (HttpContext http) { await http.Response.WriteAsync("delegate RequestDelegate2"); };
//RequestDelegate requestDelegate = async (cont) => { await cont.Response.WriteAsync("rt"); };
//app.Map("/branch", branch => { branch.Run(request);});

// или так
//Не существует эквивалента метода UseMiddleware для ПО промежуточного слоя терминала,
//поэтому необходимо использовать метод Run, создав новый экземпляр класса промежуточного
//программного обеспечения и выбор его метода Invoke. Использование метода Run
//не изменяет вывод из ПО промежуточного слоя

//app.Map("/branch3", branch3 => branch3.Run(new QueryStringMiddleWare().Invoke));
//app.Map("/branch5", branch3 => branch3.Run(requestDelegate));
//QueryStringMiddleWare query = new QueryStringMiddleWare();
//app.Map("/br", br => br.Run(query.Invoke2));
//app.Map("/br2", br2 => br2.Run(QueryStringMiddleWare.Invoke3));
//app.Map("/branch", branch =>
//{
//    branch.UseMiddleware<QueryStringMiddleWare>();
//    branch.Use(async (context, next) => { await context.Response.WriteAsync("Nata"); });
//});

//Func<HttpContext, bool> predicate = http => http.Request.Query["id"] == "5";


///////////////////////////////////////////////






////короткое замыкание
//app.Use(async(cont,next) => {

//    if (cont.Request.Path == "/short")
//    {
//       await cont.Response.WriteAsync("\n short");
//    }
//    else
//    {
//        await next.Invoke();    
//    }

//});




//RequestDelegate request = async delegate (HttpContext http) { await http.Response.WriteAsync("delegate RequestDelegate2"); };
////RequestDelegate request1 = async (con) => await con.Response.WriteAsync("lymbda RequestDelegate2");


////Использование непосредственно делегата RequestDelegate и Action
//RequestDelegate request3 = async delegate (HttpContext http)
//{
//    await http.Response.WriteAsync("run");

//};
//Action<IApplicationBuilder> configuration = configuration => configuration.Run(request3);
//app.Map("/examination", configuration);




//работа делегата  RequestDelegate и метода Invoke класса Middleware
//public delegate Task RequestDelegate(HttpContext context) - сигнатура
//app.Map("/loc2", config => config.Run(new QueryStringMiddleWare(request2).Invoke));
//app.Map("/loc1", config => config.Run(new QueryStringMiddleWare().Invoke));
//RequestDelegate requestDelegate = async (cont) => { await cont.Response.WriteAsync("rt\n");};
//RequestDelegate requestDelegate2 = async (cont) => { await cont.Response.WriteAsync("rt\n"); };
//RequestDelegate requestDelegate1 = null;

//RequestDelegate request = async delegate (HttpContext http) {
//    await http.Response.WriteAsync("delegate RequestDelegate2\n");
//    if (requestDelegate1 != null)
//    {
//        await requestDelegate1(http);
//    }              
//    await requestDelegate(http);
//    await requestDelegate2(http);
//};

//app.Map("/loc3", config => config.Run(request));




//Func<HttpContext, Func<Task>, Task> func = async delegate (HttpContext context, Func<Task> task)
//{
//    //выполнение в конвеере одной задачи
//    await context.Response.WriteAsync("func\n");
//    // ссылка в конвеере на следующее ПО(Use()) и так далее
//    await task();
//};
////делегат для метода MapWhen
//Func<HttpContext, bool> predicate = delegate (HttpContext context)
//{
//    if (context.Request.Query.ContainsKey("pre"))
//    {
//        return true;
//    }
//    else
//    {
//        return false;
//    }
//};



//// делегат RequestDelegate
//RequestDelegate request2 = async delegate (HttpContext http) { await http.Response.WriteAsync("delegate RequestDelegate2"); };

//// метод MapWhen - форма запроса(с предикатом) localhost/?id
//app.MapWhen(predicate,
//           branch =>
//           {   //запросы ПО по конвееру 
//               branch.Use(async (context, next) =>
//               {
//                   await context.Response.WriteAsync("Custom Middleware2 \n");
//                   await next();
//               });
//               branch.Use(func);
//               branch.Use(func);
//               //branch.Run(request2);
//               //branch.Map("/branch", br => br.Use(func));

//               //Останавливает запросы и выполняет делегат( RequestDelegate) если он есть в классе QueryStringMiddleWare
//               branch.Run(new QueryStringMiddleWare().Invoke2);


//               branch.Run(new QueryStringMiddleWare(request2).Invoke);


//           });


//app.MapWhen(predicate => predicate.Request.Query.ContainsKey("pre2"),
//       action => {
//           action.Use(func);
//           action.Use(func);

//       });



//Работа с общим классом using Microsoft.Extensions.Options;
//app.Use(async (context, next) =>
//{

//    if (context.Request.Path == "/location")
//    {
//        MessageOptions opts = msgOptions.Value;
//        await context.Response.WriteAsync($"{opts.CityName}\t,{opts.CountryName}\n");

//    }

//    else
//    {
//        await next();
//    }

//});


//app.UseMiddleware<LocationMiddleware>();
//RequestDelegate request = null;
//app.Map("/v", v => v.Run(new LocationMiddleware(request, msgOptions).Invoke2));

