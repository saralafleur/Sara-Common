using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Sara.NETStandard.Common.Extension
{
    public static class PropertyExt
    {
        public static PropertyInfo GetPropertyInfo<T>(this T obj, Expression<Func<T, object>> propertyAccessor)
        {
            var lambda = (LambdaExpression)propertyAccessor;
            var unary = lambda.Body as UnaryExpression;
            var memberExpression = unary != null ? (MemberExpression)unary.Operand : (MemberExpression)lambda.Body;

            return (PropertyInfo)memberExpression.Member;
        }
        public static string GetPropertyName<T>(this T t, Expression<Func<T, object>> propertyAccessor)
        {
            return GetPropertyInfo(t, propertyAccessor).Name;
        }

    }
}
