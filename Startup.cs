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
using System.Security.Policy;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Primitives;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing.Patterns;
using Platform.Services;
using System.Threading;

namespace Platform
{
    
   
    

    public class Startup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        //������ �������������� � ������ ConfigureServices ������ Startup � �������������� ������� ���������� ��
        // �������� IServiceCollection. ����� �������� ������ ��� ���������� IResponseFormatter.
        
        public void ConfigureServices(IServiceCollection services)
        {
            //����� AddSingleton �������� ����� �� ������� ����������, ��������� ��� �����, � �������� ASP.NET Core, ��� ���� ������
            //������� ������������ ��� �������������� ������������ � �������.
            // ��������� � ����� ���������� ����������� ��� ��������� �������������� ����.����� ������������ ������, � �������
            //�������� � ������ Configure.
            services.AddSingleton<IResponseFormatter, HtmlResponseFormatter>();

        }

        // ����� ����������  �������������� ������������ ����������� - ������ �������(IOptions<MessageOptions> msgOptions)

        //����� �������� ��������� ����������� �� ���������� IResponseFormatter, � ���������, ��� ����� ������� ��
        //����������.����� ������� ������ Configure ����������� ��� ���������, �������������� ����������� �
        //������ �����������, ����� ����������, �������� �� ��������� �����������.����������� � ������ ConfigureServices
        //�������� ������� ��������� ������������, ��� ����������� �� ���������� IResponseFormatter ����� ���� ��������� �
        //������� ������� HtmlResponseFormatter.������ ��������� � ������������ � �������� ��������� ��� ������ ������.
        //��������� ������, ������� ��������� �����������, ��������������� ����� ������ ��� �������, ������� �� ����������,
        //�������, ��� ��� ���� ��������, �������������� �������� ��� ��������� ������������.

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IResponseFormatter formatter_html)
        {
            app.UseDeveloperExceptionPage();
            app.UseRouting();

            app.UseMiddleware<Middelware_service>();

            app.Use(delegate (HttpContext http,Func<Task> tsk) {
                if (http.Request.Path == "/dron")
                {
                    return new Middelware_service().Invoke2(http);
                }
                else
                {
                    return tsk();   
                }          
            });


            app.Use(delegate (RequestDelegate request) { return new Middelware_service(request,formatter_html).Invoke3; });

            app.Map("/method2", delegate (IApplicationBuilder builder2)
            {

                builder2.Use(delegate (RequestDelegate request)
                {

                    return new Middelware_service(request, formatter_html).Invoke3;
                });

            });


            Func<HttpContext, Func<Task>, Task> func = async delegate (HttpContext http, Func<Task> tsk)
            {
              if (http.Request.Path == "/path")
              {
                    await Middelware_service.Invoke_format(http,"dron",formatter_html);
              }
              //������ �������� �� ��������
              else
              {
                await tsk();
              }
           };
            app.Use(func);


            app.Use(async(cont,next) => {

                if (cont.Request.Path == "/use")
                {
                    await Middelware_service.Invoke_format(cont, "use", formatter_html);
                }

                else { await next(); }
            
            });

            app.Use(async delegate (HttpContext context,Func<Task> tsk) 
            {
                if (context.Request.Path == "/delegate_use")
                {
                    await Middelware_service.Invoke_format(context, "delegate_use", formatter_html);
                }


                else { await tsk(); }



            });



            //app.Use(delegate(RequestDelegate request) 
            //{
            //    request = async delegate (HttpContext context)
            //    {

            //        if (context.Request.Path == "/use_delegate")
            //        {
            //            await Middelware_service.Invoke_format(context, "use_delegate", formatter_html);
            //        }


            //    };

            //    return request;
            //});

            // ����� Map ���������� ����� ����("/map") � �������  Action<IApplicationBuilder>
            // c �������� ����� IApplicationBuilder - ����� ����(������ ����������  IApplicationBuilder) �� ���������� � ������ Use
            // � ������ � ���������� IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware);
            // � �������� �������(request) �������� ������ ������ Invoke_format � ������ ��������(RequestDelegate) � �������(IResponseFormatter formatter_html)
            app.Map("/map", delegate (IApplicationBuilder builder)
            {
                builder.Use(delegate (RequestDelegate request)
                {

                    request = async delegate (HttpContext http)
                    {
                        await Middelware_service.Invoke_format(http,"Map",formatter_html);
                        
                    };
                    return request;


                });
            });





            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/br", WeatherEndpoint.Endpoint_service);
                endpoints.Map("/br2", async (cont) => await WeatherEndpoint.Endpoint_service(cont));
                endpoints.MapGet("/br3", async delegate (HttpContext context) { await WeatherEndpoint.Endpoint_service(context); });
                endpoints.MapGet("/br4",  delegate (HttpContext context) { return WeatherEndpoint.Endpoint_service(context); });

                endpoints.MapGet("/br5", async delegate (HttpContext http) { await WeatherEndpoint.Endpoint_format(http, "await_dron", formatter_html); });
                endpoints.MapGet("/br6", delegate (HttpContext http) { return WeatherEndpoint.Endpoint_format(http, "return_dron", formatter_html); });
                endpoints.Map("/br7", async (cont) => await WeatherEndpoint.Endpoint_format(cont, "lymbda", formatter_html));

                endpoints.MapGet("/br8",  delegate (HttpContext context) { return  formatter_html.Format(context, "class HtmlResponseFormatter"); });

                // ����� Middelware_service()
                endpoints.MapGet("/br9", delegate (HttpContext http) { return new Middelware_service().Invoke2(http); });

                endpoints.MapWeather("/endpoint/class");
                endpoints.MapUser("/endpoint/user");
                
                Action<IEndpointRouteBuilder> action = delegate (IEndpointRouteBuilder routeBuilder) {

                    Type type = typeof(IResponseFormatter);

                    IResponseFormatter formatter = (IResponseFormatter)routeBuilder.ServiceProvider.GetService(type);
                    routeBuilder.Map("/t", delegate (HttpContext context) { return WeatherEndpoint.Endpoint_format2(context, formatter); });

                };
                action(endpoints);

                Action<IEndpointRouteBuilder> action2 = delegate (IEndpointRouteBuilder routeBuilder) {
                    Type type = typeof(IResponseFormatter);
                    IResponseFormatter formatter = (IResponseFormatter)routeBuilder.ServiceProvider.GetService(type);
                    routeBuilder.MapGet("/action2", delegate (HttpContext context) { return WeatherEndpoint.Endpoint_format(context,
                        "Action<IEndpointRouteBuilder> action2", formatter); 
                    });
                };
                action2(endpoints);

                endpoints.MapUserr("/endpoint/user3");
                endpoints.MapWeather("/endpoint/user4");

            });


        }
    }
}


