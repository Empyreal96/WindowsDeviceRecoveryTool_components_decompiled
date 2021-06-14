using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000EC RID: 236
	internal static class JsonTokenUtils
	{
		// Token: 0x06000B38 RID: 2872 RVA: 0x0002D69C File Offset: 0x0002B89C
		internal static bool IsEndToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.EndObject:
			case JsonToken.EndArray:
			case JsonToken.EndConstructor:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x0002D6C8 File Offset: 0x0002B8C8
		internal static bool IsStartToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.StartObject:
			case JsonToken.StartArray:
			case JsonToken.StartConstructor:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0002D6F0 File Offset: 0x0002B8F0
		internal static bool IsPrimitiveToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				return true;
			}
			return false;
		}
	}
}
