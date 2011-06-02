namespace SharpArchContrib.Core.Extensions
{
    using System;
    using System.Linq;

    public static class TypeExtensions
  {
    public static bool IsImplementationOf<T>(this Type type)
    {
      return type.GetInterfaces().Any(x => x == typeof(T));
    }
  }
}