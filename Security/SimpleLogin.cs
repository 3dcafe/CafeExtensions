using Microsoft.OpenApi.Any;

namespace CafeExtensions.Security
{
    /// <summary>
    /// Объект для получения токена
    /// </summary>
    public class SimpleLogin : ISchema
    {
        /// <summary>
        /// Логин авторизации в системе
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// Код из смс
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
                ["phone"] = new OpenApiString("79056684580"),
                ["code"] = new OpenApiString("1122")
            };
        }
    }
}