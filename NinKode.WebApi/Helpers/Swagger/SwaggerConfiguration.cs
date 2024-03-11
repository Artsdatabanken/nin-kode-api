using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace NinKode.WebApi.Helpers.Swagger;

public static class SwaggerConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var docs = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            options.IncludeXmlComments(docs);
            options.OperationFilter<SwaggerDefaultValues>();
        });
    }

    public static void ConfigureSwagger(this WebApplication app)
    {
        app.UseSwagger();

        var versionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwaggerUI(options =>
        {
            //turn of syntaxhighlight in swagger for better responsetime
            if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
            {
                options.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
                {
                    ["activated"] = false
                };
            }

            var descriptions = versionDescriptionProvider.ApiVersionDescriptions.Reverse().ToList();

            foreach (var description in descriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }

            options.DisplayRequestDuration();
        });
    }
}