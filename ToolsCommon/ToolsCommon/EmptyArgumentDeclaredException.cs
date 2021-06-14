using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000010 RID: 16
	[Serializable]
	public class EmptyArgumentDeclaredException : ParseException
	{
		// Token: 0x0600007B RID: 123 RVA: 0x00005784 File Offset: 0x00003984
		public EmptyArgumentDeclaredException() : base("You cannot define an argument with ID: \"\"")
		{
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00005791 File Offset: 0x00003991
		public EmptyArgumentDeclaredException(string message) : base("You cannot define an argument with ID: " + message)
		{
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000057A4 File Offset: 0x000039A4
		public EmptyArgumentDeclaredException(string message, Exception except) : base("Program error:" + message, except)
		{
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000057B8 File Offset: 0x000039B8
		protected EmptyArgumentDeclaredException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
