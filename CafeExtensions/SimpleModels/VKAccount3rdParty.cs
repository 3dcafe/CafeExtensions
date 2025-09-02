using CafeExtensions.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        protected override string ValidationUrl { get { return "https://api.vk.ru/method/secure.checkToken?token={0}&access_token=d5517937d5517937d55179374fd643fa97dd551d5517937b6b1ccb973fa59e8735d67fc&v=5.131"; } }
        /// <summary>
        /// Проверить токен
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public override async Task<AccoResponse> ValidateAccount(string token)
        {
            var validationRespone = await GetResultAsync(token);
            if (validationRespone == null || validationRespone.Message == null) throw new AccessErrorException("Error - validation error");

            var validation = JsonSerializer.Deserialize<ValidationResponseVK>(validationRespone.Message);

            var userInfoUrl = $"https://api.vk.ru/method/users.get?user_ids={validation.response.user_id}&access_token=d5517937d5517937d55179374fd643fa97dd551d5517937b6b1ccb973fa59e8735d67fc&v=5.131&fields=first_name,last_name,photo_200,city,bdate,sex&lang=ru";
            var response = await GetResultByUrlAsync(userInfoUrl);
            var vkObj = JsonSerializer.Deserialize<ResponseVK>(response.Message);
            var accoResponse = new AccoResponse();
            if (response.Code != System.Net.HttpStatusCode.OK)
                accoResponse.Error = new AccoResponseError { Message = vkObj.error.error_msg, Code = vkObj.error.error_code.ToString() };
            else
                accoResponse.Social = new AccountSocialInfo 
                { 
                    Name = vkObj.response?[0].first_name,
                    Locale = "Ru",
                    ExternalId = vkObj.response?[0].id.ToString(),
                    Provider = ProviderName.VkSocial 
                };

            return accoResponse;
        }

        #region VK Object answers
        public class ValidationResponseVK
        {
            public ValidationResponseResultVK response { get; set; }
        }

        public class ValidationResponseResultVK
        {
            public int date { get; set; }
            public int expire { get; set; }
            public int success { get; set; }
            public int user_id { get; set; }
        }

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
            public List<ResponseVKAnswer>? response { get; set; }
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
