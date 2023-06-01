using Hellang.Middleware.ProblemDetails;
using NinKode.WebApi.Helpers;
using NinKode.WebApi.Helpers.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddProblemDetails(options => { options.IncludeExceptionDetails = (_, _) => builder.Environment.IsDevelopment(); });

var app = builder.Build();

app.MapControllers();

app.ConfigureSwagger();

app.UseProblemDetails();

app.Run();