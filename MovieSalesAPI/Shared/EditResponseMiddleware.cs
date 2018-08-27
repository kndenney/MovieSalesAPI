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

                //Retrieve the current Http Response body and store it
                var originBody = context.Response.Body;
                var newBody = new MemoryStream();

                context.Response.Body = newBody;

                throw new Exception();

                await _next(context);

                newBody.Seek(0, SeekOrigin.Begin);

                string json = new StreamReader(newBody).ReadToEnd();

                //Reset the response body to what it was originally before beginning other pipelin requests
                context.Response.Body = originBody;

                await context.Response.WriteAsync(json);
            } else
            {
                await _next(context);
            }
        }
    }

    /// <summary>
    /// Extension method used to add the middleware to the HTTP request pipeline.
    /// </summary>
    public static class EditResponseMiddlewareExtensions
    {
        public static IApplicationBuilder UseEditResponseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EditResponseMiddleware>();
        }
    }
}
