using System;
using System.Collections.Generic;
using System.Windows;

namespace MS.Internal.Ink
{
	// Token: 0x02000686 RID: 1670
	internal abstract class ElementsClipboardData : ClipboardData
	{
		// Token: 0x06006D62 RID: 28002 RVA: 0x001F68DE File Offset: 0x001F4ADE
		internal ElementsClipboardData()
		{
		}

		// Token: 0x06006D63 RID: 28003 RVA: 0x001F68E6 File Offset: 0x001F4AE6
		internal ElementsClipboardData(UIElement[] elements)
		{
			if (elements != null)
			{
				this.ElementList = new List<UIElement>(elements);
			}
		}

		// Token: 0x17001A18 RID: 6680
		// (get) Token: 0x06006D64 RID: 28004 RVA: 0x001F68FD File Offset: 0x001F4AFD
		internal List<UIElement> Elements
		{
			get
			{
				if (this.ElementList != null)
				{
					return this._elementList;
				}
				return new List<UIElement>();
			}
		}

		// Token: 0x17001A19 RID: 6681
		// (get) Token: 0x06006D65 RID: 28005 RVA: 0x001F6913 File Offset: 0x001F4B13
		// (set) Token: 0x06006D66 RID: 28006 RVA: 0x001F691B File Offset: 0x001F4B1B
		protected List<UIElement> ElementList
		{
			get
			{
				return this._elementList;
			}
			set
			{
				this._elementList = value;
			}
		}

		// Token: 0x040035E5 RID: 13797
		private List<UIElement> _elementList;
	}
}
