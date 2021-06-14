using System;
using System.Collections.Generic;
using System.Reflection;
using System.Spatial;

namespace System.Data.Services.Client
{
	// Token: 0x020000DB RID: 219
	internal static class TypeSystem
	{
		// Token: 0x06000703 RID: 1795 RVA: 0x0001CD00 File Offset: 0x0001AF00
		static TypeSystem()
		{
			TypeSystem.expressionMethodMap = new Dictionary<MethodInfo, string>(EqualityComparer<MethodInfo>.Default);
			TypeSystem.expressionMethodMap.Add(typeof(string).GetMethod("Contains", new Type[]
			{
				typeof(string)
			}), "substringof");
			TypeSystem.expressionMethodMap.Add(typeof(string).GetMethod("EndsWith", new Type[]
			{
				typeof(string)
			}), "endswith");
			TypeSystem.expressionMethodMap.Add(typeof(string).GetMethod("StartsWith", new Type[]
			{
				typeof(string)
			}), "startswith");
			TypeSystem.expressionMethodMap.Add(typeof(string).GetMethod("IndexOf", new Type[]
			{
				typeof(string)
			}), "indexof");
			TypeSystem.expressionMethodMap.Add(typeof(string).GetMethod("Replace", new Type[]
			{
				typeof(string),
				typeof(string)
			}), "replace");
			TypeSystem.expressionMethodMap.Add(typeof(string).GetMethod("Substring", new Type[]
			{
				typeof(int)
			}), "substring");
			TypeSystem.expressionMethodMap.Add(typeof(string).GetMethod("Substring", new Type[]
			{
				typeof(int),
				typeof(int)
			}), "substring");
			TypeSystem.expressionMethodMap.Add(typeof(string).GetMethod("ToLower", PlatformHelper.EmptyTypes), "tolower");
			TypeSystem.expressionMethodMap.Add(typeof(string).GetMethod("ToUpper", PlatformHelper.EmptyTypes), "toupper");
			TypeSystem.expressionMethodMap.Add(typeof(string).GetMethod("Trim", PlatformHelper.EmptyTypes), "trim");
			TypeSystem.expressionMethodMap.Add(typeof(string).GetMethod("Concat", new Type[]
			{
				typeof(string),
				typeof(string)
			}), "concat");
			TypeSystem.expressionMethodMap.Add(typeof(string).GetProperty("Length", typeof(int)).GetGetMethod(), "length");
			TypeSystem.expressionMethodMap.Add(typeof(DateTime).GetProperty("Day", typeof(int)).GetGetMethod(), "day");
			TypeSystem.expressionMethodMap.Add(typeof(DateTime).GetProperty("Hour", typeof(int)).GetGetMethod(), "hour");
			TypeSystem.expressionMethodMap.Add(typeof(DateTime).GetProperty("Month", typeof(int)).GetGetMethod(), "month");
			TypeSystem.expressionMethodMap.Add(typeof(DateTime).GetProperty("Minute", typeof(int)).GetGetMethod(), "minute");
			TypeSystem.expressionMethodMap.Add(typeof(DateTime).GetProperty("Second", typeof(int)).GetGetMethod(), "second");
			TypeSystem.expressionMethodMap.Add(typeof(DateTime).GetProperty("Year", typeof(int)).GetGetMethod(), "year");
			TypeSystem.expressionMethodMap.Add(typeof(DateTimeOffset).GetProperty("Day", typeof(int)).GetGetMethod(), "day");
			TypeSystem.expressionMethodMap.Add(typeof(DateTimeOffset).GetProperty("Hour", typeof(int)).GetGetMethod(), "hour");
			TypeSystem.expressionMethodMap.Add(typeof(DateTimeOffset).GetProperty("Month", typeof(int)).GetGetMethod(), "month");
			TypeSystem.expressionMethodMap.Add(typeof(DateTimeOffset).GetProperty("Minute", typeof(int)).GetGetMethod(), "minute");
			TypeSystem.expressionMethodMap.Add(typeof(DateTimeOffset).GetProperty("Second", typeof(int)).GetGetMethod(), "second");
			TypeSystem.expressionMethodMap.Add(typeof(DateTimeOffset).GetProperty("Year", typeof(int)).GetGetMethod(), "year");
			TypeSystem.expressionMethodMap.Add(typeof(TimeSpan).GetProperty("Hours", typeof(int)).GetGetMethod(), "hour");
			TypeSystem.expressionMethodMap.Add(typeof(TimeSpan).GetProperty("Minutes", typeof(int)).GetGetMethod(), "minute");
			TypeSystem.expressionMethodMap.Add(typeof(TimeSpan).GetProperty("Seconds", typeof(int)).GetGetMethod(), "second");
			TypeSystem.expressionMethodMap.Add(typeof(Math).GetMethod("Round", new Type[]
			{
				typeof(double)
			}), "round");
			TypeSystem.expressionMethodMap.Add(typeof(Math).GetMethod("Round", new Type[]
			{
				typeof(decimal)
			}), "round");
			TypeSystem.expressionMethodMap.Add(typeof(Math).GetMethod("Floor", new Type[]
			{
				typeof(double)
			}), "floor");
			MethodInfo key = null;
			if (typeof(Math).TryGetMethod("Floor", new Type[]
			{
				typeof(decimal)
			}, out key))
			{
				TypeSystem.expressionMethodMap.Add(key, "floor");
			}
			TypeSystem.expressionMethodMap.Add(typeof(Math).GetMethod("Ceiling", new Type[]
			{
				typeof(double)
			}), "ceiling");
			if (typeof(Math).TryGetMethod("Ceiling", new Type[]
			{
				typeof(decimal)
			}, out key))
			{
				TypeSystem.expressionMethodMap.Add(key, "ceiling");
			}
			TypeSystem.expressionMethodMap.Add(typeof(GeographyOperationsExtensions).GetMethod("Distance", new Type[]
			{
				typeof(GeographyPoint),
				typeof(GeographyPoint)
			}, true, true), "geo.distance");
			TypeSystem.expressionMethodMap.Add(typeof(GeometryOperationsExtensions).GetMethod("Distance", new Type[]
			{
				typeof(GeometryPoint),
				typeof(GeometryPoint)
			}, true, true), "geo.distance");
			TypeSystem.expressionVBMethodMap = new Dictionary<string, string>(StringComparer.Ordinal);
			TypeSystem.expressionVBMethodMap.Add("Microsoft.VisualBasic.Strings.Trim", "trim");
			TypeSystem.expressionVBMethodMap.Add("Microsoft.VisualBasic.Strings.Len", "length");
			TypeSystem.expressionVBMethodMap.Add("Microsoft.VisualBasic.Strings.Mid", "substring");
			TypeSystem.expressionVBMethodMap.Add("Microsoft.VisualBasic.Strings.UCase", "toupper");
			TypeSystem.expressionVBMethodMap.Add("Microsoft.VisualBasic.Strings.LCase", "tolower");
			TypeSystem.expressionVBMethodMap.Add("Microsoft.VisualBasic.DateAndTime.Year", "year");
			TypeSystem.expressionVBMethodMap.Add("Microsoft.VisualBasic.DateAndTime.Month", "month");
			TypeSystem.expressionVBMethodMap.Add("Microsoft.VisualBasic.DateAndTime.Day", "day");
			TypeSystem.expressionVBMethodMap.Add("Microsoft.VisualBasic.DateAndTime.Hour", "hour");
			TypeSystem.expressionVBMethodMap.Add("Microsoft.VisualBasic.DateAndTime.Minute", "minute");
			TypeSystem.expressionVBMethodMap.Add("Microsoft.VisualBasic.DateAndTime.Second", "second");
			TypeSystem.propertiesAsMethodsMap = new Dictionary<PropertyInfo, MethodInfo>(EqualityComparer<PropertyInfo>.Default);
			TypeSystem.propertiesAsMethodsMap.Add(typeof(string).GetProperty("Length", typeof(int)), typeof(string).GetProperty("Length", typeof(int)).GetGetMethod());
			TypeSystem.propertiesAsMethodsMap.Add(typeof(DateTime).GetProperty("Day", typeof(int)), typeof(DateTime).GetProperty("Day", typeof(int)).GetGetMethod());
			TypeSystem.propertiesAsMethodsMap.Add(typeof(DateTime).GetProperty("Hour", typeof(int)), typeof(DateTime).GetProperty("Hour", typeof(int)).GetGetMethod());
			TypeSystem.propertiesAsMethodsMap.Add(typeof(DateTime).GetProperty("Minute", typeof(int)), typeof(DateTime).GetProperty("Minute", typeof(int)).GetGetMethod());
			TypeSystem.propertiesAsMethodsMap.Add(typeof(DateTime).GetProperty("Second", typeof(int)), typeof(DateTime).GetProperty("Second", typeof(int)).GetGetMethod());
			TypeSystem.propertiesAsMethodsMap.Add(typeof(DateTime).GetProperty("Month", typeof(int)), typeof(DateTime).GetProperty("Month", typeof(int)).GetGetMethod());
			TypeSystem.propertiesAsMethodsMap.Add(typeof(DateTime).GetProperty("Year", typeof(int)), typeof(DateTime).GetProperty("Year", typeof(int)).GetGetMethod());
			TypeSystem.propertiesAsMethodsMap.Add(typeof(DateTimeOffset).GetProperty("Day", typeof(int)), typeof(DateTimeOffset).GetProperty("Day", typeof(int)).GetGetMethod());
			TypeSystem.propertiesAsMethodsMap.Add(typeof(DateTimeOffset).GetProperty("Hour", typeof(int)), typeof(DateTimeOffset).GetProperty("Hour", typeof(int)).GetGetMethod());
			TypeSystem.propertiesAsMethodsMap.Add(typeof(DateTimeOffset).GetProperty("Minute", typeof(int)), typeof(DateTimeOffset).GetProperty("Minute", typeof(int)).GetGetMethod());
			TypeSystem.propertiesAsMethodsMap.Add(typeof(DateTimeOffset).GetProperty("Second", typeof(int)), typeof(DateTimeOffset).GetProperty("Second", typeof(int)).GetGetMethod());
			TypeSystem.propertiesAsMethodsMap.Add(typeof(DateTimeOffset).GetProperty("Month", typeof(int)), typeof(DateTimeOffset).GetProperty("Month", typeof(int)).GetGetMethod());
			TypeSystem.propertiesAsMethodsMap.Add(typeof(DateTimeOffset).GetProperty("Year", typeof(int)), typeof(DateTimeOffset).GetProperty("Year", typeof(int)).GetGetMethod());
			TypeSystem.propertiesAsMethodsMap.Add(typeof(TimeSpan).GetProperty("Hours", typeof(int)), typeof(TimeSpan).GetProperty("Hours", typeof(int)).GetGetMethod());
			TypeSystem.propertiesAsMethodsMap.Add(typeof(TimeSpan).GetProperty("Minutes", typeof(int)), typeof(TimeSpan).GetProperty("Minutes", typeof(int)).GetGetMethod());
			TypeSystem.propertiesAsMethodsMap.Add(typeof(TimeSpan).GetProperty("Seconds", typeof(int)), typeof(TimeSpan).GetProperty("Seconds", typeof(int)).GetGetMethod());
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0001DA6C File Offset: 0x0001BC6C
		internal static bool TryGetQueryOptionMethod(MethodInfo mi, out string methodName)
		{
			return TypeSystem.expressionMethodMap.TryGetValue(mi, out methodName) || (TypeSystem.IsVisualBasicAssembly(mi.DeclaringType.GetAssembly()) && TypeSystem.expressionVBMethodMap.TryGetValue(mi.DeclaringType.FullName + "." + mi.Name, out methodName));
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0001DAC3 File Offset: 0x0001BCC3
		internal static bool TryGetPropertyAsMethod(PropertyInfo pi, out MethodInfo mi)
		{
			return TypeSystem.propertiesAsMethodsMap.TryGetValue(pi, out mi);
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0001DAD4 File Offset: 0x0001BCD4
		internal static Type GetElementType(Type seqType)
		{
			Type type = TypeSystem.FindIEnumerable(seqType);
			if (type == null)
			{
				return seqType;
			}
			return type.GetGenericArguments()[0];
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0001DAFC File Offset: 0x0001BCFC
		internal static bool IsPrivate(PropertyInfo pi)
		{
			MethodInfo methodInfo = pi.GetGetMethod() ?? pi.GetSetMethod();
			return !(methodInfo != null) || methodInfo.IsPrivate;
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0001DB2C File Offset: 0x0001BD2C
		internal static Type FindIEnumerable(Type seqType)
		{
			if (seqType == null || seqType == typeof(string) || seqType.IsPrimitive() || seqType.IsValueType() || Nullable.GetUnderlyingType(seqType) != null)
			{
				return null;
			}
			Type type;
			lock (TypeSystem.ienumerableElementTypeCache)
			{
				if (!TypeSystem.ienumerableElementTypeCache.TryGetValue(seqType, out type))
				{
					type = TypeSystem.FindIEnumerableForNonPrimitiveType(seqType);
					TypeSystem.ienumerableElementTypeCache.Add(seqType, type);
				}
			}
			return type;
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0001DBC4 File Offset: 0x0001BDC4
		private static Type FindIEnumerableForNonPrimitiveType(Type seqType)
		{
			if (seqType.IsArray)
			{
				return typeof(IEnumerable<>).MakeGenericType(new Type[]
				{
					seqType.GetElementType()
				});
			}
			if (seqType.IsGenericType())
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
			IEnumerable<Type> interfaces = seqType.GetInterfaces();
			if (interfaces != null)
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
			if (seqType.GetBaseType() != null && seqType.GetBaseType() != typeof(object))
			{
				return TypeSystem.FindIEnumerable(seqType.GetBaseType());
			}
			return null;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0001DCE8 File Offset: 0x0001BEE8
		private static bool IsVisualBasicAssembly(Assembly assembly)
		{
			string fullName = assembly.FullName;
			return fullName.Contains("Microsoft.VisualBasic,") && assembly.FullName.Contains("PublicKeyToken=b03f5f7f11d50a3a");
		}

		// Token: 0x04000443 RID: 1091
		private const string OfficialSilverLightPublicKeyToken = "31bf3856ad364e35";

		// Token: 0x04000444 RID: 1092
		private const string OfficialDesktopPublicKeyToken = "b03f5f7f11d50a3a";

		// Token: 0x04000445 RID: 1093
		private const string VisualBasicAssemblyName = "Microsoft.VisualBasic,";

		// Token: 0x04000446 RID: 1094
		private const string VisualBasicAssemblyPublicKeyToken = "PublicKeyToken=b03f5f7f11d50a3a";

		// Token: 0x04000447 RID: 1095
		private static readonly Dictionary<MethodInfo, string> expressionMethodMap;

		// Token: 0x04000448 RID: 1096
		private static readonly Dictionary<string, string> expressionVBMethodMap;

		// Token: 0x04000449 RID: 1097
		private static readonly Dictionary<PropertyInfo, MethodInfo> propertiesAsMethodsMap;

		// Token: 0x0400044A RID: 1098
		private static readonly Dictionary<Type, Type> ienumerableElementTypeCache = new Dictionary<Type, Type>(EqualityComparer<Type>.Default);
	}
}
