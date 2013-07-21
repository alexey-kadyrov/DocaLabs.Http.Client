using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DocaLabs.Testing.Common
{
    public class Reflect<T> where T : class
    {
        public static PropertyInfo GetPropertyInfo<TReturn>(Expression<Func<T, TReturn>> expression)
        {
            return ReflectionTools.GetPropertyInfoFromExpression(expression);
        }

        public static PropertyInfo GetIndexerInfo(params Type[] expectedIndexParameters)
        {
            return typeof (T).GetProperties()
                .Where(x => x.Name == "Item")
                .FirstOrDefault(x => x.CompareIndexParameters(expectedIndexParameters));
        }

        public static PropertyInfo GetIndexerInfo<T1>()
        {
            return GetIndexerInfo(typeof (T1));
        }

        public static PropertyInfo GetIndexerInfo<T1, T2>()
        {
            return GetIndexerInfo(typeof(T1), typeof(T2));
        }

        public static PropertyInfo GetIndexerInfo<T1, T2, T3>()
        {
            return GetIndexerInfo(typeof(T1), typeof(T2), typeof(T3));
        }
    }

    public class Reflect
    {
        public static PropertyInfo GetPropertyInfo<TReturn>(Expression<Func<TReturn>> expression)
        {
            return ReflectionTools.GetPropertyInfoFromExpression(expression);
        }
    }

    public static class ReflectionTools
    {
        public static PropertyInfo GetPropertyInfo<T, TProperty>(this T obj, Expression<Func<T, TProperty>> expression)
        {
            return GetPropertyInfoFromExpression(expression);
        }

        public static PropertyInfo GetIndexerInfo<T>(this T obj, params Type[] expectedIndexParameters)
        {
            return typeof(T).GetProperties()
                .Where(x => x.Name == "Item")
                .FirstOrDefault(x => x.CompareIndexParameters(expectedIndexParameters));
        }

        public static PropertyInfo GetIndexerInfo<T, T1>(this T obj, params Type[] expectedIndexParameters)
        {
            return GetIndexerInfo(obj, typeof (T1));
        }

        public static PropertyInfo GetIndexerInfo<T, T1, T2>(this T obj, params Type[] expectedIndexParameters)
        {
            return GetIndexerInfo(obj, typeof(T1), typeof(T2));
        }

        public static PropertyInfo GetIndexerInfo<T, T1, T2, T3>(this T obj, params Type[] expectedIndexParameters)
        {
            return GetIndexerInfo(obj, typeof(T1), typeof(T2), typeof(T3));
        }

        public static PropertyInfo GetPropertyInfoFromExpression(LambdaExpression expression)
        {
            var body = expression.Body;
            if (body.NodeType == ExpressionType.Convert)
                body = ((UnaryExpression)body).Operand;

            var memberExpression = body as MemberExpression;

            if (memberExpression == null || memberExpression.Member.MemberType != MemberTypes.Property)
                throw new ArgumentException(string.Format("The Expression {0} must define the property", expression), "expression");

            return (PropertyInfo)memberExpression.Member;
        }

        public static bool CompareIndexParameters(this PropertyInfo info, Type[] expectedIndexParameters)
        {
            var indexParameters = info.GetIndexParameters();

            if (indexParameters.Length == expectedIndexParameters.Length)
            {
                if (!indexParameters.Where((t, i) => t.ParameterType != expectedIndexParameters[i]).Any())
                    return true;
            }

            return false;
        }
    }
}
