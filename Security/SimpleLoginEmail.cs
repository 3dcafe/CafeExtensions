using Microsoft.OpenApi.Any;

namespace CafeExtensions.Security;
/// <summary>
/// Login by security
/// </summary>
public class SimpleLoginEmail : ISchema
{
    /// <summary>
    /// адрес электронной почты
    /// </summary>
    public string? Email { get; set; }
    /// <summary>
    /// Код из почты
    /// </summary>
    public string? Code { get; set; }
    /// <summary>
    /// Пример заполнения модели
    /// </summary>
    /// <returns></returns>
    public IOpenApiAny GetDefaultValue()
    {
        return new OpenApiObject
        {
            ["email"] = new OpenApiString("olive@3dcafe.ru"),
            ["code"] = new OpenApiString("WERT5523")
        };
    }
}