using System;
using System.Runtime.Serialization;

namespace <CrtImplementationDetails>
{
	// Token: 0x0200009D RID: 157
	[Serializable]
	internal class ModuleLoadException : Exception
	{
		// Token: 0x06000155 RID: 341 RVA: 0x0001721C File Offset: 0x0001661C
		protected ModuleLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00017200 File Offset: 0x00016600
		public ModuleLoadException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000171E4 File Offset: 0x000165E4
		public ModuleLoadException(string message) : base(message)
		{
		}

		// Token: 0x04000133 RID: 307
		public const string Nested = "A nested exception occurred after the primary exception that caused the C++ module to fail to load.\n";
	}
}
