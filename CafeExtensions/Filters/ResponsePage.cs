using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CafeExtensions.Filters
{
    /// <summary>
    /// Расширеный ответ сервера
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponsePage<T>
    {
        /// <summary>
        /// Базовый конструктор
        /// </summary>
        public ResponsePage() { }
        /// <summary>
        /// Расширеный конструктор
        /// </summary>
        /// <param name="data"></param>
        public ResponsePage(T data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }
        /// <summary>
        /// Объект сериализации
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// Состояние
        /// </summary>
        public bool Succeeded { get; set; }
        /// <summary>
        /// Ошибки
        /// </summary>
        public string[] Errors { get; set; }
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }
    }


}
