using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000013 RID: 19
	[Serializable]
	public class RequiredParameterAfterOptionalParameterException : ParseException
	{
		// Token: 0x06000089 RID: 137 RVA: 0x000058D4 File Offset: 0x00003AD4
		public RequiredParameterAfterOptionalParameterException() : base("An optional parameter can't be followed by a required one")
		{
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000058E1 File Offset: 0x00003AE1
		public RequiredParameterAfterOptionalParameterException(string message) : base("Program error:" + message)
		{
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000058F4 File Offset: 0x00003AF4
		public RequiredParameterAfterOptionalParameterException(string message, Exception except) : base("Program error:" + message, except)
		{
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00005908 File Offset: 0x00003B08
		protected RequiredParameterAfterOptionalParameterException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
