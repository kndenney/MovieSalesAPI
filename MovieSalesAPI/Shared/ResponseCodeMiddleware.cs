using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MovieSalesAPI.Shared
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ResponseCodeMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseCodeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        async public Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.ToString().Contains("swagger"))
            {
                await _next(httpContext);
            } else
            {
                try
                {
                    //Start the next portion of the pipeline call so that you get your response
                    //then start mapping the below code wrapper around it
                    //So if it is a controller you get the response before starting to wrap the data
                    await _next(httpContext);

                    //This is where we need to create a custom Error() class
                    int statusCode = httpContext.Response.StatusCode;
                    string message = "";

                    //https://stackoverflow.com/questions/44508028/modify-middleware-response
                    //https://stackoverflow.com/questions/47181356/c-sharp-dotnet-core-middleware-wrap-response

                    switch (statusCode)
                    {
                        case 200:
                            message = "Response is ok";
                            break;
                        case 404:
                            message = "Resource not found";
                            break;
                        case 500:
                            message = "An unhandled error occurred";
                            break;
                        default:
                            message = "An unknown non-status code error occurred";
                            break;
                    }

                    var error = httpContext.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        message = message + error.Error.Message.ToString();
                    }

                    string addComma = "";
                    //There may be a previous response so add a comma
                    if (httpContext.Response.Body.Length > 0)
                    {
                        addComma = ",";
                    }

                    string json = "\"message\": [" + JsonConvert.SerializeObject(
                        new Error()
                        {
                            Code = statusCode.ToString(),
                            Message = message.ToString(),
                            Path = httpContext.Request.Path.ToString() + httpContext.Request.QueryString
                        }
                    ) + "]";

                    await httpContext.Response.WriteAsync(addComma + json);
                }
                catch (Exception ex)
                {
                    await HandleExceptionAsync(httpContext, ex);
                }
            } 
        }
  

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            //if (exception is MyNotFoundException) code = HttpStatusCode.NotFound;
            //else if (exception is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
            //else if (exception is MyException) code = HttpStatusCode.BadRequest;

            var result = JsonConvert.SerializeObject(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            int statusCode = (int)code;


            string addComma = "";
            //There may be a previous response so add a comma
            if (context.Response.Body.Length > 0)
            {
                addComma = ",";
            }

            string json = "\"message\": [" + JsonConvert.SerializeObject(
                new Error()
                {
                    Code = statusCode.ToString(),
                    Message = result.ToString(),
                    Path = context.Request.Path.ToString() + context.Request.QueryString
                }
            ) + "]";

            return context.Response.WriteAsync(addComma + json);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ResponseCodeMiddlewareExtensions
    {
        public static IApplicationBuilder UseResponseCodeMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseCodeMiddleware>();
        }
    }

    public class Error {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Path { get; set; }
    }
}
