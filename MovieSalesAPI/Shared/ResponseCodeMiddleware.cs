using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
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

                    //Retrieve the current Http Response body and store it
                    var originBody = httpContext.Response.Body;

                    //Create a new body and set the current contexted body to
                    //that blank stream
                    var newBody = new MemoryStream();

                    httpContext.Response.Body = newBody;

                    //Run the next part of the middleware pipeline which is
                    //the .Mvc() portion of the Startup.cs
                    //so this will return data from Controller methods
                    await _next(httpContext);

                    //Now that we have ran the data call above we will then retrieve that data from the pipeline
                    newBody.Seek(0, SeekOrigin.Begin);

                    string d = new StreamReader(newBody).ReadToEnd();

                    //Reset the response body to what it was originally before beginning other pipelin requests
                    httpContext.Response.Body = originBody;

                    string exceptionText = "";

                    if (d.StartsWith("\"message\": [{\"Code\""))
                    {
                        d = "{ \"data\": [], " + d;
                    }
                    else if (d.StartsWith("{\"error\":\""))
                    {
                        d = "{ \"data\": [], " + d;
                    }
                    else
                    {
                        if (d.StartsWith("{\"exception\":\""))
                        {
                            //if there is an exception from a controller or data class
                            //assign its value to the exception variable text
                            exceptionText = d;
                            //Clear the data variable
                            //and assign blank 'data' since there was an error
                            d = "{ \"data\": []";
                        }
                        else
                        {
                            d = "{ \"data\": " + d;
                        }
                    }

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
                        case 400:
                            message = "Bad Request";
                            break;
                        case 401:
                            message = "Unauthorized access";
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

                    string json = d + ", " + "\"message\": [" +
                        JsonConvert.SerializeObject(new Response()
                        {   
                            Code = statusCode.ToString(),
                            Message = exceptionText + " " + message.ToString(),
                            Path = httpContext.Request.Path.ToString() + httpContext.Request.QueryString
                        }) + "]}";

                    httpContext.Response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
                    await httpContext.Response.WriteAsync(json, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    await HandleExceptionAsync(httpContext, ex);
                }
            } 
        }
  

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            var result = new ExceptionJson()
            {
                Exception = exception.Message,
                Stacktrace = exception.StackTrace,
                InnerException = (exception.InnerException != null) ? exception.InnerException.ToString() : ""
            };

            string exceptionMessage = "";
            exceptionMessage = "\"exception\": \"" + result.Exception + "\",";
            exceptionMessage += "\"stacktrace\": \"" + result.Stacktrace + "\",";
            exceptionMessage += "\"innerException\": \"" + result.InnerException + "\"";


            context.Response.StatusCode = (int)code;

            int statusCode = (int)code;

            string addComma = "";
            //There may be a previous response so add a comma
            if (context.Response.Body.Length > 0)
            {
                addComma = ",";
            }

            string json = "{ \"data\": [], " + "\"message\": [" + JsonConvert.SerializeObject(
                new Response()
                {
                    Code = statusCode.ToString(),
                    Message = exceptionMessage,
                    Path = context.Request.Path.ToString() + context.Request.QueryString
                }
            ) + "]}";

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

    public class Response {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Path { get; set; }
    }
}
