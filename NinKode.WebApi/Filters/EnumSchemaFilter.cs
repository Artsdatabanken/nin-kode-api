using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Runtime.Serialization;
using System.Text.Json.Nodes;

namespace NinKode.WebApi.Filters
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(IOpenApiSchema model, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                var schema = model switch
                {
                    OpenApiSchema concrete => concrete,
                    OpenApiSchemaReference reference when context.SchemaRepository.Schemas.TryGetValue(reference.Reference.Id, out var target) && target is OpenApiSchema concrete => concrete,
                    _ => null
                } ?? throw new InvalidOperationException($"Cannot resolve enum schema for {context.Type}.");
                schema.Enum = new List<JsonNode>();
                foreach (string enumName in Enum.GetNames(context.Type))
                {
                    System.Reflection.MemberInfo memberInfo = context.Type.GetMember(enumName).FirstOrDefault(m => m.DeclaringType == context.Type);
                    EnumMemberAttribute enumMemberAttribute = memberInfo == null
                     ? null
                     : memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false).OfType<EnumMemberAttribute>().FirstOrDefault();
                    string label = enumMemberAttribute == null || string.IsNullOrWhiteSpace(enumMemberAttribute.Value)
                     ? enumName
                     : enumMemberAttribute.Value;
                    schema.Enum.Add(JsonValue.Create(label));
                }
            }
        }
    }
}