//Func<HttpContext, Func<Task>, Task> func = async delegate (HttpContext http, Func<Task> task) 
//{
//    if (http.Request.Path == "/middleware/functions")
//    {
//        await formatter.Format(http, "Middleware Function: It is snowing in Chicago");
//    }
//    else { await task.Invoke(); }

//};
//app.Use(func);

//RequestDelegate request = async delegate (HttpContext context) { await context.Response.WriteAsync("Endpoint Function: It is sunny in LA"); };
//Action<IEndpointRouteBuilder> action = delegate(IEndpointRouteBuilder endpoint) {

//    endpoint.MapGet("/endpoint/class",WeatherEndpoint.Invoke);

//    endpoint.MapGet("/endpoint/function",request);


//};    


//app.UseEndpoints(action);














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







////� ������ ������ �� ���������� ���������� ������ Use, ������� � ��������
////���������� ��������� �������� ������� -������ HttpContext � ������� Func<Task>,
////������� ������������ ����� ������ �� ��������� � ��������� ��������� middleware.
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
//    await next.Invoke();                 // �������� middleware �� app.Run

//});

//app.Run(async (context) => await context.Response.WriteAsync($"Date: {date}"));

//app.UseEndpoints(end => { end.MapGet("/", async c =>  await c.Response.WriteAsync("Kampot")); });






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





////���������������� ������������� ��������� Func,Action � ������ Map
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






//RequestDelegate request = async delegate (HttpContext http) { await http.Response.WriteAsync("delegate RequestDelegate2"); };
//////��� � ������� ��������, ����������� ����� ����
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
//// ������� ����� ��������� http://localhost:1981/?pre
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












//�������� ������ app.MapWhen(pred, action)
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





/////////////////////////////////////////������ ������


//QueryStringMiddleWare ware = new QueryStringMiddleWare();
//app.Map("/branch2", branch2 => { branch2.Run(ware.Invoke2);});
//app.Map("/branch3", branch3 => branch3.Run(new QueryStringMiddleWare().Invoke));

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


///////////////////////////////////////////////






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




//RequestDelegate request = async delegate (HttpContext http) { await http.Response.WriteAsync("delegate RequestDelegate2"); };
////RequestDelegate request1 = async (con) => await con.Response.WriteAsync("lymbda RequestDelegate2");


////������������� ��������������� �������� RequestDelegate � Action
//RequestDelegate request3 = async delegate (HttpContext http)
//{
//    await http.Response.WriteAsync("run");

//};
//Action<IApplicationBuilder> configuration = configuration => configuration.Run(request3);
//app.Map("/examination", configuration);




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




//Func<HttpContext, Func<Task>, Task> func = async delegate (HttpContext context, Func<Task> task)
//{
//    //���������� � �������� ����� ������
//    await context.Response.WriteAsync("func\n");
//    // ������ � �������� �� ��������� ��(Use()) � ��� �����
//    await task();
//};
////������� ��� ������ MapWhen
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



//// ������� RequestDelegate
//RequestDelegate request2 = async delegate (HttpContext http) { await http.Response.WriteAsync("delegate RequestDelegate2"); };

//// ����� MapWhen - ����� �������(� ����������) localhost/?id
//app.MapWhen(predicate,
//           branch =>
//           {   //������� �� �� �������� 
//               branch.Use(async (context, next) =>
//               {
//                   await context.Response.WriteAsync("Custom Middleware2 \n");
//                   await next();
//               });
//               branch.Use(func);
//               branch.Use(func);
//               //branch.Run(request2);
//               //branch.Map("/branch", br => br.Use(func));

//               //������������� ������� � ��������� �������( RequestDelegate) ���� �� ���� � ������ QueryStringMiddleWare
//               branch.Run(new QueryStringMiddleWare().Invoke2);


//               branch.Run(new QueryStringMiddleWare(request2).Invoke);


//           });


//app.MapWhen(predicate => predicate.Request.Query.ContainsKey("pre2"),
//       action => {
//           action.Use(func);
//           action.Use(func);

//       });



//������ � ����� ������� using Microsoft.Extensions.Options;
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

