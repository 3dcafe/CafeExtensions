using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CafeExtensions.Filters
{
    /// <summary>
    /// Расширеный ответ на табличный вывод
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResponse<T> : ResponsePage<T>
    {
        /// <summary>
        /// Номер страницы
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Размер массива
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Первая страница
        /// </summary>
        public Uri FirstPage { get; set; }
        /// <summary>
        /// Последняя страница
        /// </summary>
        public Uri LastPage { get; set; }
        /// <summary>
        /// Всего страниц
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// Всего записей
        /// </summary>
        public int TotalRecords { get; set; }
        /// <summary>
        /// Следующая страница
        /// </summary>
        public Uri NextPage { get; set; }
        /// <summary>
        /// Предыдущая страница
        /// </summary>
        public Uri PreviousPage { get; set; }
        /// <summary>
        /// Основной конструтор
        /// </summary>
        /// <param name="data">Объект</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Размер</param>
        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            this.Message = null;
            this.Succeeded = true;
            this.Errors = null;
        }
    }

}
