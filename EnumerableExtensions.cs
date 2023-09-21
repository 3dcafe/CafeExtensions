using System.Linq.Expressions;
using System.Reflection;


namespace CafeExtensions
{
    /// <summary>
    /// Расширения для коллекций
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Загрузить случайные записи из коллекции
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="count">Кол-во элементов</param>
        /// <returns></returns>
        public static IEnumerable<T> Random<T>(this IEnumerable<T> enumerable, int count = 1)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            var r = new Random();
            var list = enumerable as IList<T> ?? enumerable.ToList();
            if (list.Count <= count)
                return enumerable;
            return list.Randomize().Take(count);
        }
        /// <summary>
        /// Пермещать список
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        {
            Random rnd = new Random();
            return source.OrderBy((item) => rnd.Next());
        }
        /// <summary>
        /// Получение свойства по названию
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static PropertyInfo GetPropertyInfo(Type objType, string name)
        {
            var properties = objType.GetProperties();
            var matchedProperty = properties.FirstOrDefault(p => p.Name == name);
            if (matchedProperty == null)
                throw new ArgumentException("Недопустимое поле для сортировки");

            return matchedProperty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi)
        {
            var paramExpr = Expression.Parameter(objType);
            var propAccess = Expression.PropertyOrField(paramExpr, pi.Name);
            var expr = Expression.Lambda(propAccess, paramExpr);
            return expr;
        }
        
        /// <summary>
        /// Сортировка расширение
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IQueryable<T>? OrderBy<T>(this IQueryable<T> query, string name)
        {
            var propInfo = GetPropertyInfo(typeof(T), name);
            var expr = GetOrderExpression(typeof(T), propInfo);

            var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
            var genericMethod = method?.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IQueryable<T>?)genericMethod?.Invoke(null, new object[] { query, expr });
        }
        /// <summary>
        /// Обратная сортировка расширение
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IQueryable<T>? OrderByDescending<T>(this IQueryable<T> query, string name)
        {
            var propInfo = GetPropertyInfo(typeof(T), name);
            var expr = GetOrderExpression(typeof(T), propInfo);

            var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);
            var genericMethod = method?.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IQueryable<T>?)genericMethod?.Invoke(null, new object[] { query, expr });
        }
    }
}
