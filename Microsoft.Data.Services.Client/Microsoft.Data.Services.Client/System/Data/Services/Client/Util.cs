using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x02000129 RID: 297
	internal static class Util
	{
		// Token: 0x060009E0 RID: 2528 RVA: 0x00028368 File Offset: 0x00026568
		internal static Version GetVersionFromMaxProtocolVersion(DataServiceProtocolVersion maxProtocolVersion)
		{
			switch (maxProtocolVersion)
			{
			case DataServiceProtocolVersion.V1:
				return Util.DataServiceVersion1;
			case DataServiceProtocolVersion.V2:
				return Util.DataServiceVersion2;
			case DataServiceProtocolVersion.V3:
				return Util.DataServiceVersion3;
			default:
				return Util.DataServiceVersion2;
			}
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x000283A2 File Offset: 0x000265A2
		[Conditional("DEBUG")]
		internal static void DebugInjectFault(string state)
		{
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x000283A4 File Offset: 0x000265A4
		internal static T CheckArgumentNull<T>([Util.ValidatedNotNullAttribute] T value, string parameterName) where T : class
		{
			if (value == null)
			{
				throw Error.ArgumentNull(parameterName);
			}
			return value;
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x000283B6 File Offset: 0x000265B6
		internal static void CheckArgumentNullAndEmpty([Util.ValidatedNotNullAttribute] string value, string parameterName)
		{
			Util.CheckArgumentNull<string>(value, parameterName);
			Util.CheckArgumentNotEmpty(value, parameterName);
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x000283C7 File Offset: 0x000265C7
		internal static void CheckArgumentNotEmpty(string value, string parameterName)
		{
			if (value != null && value.Length == 0)
			{
				throw Error.Argument(Strings.Util_EmptyString, parameterName);
			}
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x000283E0 File Offset: 0x000265E0
		internal static void CheckArgumentNotEmpty<T>(T[] value, string parameterName) where T : class
		{
			Util.CheckArgumentNull<T[]>(value, parameterName);
			if (value.Length == 0)
			{
				throw Error.Argument(Strings.Util_EmptyArray, parameterName);
			}
			for (int i = 0; i < value.Length; i++)
			{
				if (object.ReferenceEquals(value[i], null))
				{
					throw Error.Argument(Strings.Util_NullArrayElement, parameterName);
				}
			}
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x00028434 File Offset: 0x00026634
		internal static MergeOption CheckEnumerationValue(MergeOption value, string parameterName)
		{
			switch (value)
			{
			case MergeOption.AppendOnly:
			case MergeOption.OverwriteChanges:
			case MergeOption.PreserveChanges:
			case MergeOption.NoTracking:
				return value;
			default:
				throw Error.ArgumentOutOfRange(parameterName);
			}
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00028464 File Offset: 0x00026664
		internal static DataServiceProtocolVersion CheckEnumerationValue(DataServiceProtocolVersion value, string parameterName)
		{
			switch (value)
			{
			case DataServiceProtocolVersion.V1:
			case DataServiceProtocolVersion.V2:
			case DataServiceProtocolVersion.V3:
				return value;
			default:
				throw Error.ArgumentOutOfRange(parameterName);
			}
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x00028490 File Offset: 0x00026690
		internal static HttpStack CheckEnumerationValue(HttpStack value, string parameterName)
		{
			if (value == HttpStack.Auto)
			{
				return value;
			}
			throw Error.ArgumentOutOfRange(parameterName);
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x000284AC File Offset: 0x000266AC
		internal static char[] GetWhitespaceForTracing(int depth)
		{
			char[] array = Util.whitespaceForTracing;
			while (array.Length <= depth)
			{
				char[] array2 = new char[2 * array.Length];
				array2[0] = '\r';
				array2[1] = '\n';
				for (int i = 2; i < array2.Length; i++)
				{
					array2[i] = ' ';
				}
				Interlocked.CompareExchange<char[]>(ref Util.whitespaceForTracing, array2, array);
				array = array2;
			}
			return array;
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x000284FF File Offset: 0x000266FF
		internal static void Dispose<T>(ref T disposable) where T : class, IDisposable
		{
			Util.Dispose<T>(disposable);
			disposable = default(T);
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x00028513 File Offset: 0x00026713
		internal static void Dispose<T>(T disposable) where T : class, IDisposable
		{
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0002852A File Offset: 0x0002672A
		internal static bool IsKnownClientExcption(Exception ex)
		{
			return ex is DataServiceClientException || ex is DataServiceQueryException || ex is DataServiceRequestException;
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00028547 File Offset: 0x00026747
		internal static T NullCheck<T>(T value, InternalError errorcode) where T : class
		{
			if (object.ReferenceEquals(value, null))
			{
				Error.ThrowInternalError(errorcode);
			}
			return value;
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00028560 File Offset: 0x00026760
		internal static bool DoesNullAttributeSayTrue(XmlReader reader)
		{
			string attribute = reader.GetAttribute("null", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
			return attribute != null && XmlConvert.ToBoolean(attribute);
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0002858C File Offset: 0x0002678C
		internal static void SetNextLinkForCollection(object collection, DataServiceQueryContinuation continuation)
		{
			foreach (PropertyInfo propertyInfo in collection.GetType().GetPublicProperties(true))
			{
				if (!(propertyInfo.Name != "Continuation") && propertyInfo.CanWrite && typeof(DataServiceQueryContinuation).IsAssignableFrom(propertyInfo.PropertyType))
				{
					propertyInfo.SetValue(collection, continuation, null);
				}
			}
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00028614 File Offset: 0x00026814
		internal static bool IsNullableType(Type t)
		{
			return t.IsClass() || Nullable.GetUnderlyingType(t) != null;
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x00028631 File Offset: 0x00026831
		internal static object ActivatorCreateInstance(Type type, params object[] arguments)
		{
			if (arguments.Length == 0)
			{
				return Activator.CreateInstance(type);
			}
			return Activator.CreateInstance(type, arguments);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00028646 File Offset: 0x00026846
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		internal static object ConstructorInvoke(ConstructorInfo constructor, object[] arguments)
		{
			if (constructor == null)
			{
				throw new MissingMethodException();
			}
			return constructor.Invoke(arguments);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0002865E File Offset: 0x0002685E
		internal static bool IsFlagSet(SaveChangesOptions options, SaveChangesOptions flag)
		{
			return (options & flag) == flag;
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x00028666 File Offset: 0x00026866
		internal static bool IsBatch(SaveChangesOptions options)
		{
			return Util.IsBatchWithSingleChangeset(options) || Util.IsBatchWithIndependentOperations(options);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00028678 File Offset: 0x00026878
		internal static bool IsBatchWithSingleChangeset(SaveChangesOptions options)
		{
			return Util.IsFlagSet(options, SaveChangesOptions.Batch);
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x00028686 File Offset: 0x00026886
		internal static bool IsBatchWithIndependentOperations(SaveChangesOptions options)
		{
			return Util.IsFlagSet(options, SaveChangesOptions.BatchWithIndependentOperations);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x00028695 File Offset: 0x00026895
		internal static bool IncludeLinkState(EntityStates x)
		{
			return EntityStates.Modified == x || EntityStates.Unchanged == x;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x000286A4 File Offset: 0x000268A4
		[Conditional("TRACE")]
		internal static void TraceElement(XmlReader reader, TextWriter writer)
		{
			if (writer != null)
			{
				writer.Write(Util.GetWhitespaceForTracing(2 + reader.Depth), 0, 2 + reader.Depth);
				writer.Write("<{0}", reader.Name);
				if (reader.MoveToFirstAttribute())
				{
					do
					{
						writer.Write(" {0}=\"{1}\"", reader.Name, reader.Value);
					}
					while (reader.MoveToNextAttribute());
					reader.MoveToElement();
				}
				writer.Write(reader.IsEmptyElement ? " />" : ">");
			}
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x00028729 File Offset: 0x00026929
		[Conditional("TRACE")]
		internal static void TraceEndElement(XmlReader reader, TextWriter writer, bool indent)
		{
			if (writer != null)
			{
				if (indent)
				{
					writer.Write(Util.GetWhitespaceForTracing(2 + reader.Depth), 0, 2 + reader.Depth);
				}
				writer.Write("</{0}>", reader.Name);
			}
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0002875E File Offset: 0x0002695E
		[Conditional("TRACE")]
		internal static void TraceText(TextWriter writer, string value)
		{
			if (writer != null)
			{
				writer.Write(value);
			}
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0002876C File Offset: 0x0002696C
		internal static IEnumerable<T> GetEnumerable<T>(IEnumerable enumerable, Func<object, T> valueConverter)
		{
			List<T> list = new List<T>();
			foreach (object arg in enumerable)
			{
				list.Add(valueConverter(arg));
			}
			return list;
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x000287C8 File Offset: 0x000269C8
		internal static Version ToVersion(this DataServiceProtocolVersion protocolVersion)
		{
			switch (protocolVersion)
			{
			case DataServiceProtocolVersion.V1:
				return Util.DataServiceVersion1;
			case DataServiceProtocolVersion.V2:
				return Util.DataServiceVersion2;
			default:
				return Util.DataServiceVersion3;
			}
		}

		// Token: 0x040005AC RID: 1452
		internal const string VersionSuffix = ";NetFx";

		// Token: 0x040005AD RID: 1453
		internal const string CodeGeneratorToolName = "System.Data.Services.Design";

		// Token: 0x040005AE RID: 1454
		internal const string LoadPropertyMethodName = "LoadProperty";

		// Token: 0x040005AF RID: 1455
		internal const string ExecuteMethodName = "Execute";

		// Token: 0x040005B0 RID: 1456
		internal const string ExecuteMethodNameForVoidResults = "ExecuteVoid";

		// Token: 0x040005B1 RID: 1457
		internal const string SaveChangesMethodName = "SaveChanges";

		// Token: 0x040005B2 RID: 1458
		internal static readonly Version DataServiceVersionEmpty = new Version(0, 0);

		// Token: 0x040005B3 RID: 1459
		internal static readonly Version DataServiceVersion1 = new Version(1, 0);

		// Token: 0x040005B4 RID: 1460
		internal static readonly Version DataServiceVersion2 = new Version(2, 0);

		// Token: 0x040005B5 RID: 1461
		internal static readonly Version DataServiceVersion3 = new Version(3, 0);

		// Token: 0x040005B6 RID: 1462
		internal static readonly Version[] SupportedResponseVersions = new Version[]
		{
			Util.DataServiceVersion1,
			Util.DataServiceVersion2,
			Util.DataServiceVersion3
		};

		// Token: 0x040005B7 RID: 1463
		private static char[] whitespaceForTracing = new char[]
		{
			'\r',
			'\n',
			' ',
			' ',
			' ',
			' ',
			' '
		};

		// Token: 0x0200012A RID: 298
		private sealed class ValidatedNotNullAttribute : Attribute
		{
		}
	}
}