//������ � ����������� � ���������
//endpoints.MapGet("{first}/{second}/{third}",async cont => {
//    await cont.Response.WriteAsync("Request Was Routed\n");

//    //IEnumerator<KeyValuePair<string,object>> people_struct2 = cont.Request.RouteValues.GetEnumerator();
//    //while (people_struct2.MoveNext())
//    //{
//    //    await cont.Response.WriteAsync($"{people_struct2.Current.Value}\t {people_struct2.Current.Key}\n"); 
//    //}
//    //people_struct2.Reset();

//    if (cont.Request.RouteValues["d"] is string)
//    {

//    }

//    foreach (KeyValuePair<string, object> item in cont.Request.RouteValues)
//    {
//        await cont.Response.WriteAsync($"{item.Key}\t, {item.Value}\n");
//    }
//});

//������ � ����������� ��������
//RequestDelegate requestDelegate = async delegate (HttpContext http) { await http.Response.WriteAsync("Midelwaire"); };
//RequestDelegate requestDelegate2 = async delegate (HttpContext context) {   context.Response.StatusCode = StatusCodes.Status414UriTooLong;};


//    //endpoints.MapGet("middleware", new QueryStringMiddleWare(requestDelegate).Invoke2);
//    //endpoints.MapGet("routing", async context => await context.Response.WriteAsync("Request Was Routed"));
//    //endpoints.MapGet("population/london", requestDelegate);

//��� ������������� - ���������� �������������  ������� url ����("population/paris" ��� "capital/paris")
// � �������� ����� � ���� ������� ������(new Population().Invoke ��� new Capital().Invoke),
//endpoints.MapGet("population/paris", new Population().Invoke);
//endpoints.MapGet("capital/uk", new Capital().Invoke);

//������ �������� �����(����� ����������� �������) � �����������({city � {coutry}) ������� �������� url
//endpoints.MapGet("population/{city}", PopulationStatic.Endpointe);
//endpoints.MapGet("capital/{coutry}", CapitalStatic2.Endpointe);
//��� ��� � �������� ������
//endpoints.MapGet("capital/{country}", new CapitalStatic().Endpoint);
//endpoints.MapGet("{key}", new City().Endpoints);




//������������� ���������� �������� �� ������ �������� URL

//endpoints.MapGet("files/{filename}.{ext}",async(cont) =>
//{
//    await cont.Response.WriteAsync("Request Was Routed\n");

//    foreach (var item in cont.Request.RouteValues)
//    {
//        await cont.Response.WriteAsync($"{item.Key}: {item.Value}\n");
//    }
//});



//RequestDelegate requestDelegate3 = async delegate (HttpContext http) {
//    foreach (KeyValuePair<string,object> item in http.Request.RouteValues)
//    {
//        await http.Response.WriteAsync($" {item.Key}:\t {item.Value}\n");
//    }

//};
//endpoints.MapGet("{one}/{two}/{three}.{d}",requestDelegate3);



//endpoints.MapGet("f/{name}.{exe}",requestDelegate3);




//������ � ������������� ����������(���������)
//StringValues strings = new StringValues("password3");
////strings = "password2";

//RequestDelegate request3 = async delegate (HttpContext context) {

//    //string s = context.Request.Query["password"];
//    string s2 = context.Request.Query[strings];
//    if (context.Request.Method == HttpMethods.Get && s2 == "dron")
//    {

//        await context.Response.WriteAsync("Query_dron\n");

//        foreach (KeyValuePair<string, object> item in context.Request.RouteValues)
//        {
//            await context.Response.WriteAsync($"{item.Key}\t{item.Value}\n");
//        }

//    }          
//};
//endpoints.MapGet("{one}/{two}/{*thee}", request3);



//������������� �����������  � ����������
//endpoints.MapGet("capital/{coutry:regex(^uk|monaco$|shtat$)}", CapitalStatic2.Endpointe);
//endpoints.MapGet("size/{city?}", PopulationStatic.Endpointe).WithMetadata(new RouteNameMetadata("population"));

//endpoints.MapGet("{one:alpha:length(4)}/{two:bool}", async (cont) => {
//    foreach (KeyValuePair<string, object> item in cont.Request.RouteValues)
//    {
//        await cont.Response.WriteAsync($"{item.Key}\t{item.Value}\n");
//    }

//});


//endpoints.MapGet("capital/{coutry:countryName}", CapitalStatic2.Endpointe);


///////////////////// ����� Startap  �� �������� ��������� ������������
//public class Startup
//{

//    // This method gets called by the runtime. Use this method to add services to the container.
//    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
//    public void ConfigureServices(IServiceCollection services)
//    {
//        //��������� �������������� ������������ ����������� - ����� ������
//        //����� ������
//        services.Configure<MessageOptions>(options => { options.CityName = "Albany"; });
//        // ��� ����� �������
//        //Action<MessageOptions> action = delegate (MessageOptions messageOptions) { messageOptions.CityName = "Japan"; };
//        //services.Configure<MessageOptions>(action);
//        //services.Configure(action);

//        Type type = typeof(CountryRouteConstraint);
//        Action<RouteOptions> action1 = delegate (RouteOptions routeOptions) { routeOptions.ConstraintMap.Add("countryName", type); };
//        services.Configure<RouteOptions>(action1);
//        //��� ����� ������
//        //services.Configure<RouteOptions>(opts => opts.ConstraintMap.Add("countryName", typeof(CountryRouteConstraint)));

//    }

