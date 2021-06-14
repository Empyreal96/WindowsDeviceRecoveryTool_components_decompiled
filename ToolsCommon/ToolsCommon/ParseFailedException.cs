using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000015 RID: 21
	[Serializable]
	public class ParseFailedException : ParseException
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00005941 File Offset: 0x00003B41
		public ParseFailedException(string message) : base(message)
		{
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000594A File Offset: 0x00003B4A
		public ParseFailedException()
		{
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00005952 File Offset: 0x00003B52
		public ParseFailedException(string message, Exception except) : base("Program error:" + message, except)
		{
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00005966 File Offset: 0x00003B66
		protected ParseFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
