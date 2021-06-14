using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x02000182 RID: 386
	public abstract class EdmType : EdmElement, IEdmType, IEdmElement
	{
		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000883 RID: 2179
		public abstract EdmTypeKind TypeKind { get; }

		// Token: 0x06000884 RID: 2180 RVA: 0x00017ED6 File Offset: 0x000160D6
		public override string ToString()
		{
			return this.ToTraceString();
		}
	}
}
