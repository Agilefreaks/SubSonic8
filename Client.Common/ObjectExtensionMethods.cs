using System;
using System.Linq.Expressions;
using System.Reflection;
using MetroLog;

namespace Client.Common
{
    public static class ObjectExtensionMethods
    {
        public static string GetPropertyName<TProperty>(this object target, Expression<Func<TProperty>> property)
        {
            return property.GetMemberInfo().Name;
        }

        private static MemberInfo GetMemberInfo(this Expression expression)
        {
            var lambda = (LambdaExpression)expression;

            MemberExpression memberExpression;
            var body = lambda.Body as UnaryExpression;

            if (body != null)
            {
                memberExpression = (MemberExpression)body.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }

            return memberExpression.Member;
        }

        public static void Log<T>(this T source, string message)
        {
            LogManagerFactory.DefaultLogManager.GetLogger<T>().Info(message);
        }

        public static void Log<T>(this T source, Exception exception)
        {
            LogManagerFactory.DefaultLogManager.GetLogger<T>().Error("Exception", exception);
        }
    }
}
