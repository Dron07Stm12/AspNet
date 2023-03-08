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

namespace Platform
{
    
   
    public class Startup
    {
      
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //��������� �������������� ������������ ����������� - ����� ������
            //services.Configure<MessageOptions>(options => { options.CityName = "Albany"; });

            Action<MessageOptions> action = delegate (MessageOptions messageOptions) { messageOptions.CityName = "Japan"; };
            services.Configure<MessageOptions>(action);
            //services.Configure(action);

        }

        // ����� ����������  �������������� ������������ ����������� - ������ �������(IOptions<MessageOptions> msgOptions)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<MessageOptions> msgOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Use(async (cont, next) =>
            //{
            //    await next.Invoke();
            //    await cont.Response.WriteAsync($"\n status code: {cont.Response.StatusCode}");
            //});


            //app.MapWhen(cont => cont.Request.Query.Keys.Contains("id"),
            //           branch => branch.Use(async (cont, next) => { await cont.Response.WriteAsync("Lero"); }));

            //app.MapWhen(context => {

            //    return context.Request.Query.ContainsKey("id") &&
            //            context.Request.Query["id"] == "5";
            //},  branch => branch.Use(async (cont, next) => { await cont.Response.WriteAsync("Lero"); }));



            //�����
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

            //    //    //��� ������������ ��(�iddleware) ����� �����
            //    //    branch.UseMiddleware<QueryStringMiddleWare>();

            //    RequestDelegate handler = async delegate (HttpContext http) { await http.Response.WriteAsync("delegate RequestDelegate in methods Run"); };
            //    branch.Run(handler);
            //    //branch.Run(async(context) => { await context.Response.WriteAsync("methods Run"); });
            //});


            //RequestDelegate request = async delegate (HttpContext http) { await http.Response.WriteAsync("delegate RequestDelegate2"); };
            ////RequestDelegate request1 = async (con) => await con.Response.WriteAsync("lymbda RequestDelegate2");


            //������������� ��������������� �������� RequestDelegate � Action
            //RequestDelegate request3 = async delegate (HttpContext http)
            //{
            // await http.Response.WriteAsync("run");

            //};
            //Action<IApplicationBuilder> configuration = configuration => configuration.Run(request3);
            //app.Map("/examination", configuration);




            //���������������� ������������� ��������� Func,Action � ������ Map
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




            //app.Map("/br", value => value.Run(request2));
            //app.Map("/branch2", branch2 => branch2.Run(request));
            //QueryStringMiddleWare query = new QueryStringMiddleWare();
            //app.Map("/branch3", branch3 => branch3.Run(query.Invoke));
            //app.Map("/branch", branch => branch.Run(new QueryStringMiddleWare().Invoke));




            //Func<HttpContext, Func<Task>, Task> func = async delegate (HttpContext context, Func<Task> task)
            //{
            //    if (context.Request.Method == HttpMethods.Get && context.Request.Query["myRequest"] == "true")
            //    {
            //        await context.Response.WriteAsync("myRequest\n");
            //    }
            //    await task();
            //};

            //app.Use(func);

            //����� ������������� �� ���������� �������� ��������� ����� ��� �������� ������� �� ���������,
            //� ����� ���������� ����� WriteAsync. ����� ��� ���������� ������ � ���� ������.
            //app.Use(async (cont, next) =>
            //{
            //    await next();
            //    await cont.Response.WriteAsync($"\n Status code: {cont.Response.StatusCode}"); // ����� WriteAsync - ��� ���������� ������ � ���� ������
            //    //await next();
            //});


            //app.Use(async (cont, next) =>
            //{
            //    await next.Invoke();
            //    await cont.Response.WriteAsync($"\n status code: {cont.Response.StatusCode}");
            //});






            //�������� ���������(��������) � �������(Request)
            //app.Use(async (cont, next) =>
            //{
            //    if (cont.Request.Path == "/kz")
            //    {
            //        await cont.Response.WriteAsync($"Request Short Circuited");

            //    }
            //    else { await next(); }

            //});

            //����� Use ������������ ��������� �������������� ����, ������� ������ ���������� � ���� ������-�������, ������� �������� ������ ������.
            //�� ���� ����������� �� ���������(���� ��� ���� �����, ������������ ��� �������, ��� ������� � ��������� �������).
            //��������� ������-������� � ��� ������ HttpContext � �������, ������� ����������, ����� ������� ASP.NET Core ��������
            //������ � ���������� ���������� �������������� ������������ ����������� � ���������.
            //������ HttpContext ��������� HTTP-������ � HTTP-�����, � ����� ������������� �������������� ��������, ������� ��������.
            //������������, ���������� � ��������.

            //����� Use ������������ ��������� �������������� ����(��) � ������ Configure
            //��� ��������� ���(����������) ����� ������� ��������
            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Method == HttpMethods.Get
            //    && context.Request.Query["custom"] == "true")
            //    {
            //        await context.Response.WriteAsync("Custom Middleware \n");

