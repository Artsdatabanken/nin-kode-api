using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NinKode.WebApi.Filters
{
    public class EnumTypeSchemaFilter : ISchemaFilter
    {
        public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                var mutableSchema = schema switch
                {
                    OpenApiSchema concrete => concrete,
                    OpenApiSchemaReference reference when context.SchemaRepository.Schemas.TryGetValue(reference.Reference.Id, out var target) && target is OpenApiSchema concrete => concrete,
                    _ => null
                } ?? throw new InvalidOperationException($"Cannot resolve enum schema for {context.Type}.");
                mutableSchema.Type = JsonSchemaType.String;
            }
        }
    }
}
