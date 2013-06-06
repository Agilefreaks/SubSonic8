namespace Common.ExtensionsMethods
{
    using System;
    using System.Linq.Expressions;

    public static class ObjectExtensionMethods
    {
        public static string GetPropertyName<TTarget, TProperty>(this TTarget target, Expression<Func<TProperty>> property)
            where TTarget : class
        {
            return property.GetOperandName();
        }
    }
}