            //    }

            //    await next();

            //});






            //RequestDelegate request2 = async delegate (HttpContext http) { await http.Response.WriteAsync("delegate RequestDelegate2"); };
            //// ������������� � ������� ������ QueryStringMiddleWare ������ Run() - �� �������� �������� �������� RequestDelegate
            //// ����� MapWhen - ����� �������(� ����������) localhost/?id
            //app.MapWhen(cont => cont.Request.Query.Keys.Contains("id"),
            //           branch =>
            //           {                       
            //               branch.Use(async (context, next) =>
            //               {
            //                   await context.Response.WriteAsync("Custom Middleware2 \n");
            //                   await next();
            //               });

            //               branch.Run(new QueryStringMiddleWare(request2).Invoke);


            //           });
            ////��� � ������� ��������, ����������� ����� ����
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
            //app.MapWhen(predicate,
            //          branch =>
            //          {
            //              branch.Use(async (context, next) =>
            //              {
            //                  await context.Response.WriteAsync("Custom Middleware2 \n");
            //                  await next();
            //              });

            //              branch.Run(new QueryStringMiddleWare(request2).Invoke);


            //          });

            ////����� Map - ��� ������� �������(���������)
            //app.Map("/dron", cont =>
            //{

            //    cont.Use(async (cont, next) => { await cont.Response.WriteAsync("PO\n"); await next(); });
            //    cont.UseMiddleware<QueryStringMiddleWare>();
            //    cont.Run(new QueryStringMiddleWare(request2).Invoke);

            //    cont.UseMiddleware<QueryStringMiddleWare>();


            //});






            //���������� �� ������ ������� ����� ���� �������� ���, ����� �� ����� ���� ������������
            //��� ������� ������������� ��, ��� � ��� ������������ ������������� ��, ��� ��������.
            //��������� ����� ���������� ������� ������ � ��� ������, ���� ������������ ���� �������������
            //��������� �������� ��� nextDelegate ��������

            //QueryStringMiddleWare ware = new QueryStringMiddleWare();
            //app.Map("/branch2", branch2 => { branch2.Run(ware.Invoke);});
            //RequestDelegate requestDelegate = async (cont) => { await cont.Response.WriteAsync("rt"); };
            //RequestDelegate request = async delegate (HttpContext http) { await http.Response.WriteAsync("delegate RequestDelegate2"); };
            //RequestDelegate requestDelegate = async (cont) => { await cont.Response.WriteAsync("rt"); };
            //app.Map("/branch", branch => { branch.Run(request);});

            // ��� ���
            //�� ���������� ����������� ������ UseMiddleware ��� �� �������������� ���� ���������,
            //������� ���������� ������������ ����� Run, ������ ����� ��������� ������ ��������������
            //������������ ����������� � ����� ��� ������ Invoke. ������������� ������ Run
            //�� �������� ����� �� �� �������������� ����

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




            //����� UseMiddleware ������������ � ��������� ����� ��� ���������  ��������� �� �������������� ����
            //app.UseMiddleware<QueryStringMiddleWare>();






            //app.Use(async (context, next) =>
            //{
            //     await context.Response.WriteAsync("Custom Middleware \n");
            //     await next();

            //});
            //app.Map("/branch3", branch3 => branch3.Run(requestDelegate));
            //app.UseMiddleware<QueryStringMiddleWare>();



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
            RequestDelegate request = null;
            app.Map("/v", v => v.Run(new LocationMiddleware(request, msgOptions).Invoke2));




            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });


        }


    }
}



////� ������ ������ �� ���������� ���������� ������ Use, ������� � ��������
////���������� ��������� �������� ������� -������ HttpContext � ������� Func<Task>,
////������� ������������ ����� ������ �� ��������� � ��������� ��������� middleware.
//app.Use(async (func2, nex) =>
//{
//    await func2.Response.WriteAsync("Dron \t");
//    await nex();
//});



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
//    await next.Invoke();                 // �������� middleware �� app.Run

//});

//app.Run(async (context) => await context.Response.WriteAsync($"Date: {date}"));

//app.UseEndpoints(end => { end.MapGet("/", async c =>  await c.Response.WriteAsync("Kampot")); });




//������������� ����� branch ������ Map ��� ������������ ������ ����� � �������� ��������
//�  �����  �������� ������ ����� Run, �� ������ ������ � ������� ��������� ��������� 
//app.Map("/branch", branch =>
//{
//    branch.UseMiddleware<QueryStringMiddleWare>();


//    branch.Use(async (cont, next) =>
//    {

//        await cont.Response.WriteAsync("dron\n");
//        await next();

//    });

//    // ����� Run() �������� ����� �������� ����� ��������
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


////�������� ���������
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


//������ ��������  RequestDelegate � ������ Invoke ������ Middleware
//public delegate Task RequestDelegate(HttpContext context) - ���������
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

