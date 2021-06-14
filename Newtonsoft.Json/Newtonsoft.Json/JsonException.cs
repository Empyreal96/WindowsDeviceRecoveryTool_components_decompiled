using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x0200004B RID: 75
	[Serializable]
	public class JsonException : Exception
	{
		// Token: 0x060002C8 RID: 712 RVA: 0x0000ACE0 File Offset: 0x00008EE0
		public JsonException()
		{
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000ACE8 File Offset: 0x00008EE8
		public JsonException(string message) : base(message)
		{
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000ACF1 File Offset: 0x00008EF1
		public JsonException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000ACFB File Offset: 0x00008EFB
		public JsonException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000AD05 File Offset: 0x00008F05
		internal static JsonException Create(IJsonLineInfo lineInfo, string path, string message)
		{
			message = JsonPosition.FormatMessage(lineInfo, path, message);
			return new JsonException(message);
		}
	}
}