//    // ����� ����������  �������������� ������������ ����������� - ������ �������(IOptions<MessageOptions> msgOptions)
//    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<MessageOptions> msgOptions)
//    {
//        if (env.IsDevelopment())
//        {
//            app.UseDeveloperExceptionPage();
//        }

//        //����� UseMiddleware ������������ � ��������� ����� ��� ���������  ��������� �� �������������� ����
//        //app.UseMiddleware<QueryStringMiddleWare>();

//        //������ �� 
//        //app.UseMiddleware<Population>();
//        //app.UseMiddleware<Capital>();




//        //���������� �� � ������� �������� 
//        app.UseRouting();

//        //����� UseEndpoints � ������� ���������
//        RequestDelegate request = async delegate (HttpContext http) { await http.Response.WriteAsync("Routed"); };
//        RequestDelegate request2 = delegate (HttpContext context) { return context.Response.WriteAsync("mehtod return"); };
//        //Action<IEndpointRouteBuilder> action = delegate(IEndpointRouteBuilder endpoint) { endpoint.MapGet("route",request2); };
//        //app.UseEndpoints(action);



//        //����� UseEndpoints ������������ ��� ����������� ���������, ������� ������������ URL-������ � ��������� �������
//        //URL-������ �������������� � �������������� ��������("routing"), ������� ������������ � ����� URL-������� �������,
//        //� ������ ������� ������� ��������� ����� ����� �������� URL � ����� �������� ������
//        //�������� ����� ������������ � ������� RequestDelegate, ������� �������� ��� �� ���������, ������� ������������
//        //������� ������������� ����������� ������������. ������� �������� ����� � ��� ����������� ������,
//        //������� �������� ������ HttpContext � ���������� ��� ��� �������� ������.

//        //� �������� ���������� ��������� �������� ������� - ������ HttpContext � ������� Func<Task>,
//        //������� ������������ ����� ������ �� ��������� � ��������� ��������� middleware.

//        //( Func<Task> task)������������� �����, ������� �� ����� ��������� � ���������� �������� ����, ���������� � ��������� Task.
//        //public delegate TResult Func<in T1, in T2, out TResult>(T1 arg1, T2 arg2);
//        //out TResult - ������������ �������� ������, ������� ������������� ���� �������.
//        Func<HttpContext, Func<Task>, Task> func = async delegate (HttpContext context, Func<Task> task)
//        {
//            Endpoint end = context.GetEndpoint();

//            if (end != null)
//            {
//                await context.Response.WriteAsync($"{end.DisplayName} Selected\n");
//            }
//            else { await context.Response.WriteAsync("No Endpoint Selected \n"); }
//            await task.Invoke();
//        };
//        app.Use(func);


//        app.UseEndpoints(endpoints =>
//        {

//            //��� ����� ������
//            endpoints.Map("{number:int}", request2).WithDisplayName("Int Endpoint").Add(b => ((RouteEndpointBuilder)b).Order = 1);
//            //endpoints.MapFallback(request);
//            //���������� ��������������. Downcasting, ����� ��������� �����������(� �������� Order) ������������ ������(RouteEndpointBuilder)
//            Action<EndpointBuilder> action1 = delegate (EndpointBuilder route) { ((RouteEndpointBuilder)route).Order = 2; };
//            endpoints.Map("{number:double}", request).WithDisplayName("double Endpoint").Add(action1);


//        });

//        //app.Use(async (cont, next) => { await cont.Response.WriteAsync("\nPath2"); });

//    }





////////////////////////////////////////////////////////////////////////////////////////////
//Func<RequestDelegate, RequestDelegate> middleware = delegate (RequestDelegate request)
//{
//    //request = async delegate (HttpContext http) { await http.Response.WriteAsync("dron"); };
//    //return request;
//    return new WeatherMiddleware().Invoke2;

//};

//Func<HttpContext, Func<Task>, Task> func2 = async delegate (HttpContext http, Func<Task> task)
//{
//    //await task.Invoke();

//    await task();
//};


//app.Map("/branch", delegate (IApplicationBuilder builder) {

//    builder.Use(func2);

//    builder.UseMiddleware<WeatherMiddleware>();
//    builder.Use(middleware);
//    //builder.Use(func2);
//    //builder.Use(func2);
//    //builder.UseMiddleware<WeatherMiddleware>();
//    builder.Use(middleware);
//    //builder.Use(func2);
//    //builder.Run(new WeatherMiddleware().Invoke2);


//});
/////////////////////////////////////////////////////////////////////////////////////////////



//���������������� ������������� ��������
//RequestDelegate request = async delegate (HttpContext context) { await context.Response.WriteAsync("Endpoint Function: It is sunny in LA"); };
//Action<IEndpointRouteBuilder> action = delegate (IEndpointRouteBuilder endpoint) {

//    endpoint.MapGet("/endpoint/function5", request);

//};

//app.UseEndpoints(action);


//���������� ������ ��������� ������������ 


//public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//{
//    app.UseDeveloperExceptionPage();
//    app.UseRouting();

//    app.UseMiddleware<WeatherMiddleware>();

//    //��������� ������������
//    IResponseFormatter formatter = new TextResponseFormatter();
//    app.Use(async (context, next) =>
//    {
//        if (context.Request.Path == "/middleware/function")
//        {
//            //������� �������������� ������������ �����������,����� ������
//            //await formatter.Format(context, "Middleware Function: It is snowing in Chicago\n");
//            //������ TextResponseFormatter �������� ����� ������ ����� ����������� �������� Singleton
//            //await TextResponseFormatter.Singleton.Format(context, "Middleware static Function\n");


