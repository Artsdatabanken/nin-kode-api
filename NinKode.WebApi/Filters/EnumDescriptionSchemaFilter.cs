using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Reflection;

namespace NinKode.WebApi.Filters
{
    public class EnumDescriptionSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                var enumType = context.Type;
                var enumValues = Enum.GetValues(enumType);

                foreach (var enumValue in enumValues)
                {
                    var memberInfo = enumType.GetMember(enumValue.ToString());
                    var descriptionAttribute = memberInfo[0].GetCustomAttribute<DescriptionAttribute>();
                    var description = descriptionAttribute != null ? descriptionAttribute.Description : enumValue.ToString();

                    if (schema.Enum.Count > 0)
                    {
                        var enumIndex = (int)enumValue;
                        if (enumIndex >= 0 && enumIndex < schema.Enum.Count)
                        {
                            schema.Enum[enumIndex] = new OpenApiString(description);
                        }
                    }
                }
            }
        }
    }
}
