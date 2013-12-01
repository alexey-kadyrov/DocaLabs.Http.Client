using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DocaLabs.Http.Client.Utils
{
    public class Reflect<T>
    {
        public static FieldInfo GetFieldInfo<TReturn>(Expression<Func<T, TReturn>> expression)
        {
            return ReflectionTools.GetFieldInfoFromExpression(expression);
        }
    }

    public static class ReflectionTools
    {
        public static FieldInfo GetFieldInfoFromExpression(LambdaExpression expression)
        {
            var body = expression.Body;
            if (body.NodeType == ExpressionType.Convert)
                body = ((UnaryExpression)body).Operand;

            var memberExpression = body as MemberExpression;

            if (memberExpression == null || !(memberExpression.Member is FieldInfo))
                throw new ArgumentException(string.Format("The Expression {0} must define the property", expression), "expression");

            return (FieldInfo)memberExpression.Member;
        }
    }
}
