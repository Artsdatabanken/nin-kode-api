using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NiN.Database;
using NiN3.Infrastructure.DbContexts;
using NiN3.Infrastructure.Services;
using NinKode.WebApi.Filters;
using NinKode.WebApi.Helpers;
using NinKode.WebApi.Helpers.Swagger;
using System.Text.Json.Serialization;
using System.Xml.Linq;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); 

// ef core database
var connectionString = builder.Configuration.GetConnectionString("Default");
var nin3ConnectionString = builder.Configuration.GetConnectionString("NiN3");
if (string.IsNullOrEmpty(connectionString))
    throw new Exception("Could not find ConnectionString 'Default'");
if (string.IsNullOrEmpty(nin3ConnectionString))
    throw new Exception("Could not find ConnectionString 'NiN3'");

builder.Services.AddDbContext<NiNDbContext>(o => { o.UseSqlite(connectionString); });
builder.Services.AddDbContext<NiN3DbContext>(o => { o.UseSqlite(nin3ConnectionString); });

// Adding services nin3
builder.Services.AddScoped<IRapportService, RapportService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IVariabelApiService, VariabelApiService>();
builder.Services.AddScoped<ITypeApiService, TypeApiService>();

// misc project services
builder.Services.AddApplicationServices();

// api versioning
builder.Services.ConfigureVersioning();

// swagger
builder.Services.AddSwagger();
builder.Services.ConfigureOptions<SwaggerOptions>();
builder.Services.AddEndpointsApiExplorer();

// Adding swagger doc for enums
builder.Services.AddSingleton(x => XDocument.Load("NiN3.WebApi.xml"));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NiN3 API", Version = "v3.0" });
    var xmlFile = "NiN3.WebApi.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.SchemaFilter<EnumSchemaFilter>();
});

builder.Services.AddProblemDetails(options => { options.IncludeExceptionDetails = (_, _) => builder.Environment.IsDevelopment(); });

var app = builder.Build();

// redirect homepage to swagger ui
//app.MapGet("/", (HttpContext context) => context.Response.Redirect("./swagger/index.html", permanent: true));

app.MapControllers();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/index.html")
    {
        context.Response.Headers.Add("env", app.Environment.EnvironmentName);
        context.Response.Redirect("/swagger/index.html", permanent: true);
        return;
    }

    await next();
});

app.ConfigureSwagger();

app.UseProblemDetails();

app.Run();