using System;

namespace System.Data.Services.Client
{
	// Token: 0x02000103 RID: 259
	public sealed class ChangeOperationResponse : OperationResponse
	{
		// Token: 0x06000866 RID: 2150 RVA: 0x000232AA File Offset: 0x000214AA
		internal ChangeOperationResponse(HeaderCollection headers, Descriptor descriptor) : base(headers)
		{
			this.descriptor = descriptor;
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x000232BA File Offset: 0x000214BA
		public Descriptor Descriptor
		{
			get
			{
				return this.descriptor;
			}
		}

		// Token: 0x040004F8 RID: 1272
		private Descriptor descriptor;
	}
}
