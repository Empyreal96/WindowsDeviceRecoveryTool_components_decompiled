using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000F5 RID: 245
	internal static class FSharpUtils
	{
		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x0002EA6B File Offset: 0x0002CC6B
		// (set) Token: 0x06000B73 RID: 2931 RVA: 0x0002EA72 File Offset: 0x0002CC72
		public static Assembly FSharpCoreAssembly { get; private set; }

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000B74 RID: 2932 RVA: 0x0002EA7A File Offset: 0x0002CC7A
		// (set) Token: 0x06000B75 RID: 2933 RVA: 0x0002EA81 File Offset: 0x0002CC81
		public static MethodCall<object, object> IsUnion { get; private set; }

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000B76 RID: 2934 RVA: 0x0002EA89 File Offset: 0x0002CC89
		// (set) Token: 0x06000B77 RID: 2935 RVA: 0x0002EA90 File Offset: 0x0002CC90
		public static MethodCall<object, object> GetUnionFields { get; private set; }

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000B78 RID: 2936 RVA: 0x0002EA98 File Offset: 0x0002CC98
		// (set) Token: 0x06000B79 RID: 2937 RVA: 0x0002EA9F File Offset: 0x0002CC9F
		public static MethodCall<object, object> GetUnionCases { get; private set; }

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000B7A RID: 2938 RVA: 0x0002EAA7 File Offset: 0x0002CCA7
		// (set) Token: 0x06000B7B RID: 2939 RVA: 0x0002EAAE File Offset: 0x0002CCAE
		public static MethodCall<object, object> MakeUnion { get; private set; }

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000B7C RID: 2940 RVA: 0x0002EAB6 File Offset: 0x0002CCB6
		// (set) Token: 0x06000B7D RID: 2941 RVA: 0x0002EABD File Offset: 0x0002CCBD
		public static Func<object, object> GetUnionCaseInfoName { get; private set; }

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000B7E RID: 2942 RVA: 0x0002EAC5 File Offset: 0x0002CCC5
		// (set) Token: 0x06000B7F RID: 2943 RVA: 0x0002EACC File Offset: 0x0002CCCC
		public static Func<object, object> GetUnionCaseInfo { get; private set; }

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x0002EAD4 File Offset: 0x0002CCD4
		// (set) Token: 0x06000B81 RID: 2945 RVA: 0x0002EADB File Offset: 0x0002CCDB
		public static Func<object, object> GetUnionCaseFields { get; private set; }

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x0002EAE3 File Offset: 0x0002CCE3
		// (set) Token: 0x06000B83 RID: 2947 RVA: 0x0002EAEA File Offset: 0x0002CCEA
		public static MethodCall<object, object> GetUnionCaseInfoFields { get; private set; }

		// Token: 0x06000B84 RID: 2948 RVA: 0x0002EAF4 File Offset: 0x0002CCF4
		public static void EnsureInitialized(Assembly fsharpCoreAssembly)
		{
			if (!FSharpUtils._initialized)
			{
				lock (FSharpUtils.Lock)
				{
					if (!FSharpUtils._initialized)
					{
						FSharpUtils.FSharpCoreAssembly = fsharpCoreAssembly;
						Type type = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpType");
						MethodInfo method = type.GetMethod("IsUnion", BindingFlags.Static | BindingFlags.Public);
						FSharpUtils.IsUnion = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
						MethodInfo method2 = type.GetMethod("GetUnionCases", BindingFlags.Static | BindingFlags.Public);
						FSharpUtils.GetUnionCases = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method2);
						Type type2 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpValue");
						MethodInfo method3 = type2.GetMethod("GetUnionFields", BindingFlags.Static | BindingFlags.Public);
						FSharpUtils.GetUnionFields = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method3);
						FSharpUtils.GetUnionCaseInfo = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(method3.ReturnType.GetProperty("Item1"));
						FSharpUtils.GetUnionCaseFields = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(method3.ReturnType.GetProperty("Item2"));
						MethodInfo method4 = type2.GetMethod("MakeUnion", BindingFlags.Static | BindingFlags.Public);
						FSharpUtils.MakeUnion = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method4);
						Type type3 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.UnionCaseInfo");
						FSharpUtils.GetUnionCaseInfoName = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("Name"));
						FSharpUtils.GetUnionCaseInfoFields = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(type3.GetMethod("GetFields"));
						Type type4 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.ListModule");
						FSharpUtils._ofSeq = type4.GetMethod("OfSeq");
						FSharpUtils._mapType = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.FSharpMap`2");
						Thread.MemoryBarrier();
						FSharpUtils._initialized = true;
					}
				}
			}
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0002ECA8 File Offset: 0x0002CEA8
		public static ObjectConstructor<object> CreateSeq(Type t)
		{
			MethodInfo method = FSharpUtils._ofSeq.MakeGenericMethod(new Type[]
			{
				t
			});
			return JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(method);
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0002ECD8 File Offset: 0x0002CED8
		public static ObjectConstructor<object> CreateMap(Type keyType, Type valueType)
		{
			MethodInfo method = typeof(FSharpUtils).GetMethod("BuildMapCreator");
			MethodInfo methodInfo = method.MakeGenericMethod(new Type[]
			{
				keyType,
				valueType
			});
			return (ObjectConstructor<object>)methodInfo.Invoke(null, null);
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0002ED90 File Offset: 0x0002CF90
		public static ObjectConstructor<object> BuildMapCreator<TKey, TValue>()
		{
			Type type = FSharpUtils._mapType.MakeGenericType(new Type[]
			{
				typeof(TKey),
				typeof(TValue)
			});
			ConstructorInfo constructor = type.GetConstructor(new Type[]
			{
				typeof(IEnumerable<Tuple<TKey, TValue>>)
			});
			ObjectConstructor<object> ctorDelegate = JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(constructor);
			return delegate(object[] args)
			{
				IEnumerable<KeyValuePair<TKey, TValue>> source = (IEnumerable<KeyValuePair<TKey, TValue>>)args[0];
				IEnumerable<Tuple<TKey, TValue>> enumerable = from kv in source
				select new Tuple<TKey, TValue>(kv.Key, kv.Value);
				return ctorDelegate(new object[]
				{
					enumerable
				});
			};
		}

		// Token: 0x0400041F RID: 1055
		public const string FSharpSetTypeName = "FSharpSet`1";

		// Token: 0x04000420 RID: 1056
		public const string FSharpListTypeName = "FSharpList`1";

		// Token: 0x04000421 RID: 1057
		public const string FSharpMapTypeName = "FSharpMap`2";

		// Token: 0x04000422 RID: 1058
		private static readonly object Lock = new object();

		// Token: 0x04000423 RID: 1059
		private static bool _initialized;

		// Token: 0x04000424 RID: 1060
		private static MethodInfo _ofSeq;

		// Token: 0x04000425 RID: 1061
		private static Type _mapType;
	}
}
