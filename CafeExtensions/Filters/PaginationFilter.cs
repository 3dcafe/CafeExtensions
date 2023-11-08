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
        public string search { get; set; }
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
            this.PageNumber = 1;
            this.PageSize = 10;
            this.search = string.Empty;
        }
        /// <summary>
        /// Расширеный конструктор с параметрами
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
            this.search = string.Empty;
        }
        /// <summary>
        /// Расширеный конструктор с поиском
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="search"></param>
        public PaginationFilter(int pageNumber, int pageSize, string search)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
            this.search = search;
        }
    }

}
