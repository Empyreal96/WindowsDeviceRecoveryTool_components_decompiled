using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x02000052 RID: 82
	[Serializable]
	public class JsonReaderException : JsonException
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002FE RID: 766 RVA: 0x0000B237 File Offset: 0x00009437
		// (set) Token: 0x060002FF RID: 767 RVA: 0x0000B23F File Offset: 0x0000943F
		public int LineNumber { get; private set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0000B248 File Offset: 0x00009448
		// (set) Token: 0x06000301 RID: 769 RVA: 0x0000B250 File Offset: 0x00009450
		public int LinePosition { get; private set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000302 RID: 770 RVA: 0x0000B259 File Offset: 0x00009459
		// (set) Token: 0x06000303 RID: 771 RVA: 0x0000B261 File Offset: 0x00009461
		public string Path { get; private set; }

		// Token: 0x06000304 RID: 772 RVA: 0x0000B26A File Offset: 0x0000946A
		public JsonReaderException()
		{
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000B272 File Offset: 0x00009472
		public JsonReaderException(string message) : base(message)
		{
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000B27B File Offset: 0x0000947B
		public JsonReaderException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000B285 File Offset: 0x00009485
		public JsonReaderException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000B28F File Offset: 0x0000948F
		internal JsonReaderException(string message, Exception innerException, string path, int lineNumber, int linePosition) : base(message, innerException)
		{
			this.Path = path;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000B2B0 File Offset: 0x000094B0
		internal static JsonReaderException Create(JsonReader reader, string message)
		{
			return JsonReaderException.Create(reader, message, null);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000B2BA File Offset: 0x000094BA
		internal static JsonReaderException Create(JsonReader reader, string message, Exception ex)
		{
			return JsonReaderException.Create(reader as IJsonLineInfo, reader.Path, message, ex);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000B2D0 File Offset: 0x000094D0
		internal static JsonReaderException Create(IJsonLineInfo lineInfo, string path, string message, Exception ex)
		{
			message = JsonPosition.FormatMessage(lineInfo, path, message);
			int lineNumber;
			int linePosition;
			if (lineInfo != null && lineInfo.HasLineInfo())
			{
				lineNumber = lineInfo.LineNumber;
				linePosition = lineInfo.LinePosition;
			}
			else
			{
				lineNumber = 0;
				linePosition = 0;
			}
			return new JsonReaderException(message, ex, path, lineNumber, linePosition);
		}
	}
}
