using System;
using System.Linq.Expressions;

namespace Sara.NETStandard.Common.Extension
{
    public static class ExpressionExt
    {
        public static string GetMethodName(this object obj, Expression<Action> methodNameExpression)
        {
            var member = methodNameExpression.Body as MethodCallExpression;
            if (member == null)
                throw new ArgumentException(@"Value was not a MethodCallExpression.", "methodNameExpression");

            return member.Method.Name;
        }

    }
}
