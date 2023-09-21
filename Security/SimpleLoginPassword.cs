using Microsoft.OpenApi.Any;

namespace CafeExtensions.Security
{
    /// <summary>
    /// Объект для получения токена
    /// </summary>
    public class SimpleLoginPassword : ISchema
    {
        /// <summary>
        /// Логин авторизации в системе
        /// </summary>
        public string? Login { get; set; }
        /// <summary>
        /// Код из смс
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// Пример заполнения модели
        /// </summary>
        /// <returns></returns>
        public IOpenApiAny GetDefaultValue()
        {
            return new OpenApiObject
            {
                ["login"] = new OpenApiString("1054"),
                ["password"] = new OpenApiString("e3dl93tHe")
            };
        }
    }
}