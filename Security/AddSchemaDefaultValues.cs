using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CafeExtensions.Security
{
    /// <summary>
    /// Add Schema Default Values on property fields
    /// </summary>
    public class AddSchemaDefaultValues : ISchemaFilter
    {
        /// <summary>
        /// Apply method schema
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            Type listType = context.Type;
            if (typeof(ISchema).IsAssignableFrom(listType))
            {
                var instance = Activator.CreateInstance(listType);
                if (instance != null)
                    schema.Example = ((ISchema)instance).GetDefaultValue();
            }
        }
    }
    /// <summary>
    /// Set default value for swagger attribute
    /// </summary>
    interface ISchema
    {
        IOpenApiAny GetDefaultValue();
    }
}
