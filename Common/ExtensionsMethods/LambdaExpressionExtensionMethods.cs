namespace Common.ExtensionsMethods
{
    using System.Linq.Expressions;

    public static class LambdaExpressionExtensionMethods
    {
        public static string GetOperandName(this LambdaExpression expression)
        {
            MemberExpression memberExpression;
            var body = expression.Body as UnaryExpression;

            if (body != null)
            {
                memberExpression = (MemberExpression)body.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)expression.Body;
            }

            return memberExpression.Member.Name;
        }
    }
}