//            await TypeBroker.Formatter.Format(context, "class broker");


//            // await TextResponseFormatter.Format_Static(context, "Middleware static Function");

//        }

//        else { await next.Invoke(); }

//    });

//    //����� �������
//    TextResponseFormatter text = new TextResponseFormatter();
//    Func<HttpContext, Func<Task>, Task> func = async delegate (HttpContext http, Func<Task> task)
//    {
//        if (http.Request.Path == "/middleware/functions2")
//        {
//            await text.Format(http, "Func Middleware, new TextResponseFormatter()\n");
//            await formatter.Format(http, "Func Middleware, interface IResponseFormatter\n");
//        }
//        else { await task.Invoke(); }

//    };
//    app.Use(func);


//    app.UseEndpoints(endpoints =>
//    {
//        TextResponseFormatter text2 = new TextResponseFormatter();
//        //endpoints.MapGet("/endpoint/class", WeatherEndpoint.Endpoint);

//        //endpoints.MapGet("/endpoint/function", async delegate (HttpContext context) { await text2.Format(context, "string delegat"); });
//        //endpoints.MapGet("/endpoint/function2", async cont => { await text2.Format(cont, "string lymbda"); });

//        //endpoints.MapGet("/endpoint/function3",async delegate(HttpContext context)
//        //{
//        //    //������  ����������� �������� Singleton ������ TextResponseFormatter,
//        //    // ������� ���������� ����(shared), � ������� �������  ������ TextResponseFormatter
//        //    await TextResponseFormatter.Singleton.Format(context,"static delegat Singleton"); 
//        //});

//        endpoints.MapGet("/endpoint/function4", async (cont) => { await TextResponseFormatter.Singleton.Format(cont, "static lymbda Singleto"); });

//        endpoints.MapGet("/endpoint/function5", async delegate (HttpContext context) { await TypeBroker.Formatter.Format(context, "endpoint_broker"); });

//        endpoints.MapGet("/endpoint/function6", async delegate (HttpContext http) { await text2.Format2(http, "������ new TextResponseFormatter()", 3); });

//        //endpoints.MapGet("/endpoint/function3", async cont => { await TextResponseFormatter.Singleton.Format(cont, "function3"); });
//        ////endpoints.Map("/br", new WeatherMiddleware().Invoke2);
//        //endpoints.MapGet("/endpoint/function", async context =>
//        //{
//        //    await context.Response.WriteAsync("Endpoint Function: It is sunny in LA");
//        //});
//    });


//}


//������ ����� ��������
//Func<HttpContext, Func<Task>, Task> func = async delegate (HttpContext context, Func<Task> task)
//{
//    if (context.Request.Method == HttpMethods.Get && context.Request.Query["myRequest"] == "true")
//    {
//        await WeatherMiddleware.Format(context, "func");
//    }
//    await task();
//};

//Action<IApplicationBuilder> value = delegate (IApplicationBuilder builder)
//{

//    builder.Use(func);

//};
//app.Map("/branch", value);

//app.Map("/br", delegate (IApplicationBuilder builder)
//{

//    builder.Use(func);
//});



//����� Startap
//public class Startup
//{

//    // This method gets called by the runtime. Use this method to add services to the container.
//    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
//    public void ConfigureServices(IServiceCollection services)
//    {


//    }

//    // ����� ����������  �������������� ������������ ����������� - ������ �������(IOptions<MessageOptions> msgOptions)
//    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//    {
//        app.UseDeveloperExceptionPage();
//        app.UseRouting();

//        //app.UseMiddleware<WeatherMiddleware>();

//        //��������� ������������

//        app.Use(async (context, next) =>
//        {
//            if (context.Request.Path == "/middleware/function")
//            {

//                await TypeBroker.Formatter.Format(context, "class broker");

//            }
//            else { await next.Invoke(); }

//        });

//        app.Use(async (context, next) =>
//        {
//            if (context.Request.Path == "/middleware/function2")
//            {

//                await WeatherMiddleware.Format(context, "middl");

//            }
//            else { await next.Invoke(); }

//        });


//        app.Map("/b", delegate (IApplicationBuilder builder) { builder.Run(async delegate (HttpContext http) { await http.Response.WriteAsync("Map"); }); });

//        app.UseEndpoints(endpoints =>
//        {
//            endpoints.MapGet("/endpoint/function", async delegate (HttpContext context) { await TypeBroker.Formatter.Format(context, "endpoint_broker"); });

//            //����� ����� TypeBroker � ������� ���� ������ �� ���������(IResponseFormatter) � �������(������ ����������) ��������� �� �������
//            //�������(new TextResponseFormatter(),new HtmlResponseFormatter()) - ������� ����� ������ �� ���������(IResponseFormatter), �������(������ ����������)
//            //��������� �� ������� �������, ��������� � ������������  ������ Format � �������  � ��� ����� �  ��������� RequestDelegate ������� -  �������� �����
//            //������ UseEndpoint, � ����� string ������  Format
//            endpoints.MapGet("/endpoint/function_html", async delegate (HttpContext context) { await TypeBroker.Formatterhtml.Format(context, "endpoint_broker_html"); });

//            endpoints.MapGet("/endpoint/function_middl", async delegate (HttpContext context) { await WeatherMiddleware.Format(context, "endpoint_midd_weath"); });

