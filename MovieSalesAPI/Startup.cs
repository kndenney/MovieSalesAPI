using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MovieSalesAPI.Shared;
using Swashbuckle.AspNetCore.Swagger;
using MovieSalesAPILogic.Authorization;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MovieSalesAPI.Data;
using MovieSalesAPILogic;
using MovieSalesAPI.Data.User;

namespace MovieSalesAPI
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public IConfiguration _configuration;
        private string DefaultCorsPolicyName;

        //A few key things:
        //1. Install Swagger UI / Swashbuckle for API documentation
        //https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.1&tabs=visual-studio%2Cvisual-studio-xml

        //2. Install NSwag to have a client code generator
        //https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-nswag?view=aspnetcore-2.1&tabs=visual-studio%2Cvisual-studio-xml
        //https://github.com/RSuter/NSwag

        //3. Install CorsPolicy so that you can access the API from a domain name that is not the same as the API
        //   (like a subdomain etc.)
        //4. Make sure to modify the response to make sure it is consistent via the Middleware

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            _configuration = configuration;

            var builder = new ConfigurationBuilder()
           .SetBasePath(env.ContentRootPath)
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
           .AddEnvironmentVariables();
           //This allows us to switch between API endpoints based on our environment
           //.AddJsonFile($"somefile.{env.EnvironmentName}.json", optional: false);

            Configuration = builder.Build();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                //https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.1&tabs=visual-studio%2Cvisual-studio-xml
                //https://garywoodfine.com/documenting-dotnet-core-apis-swagger/
                //How to use different kinds of comments - https://docs.microsoft.com/en-us/dotnet/csharp/codedoc
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Movies API",
                    Description = "Explore Movies ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Kyle Denney",
                        Email = string.Empty,
                        Url = "https://twitter.com/kndenney"
                    },
                    License = new License
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    }
                });

                c.IncludeXmlComments(string.Format(@"{0}\MovieSalesAPI.XML", System.AppDomain.CurrentDomain.BaseDirectory));

            });

            string issuer = "";
            string audience = "";
            switch (_configuration["Environment"])
            {
                case "Development":
                    issuer = _configuration["EnvironmentInfo:DevelopmentIssuerURL"];
                    audience = _configuration["EnvironmentInfo:DevelopmentAudienceURL"];
                    break;
                case "UAT":
                    issuer = _configuration["EnvironmentInfo.UATIssuerURL"];
                    audience = _configuration["EnvironmentInfo.UATAudienceURL"];
                    break;
                case "Production":
                    issuer = _configuration["EnvironmentInfo.ProductionIssuerURL"];
                    audience = _configuration["EnvironmentInfo.ProductionAudienceURL"];
                    break;
                default:
                    issuer = "";
                    audience = "";
                    break;
            }

            DefaultCorsPolicyName = issuer;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                //.AddCookie(options => {
                //    options.LoginPath = "/Account/Unauthorized/";
                //    options.AccessDeniedPath = "/Account/Forbidden/";
                //})
                .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "Issuer",
                    ValidAudience = "Audience",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey"])),
                    ClockSkew = TimeSpan.FromMinutes(0),
                    RequireExpirationTime = true
                };
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("APIMovieAccess",
                            policy => policy.RequireClaim("CanAccessMovies"));
            });


            /*
            // Configure CORS for angular2 UI
            var origins = issuer.Split(',').ToArray();

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .Build();
                });
            });*/


            // Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials().Build());
            });

            services.AddSwaggerDocumentation();

            //Load our application custom configuration
            // Add our Config object so it can be injected
            //https://stackoverflow.com/questions/31453495/how-to-read-appsettings-values-from-config-json-in-asp-net-core
            // Add functionality to inject IOptions<T>
            services.AddOptions();

            // Add our Config object so it can be injected
            services.Configure<AppConfig>(Configuration);

            // *If* you need access to generic IConfiguration this is **required**
            services.AddSingleton<IConfiguration>(Configuration);

            //Add dependency injection for Data classes, models etc.
            services.AddTransient<IMovieData, MovieData>();
            services.AddTransient<IUserData, UserData>();
            services.AddTransient<ITokenRequest, TokenRequest>();

            services.AddMvc(config =>
            {
                // Add XML Content Negotiation
                /* config.RespectBrowserAcceptHeader = true;
                 config.InputFormatters.Add(new XmlSerializerInputFormatter());
                 config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                 */
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseCors("CorsPolicy");

            app.UseEditResponseMiddleware();

            app.UseResponseCodeMiddleware();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
               app.UseHsts();
               app.UseHttpsRedirection();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerDocumentation();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movies API V1");
            });

            app.UseAuthentication();
   
            app.UseMvc();
        }
    }
}
