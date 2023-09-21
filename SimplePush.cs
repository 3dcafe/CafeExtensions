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


}