//            endpoints.MapGet("/endpoint/html", async delegate (HttpContext context) { await new HtmlResponseFormatter().Format(context, "class_html"); });
//            // �� ���������
//            endpoints.MapGet("/", async delegate (HttpContext context) { await new HtmlResponseFormatter().Format(context, "class_html"); });
//            endpoints.Map("/br", async delegate (HttpContext context) { await new HtmlResponseFormatter().Format(context, "class_html"); });

//        });


//    }
//}

//��� ���� ���������� MapGet � endpoints
//endpoints.MapGet("/endpoint/class", async delegate (HttpContext context) {
//    if (context.Request.Query["key"] == "dron")
//    {
//        await WeatherEndpoint.Endpoint(context);
//    }

//});




//������ ��������� � service 
//Func<RequestDelegate, RequestDelegate> middl = delegate (RequestDelegate request)
//{
//    request = async delegate (HttpContext http) { await http.Response.WriteAsync("dron"); };
//    return request;

//};

//app.Map("", delegate (IApplicationBuilder builder)
//{
//    builder.Use(delegate (RequestDelegate request)
//    {

//        request = async delegate (HttpContext http)
//        {

//            await new Middelware_service().Invoke2(http);
//            //await http.Response.WriteAsync("dron");
//        };
//        return request;


//    });
//});

//Func<RequestDelegate, RequestDelegate> middleware = delegate (RequestDelegate request)
//{
//    //request = async delegate (HttpContext http) { await http.Response.WriteAsync("dron"); };
//    //return request;

//    return new Middelware_service().Invoke2;

//};


//����� Startap - ������ ��������
//public class Startup
//{

//    // This method gets called by the runtime. Use this method to add services to the container.
//    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

//    //������ �������������� � ������ ConfigureServices ������ Startup � �������������� ������� ���������� ��
//    // �������� IServiceCollection. ����� �������� ������ ��� ���������� IResponseFormatter.

//    public void ConfigureServices(IServiceCollection services)
//    {
//        //����� AddSingleton �������� ����� �� ������� ����������, ��������� ��� �����, � �������� ASP.NET Core, ��� ���� ������
//        //������� ������������ ��� �������������� ������������ � �������.
//        // ��������� � ����� ���������� ����������� ��� ��������� �������������� ����.����� ������������ ������, � �������
//        //�������� � ������ Configure.
//        services.AddSingleton<IResponseFormatter, HtmlResponseFormatter>();

//    }

//    // ����� ����������  �������������� ������������ ����������� - ������ �������(IOptions<MessageOptions> msgOptions)

//    //����� �������� ��������� ����������� �� ���������� IResponseFormatter, � ���������, ��� ����� ������� ��
//    //����������.����� ������� ������ Configure ����������� ��� ���������, �������������� ����������� �
//    //������ �����������, ����� ����������, �������� �� ��������� �����������.����������� � ������ ConfigureServices
//    //�������� ������� ��������� ������������, ��� ����������� �� ���������� IResponseFormatter ����� ���� ��������� �
//    //������� ������� HtmlResponseFormatter.������ ��������� � ������������ � �������� ��������� ��� ������ ������.
//    //��������� ������, ������� ��������� �����������, ��������������� ����� ������ ��� �������, ������� �� ����������,
//    //�������, ��� ��� ���� ��������, �������������� �������� ��� ��������� ������������.

//    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IResponseFormatter formatter_html)
//    {
//        app.UseDeveloperExceptionPage();
//        app.UseRouting();

//        //app.UseMiddleware<WeatherMiddleware>();
//        app.UseMiddleware<Middelware_service>();


//        app.Map("/method", delegate (IApplicationBuilder builder)
//        {
//            builder.Use(delegate (RequestDelegate request)
//            {

//                Func<RequestDelegate, RequestDelegate> middleware2 = delegate (RequestDelegate request_func) { return new Middelware_service(request_func, formatter_html).Invoke; };
//                return middleware2(request);

//            });
//        });

//        app.Map("/method2", delegate (IApplicationBuilder builder2)
//        {

//            builder2.Use(delegate (RequestDelegate request)
//            {

//                return new Middelware_service(request, formatter_html).Invoke;
//            });

//        });


//        app.Use(delegate (RequestDelegate request)
//        {

//            return new Middelware_service(request, formatter_html).Invoke;
//            // Middelware_service service = new Middelware_service(request, formatter_html);
//            //return service.Invoke;

//        });

//        app.Use(req => { return new Middelware_service(req, formatter_html).Invoke; });


//        //��������� ������������

//        app.Use(async (context, next) =>
//        {
//            if (context.Request.Path == "/middleware/function")
//            {

//                await TypeBroker.Formatter.Format(context, "class broker");

//            }
//            else { await next.Invoke(); }

//        });

//        Func<HttpContext, Func<Task>, Task> func = async delegate (HttpContext http, Func<Task> tsk)
//        {
//            if (http.Request.Path == "/path")
//            {
//                await formatter_html.Format(http, "func_delegate_services");
//            }
//            //������ �������� �� ��������
//            else
//            {
//                await tsk();
//            }
//        };
//        app.Use(func);




//        RequestDelegate request2 = async delegate (HttpContext context) { await context.Response.WriteAsync("Middleware"); };
//        Func<HttpContext, Func<Task>, Task> func2 = async delegate (HttpContext http, Func<Task> tsk)
//        {
//            if (http.Request.Path == "/path2")
//            {
//                await new WeatherMiddleware(request2, formatter_html).Invoke(http);
//            }
//            //������ �������� �� ��������
//            else
//            {
//                await tsk();
//            }
//        };
//        app.Use(func2);


