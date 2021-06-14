using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x0200008F RID: 143
	[Serializable]
	public class JsonSchemaException : JsonException
	{
		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x0001C7D1 File Offset: 0x0001A9D1
		// (set) Token: 0x0600076D RID: 1901 RVA: 0x0001C7D9 File Offset: 0x0001A9D9
		public int LineNumber { get; private set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x0001C7E2 File Offset: 0x0001A9E2
		// (set) Token: 0x0600076F RID: 1903 RVA: 0x0001C7EA File Offset: 0x0001A9EA
		public int LinePosition { get; private set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x0001C7F3 File Offset: 0x0001A9F3
		// (set) Token: 0x06000771 RID: 1905 RVA: 0x0001C7FB File Offset: 0x0001A9FB
		public string Path { get; private set; }

		// Token: 0x06000772 RID: 1906 RVA: 0x0001C804 File Offset: 0x0001AA04
		public JsonSchemaException()
		{
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0001C80C File Offset: 0x0001AA0C
		public JsonSchemaException(string message) : base(message)
		{
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0001C815 File Offset: 0x0001AA15
		public JsonSchemaException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0001C81F File Offset: 0x0001AA1F
		public JsonSchemaException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0001C829 File Offset: 0x0001AA29
		internal JsonSchemaException(string message, Exception innerException, string path, int lineNumber, int linePosition) : base(message, innerException)
		{
			this.Path = path;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}
	}
}
