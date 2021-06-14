using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200000E RID: 14
	[Serializable]
	public class ParseException : IUException
	{
		// Token: 0x06000073 RID: 115 RVA: 0x000056F5 File Offset: 0x000038F5
		public ParseException(string message) : base("Program error:" + message)
		{
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00005708 File Offset: 0x00003908
		public ParseException()
		{
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00005710 File Offset: 0x00003910
		public ParseException(string message, Exception except) : base(except, "Program error:" + message)
		{
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00005724 File Offset: 0x00003924
		protected ParseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
