using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using NiN.Database;
using NiN3.Infrastructure.DbContexts;
using NiN3.Infrastructure.Services;
using NinKode.WebApi.Helpers;
using NinKode.WebApi.Helpers.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// ef core database
var connectionString = builder.Configuration.GetConnectionString("Default");
var nin3ConnectionString = builder.Configuration.GetConnectionString("NiN3");
if (string.IsNullOrEmpty(connectionString))
    throw new Exception("Could not find ConnectionString 'Default'");
if (string.IsNullOrEmpty(nin3ConnectionString))
    throw new Exception("Could not find ConnectionString 'NiN3'");

builder.Services.AddDbContext<NiNDbContext>(o => { o.UseSqlite(connectionString); });
builder.Services.AddDbContext<NiN3DbContext>(o => { o.UseSqlite(nin3ConnectionString); });

// Adding services
builder.Services.AddScoped<IRapportService, RapportService>();
builder.Services.AddScoped<ISearchService, SearchService>();

// misc project services
builder.Services.AddApplicationServices();

// api versioning
builder.Services.ConfigureVersioning();

// swagger
builder.Services.AddSwagger();
builder.Services.ConfigureOptions<SwaggerOptions>();

builder.Services.AddProblemDetails(options => { options.IncludeExceptionDetails = (_, _) => builder.Environment.IsDevelopment(); });

var app = builder.Build();

app.MapControllers();

app.ConfigureSwagger();

app.UseProblemDetails();

app.Run();