﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Monkey.Data.EF.Factory;
using Monkey.Filters;
using Monkey.Mapper;
using Monkey.Models;
using Puppy.Web.Render;
using System.Reflection;

namespace Monkey
{
    public partial class Startup
    {
        public static IConfigurationRoot ConfigurationRoot;
        public static IHostingEnvironment Environment;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
                builder.AddUserSecrets<Startup>();

            ConfigurationRoot = builder.Build();
            Environment = env;
        }

        public static string DeveloperAccessKeyConfig => ConfigurationRoot.GetValue<string>("Developers:AccessKey");

        public static bool IsDeveloperCanAccess(HttpContext httpContext)
        {
            return DeveloperHelper.IsCanAccess(httpContext, DeveloperAccessKeyConfig);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(ConfigurationRoot);
            services.AddSingleton(Environment);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IViewRenderService, ViewRenderService>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<DeveloperAccessFilter>();
            services.AddScoped<ApiExceptionFilter>();

            Cros.Service(services);

            Swagger.Service(services);

            Hangfire.Service(services);

            Cache.Service(services);

            Mvc.Service(services);

            MapperConfiguration.Add(services);

            MapperConfiguration.Configure();

            // keep in last service
            DependencyInjection.Service(services);

            // Use Entity Framework
            services.AddDbContext<DbContext>(builder =>
                builder.UseSqlServer(
                    ConfigurationRoot.GetConnectionString(Environment.EnvironmentName),
                    options => options.MigrationsAssembly(typeof(IDataModule).GetTypeInfo().Assembly.GetName().Name)));
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime, IHttpContextAccessor httpContextAccessor, IHostingEnvironment env)
        {
            // Currently, ASPNETCORE have a bug hit twice when change appsetting.json from 20/03/17
            // (see more: https://github.com/aspnet/Configuration/issues/624)
            ChangeToken.OnChange(ConfigurationRoot.GetReloadToken, () => loggerFactory.CreateLogger<Startup>().LogWarning("Configuration Changed"));

            // [Important] The order of middleware very important for request and response handle!
            // Don't mad it

            // [Important] Use Cros first
            Cros.Middleware(app);
            app.UseMiddleware<Cros.ResponseMiddleware>();

            // Response Information
            app.UseMiddleware<SystemInfoMiddleware>();
            app.UseMiddleware<ProcessingTimeMiddleware>();

            // Exception
            Log.Middleware(app, loggerFactory);
            Exception.Middleware(app);

            // Swagger
            Swagger.Middleware(app);
            app.UseMiddleware<Swagger.AccessMiddleware>();

            // Hangfire
            Hangfire.Middleware(app);

            IdentityServer.Middleware(app);

            // [Final] Execute Middleware: MVC
            Mvc.Middleware(app, httpContextAccessor, env);
        }
    }
}