//        Func<HttpContext, Func<Task>, Task> func3 = async delegate (HttpContext context, Func<Task> task)
//        {
//            await new WeatherMiddleware().Invoke(context);
//            await task();
//        };

//        Action<IApplicationBuilder> value = delegate (IApplicationBuilder builder)
//        {
//            builder.Use(func3);
//        };
//        app.Map("/middl", value);







//        app.UseEndpoints(endpoints =>
//        {

//            endpoints.MapGet("/endpoint/function_html_services", async delegate (HttpContext http) { await formatter_html.Format(http, "endpoint_services"); });

//            RequestDelegate request = async delegate (HttpContext context) { await context.Response.WriteAsync("Endpoint"); };
//            //����� ����� TypeBroker � ������� ���� ������ �� ���������(IResponseFormatter) � �������(������ ����������) ��������� �� �������
//            //�������(new TextResponseFormatter(),new HtmlResponseFormatter()) - ������� ����� ������ �� ���������(IResponseFormatter), �������(������ ����������)
//            //��������� �� ������� �������, ��������� � ������������  ������ Format � �������  � ��� ����� �  ��������� RequestDelegate ������� -  �������� �����
//            //������ UseEndpoint, � ����� string ������  Format
//            endpoints.MapGet("/endpoint/function_html", async delegate (HttpContext context) { await TypeBroker.Formatterhtml.Format(context, "endpoint_broker_html"); });

//            //endpoints.MapGet("/endpoint", async delegate (HttpContext context) { await new WeatherMiddleware(request, formatter_html).Invoke(context); });

//            endpoints.Map("/br", WeatherEndpoint.Endpoint);
//            endpoints.Map("/br2", async delegate (HttpContext http) {
//                await WeatherMiddleware.Format(http, "static_method_Weath...");
//            });
//            endpoints.Map("/br3", new WeatherMiddleware().Invoke);
//            endpoints.MapGet("/br4", new WeatherMiddleware().Invoke);
//            endpoints.Map("/br5", WeatherEndpoint.Endpoint_service);
//        });


//    }
//}


//��������� ������ ������� Middelware_service,
//public class Middelware_service
//{
//    private RequestDelegate request;
//    private IResponseFormatter formatter;

//    public Middelware_service(RequestDelegate request, IResponseFormatter formatter)
//    {
//        this.request = request;
//        this.formatter = formatter;
//    }

//    public Middelware_service()
//    {

//    }

//    public async Task Invoke(HttpContext http)
//    {
//        if (http.Request.Path == "/class_middlware_service")
//        {
//            await formatter.Format(http, "class_middlware_service");
//        }

//        else { await request(http); }
//    }

//    public static async Task Invoke_format(HttpContext http, string s, IResponseFormatter response)
//    {
//        await response.Format(http, "class_middlware_service_static");
//    }

//    public async Task Invoke2(HttpContext context)
//    {
//        await context.Response.WriteAsync("method run\t");
//    }

//    public async Task Invoke3(HttpContext context)
//    {
//        if (context.Request.Path == "/Invoke3")
//        {
//            await formatter.Format(context, "Invoke3");

//        }
//        else
//        {
//            await request(context);
//        }

//    }
//}

// � ������ WeatherEndpoin
//public class WeatherEndpoint
//{
//    private static int responce;
//    public static async Task Endpoint(HttpContext http)
//    {
//        await http.Response.WriteAsync($"Endpoint Class: It is cloudy in Milan {++responce}");
//    }

//    //�������� HttpContext.RequestServices ���������� ������, ����������� ���������� IServiceProvider, �������
//    // ������������� ������ � �������, ����������� � ������ Start.ConfigureServices ����������.Microsoft.
//    //������������ ���� Extensions.DependencyInjection,
//    public static async Task Endpoint_service(HttpContext http)
//    {
//        //GetRequiredService<IResponseFormatter>() -    ����� ���������� ������, ��������� ���������� �������������� ����. 
//        //������ ������� ��������� �  Startape
//        IResponseFormatter formatter = http.RequestServices.GetRequiredService<IResponseFormatter>();
//        await formatter.Format(http, "Static_Endpoint_service");
//    }

//    public static async Task Endpoint_format(HttpContext http, string s, IResponseFormatter response)
//    {
//        await response.Format(http, s);

//    }

//}

// �� ���������� ����� ������ ������ HtmlResponseFormatter
//public class HtmlResponseFormatter : IResponseFormatter
//{
//    public static int responseCounter;

//    public async Task Format(HttpContext context, string content)
//    {
//        context.Response.ContentType = "text/html";
//        await context.Response.WriteAsync($@"

//             <!DOCTYPE html>
//             <html lang=""en"">
//               <head>
//               <title>Response</title>

//                <style>
//                    body {{
//                    color:green;
//                        }}
//                     </style>

//               </head>
//                 <body>
//                    <h2>Formatted Response</h2>
//                           <span>{content}</span>
//                             <span>{++HtmlResponseFormatter.responseCounter}</span>
//                </body>
//             </html> "
//        );

//    }
//}

// � Startape
//public class Startup
//{

//    // This method gets called by the runtime. Use this method to add services to the container.
//    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

//    //������ �������������� � ������ ConfigureServices ������ Startup � �������������� ������� ���������� ��
//    // �������� IServiceCollection. ����� �������� ������ ��� ���������� IResponseFormatter.

