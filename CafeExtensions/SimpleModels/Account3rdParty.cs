using System.Net;

namespace CafeExtensions.SimpleModels
{
    /// <summary>
    /// Абстратный класс для использования технологии авторизации через 3rd
    /// </summary>
    public abstract class Account3rdParty
    {
        /// <summary>
        /// Адресс проверки токена
        /// </summary>
        protected abstract string ValidationUrl { get; }
        /// <summary>
        /// Проверка акканута
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public abstract Task<AccoResponse> ValidateAccount(string token);
        /// <summary>
        /// Обработка результата
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected async Task<SocialResponse> GetResultAsync(string token)
        {
            var url = String.Format(ValidationUrl, token);
            SocialResponse result = null;
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage httpResponseMessage;
                try
                {
                    httpResponseMessage = await httpClient.GetAsync(url);

                    var res = await httpResponseMessage.Content.ReadAsStringAsync();
                    result = new SocialResponse { Code = httpResponseMessage.StatusCode, Message = res };
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
            return result;
        }

        /// <summary>
        /// Обработка результата
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected async Task<SocialResponse> GetResultByUrlAsync(string url)
        {
            SocialResponse result = null;
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage httpResponseMessage;
                try
                {
                    httpResponseMessage = await httpClient.GetAsync(url);

                    var res = await httpResponseMessage.Content.ReadAsStringAsync();
                    result = new SocialResponse { Code = httpResponseMessage.StatusCode, Message = res };
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
            return result;
        }
    }
    /// <summary>
    /// Server responses.
    /// </summary>
    public class AccoResponse
    {
        /// <summary>
        /// Social account information.
        /// </summary>
        public AccountSocialInfo? Social { get; set; }
        /// <summary>
        /// Error information.
        /// </summary>
        public AccoResponseError? Error { get; set; }
    }

    /// <summary>
    /// Объект ошибки авторизации
    /// </summary>
    public class AccoResponseError
    {
        /// <summary>
        /// Код
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Ошибка текста
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// Тип
        /// </summary>
        public string? Type { get; set; }
    }
    /// <summary>
    /// Ответ от социальной сети
    /// </summary>
    public class SocialResponse
    {
        /// <summary>
        /// Статус
        /// </summary>
        public HttpStatusCode Code { get; set; }
        /// <summary>
        /// Message answer
        /// </summary>
        public string? Message { get; set; }
    }
}
