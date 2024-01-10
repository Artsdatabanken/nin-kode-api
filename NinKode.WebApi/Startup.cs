using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using NiN.Database;
using NinKode.WebApi.Helpers;
using NinKode.WebApi.Helpers.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// ef core database
var connectionString = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrEmpty(connectionString))
    throw new Exception("Could not find ConnectionString 'Default'");

//builder.Services.AddDbContext<NiNDbContext>(o => { o.UseSqlServer(connectionString, x => x.MigrationsAssembly(typeof(NiNDbContext).Module.Name)); });
builder.Services.AddDbContext<NiNDbContext>(o => { o.UseSqlite(connectionString); });

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