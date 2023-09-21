using System.Text.Json;

namespace CafeExtensions.SimpleModels
{
    /// <summary>
    /// Авторизация через вконтаке
    /// </summary>
    public class VKAccount3rdParty : Account3rdParty
    {
        /// <summary>
        /// Адрес проверки токена
        /// </summary>
        protected override string ValidationUrl { get { return "https://api.vk.com/method/account.getProfileInfo?access_token={0}&v=5.131"; } }
        /// <summary>
        /// Проверить токен
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public override async Task<AccoResponse> ValidateAccount(string token)
        {
            var response = await GetResultAsync(token);
            var vkObj = JsonSerializer.Deserialize<ResponseVK>(response.Message);
            var accoResponse = new AccoResponse();
            if (response.Code != System.Net.HttpStatusCode.OK)
                accoResponse.Error = new AccoResponseError { Message = vkObj.error.error_msg, Code = vkObj.error.error_code.ToString() };
            else
                accoResponse.Social = new AccountSocialInfo 
                { 
                    Name = vkObj.response?.first_name,
                    Locale = "Ru",
                    ExternalId = vkObj.response?.id.ToString(),
                    Provider = ProviderName.VkSocial 
                };

            return accoResponse;
        }

        #region VK Object answers
        /// <summary>
        /// Базовый объект
        /// </summary>
        public class ResponseVK
        {
            /// <summary>
            /// Объект ошибки
            /// </summary>
            public ErrorVK? error { get; set; }
            /// <summary>
            /// Ответы
            /// </summary>
            public ResponseVKAnswer? response { get; set; }
        }

        public class ErrorVK
        {
            public int error_code { get; set; }
            public string error_msg { get; set; }
            public List<RequestParam> request_params { get; set; }
        }
        public class RequestParam
        {
            public string key { get; set; }
            public string value { get; set; }
        }

        public class ResponseVKAnswer
        {
            public int id { get; set; }
            public string home_town { get; set; }
            public string status { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string bdate { get; set; }
            public int bdate_visibility { get; set; }
            public CityVK city { get; set; }
            public CountryVK country { get; set; }
            public string phone { get; set; }
            public int relation { get; set; }
            public string screen_name { get; set; }
            public int sex { get; set; }
        }

        public class CityVK
        {
            public int id { get; set; }
            public string title { get; set; }
        }

        public class CountryVK
        {
            public int id { get; set; }
            public string title { get; set; }
        }
        #endregion
    }

}
