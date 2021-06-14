using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000012 RID: 18
	[Serializable]
	public class AmbiguousArgumentException : ParseException
	{
		// Token: 0x06000084 RID: 132 RVA: 0x0000584C File Offset: 0x00003A4C
		public AmbiguousArgumentException(string id1, string id2) : base(string.Format(CultureInfo.InvariantCulture, "Defined arguments '{0}' and '{1}' are ambiguous", new object[]
		{
			id1,
			id2
		}))
		{
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00005880 File Offset: 0x00003A80
		public AmbiguousArgumentException(string id1) : base(string.Format(CultureInfo.InvariantCulture, "Defined argument '{0}' is ambiguous", new object[]
		{
			id1
		}))
		{
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000058AE File Offset: 0x00003AAE
		public AmbiguousArgumentException()
		{
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000058B6 File Offset: 0x00003AB6
		public AmbiguousArgumentException(string message, Exception except) : base("Program error:" + message, except)
		{
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000058CA File Offset: 0x00003ACA
		protected AmbiguousArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
