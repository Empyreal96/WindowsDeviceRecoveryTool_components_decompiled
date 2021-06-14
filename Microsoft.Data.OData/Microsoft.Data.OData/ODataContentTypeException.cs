using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Microsoft.Data.OData
{
	// Token: 0x020001E6 RID: 486
	[DebuggerDisplay("{Message}")]
	[Serializable]
	public class ODataContentTypeException : ODataException
	{
		// Token: 0x06000EFB RID: 3835 RVA: 0x000352E1 File Offset: 0x000334E1
		public ODataContentTypeException() : this(Strings.ODataException_GeneralError)
		{
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x000352EE File Offset: 0x000334EE
		public ODataContentTypeException(string message) : this(message, null)
		{
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x000352F8 File Offset: 0x000334F8
		public ODataContentTypeException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x00035302 File Offset: 0x00033502
		protected ODataContentTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
