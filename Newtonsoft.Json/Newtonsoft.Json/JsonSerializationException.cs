using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x02000053 RID: 83
	[Serializable]
	public class JsonSerializationException : JsonException
	{
		// Token: 0x0600030C RID: 780 RVA: 0x0000B310 File Offset: 0x00009510
		public JsonSerializationException()
		{
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000B318 File Offset: 0x00009518
		public JsonSerializationException(string message) : base(message)
		{
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000B321 File Offset: 0x00009521
		public JsonSerializationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000B32B File Offset: 0x0000952B
		public JsonSerializationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000B335 File Offset: 0x00009535
		internal static JsonSerializationException Create(JsonReader reader, string message)
		{
			return JsonSerializationException.Create(reader, message, null);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000B33F File Offset: 0x0000953F
		internal static JsonSerializationException Create(JsonReader reader, string message, Exception ex)
		{
			return JsonSerializationException.Create(reader as IJsonLineInfo, reader.Path, message, ex);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000B354 File Offset: 0x00009554
		internal static JsonSerializationException Create(IJsonLineInfo lineInfo, string path, string message, Exception ex)
		{
			message = JsonPosition.FormatMessage(lineInfo, path, message);
			return new JsonSerializationException(message, ex);
		}
	}
}
