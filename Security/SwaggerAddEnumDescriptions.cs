using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CafeExtensions.Security
{
    /// <summary>
    /// Generate documentation in swagger for enum types
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SwaggerAddEnumDescriptions : IDocumentFilter
    {
        /// <summary>
        /// Base method
        /// add enum descriptions to result models
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // add enum descriptions to result models
            foreach (var property in swaggerDoc.Components.Schemas.Where(x => x.Value?.Enum?.Count > 0))
            {
                IList<IOpenApiAny> propertyEnums = property.Value.Enum;
                if (propertyEnums is { Count: > 0 })
                {
                    property.Value.Description += DescribeEnum(propertyEnums, property.Key);
                }
            }
            // add enum descriptions to input parameters
            foreach (var pathItem in swaggerDoc.Paths.Values)
            {
                DescribeEnumParameters(pathItem.Operations, swaggerDoc);
            }
        }

        private static void DescribeEnumParameters(IDictionary<OperationType, OpenApiOperation>? operations, OpenApiDocument swaggerDoc)
        {
            if (operations != null)
            {
                foreach (var operation in operations)
                {
                    foreach (var param in operation.Value.Parameters)
                    {
                        var paramEnum = swaggerDoc.Components.Schemas.FirstOrDefault(x => x.Key == param.Name);
                        if (paramEnum.Value != null)
                            param.Description += DescribeEnum(paramEnum.Value.Enum, paramEnum.Key);
                    }
                }
            }
        }
        private static Type? GetEnumTypeByName(string enumTypeName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .FirstOrDefault(x => x.Name == enumTypeName);
        }
        private static string? DescribeEnum(IList<IOpenApiAny> enums, string propertyTypeName)
        {
            List<string> enumDescriptions = new();
            var enumType = GetEnumTypeByName(propertyTypeName);
            if (enumType == null)
                return null;
            foreach (var openApiAny in enums)
            {
                var enumOption = (OpenApiInteger)openApiAny;
                int enumInt = enumOption.Value;

                enumDescriptions.Add($"{enumInt} = {Enum.GetName(enumType, enumInt)}");
            }
            return string.Join(", ", enumDescriptions.ToArray());
        }
    }
}
