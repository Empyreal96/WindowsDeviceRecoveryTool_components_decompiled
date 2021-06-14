using System;
using System.Runtime.Serialization;

namespace <CrtImplementationDetails>
{
	// Token: 0x02000270 RID: 624
	[Serializable]
	internal class ModuleLoadException : Exception
	{
		// Token: 0x060002B2 RID: 690 RVA: 0x00010028 File Offset: 0x0000F428
		protected ModuleLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00010010 File Offset: 0x0000F410
		public ModuleLoadException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000FFFC File Offset: 0x0000F3FC
		public ModuleLoadException(string message) : base(message)
		{
		}

		// Token: 0x040002B9 RID: 697
		public const string Nested = "A nested exception occurred after the primary exception that caused the C++ module to fail to load.\n";
	}
}
