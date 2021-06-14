using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x0200005C RID: 92
	[Serializable]
	public class JsonWriterException : JsonException
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x00010A6C File Offset: 0x0000EC6C
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x00010A74 File Offset: 0x0000EC74
		public string Path { get; private set; }

		// Token: 0x06000454 RID: 1108 RVA: 0x00010A7D File Offset: 0x0000EC7D
		public JsonWriterException()
		{
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00010A85 File Offset: 0x0000EC85
		public JsonWriterException(string message) : base(message)
		{
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00010A8E File Offset: 0x0000EC8E
		public JsonWriterException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00010A98 File Offset: 0x0000EC98
		public JsonWriterException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00010AA2 File Offset: 0x0000ECA2
		internal JsonWriterException(string message, Exception innerException, string path) : base(message, innerException)
		{
			this.Path = path;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00010AB3 File Offset: 0x0000ECB3
		internal static JsonWriterException Create(JsonWriter writer, string message, Exception ex)
		{
			return JsonWriterException.Create(writer.ContainerPath, message, ex);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00010AC2 File Offset: 0x0000ECC2
		internal static JsonWriterException Create(string path, string message, Exception ex)
		{
			message = JsonPosition.FormatMessage(null, path, message);
			return new JsonWriterException(message, ex, path);
		}
	}
}
