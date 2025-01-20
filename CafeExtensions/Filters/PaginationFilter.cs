namespace CafeExtensions.Filters
{
    /// <summary>
    /// Фильтр для пагинации
    /// </summary>
    public class PaginationFilter
    {
        /// <summary>
        /// Поисковое слово
        /// </summary>
        public string Search { get; set; }
        /// <summary>
        /// Номер страницы
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Размер страницы
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Базовый конструктор
        /// </summary>
        public PaginationFilter()
        {
            PageNumber = 1;
            PageSize = 10;
            Search = string.Empty;
        }
        /// <summary>
        /// Расширеный конструктор с параметрами
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 10 ? 10 : pageSize;
            Search = string.Empty;
        }
        /// <summary>
        /// Расширеный конструктор с поиском
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="search"></param>
        public PaginationFilter(int pageNumber, int pageSize, string search)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 10 ? 10 : pageSize;
            Search = search;
        }
    }

}
