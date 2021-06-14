using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Microsoft.Data.OData
{
	// Token: 0x0200004F RID: 79
	[DebuggerDisplay("{Message}")]
	[Serializable]
	public class ODataException : InvalidOperationException
	{
		// Token: 0x06000217 RID: 535 RVA: 0x000081AD File Offset: 0x000063AD
		public ODataException() : this(Strings.ODataException_GeneralError)
		{
		}

		// Token: 0x06000218 RID: 536 RVA: 0x000081BA File Offset: 0x000063BA
		public ODataException(string message) : this(message, null)
		{
		}

		// Token: 0x06000219 RID: 537 RVA: 0x000081C4 File Offset: 0x000063C4
		public ODataException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000081CE File Offset: 0x000063CE
		protected ODataException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
