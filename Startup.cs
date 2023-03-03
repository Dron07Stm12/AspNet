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

namespace Platform
{
    
   
    public class Startup
    {
      
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public  void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            //RequestDelegate request2 = async delegate (HttpContext http)
            //{
            //    if (http != null)
            //    {
            //        await http.Response.WriteAsync("");
            //    }

            //};



            //Action<IApplicationBuilder> configuration = configuration =>  configuration.Run(request2);
            ////app.Map("/examination",configuration);



            //Func<HttpContext, Func<Task>, Task> func = async delegate (HttpContext context, Func<Task> task)
            //{
            //    await context.Response.WriteAsync("func");
            //    await task();
            //};




            //Action<IApplicationBuilder> value = delegate (IApplicationBuilder builder)
            //{

            //    builder.Use(func);

            //    //builder.Map("/branch5", brench => brench.Run(request1));
            //    //builder.Use(async (cont, next) => { await cont.Response.WriteAsync("use");
            //    //    if (next != null)
            //    //    {
            //    //        await next();

            //    //    }

            //    //});
            //    //builder.Run(request);         
            //};


            //app.Map("/branch",value);
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





            //app.Use(async(cont,next) => {
            //    if (cont.Request.Path == "/dron")
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


            //// ����� MapWhen - ����� ������� localhost/?id
            //app.MapWhen(cont => cont.Request.Query.Keys.Contains("id"),
            //           branch => {
            //               branch.UseMiddleware<QueryStringMiddleWare>();
            //               branch.UseMiddleware<QueryStringMiddleWare>();
            //               branch.UseMiddleware<QueryStringMiddleWare>();
            //               branch.UseMiddleware<QueryStringMiddleWare>();
            //               branch.Use(async (cont, next) => { await cont.Response.WriteAsync("Lero"); });
            //           });




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


            //���������� �� ������ ������� ����� ���� �������� ���, ����� �� ����� ���� ������������
            //��� ������� ������������� ��, ��� � ��� ������������ ������������� ��, ��� ��������.
            //��������� ����� ���������� ������� ������ � ��� ������, ���� ������������ ���� �������������
            //��������� �������� ��� nextDelegate ��������

            //QueryStringMiddleWare ware = new QueryStringMiddleWare();
            //app.Map("/branch2", branch2 => { branch2.Run(ware.Invoke); });

            //RequestDelegate request = async delegate (HttpContext http) { await http.Response.WriteAsync("delegate RequestDelegate2"); };
            //app.Map("/branch", branch => { branch.Run(request); });

            // ��� ���
            app.Map("/branch3", branch3 => branch3.Run(new QueryStringMiddleWare().Invoke));


            //app.Map("/branch", branch =>
            //{
            //    branch.UseMiddleware<QueryStringMiddleWare>();
            //    branch.Use(async (context, next) => { await context.Response.WriteAsync("Nata"); });
            //});



            //����� UseMiddleware ������������ � ��������� ����� ��� ���������  ��������� �� �������������� ����
            app.UseMiddleware<QueryStringMiddleWare>();

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

