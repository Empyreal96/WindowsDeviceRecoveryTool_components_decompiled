using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000008 RID: 8
	internal static class CommonUtil
	{
		// Token: 0x06000025 RID: 37 RVA: 0x0000278C File Offset: 0x0000098C
		internal static bool IsCatchableExceptionType(Exception e)
		{
			if (e == null)
			{
				return true;
			}
			Type type = e.GetType();
			return type != CommonUtil.ThreadAbortType && type != CommonUtil.StackOverflowType && type != CommonUtil.OutOfMemoryType;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000027E4 File Offset: 0x000009E4
		internal static bool IsUnsupportedType(Type type)
		{
			if (type.IsGenericType())
			{
				type = type.GetGenericTypeDefinition();
			}
			return CommonUtil.unsupportedTypes.Any((Type t) => t.IsAssignableFrom(type));
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002838 File Offset: 0x00000A38
		internal static string GetCollectionItemTypeName(string typeName, bool isNested)
		{
			if (typeName == null || !typeName.StartsWith("Collection(", StringComparison.Ordinal) || typeName[typeName.Length - 1] != ')' || typeName.Length == "Collection()".Length)
			{
				return null;
			}
			if (isNested)
			{
				throw Error.InvalidOperation(Strings.ClientType_CollectionOfCollectionNotSupported);
			}
			string text = typeName.Substring("Collection(".Length, typeName.Length - "Collection()".Length);
			CommonUtil.GetCollectionItemTypeName(text, true);
			return text;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000028B8 File Offset: 0x00000AB8
		internal static ODataVersion ConvertToODataVersion(DataServiceProtocolVersion maxProtocolVersion)
		{
			switch (maxProtocolVersion)
			{
			case DataServiceProtocolVersion.V1:
				return ODataVersion.V1;
			case DataServiceProtocolVersion.V2:
				return ODataVersion.V2;
			case DataServiceProtocolVersion.V3:
				return ODataVersion.V3;
			default:
				return (ODataVersion)(-1);
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000028E2 File Offset: 0x00000AE2
		internal static ODataVersion ConvertToODataVersion(Version version)
		{
			if (version.Major == 1 && version.Minor == 0)
			{
				return ODataVersion.V1;
			}
			if (version.Major == 2 && version.Minor == 0)
			{
				return ODataVersion.V2;
			}
			return ODataVersion.V3;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000290C File Offset: 0x00000B0C
		internal static string GetModelTypeName(Type type)
		{
			if (type.IsGenericType())
			{
				Type[] genericArguments = type.GetGenericArguments();
				StringBuilder stringBuilder = new StringBuilder(type.Name.Length * 2 * (1 + genericArguments.Length));
				if (type.IsNested)
				{
					stringBuilder.Append(CommonUtil.GetModelTypeName(type.DeclaringType));
					stringBuilder.Append('_');
				}
				stringBuilder.Append(type.Name);
				stringBuilder.Append('[');
				for (int i = 0; i < genericArguments.Length; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(' ');
					}
					if (genericArguments[i].IsGenericParameter)
					{
						stringBuilder.Append(genericArguments[i].Name);
					}
					else
					{
						string modelTypeNamespace = CommonUtil.GetModelTypeNamespace(genericArguments[i]);
						if (!string.IsNullOrEmpty(modelTypeNamespace))
						{
							stringBuilder.Append(modelTypeNamespace);
							stringBuilder.Append('.');
						}
						stringBuilder.Append(CommonUtil.GetModelTypeName(genericArguments[i]));
					}
				}
				stringBuilder.Append(']');
				return stringBuilder.ToString();
			}
			if (type.IsNested)
			{
				return CommonUtil.GetModelTypeName(type.DeclaringType) + "_" + type.Name;
			}
			return type.Name;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A1E File Offset: 0x00000C1E
		internal static string GetModelTypeNamespace(Type type)
		{
			return type.Namespace ?? string.Empty;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002A30 File Offset: 0x00000C30
		internal static bool TryReadVersion(string text, out KeyValuePair<Version, string> result)
		{
			int num = text.IndexOf(';');
			string text2;
			string value;
			if (num >= 0)
			{
				text2 = text.Substring(0, num);
				value = text.Substring(num + 1).Trim();
			}
			else
			{
				text2 = text;
				value = null;
			}
			result = default(KeyValuePair<Version, string>);
			text2 = text2.Trim();
			bool flag = false;
			for (int i = 0; i < text2.Length; i++)
			{
				if (text2[i] == '.')
				{
					if (flag)
					{
						return false;
					}
					flag = true;
				}
				else if (text2[i] < '0' || text2[i] > '9')
				{
					return false;
				}
			}
			bool result2;
			try
			{
				result = new KeyValuePair<Version, string>(new Version(text2), value);
				result2 = true;
			}
			catch (Exception ex)
			{
				if (!CommonUtil.IsCatchableExceptionType(ex) || (!(ex is FormatException) && !(ex is OverflowException) && !(ex is ArgumentException)))
				{
					throw;
				}
				result2 = false;
			}
			return result2;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002B14 File Offset: 0x00000D14
		internal static void SetDefaultMessageQuotas(ODataMessageQuotas messageQuotas)
		{
			messageQuotas.MaxReceivedMessageSize = long.MaxValue;
			messageQuotas.MaxPartsPerBatch = int.MaxValue;
			messageQuotas.MaxOperationsPerChangeset = int.MaxValue;
			messageQuotas.MaxNestingDepth = int.MaxValue;
			messageQuotas.MaxEntityPropertyMappingsPerType = int.MaxValue;
		}

		// Token: 0x04000005 RID: 5
		private static readonly Type OutOfMemoryType = typeof(OutOfMemoryException);

		// Token: 0x04000006 RID: 6
		private static readonly Type StackOverflowType = typeof(StackOverflowException);

		// Token: 0x04000007 RID: 7
		private static readonly Type ThreadAbortType = typeof(ThreadAbortException);

		// Token: 0x04000008 RID: 8
		private static readonly Type[] unsupportedTypes = new Type[]
		{
			typeof(IDynamicMetaObjectProvider),
			typeof(Tuple<>),
			typeof(Tuple<, >),
			typeof(Tuple<, , >),
			typeof(Tuple<, , , >),
			typeof(Tuple<, , , , >),
			typeof(Tuple<, , , , , >),
			typeof(Tuple<, , , , , , >),
			typeof(Tuple<, , , , , , , >)
		};
	}
}
