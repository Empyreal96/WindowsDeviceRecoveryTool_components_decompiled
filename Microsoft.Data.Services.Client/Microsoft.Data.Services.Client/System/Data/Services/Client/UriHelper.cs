using System;
using System.Data.Services.Client.Metadata;
using System.Data.Services.Common;
using System.Text;

namespace System.Data.Services.Client
{
	// Token: 0x020000DC RID: 220
	internal static class UriHelper
	{
		// Token: 0x0600070B RID: 1803 RVA: 0x0001DD20 File Offset: 0x0001BF20
		internal static string GetTypeNameForUri(Type type, DataServiceContext context)
		{
			type = (Nullable.GetUnderlyingType(type) ?? type);
			PrimitiveType primitiveType;
			if (!PrimitiveType.TryGetPrimitiveType(type, out primitiveType))
			{
				return context.ResolveNameFromType(type) ?? type.FullName;
			}
			if (primitiveType.HasReverseMapping)
			{
				return primitiveType.EdmTypeName;
			}
			throw new NotSupportedException(Strings.ALinq_CantCastToUnsupportedPrimitive(type.Name));
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0001DD78 File Offset: 0x0001BF78
		internal static string GetEntityTypeNameForUriAndValidateMaxProtocolVersion(Type type, DataServiceContext context, ref Version uriVersion)
		{
			if (context.MaxProtocolVersionAsVersion < Util.DataServiceVersion3)
			{
				throw new NotSupportedException(Strings.ALinq_TypeAsNotSupportedForMaxDataServiceVersionLessThan3);
			}
			if (!ClientTypeUtil.TypeOrElementTypeIsEntity(type))
			{
				throw new NotSupportedException(Strings.ALinq_TypeAsArgumentNotEntityType(type.FullName));
			}
			WebUtil.RaiseVersion(ref uriVersion, Util.DataServiceVersion3);
			return context.ResolveNameFromType(type) ?? type.FullName;
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0001DDD8 File Offset: 0x0001BFD8
		internal static void AppendTypeSegment(StringBuilder stringBuilder, Type type, DataServiceContext dataServiceContext, bool inPath, ref Version version)
		{
			if (inPath && dataServiceContext.UrlConventions == DataServiceUrlConventions.KeyAsSegment)
			{
				stringBuilder.Append('$');
				stringBuilder.Append('/');
			}
			string entityTypeNameForUriAndValidateMaxProtocolVersion = UriHelper.GetEntityTypeNameForUriAndValidateMaxProtocolVersion(type, dataServiceContext, ref version);
			stringBuilder.Append(entityTypeNameForUriAndValidateMaxProtocolVersion);
		}

		// Token: 0x0400044B RID: 1099
		internal const char FORWARDSLASH = '/';

		// Token: 0x0400044C RID: 1100
		internal const char LEFTPAREN = '(';

		// Token: 0x0400044D RID: 1101
		internal const char RIGHTPAREN = ')';

		// Token: 0x0400044E RID: 1102
		internal const char QUESTIONMARK = '?';

		// Token: 0x0400044F RID: 1103
		internal const char AMPERSAND = '&';

		// Token: 0x04000450 RID: 1104
		internal const char EQUALSSIGN = '=';

		// Token: 0x04000451 RID: 1105
		internal const char ATSIGN = '@';

		// Token: 0x04000452 RID: 1106
		internal const char DOLLARSIGN = '$';

		// Token: 0x04000453 RID: 1107
		internal const char SPACE = ' ';

		// Token: 0x04000454 RID: 1108
		internal const char COMMA = ',';

		// Token: 0x04000455 RID: 1109
		internal const char COLON = ':';

		// Token: 0x04000456 RID: 1110
		internal const char QUOTE = '\'';

		// Token: 0x04000457 RID: 1111
		internal const char ASTERISK = '*';

		// Token: 0x04000458 RID: 1112
		internal const string OPTIONTOP = "top";

		// Token: 0x04000459 RID: 1113
		internal const string OPTIONSKIP = "skip";

		// Token: 0x0400045A RID: 1114
		internal const string OPTIONORDERBY = "orderby";

		// Token: 0x0400045B RID: 1115
		internal const string OPTIONFILTER = "filter";

		// Token: 0x0400045C RID: 1116
		internal const string OPTIONDESC = "desc";

		// Token: 0x0400045D RID: 1117
		internal const string OPTIONEXPAND = "expand";

		// Token: 0x0400045E RID: 1118
		internal const string OPTIONCOUNT = "inlinecount";

		// Token: 0x0400045F RID: 1119
		internal const string OPTIONSELECT = "select";

		// Token: 0x04000460 RID: 1120
		internal const string OPTIONFORMAT = "format";

		// Token: 0x04000461 RID: 1121
		internal const string COUNTALL = "allpages";

		// Token: 0x04000462 RID: 1122
		internal const string COUNT = "count";

		// Token: 0x04000463 RID: 1123
		internal const string AND = "and";

		// Token: 0x04000464 RID: 1124
		internal const string OR = "or";

		// Token: 0x04000465 RID: 1125
		internal const string EQ = "eq";

		// Token: 0x04000466 RID: 1126
		internal const string NE = "ne";

		// Token: 0x04000467 RID: 1127
		internal const string LT = "lt";

		// Token: 0x04000468 RID: 1128
		internal const string LE = "le";

		// Token: 0x04000469 RID: 1129
		internal const string GT = "gt";

		// Token: 0x0400046A RID: 1130
		internal const string GE = "ge";

		// Token: 0x0400046B RID: 1131
		internal const string ADD = "add";

		// Token: 0x0400046C RID: 1132
		internal const string SUB = "sub";

		// Token: 0x0400046D RID: 1133
		internal const string MUL = "mul";

		// Token: 0x0400046E RID: 1134
		internal const string DIV = "div";

		// Token: 0x0400046F RID: 1135
		internal const string MOD = "mod";

		// Token: 0x04000470 RID: 1136
		internal const string NEGATE = "-";

		// Token: 0x04000471 RID: 1137
		internal const string NOT = "not";

		// Token: 0x04000472 RID: 1138
		internal const string NULL = "null";

		// Token: 0x04000473 RID: 1139
		internal const string ISOF = "isof";

		// Token: 0x04000474 RID: 1140
		internal const string CAST = "cast";
	}
}
