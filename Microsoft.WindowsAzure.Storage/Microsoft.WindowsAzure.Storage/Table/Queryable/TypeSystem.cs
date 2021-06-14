using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000136 RID: 310
	internal static class TypeSystem
	{
		// Token: 0x06001434 RID: 5172 RVA: 0x0004D104 File Offset: 0x0004B304
		static TypeSystem()
		{
			TypeSystem.StaticExpressionMethodMap.Add(typeof(string).GetMethod("Contains", new Type[]
			{
				typeof(string)
			}), "substringof");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(string).GetMethod("EndsWith", new Type[]
			{
				typeof(string)
			}), "endswith");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(string).GetMethod("StartsWith", new Type[]
			{
				typeof(string)
			}), "startswith");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(string).GetMethod("IndexOf", new Type[]
			{
				typeof(string)
			}), "indexof");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(string).GetMethod("Replace", new Type[]
			{
				typeof(string),
				typeof(string)
			}), "replace");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(string).GetMethod("Substring", new Type[]
			{
				typeof(int)
			}), "substring");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(string).GetMethod("Substring", new Type[]
			{
				typeof(int),
				typeof(int)
			}), "substring");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(string).GetMethod("ToLower", Type.EmptyTypes), "tolower");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(string).GetMethod("ToUpper", Type.EmptyTypes), "toupper");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(string).GetMethod("Trim", Type.EmptyTypes), "trim");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(string).GetMethod("Concat", new Type[]
			{
				typeof(string),
				typeof(string)
			}, null), "concat");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(string).GetProperty("Length", typeof(int)).GetGetMethod(), "length");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(DateTime).GetProperty("Day", typeof(int)).GetGetMethod(), "day");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(DateTime).GetProperty("Hour", typeof(int)).GetGetMethod(), "hour");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(DateTime).GetProperty("Month", typeof(int)).GetGetMethod(), "month");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(DateTime).GetProperty("Minute", typeof(int)).GetGetMethod(), "minute");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(DateTime).GetProperty("Second", typeof(int)).GetGetMethod(), "second");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(DateTime).GetProperty("Year", typeof(int)).GetGetMethod(), "year");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(Math).GetMethod("Round", new Type[]
			{
				typeof(double)
			}), "round");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(Math).GetMethod("Round", new Type[]
			{
				typeof(decimal)
			}), "round");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(Math).GetMethod("Floor", new Type[]
			{
				typeof(double)
			}), "floor");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(Math).GetMethod("Floor", new Type[]
			{
				typeof(decimal)
			}), "floor");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(Math).GetMethod("Ceiling", new Type[]
			{
				typeof(double)
			}), "ceiling");
			TypeSystem.StaticExpressionMethodMap.Add(typeof(Math).GetMethod("Ceiling", new Type[]
			{
				typeof(decimal)
			}), "ceiling");
			TypeSystem.StaticExpressionVBMethodMap = new Dictionary<string, string>(EqualityComparer<string>.Default);
			TypeSystem.StaticExpressionVBMethodMap.Add("Microsoft.VisualBasic.Strings.Trim", "trim");
			TypeSystem.StaticExpressionVBMethodMap.Add("Microsoft.VisualBasic.Strings.Len", "length");
			TypeSystem.StaticExpressionVBMethodMap.Add("Microsoft.VisualBasic.Strings.Mid", "substring");
			TypeSystem.StaticExpressionVBMethodMap.Add("Microsoft.VisualBasic.Strings.UCase", "toupper");
			TypeSystem.StaticExpressionVBMethodMap.Add("Microsoft.VisualBasic.Strings.LCase", "tolower");
			TypeSystem.StaticExpressionVBMethodMap.Add("Microsoft.VisualBasic.DateAndTime.Year", "year");
			TypeSystem.StaticExpressionVBMethodMap.Add("Microsoft.VisualBasic.DateAndTime.Month", "month");
			TypeSystem.StaticExpressionVBMethodMap.Add("Microsoft.VisualBasic.DateAndTime.Day", "day");
			TypeSystem.StaticExpressionVBMethodMap.Add("Microsoft.VisualBasic.DateAndTime.Hour", "hour");
			TypeSystem.StaticExpressionVBMethodMap.Add("Microsoft.VisualBasic.DateAndTime.Minute", "minute");
			TypeSystem.StaticExpressionVBMethodMap.Add("Microsoft.VisualBasic.DateAndTime.Second", "second");
			TypeSystem.StaticPropertiesAsMethodsMap = new Dictionary<PropertyInfo, MethodInfo>(EqualityComparer<PropertyInfo>.Default);
			TypeSystem.StaticPropertiesAsMethodsMap.Add(typeof(string).GetProperty("Length", typeof(int)), typeof(string).GetProperty("Length", typeof(int)).GetGetMethod());
			TypeSystem.StaticPropertiesAsMethodsMap.Add(typeof(DateTime).GetProperty("Day", typeof(int)), typeof(DateTime).GetProperty("Day", typeof(int)).GetGetMethod());
			TypeSystem.StaticPropertiesAsMethodsMap.Add(typeof(DateTime).GetProperty("Hour", typeof(int)), typeof(DateTime).GetProperty("Hour", typeof(int)).GetGetMethod());
			TypeSystem.StaticPropertiesAsMethodsMap.Add(typeof(DateTime).GetProperty("Minute", typeof(int)), typeof(DateTime).GetProperty("Minute", typeof(int)).GetGetMethod());
			TypeSystem.StaticPropertiesAsMethodsMap.Add(typeof(DateTime).GetProperty("Second", typeof(int)), typeof(DateTime).GetProperty("Second", typeof(int)).GetGetMethod());
			TypeSystem.StaticPropertiesAsMethodsMap.Add(typeof(DateTime).GetProperty("Month", typeof(int)), typeof(DateTime).GetProperty("Month", typeof(int)).GetGetMethod());
			TypeSystem.StaticPropertiesAsMethodsMap.Add(typeof(DateTime).GetProperty("Year", typeof(int)), typeof(DateTime).GetProperty("Year", typeof(int)).GetGetMethod());
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x0004D958 File Offset: 0x0004BB58
		internal static bool TryGetQueryOptionMethod(MethodInfo mi, out string methodName)
		{
			return TypeSystem.StaticExpressionMethodMap.TryGetValue(mi, out methodName) || (mi.DeclaringType.Assembly.FullName == "Microsoft.VisualBasic, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" && TypeSystem.StaticExpressionVBMethodMap.TryGetValue(mi.DeclaringType.FullName + "." + mi.Name, out methodName));
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0004D9B9 File Offset: 0x0004BBB9
		internal static bool TryGetPropertyAsMethod(PropertyInfo pi, out MethodInfo mi)
		{
			return TypeSystem.StaticPropertiesAsMethodsMap.TryGetValue(pi, out mi);
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0004D9C8 File Offset: 0x0004BBC8
		internal static Type GetElementType(Type seqType)
		{
			Type type = TypeSystem.FindIEnumerable(seqType);
			if (type == null)
			{
				return seqType;
			}
			return type.GetGenericArguments()[0];
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x0004D9F0 File Offset: 0x0004BBF0
		internal static bool IsPrivate(PropertyInfo pi)
		{
			MethodInfo methodInfo = pi.GetGetMethod() ?? pi.GetSetMethod();
			return !(methodInfo != null) || methodInfo.IsPrivate;
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x0004DA20 File Offset: 0x0004BC20
		internal static Type FindIEnumerable(Type seqType)
		{
			if (seqType == null || seqType == typeof(string))
			{
				return null;
			}
			if (seqType.IsArray)
			{
				return typeof(IEnumerable<>).MakeGenericType(new Type[]
				{
					seqType.GetElementType()
				});
			}
			if (seqType.IsGenericType)
			{
				foreach (Type type in seqType.GetGenericArguments())
				{
					Type type2 = typeof(IEnumerable<>).MakeGenericType(new Type[]
					{
						type
					});
					if (type2.IsAssignableFrom(seqType))
					{
						return type2;
					}
				}
			}
			Type[] interfaces = seqType.GetInterfaces();
			if (interfaces != null && interfaces.Length > 0)
			{
				foreach (Type seqType2 in interfaces)
				{
					Type type3 = TypeSystem.FindIEnumerable(seqType2);
					if (type3 != null)
					{
						return type3;
					}
				}
			}
			if (seqType.BaseType != null && seqType.BaseType != typeof(object))
			{
				return TypeSystem.FindIEnumerable(seqType.BaseType);
			}
			return null;
		}

		// Token: 0x040006B0 RID: 1712
		private const string VisualBasicAssemblyFullName = "Microsoft.VisualBasic, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		// Token: 0x040006B1 RID: 1713
		private static readonly Dictionary<MethodInfo, string> StaticExpressionMethodMap = new Dictionary<MethodInfo, string>(24, EqualityComparer<MethodInfo>.Default);

		// Token: 0x040006B2 RID: 1714
		private static readonly Dictionary<string, string> StaticExpressionVBMethodMap;

		// Token: 0x040006B3 RID: 1715
		private static readonly Dictionary<PropertyInfo, MethodInfo> StaticPropertiesAsMethodsMap;
	}
}
