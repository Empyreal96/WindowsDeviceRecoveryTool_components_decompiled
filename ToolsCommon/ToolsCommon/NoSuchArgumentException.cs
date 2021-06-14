using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000011 RID: 17
	[Serializable]
	public class NoSuchArgumentException : ParseException
	{
		// Token: 0x0600007F RID: 127 RVA: 0x000057C4 File Offset: 0x000039C4
		public NoSuchArgumentException(string type, string id) : base(string.Format(CultureInfo.InvariantCulture, "The {0} '{1}' was not defined", new object[]
		{
			type,
			id
		}))
		{
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000057F8 File Offset: 0x000039F8
		public NoSuchArgumentException(string id) : base(string.Format(CultureInfo.InvariantCulture, "The '{0}' was not defined", new object[]
		{
			id
		}))
		{
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00005826 File Offset: 0x00003A26
		public NoSuchArgumentException()
		{
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000582E File Offset: 0x00003A2E
		public NoSuchArgumentException(string message, Exception except) : base("Program error:" + message, except)
		{
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00005842 File Offset: 0x00003A42
		protected NoSuchArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
