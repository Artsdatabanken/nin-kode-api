namespace NinKode.WebApi
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using NiN.Database;
    using NinKode.Common.Interfaces;
    using NinKode.Common.Utilities;
    using NinKode.Database.Services;
    using NinKode.Database.Services.v1;
    using NinKode.Database.Services.v2;
    using NinKode.Database.Services.v21;
    using NinKode.Database.Services.v21b;
    using NinKode.Database.Services.v22;
    using NinKode.Database.Services.v23;

    public class Startup
    {
        private IConfiguration Configuration { get; }
        private readonly string _swaggerDocumentTitle;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _swaggerDocumentTitle = Configuration.GetValue("SwaggerDocumentTitle", "NinKode API");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(o =>
            {
                o.Conventions.Add(new ControllerDocumentationConvention());
            });
            services.AddSwaggerGen(c =>
            {
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = $"{_swaggerDocumentTitle} v1",
                    Version = "v1",
                    Description = CreateDescription()
                });
                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = $"{_swaggerDocumentTitle} v2",
                    Version = "v2",
                    Description = CreateDescription()
                });
                c.SwaggerDoc("v2.1", new OpenApiInfo
                {
                    Title = $"{_swaggerDocumentTitle} v2.1",
                    Version = "v2.1",
                    Description = CreateDescription()
                });
                c.SwaggerDoc("v2.1b", new OpenApiInfo
                {
                    Title = $"{_swaggerDocumentTitle} v2.1b",
                    Version = "v2.1b",
                    Description = CreateDescription()
                });
                c.SwaggerDoc("v2.2", new OpenApiInfo
                {
                    Title = $"{_swaggerDocumentTitle} v2.2",
                    Version = "v2.2",
                    Description = CreateDescription()
                });
                c.SwaggerDoc("beta", new OpenApiInfo
                {
                    Title = $"{_swaggerDocumentTitle} beta",
                    Version = "beta",
                    Description = CreateDescription()
                });
            });

            var connectionString = Configuration.GetConnectionString("Default");
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = Configuration.GetValue("ConnectionString", "");
                if (string.IsNullOrEmpty(connectionString)) throw new Exception("Could not find 'ConnectionString'");
            }

            services.AddDbContext<NiNDbContext>(o =>
            {
                o.UseSqlServer(connectionString, x => x.MigrationsAssembly("NinKode.Database"));
            });

            // Define singleton-objects
            services.AddSingleton<ICodeV1Service, CodeV1Service>();
            services.AddSingleton<ICodeV2Service, CodeV2Service>();
            services.AddSingleton<ICodeV21Service, CodeV21Service>();
            services.AddSingleton<IVarietyV21Service, VarietyV21Service>();
            services.AddSingleton<ICodeV21BService, CodeV21BService>();
            services.AddSingleton<IVarietyV21BService, VarietyV21BService>();
            services.AddSingleton<ICodeV22Service, CodeV22Service>();
            services.AddSingleton<IVarietyV22Service, VarietyV22Service>();
            services.AddSingleton<ICodeV23Service, CodeV23Service>();
            services.AddSingleton<IVarietyV23Service, VarietyV23Service>();

            services.AddSingleton<ICodeService, CodeService>();
            services.AddSingleton<IVarietyService, VarietyService>();
            services.AddSingleton<IExportService, ExportService>();
        }

        private static string CreateDescription()
        {
            return $"<i>BuildTime: {NorwayDateTime.Convert(File.GetLastWriteTimeUtc(Assembly.GetExecutingAssembly().Location)):yyyy-MM-dd HH:mm:ss}</i>";
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = _swaggerDocumentTitle;
                //c.InjectStylesheet("/css/theme-feeling-blue.css");
                //c.InjectStylesheet("/css/theme-flattop.css");
                //c.InjectStylesheet("/css/theme-material.css");
                //c.InjectStylesheet("/css/theme-monokai.css");
                //c.InjectStylesheet("/css/theme-muted.css");
                //c.InjectStylesheet("/css/theme-newspaper.css");
                //c.InjectStylesheet("/css/theme-outline.css");
                c.DisplayRequestDuration();
                c.DefaultModelsExpandDepth(-1); // Disable swagger schemas at bottom
                c.SwaggerEndpoint("/swagger/beta/swagger.json", $"{_swaggerDocumentTitle} beta");
                c.SwaggerEndpoint("/swagger/v2.2/swagger.json", $"{_swaggerDocumentTitle} v2.2");
                c.SwaggerEndpoint("/swagger/v2.1b/swagger.json", $"{_swaggerDocumentTitle} v2.1b");
                c.SwaggerEndpoint("/swagger/v2.1/swagger.json", $"{_swaggerDocumentTitle} v2.1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", $"{_swaggerDocumentTitle} v2");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{_swaggerDocumentTitle} v1");
            });
            //}

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}");
            });

            //using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            //var dbContext = serviceScope.ServiceProvider.GetService<NiNDbContext>();
            //dbContext?.Database.Migrate();
        }
    }

    //internal class ActionHidingConvention : IActionModelConvention
    //{
    //    public void Apply(ActionModel action)
    //    {
    //        if (!action.Controller.ControllerName.Equals("Home", StringComparison.OrdinalIgnoreCase)) return;
    //        action.ApiExplorer.IsVisible = false;
    //    }
    //}

    internal class ControllerDocumentationConvention : IControllerModelConvention
    {
        void IControllerModelConvention.Apply(ControllerModel controller)
        {
            if (controller == null) return;

            foreach (var attribute in controller.Attributes)
            {
                if (attribute.GetType() != typeof(DisplayNameAttribute)) continue;
                var displayNameAttribute = (DisplayNameAttribute)attribute;
                if (!string.IsNullOrWhiteSpace(displayNameAttribute.DisplayName))
                    controller.ControllerName = displayNameAttribute.DisplayName;
            }

        }
    }
}
