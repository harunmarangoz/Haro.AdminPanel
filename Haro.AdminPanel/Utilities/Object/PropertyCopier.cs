using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Haro.AdminPanel.Utilities.Object
{
    internal static class PropertyCopier<TSource, TTarget>
    {
        private static readonly List<PropertyInfo> sourceProperties = new List<PropertyInfo>();
        private static readonly List<PropertyInfo> targetProperties = new List<PropertyInfo>();
        private static readonly Func<TSource, TTarget> creator;
        private static readonly Exception initializationException;

        internal static TTarget Copy(TSource source)
        {
            if (initializationException != null) throw initializationException;
            if ((object) source == null)
                throw new ArgumentNullException(nameof(source));
            return creator(source);
        }

        internal static void Copy(TSource source, TTarget target)
        {
            if (initializationException != null)
                throw initializationException;
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            for (int index = 0; index < sourceProperties.Count; ++index)
                targetProperties[index].SetValue(target, sourceProperties[index].GetValue(source, null), null);
        }

        static PropertyCopier()
        {
            try
            {
                creator = BuildCreator();
                initializationException = null;
            }
            catch (Exception ex)
            {
                creator = null;
                initializationException = ex;
            }
        }

        private static Func<TSource, TTarget> BuildCreator()
        {
            ParameterExpression sourceParameter = Expression.Parameter(typeof(TSource), "source");
            var bindings = new List<MemberBinding>();
            foreach (PropertyInfo sourceProperty in typeof(TSource).GetProperties(
                BindingFlags.Public | BindingFlags.Instance))
            {
                if (!sourceProperty.CanRead)
                {
                    continue;
                }

                PropertyInfo targetProperty = typeof(TTarget).GetProperty(sourceProperty.Name);
                if (targetProperty == null)
                {
                    throw new ArgumentException("Property " + sourceProperty.Name +
                                                " is not present and accessible in " + typeof(TTarget).FullName);
                }

                if (!targetProperty.CanWrite)
                {
                    throw new ArgumentException("Property " + sourceProperty.Name + " is not writable in " +
                                                typeof(TTarget).FullName);
                }

                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    throw new ArgumentException("Property " + sourceProperty.Name + " is static in " +
                                                typeof(TTarget).FullName);
                }

                if (!targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                {
                    throw new ArgumentException("Property " + sourceProperty.Name + " has an incompatible type in " +
                                                typeof(TTarget).FullName);
                }

                bindings.Add(Expression.Bind(targetProperty, Expression.Property(sourceParameter, sourceProperty)));
                sourceProperties.Add(sourceProperty);
                targetProperties.Add(targetProperty);
            }

            Expression initializer = Expression.MemberInit(Expression.New(typeof(TTarget)), bindings);
            return Expression.Lambda<Func<TSource, TTarget>>(initializer, sourceParameter).Compile();
        }
    }
}