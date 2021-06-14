using System;
using System.Collections;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004B3 RID: 1203
	internal class GetAttributesEvent : EventArgs
	{
		// Token: 0x0600510F RID: 20751 RVA: 0x0015005D File Offset: 0x0014E25D
		public GetAttributesEvent(ArrayList attrList)
		{
			this.attrList = attrList;
		}

		// Token: 0x06005110 RID: 20752 RVA: 0x0015006C File Offset: 0x0014E26C
		public void Add(Attribute attribute)
		{
			this.attrList.Add(attribute);
		}

		// Token: 0x0400344C RID: 13388
		private ArrayList attrList;
	}
}
