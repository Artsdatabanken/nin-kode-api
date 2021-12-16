namespace NinKode.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Models;
    using NiN.Database;
    using NinKode.Common.Interfaces;
    using NinKode.Common.Utilities;
    using NinKode.Database.Services;
    //using NinKode.Database.Services.v1;
    //using NinKode.Database.Services.v2;
    //using NinKode.Database.Services.v21;
    //using NinKode.Database.Services.v21b;
    //using NinKode.Database.Services.v22;
    using NinKode.WebApi.Helpers;

    public class Startup
    {
        private readonly string _apiName;
        private readonly string _authAuthority;
        private readonly string _authAuthorityEndPoint;
        private readonly string _swaggerClientId;
        private readonly string _swaggerDocumentTitle;
        private readonly string _writeAccessRole;

        private ILogger _logger;
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
            // configuration
            _apiName = Configuration.GetValue("ApiName", "api");
            _authAuthority = Configuration.GetValue("AuthAuthority", "https://demo.identityserver.io");
            _authAuthorityEndPoint = Configuration.GetValue("AuthAuthorityEndPoint", "https://demo.identityserver.io/connect/authorize");
            _swaggerClientId = Configuration.GetValue("SwaggerClientId", "implicit");
            _swaggerDocumentTitle = Configuration.GetValue("SwaggerDocumentTitle", "NinKode API");
            _writeAccessRole = Configuration.GetValue("WriteAccessRole", "my_write_access_role");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression();
            services.AddControllers(o =>
            {
                o.Conventions.Add(new ControllerDocumentationConvention());
            });
            AddIdentityServerAuthentication(services);

            AddSwaggerGenerator(services);

            // Define singleton-objects
            //services.AddSingleton<ICodeV1Service, CodeV1Service>();
            //services.AddSingleton<ICodeV2Service, CodeV2Service>();
            //services.AddSingleton<ICodeV21Service, CodeV21Service>();
            //services.AddSingleton<IVarietyV21Service, VarietyV21Service>();
            //services.AddSingleton<ICodeV21BService, CodeV21BService>();
            //services.AddSingleton<IVarietyV21BService, VarietyV21BService>();
            //services.AddSingleton<ICodeV22Service, CodeV22Service>();
            //services.AddSingleton<IVarietyV22Service, VarietyV22Service>();

            services.AddSingleton<ICodeService, CodeService>();
            services.AddSingleton<IVarietyService, VarietyService>();
            services.AddSingleton<IExportService, ExportService>();
            services.AddSingleton<IImportService, ImportService>();
            services.AddSingleton<IVersionService, VersionService>();
        }

        private void AddSwaggerGenerator(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                xmlPath = Path.Combine(AppContext.BaseDirectory, "NinKode.Database.xml");
                if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath, true);
                xmlPath = Path.Combine(AppContext.BaseDirectory, "NiN.Database.xml");
                if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath, true);
                xmlPath = Path.Combine(AppContext.BaseDirectory, "NiN.Common.xml");
                if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath, true);

                //c.SwaggerDoc("v1", new OpenApiInfo
                //{
                //    Title = $"{_swaggerDocumentTitle} v1",
                //    Version = "v1",
                //    Description = CreateDescription()
                //});
                //c.SwaggerDoc("v2", new OpenApiInfo
                //{
                //    Title = $"{_swaggerDocumentTitle} v2",
                //    Version = "v2",
                //    Description = CreateDescription()
                //});
                //c.SwaggerDoc("v2.1", new OpenApiInfo
                //{
                //    Title = $"{_swaggerDocumentTitle} v2.1",
                //    Version = "v2.1",
                //    Description = CreateDescription()
                //});
                //c.SwaggerDoc("v2.1b", new OpenApiInfo
                //{
                //    Title = $"{_swaggerDocumentTitle} v2.1b",
                //    Version = "v2.1b",
                //    Description = CreateDescription()
                //});
                //c.SwaggerDoc("v2.2", new OpenApiInfo
                //{
                //    Title = $"{_swaggerDocumentTitle} v2.2",
                //    Version = "v2.2",
                //    Description = CreateDescription()
                //});
                c.SwaggerDoc("api", new OpenApiInfo
                {
                    Title = $"{_swaggerDocumentTitle}",
                    Version = "api",
                    Description = CreateDescription()
                });

                // temporary disable auth
                c.AddSecurityDefinition(
                    "oauth2",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            Implicit = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri(_authAuthorityEndPoint, UriKind.Absolute),
                                Scopes = new Dictionary<string, string>
                                {
                                    {_apiName, "Access Api"}

                                    // { "readAccess", "Access read operations" },
                                    // { "writeAccess", "Access write operations" }
                                }
                            }
                        }
                    });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.EnableAnnotations();
            });

            var connectionString = Configuration.GetConnectionString("Default");
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = Configuration.GetValue("NinApiConnectionString", "");
                if (string.IsNullOrEmpty(connectionString)) throw new Exception("Could not find 'NinApiConnectionString'");
            }

            services.AddDbContext<NiNDbContext>(o =>
            {
                o.UseSqlServer(connectionString, x => x.MigrationsAssembly("NinKode.Database"));
            });
        }

        private static string CreateDescription()
        {
            return $"<i>BuildTime: {NorwayDateTime.Convert(File.GetLastWriteTimeUtc(Assembly.GetExecutingAssembly().Location)):yyyy-MM-dd HH:mm:ss}</i>";
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            _logger = logger;

            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            //AddSwaggerMiddleware(app);

            //app.UseHttpsRedirection();

            app.UseRouting();

            AddSwaggerMiddleware(app);

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();
            
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllerRoute(
                //    "default",
                //    "{controller=Home}/{action=Index}");
            });

            //using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            //var dbContext = serviceScope.ServiceProvider.GetService<NiNDbContext>();
            //dbContext?.Database.Migrate();
        }

        private void AddIdentityServerAuthentication(IServiceCollection services)
        {
            var roleClaim = "role";
            var roleClaimValue = _writeAccessRole;

            // Users defined at https://demo.identityserver.io has no roles.
            // Using the Issuer-claim (iss) as a substitute to allow authorization with Swagger when testing
            if (_authAuthority == "https://demo.identityserver.io")
            {
                roleClaim = "iss";
                roleClaimValue = "https://demo.identityserver.io";
            }

            services.AddAuthorization(options =>
                options.AddPolicy("WriteAccess", policy => policy.RequireClaim(roleClaim, roleClaimValue))
            );

            services.AddCors();

            services
                .AddAuthentication("token")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = _authAuthority;
                    options.RequireHttpsMetadata = false;

                    options.Audience = _apiName;
                }
                )
                .AddIdentityServerAuthentication("token", options =>
                {
                    options.Authority = _authAuthority;
                    options.RequireHttpsMetadata = false;

                    options.ApiName = _apiName;

                    options.JwtBearerEvents = new JwtBearerEvents
                    {
                        OnMessageReceived = e =>
                        {
                            _logger.LogDebug($"JWT: message received\t{e.Request.Path}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = e =>
                        {
                            _logger.LogDebug($"JWT: token validated\t{e.Request.Path}");
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = e =>
                        {
                            _logger.LogDebug($"JWT: authentication failed\t{e.Request.Path}");
                            return Task.CompletedTask;
                        },
                        OnChallenge = e =>
                        {
                            _logger.LogDebug($"JWT: challenge\t{e.Request.Path}");
                            return Task.CompletedTask;
                        },
                        OnForbidden = e =>
                        {
                            _logger.LogDebug($"JWT: forbidden\t{e.Request.Path}");
                            return Task.CompletedTask;
                        }
                    };
                });
        }



        private void AddSwaggerMiddleware(IApplicationBuilder app)
        {
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
                c.SwaggerEndpoint("/swagger/api/swagger.json", $"{_swaggerDocumentTitle}");
                //c.SwaggerEndpoint("/swagger/v2.2/swagger.json", $"{_swaggerDocumentTitle} v2.2");
                //c.SwaggerEndpoint("/swagger/v2.1b/swagger.json", $"{_swaggerDocumentTitle} v2.1b");
                //c.SwaggerEndpoint("/swagger/v2.1/swagger.json", $"{_swaggerDocumentTitle} v2.1");
                //c.SwaggerEndpoint("/swagger/v2/swagger.json", $"{_swaggerDocumentTitle} v2");
                //c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{_swaggerDocumentTitle} v1");
                c.OAuthClientId(_swaggerClientId);
                c.OAuthAppName(_apiName);
                c.OAuthScopeSeparator(" ");

                // c.OAuthAdditionalQueryStringParams(new { foo = "bar" });
                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                //c.RoutePrefix = string.Empty;
                c.RoutePrefix = "swagger";

            });
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