//    public void ConfigureServices(IServiceCollection services)
//    {
//        //����� AddSingleton �������� ����� �� ������� ����������, ��������� ��� �����, � �������� ASP.NET Core, ��� ���� ������
//        //������� ������������ ��� �������������� ������������ � �������.
//        // ��������� � ����� ���������� ����������� ��� ��������� �������������� ����.����� ������������ ������, � �������
//        //�������� � ������ Configure.
//        services.AddSingleton<IResponseFormatter, HtmlResponseFormatter>();

//    }

//    // ����� ����������  �������������� ������������ ����������� - ������ �������(IOptions<MessageOptions> msgOptions)

//    //����� �������� ��������� ����������� �� ���������� IResponseFormatter, � ���������, ��� ����� ������� ��
//    //����������.����� ������� ������ Configure ����������� ��� ���������, �������������� ����������� �
//    //������ �����������, ����� ����������, �������� �� ��������� �����������.����������� � ������ ConfigureServices
//    //�������� ������� ��������� ������������, ��� ����������� �� ���������� IResponseFormatter ����� ���� ��������� �
//    //������� ������� HtmlResponseFormatter.������ ��������� � ������������ � �������� ��������� ��� ������ ������.
//    //��������� ������, ������� ��������� �����������, ��������������� ����� ������ ��� �������, ������� �� ����������,
//    //�������, ��� ��� ���� ��������, �������������� �������� ��� ��������� ������������.

//    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IResponseFormatter formatter_html)
//    {
//        app.UseDeveloperExceptionPage();
//        app.UseRouting();

//        app.UseMiddleware<Middelware_service>();

//        app.Use(delegate (HttpContext http, Func<Task> tsk) {
//            if (http.Request.Path == "/dron")
//            {
//                return new Middelware_service().Invoke2(http);
//            }
//            else
//            {
//                return tsk();
//            }
//        });


//        app.Use(delegate (RequestDelegate request) { return new Middelware_service(request, formatter_html).Invoke3; });

//        app.Map("/method2", delegate (IApplicationBuilder builder2)
//        {

//            builder2.Use(delegate (RequestDelegate request)
//            {

//                return new Middelware_service(request, formatter_html).Invoke3;
//            });

//        });



//        Func<HttpContext, Func<Task>, Task> func = async delegate (HttpContext http, Func<Task> tsk)
//        {
//            if (http.Request.Path == "/path")
//            {
//                await Middelware_service.Invoke_format(http, "dron", formatter_html);
//            }
//            //������ �������� �� ��������
//            else
//            {
//                await tsk();
//            }
//        };
//        app.Use(func);


//        app.Use(async (cont, next) => {

//            if (cont.Request.Path == "/use")
//            {
//                await Middelware_service.Invoke_format(cont, "use", formatter_html);
//            }

//            else { await next(); }

//        });

//        app.Use(async delegate (HttpContext context, Func<Task> tsk)
//        {
//            if (context.Request.Path == "/delegate_use")
//            {
//                await Middelware_service.Invoke_format(context, "delegate_use", formatter_html);
//            }


//            else { await tsk(); }



//        });



//        //app.Use(delegate(RequestDelegate request) 
//        //{
//        //    request = async delegate (HttpContext context)
//        //    {

//        //        if (context.Request.Path == "/use_delegate")
//        //        {
//        //            await Middelware_service.Invoke_format(context, "use_delegate", formatter_html);
//        //        }


//        //    };

//        //    return request;
//        //});

//        // ����� Map ���������� ����� ����("/map") � �������  Action<IApplicationBuilder>
//        // c �������� ����� IApplicationBuilder - ����� ����(������ ����������  IApplicationBuilder) �� ���������� � ������ Use
//        // � ������ � ���������� IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware);
//        // � �������� �������(request) �������� ������ ������ Invoke_format � ������ ��������(RequestDelegate) � �������(IResponseFormatter formatter_html)
//        app.Map("/map", delegate (IApplicationBuilder builder)
//        {
//            builder.Use(delegate (RequestDelegate request)
//            {

//                request = async delegate (HttpContext http)
//                {
//                    await Middelware_service.Invoke_format(http, "Map", formatter_html);

//                };
//                return request;


//            });
//        });





//        app.UseEndpoints(endpoints =>
//        {
//            endpoints.Map("/br", WeatherEndpoint.Endpoint_service);
//            endpoints.Map("/br2", async (cont) => await WeatherEndpoint.Endpoint_service(cont));
//            endpoints.MapGet("/br3", async delegate (HttpContext context) { await WeatherEndpoint.Endpoint_service(context); });
//            endpoints.MapGet("/br4", delegate (HttpContext context) { return WeatherEndpoint.Endpoint_service(context); });

//            endpoints.MapGet("/br5", async delegate (HttpContext http) { await WeatherEndpoint.Endpoint_format(http, "await_dron", formatter_html); });
//            endpoints.MapGet("/br6", delegate (HttpContext http) { return WeatherEndpoint.Endpoint_format(http, "return_dron", formatter_html); });
//            endpoints.Map("/br7", async (cont) => await WeatherEndpoint.Endpoint_format(cont, "lymbda", formatter_html));


//            endpoints.MapGet("/br8", delegate (HttpContext context) { return formatter_html.Format(context, "class HtmlResponseFormatter"); });


//            // ����� Middelware_service()
//            endpoints.MapGet("/br9", delegate (HttpContext http) { return new Middelware_service().Invoke2(http); });
//        });


//    }
//}