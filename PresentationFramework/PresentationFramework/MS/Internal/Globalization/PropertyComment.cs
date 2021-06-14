using System;

namespace MS.Internal.Globalization
{
	// Token: 0x020006B7 RID: 1719
	internal class PropertyComment
	{
		// Token: 0x06006EC9 RID: 28361 RVA: 0x0000326D File Offset: 0x0000146D
		internal PropertyComment()
		{
		}

		// Token: 0x17001A47 RID: 6727
		// (get) Token: 0x06006ECA RID: 28362 RVA: 0x001FDC3E File Offset: 0x001FBE3E
		// (set) Token: 0x06006ECB RID: 28363 RVA: 0x001FDC46 File Offset: 0x001FBE46
		internal string PropertyName
		{
			get
			{
				return this._target;
			}
			set
			{
				this._target = value;
			}
		}

		// Token: 0x17001A48 RID: 6728
		// (get) Token: 0x06006ECC RID: 28364 RVA: 0x001FDC4F File Offset: 0x001FBE4F
		// (set) Token: 0x06006ECD RID: 28365 RVA: 0x001FDC57 File Offset: 0x001FBE57
		internal object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x04003681 RID: 13953
		private string _target;

		// Token: 0x04003682 RID: 13954
		private object _value;
	}
}
