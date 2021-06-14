using System;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000664 RID: 1636
	internal class ManagedFullPropSpec
	{
		// Token: 0x170019E5 RID: 6629
		// (get) Token: 0x06006C68 RID: 27752 RVA: 0x001F3B07 File Offset: 0x001F1D07
		internal Guid Guid
		{
			get
			{
				return this._guid;
			}
		}

		// Token: 0x170019E6 RID: 6630
		// (get) Token: 0x06006C69 RID: 27753 RVA: 0x001F3B0F File Offset: 0x001F1D0F
		internal ManagedPropSpec Property
		{
			get
			{
				return this._property;
			}
		}

		// Token: 0x06006C6A RID: 27754 RVA: 0x001F3B17 File Offset: 0x001F1D17
		internal ManagedFullPropSpec(Guid guid, uint propId)
		{
			this._guid = guid;
			this._property = new ManagedPropSpec(propId);
		}

		// Token: 0x06006C6B RID: 27755 RVA: 0x001F3B32 File Offset: 0x001F1D32
		internal ManagedFullPropSpec(FULLPROPSPEC nativePropSpec)
		{
			this._guid = nativePropSpec.guid;
			this._property = new ManagedPropSpec(nativePropSpec.property);
		}

		// Token: 0x0400353D RID: 13629
		private Guid _guid;

		// Token: 0x0400353E RID: 13630
		private ManagedPropSpec _property;
	}
}
