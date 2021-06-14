using System;
using System.Collections.Generic;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200063D RID: 1597
	internal class PageContext
	{
		// Token: 0x1700197D RID: 6525
		// (get) Token: 0x06006A1A RID: 27162 RVA: 0x001E37B6 File Offset: 0x001E19B6
		// (set) Token: 0x06006A1B RID: 27163 RVA: 0x001E37BE File Offset: 0x001E19BE
		internal PTS.FSRECT PageRect
		{
			get
			{
				return this._pageRect;
			}
			set
			{
				this._pageRect = value;
			}
		}

		// Token: 0x1700197E RID: 6526
		// (get) Token: 0x06006A1C RID: 27164 RVA: 0x001E37C7 File Offset: 0x001E19C7
		internal List<BaseParaClient> FloatingElementList
		{
			get
			{
				return this._floatingElementList;
			}
		}

		// Token: 0x06006A1D RID: 27165 RVA: 0x001E37CF File Offset: 0x001E19CF
		internal void AddFloatingParaClient(BaseParaClient floatingElement)
		{
			if (this._floatingElementList == null)
			{
				this._floatingElementList = new List<BaseParaClient>();
			}
			if (!this._floatingElementList.Contains(floatingElement))
			{
				this._floatingElementList.Add(floatingElement);
			}
		}

		// Token: 0x06006A1E RID: 27166 RVA: 0x001E37FE File Offset: 0x001E19FE
		internal void RemoveFloatingParaClient(BaseParaClient floatingElement)
		{
			if (this._floatingElementList.Contains(floatingElement))
			{
				this._floatingElementList.Remove(floatingElement);
			}
			if (this._floatingElementList.Count == 0)
			{
				this._floatingElementList = null;
			}
		}

		// Token: 0x04003428 RID: 13352
		private List<BaseParaClient> _floatingElementList;

		// Token: 0x04003429 RID: 13353
		private PTS.FSRECT _pageRect;
	}
}
