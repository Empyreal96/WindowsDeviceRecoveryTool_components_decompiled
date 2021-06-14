using System;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200063F RID: 1599
	internal sealed class RowVisual : ContainerVisual
	{
		// Token: 0x06006A2F RID: 27183 RVA: 0x001E3DCC File Offset: 0x001E1FCC
		internal RowVisual(TableRow row)
		{
			this._row = row;
		}

		// Token: 0x17001982 RID: 6530
		// (get) Token: 0x06006A30 RID: 27184 RVA: 0x001E3DDB File Offset: 0x001E1FDB
		internal TableRow Row
		{
			get
			{
				return this._row;
			}
		}

		// Token: 0x0400342C RID: 13356
		private readonly TableRow _row;
	}
}
