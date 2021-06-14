using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200000F RID: 15
	[Serializable]
	public class ArgumentAlreadyDeclaredException : ParseException
	{
		// Token: 0x06000077 RID: 119 RVA: 0x00005730 File Offset: 0x00003930
		public ArgumentAlreadyDeclaredException(string id) : base(string.Format(CultureInfo.InvariantCulture, "Argument '{0}' was already defined", new object[]
		{
			id
		}))
		{
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000575E File Offset: 0x0000395E
		public ArgumentAlreadyDeclaredException()
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00005766 File Offset: 0x00003966
		public ArgumentAlreadyDeclaredException(string message, Exception except) : base("Program error:" + message, except)
		{
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000577A File Offset: 0x0000397A
		protected ArgumentAlreadyDeclaredException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
