using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000014 RID: 20
	[Serializable]
	public class BadGroupException : ParseException
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00005912 File Offset: 0x00003B12
		public BadGroupException(string message) : base(message)
		{
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000591B File Offset: 0x00003B1B
		public BadGroupException()
		{
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00005923 File Offset: 0x00003B23
		public BadGroupException(string message, Exception except) : base("Program error:" + message, except)
		{
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005937 File Offset: 0x00003B37
		protected BadGroupException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
