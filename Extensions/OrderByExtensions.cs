namespace Extensions {

    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks; 
    #endregion

    public static class OrderByExtensions {

        [DebuggerStepThroughAttribute]
        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName) {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props) {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }

        /// <summary>
        /// Performs a logical between similar to TSQL
        /// </summary>
        /// <typeparam name="TSource">The source queryable type</typeparam>
        /// <typeparam name="TKey">The IComparable key</typeparam>
        /// <param name="source">The source queryable type</param>
        /// <param name="keySelector">The key selector</param>
        /// <param name="low">The low value to compare</param>
        /// <param name="high">The upper value to compare</param>
        /// <returns>IQueryable result</returns>
        [DebuggerStepThroughAttribute]
        public static IQueryable<TSource> Between<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, TKey low, TKey high)
            where TKey : IComparable<TKey> {
            Expression key = Expression.Invoke(keySelector, keySelector.Parameters.ToArray());
            Expression lowerBound = Expression.GreaterThanOrEqual(key, Expression.Constant(low));
            Expression upperBound = Expression.LessThanOrEqual(key, Expression.Constant(high));
            Expression and = Expression.AndAlso(lowerBound, upperBound);
            Expression<Func<TSource, bool>> lambda =
                Expression.Lambda<Func<TSource, bool>>(and, keySelector.Parameters);
            return source.Where(lambda);
        }

        /// <summary>
        /// Generic method to sort a list by string column name, and direction
        /// </summary>
        /// <typeparam name="T">The Type</typeparam>
        /// <param name="source">The source IEnumerable<T></param>
        /// <param name="propertyName">The property name on which to sort</param>
        /// <param name="sortDirection">The sort direction ("DESC" for descending)</param>
        /// <returns>The sorted IEnumerable<T></returns>
        [DebuggerStepThroughAttribute]
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string propertyName, string sortDirection = "ASC") {
            var expressionParameter = Expression.Parameter(typeof(T), "item");
            var sortExpression = Expression.Lambda<Func<T, object>>(
                Expression.Convert(Expression.Property(expressionParameter, propertyName), typeof(object)), expressionParameter);

            switch (sortDirection.ToUpperInvariant()) {
                case "DESC":
                    return source.AsQueryable<T>().OrderByDescending<T, object>(sortExpression);
                default:
                    return source.AsQueryable<T>().OrderBy<T, object>(sortExpression);

            }
        }

        [DebuggerStepThroughAttribute]
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property) {
            return ApplyOrder<T>(source, property, "OrderBy");
        }

        [DebuggerStepThroughAttribute]
        public static IEnumerable<T> OrderByDescending<T>(this IEnumerable<T> source, string propertyName) {
            var expressionParameter = Expression.Parameter(typeof(T), "item");
            var sortExpression = Expression.Lambda<Func<T, object>>(
                Expression.Convert(Expression.Property(expressionParameter, propertyName), typeof(object)), expressionParameter);
            return source.AsQueryable<T>().OrderByDescending<T, object>(sortExpression);
        }

        [DebuggerStepThroughAttribute]
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property) {
            return ApplyOrder<T>(source, property, "OrderByDescending");
        }

        [DebuggerStepThroughAttribute]
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property) {
            return ApplyOrder<T>(source, property, "ThenBy");
        }

        [DebuggerStepThroughAttribute]
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property) {
            return ApplyOrder<T>(source, property, "ThenByDescending");
        }

    }
}
