using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000066 RID: 102
	[Serializable]
	public class XsdValidatorException : Exception
	{
		// Token: 0x06000284 RID: 644 RVA: 0x0000BDD3 File Offset: 0x00009FD3
		public XsdValidatorException()
		{
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000BDDB File Offset: 0x00009FDB
		public XsdValidatorException(string message) : base(message)
		{
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000BDE4 File Offset: 0x00009FE4
		public XsdValidatorException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000BDEE File Offset: 0x00009FEE
		protected XsdValidatorException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000BDF8 File Offset: 0x00009FF8
		public override string ToString()
		{
			string text = this.Message;
			if (base.InnerException != null)
			{
				text += base.InnerException.ToString();
			}
			return text;
		}
	}
}
