using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MovieSalesAPI.Shared
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class EditResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public EditResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            if (!context.Request.Path.ToString().Contains("/swagger")) {

                var originBody = context.Response.Body;

                var newBody = new MemoryStream();

                context.Response.Body = newBody;

                await _next(context);

                newBody.Seek(0, SeekOrigin.Begin);

                string json = new StreamReader(newBody).ReadToEnd();

                context.Response.Body = originBody;

                string modifiedJson = json; //This is where we will check for swagger like the route being
                                            //'/swagger/index.html or something along those lines
                                            //or in the responeCodemiddleware... then in here we will for sure if the route is swagger
                                            //do something but also if the route is not swagger tack on the brackets [ ] and json response
                                            //and all that

                if (!json.ToString().Contains("<title>Swagger UI</title>"))
                {
                   if (json.StartsWith("\"message\": [{\"Code\""))
                    {
                        json = "{ \"data\": [], " + json + " }";
                    
                    }
                    else if (json.StartsWith("{\"error\":\""))
                    {
                        json = "{ \"data\": [], " + json + " }";
                    }
                    else
                    {
                        json = "{ \"data\": " + json + " }";
                    }

                    //Throw new exception
                    //{ "data:": {"error":"Parameter cannot be null\r\nParameter name: original"} }
                    //{ "data:": ["value1","value2"],"message": [{"Code":"200","Message":"Response is ok","Path":"/api/values"}] }
                    //{ "data:": [], {"error":"Parameter cannot be null\r\nParameter name: original"} }

                }

                await context.Response.WriteAsync(json);
            } else
            {
                await _next(context);
            }

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class EditResponseMiddlewareExtensions
    {
        public static IApplicationBuilder UseEditResponseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EditResponseMiddleware>();
        }
    }
}
