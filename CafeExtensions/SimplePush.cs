using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeExtensions
{
    /// <summary>
    /// Простая модель уведомлений
    /// </summary>
    public class SimplePush
    {
        /// <summary>
        /// Идентификатор приложения
        /// </summary>
        public string app_id { get; set; }
        /// <summary>
        /// Ид пользователей
        /// </summary>
        public List<string> include_player_ids { get; set; }
        /// <summary>
        /// Заголовок
        /// </summary>
        public Headings headings { get; set; }
        /// <summary>
        /// Контект который отправляем
        /// </summary>
        public Contents contents { get; set; }
        /// <summary>
        /// Подзаголовок
        /// </summary>
        public Subtitle? subtitle { get; set; }
        /// <summary>
        /// Color push on android
        /// </summary>
        public string? android_accent_color { get; set; } = null;
        /// <summary>
        /// Data on push message
        /// </summary>
        public DataSimplePush? data { get; set; } = null;
    }

    public class DataSimplePush
    {
        public string? userId { get; set; }

        public int? type { get; set; }

#warning add this fields
        //userId = from.Id,
        //            inits = callerInits,
        //            my_inits = callerInits,
        //            ttl = 30000,
        //            with_video = withVideo,
        //            discard,
        //            type = typePush,
        //            avatar = from.Avatar ?? "",
        //            my_avatar = from.Avatar ?? "",
        //            phone = from.PhoneNumber ?? "-",
        //            roomId,
        //            callId
    }

    /// <summary>
    /// Заголовок
    /// </summary>
    public class Headings
    {
        /// <summary>
        /// Язык - текст
        /// </summary>
        public string en { get; set; }
    }
    /// <summary>
    /// Контент который отправляем
    /// </summary>
    public class Contents
    {
        /// <summary>
        /// Текст сообщея
        /// </summary>
        public string en { get; set; }
    }

    /// <summary>
    /// Подзаголовок
    /// </summary>
    public class Subtitle
    {
        public string en { get; set; }
    }
}
