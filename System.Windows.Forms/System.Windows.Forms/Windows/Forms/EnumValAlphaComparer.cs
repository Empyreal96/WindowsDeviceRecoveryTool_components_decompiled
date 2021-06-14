using System;
using System.Collections;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x02000239 RID: 569
	internal class EnumValAlphaComparer : IComparer
	{
		// Token: 0x060021C0 RID: 8640 RVA: 0x000A52D1 File Offset: 0x000A34D1
		internal EnumValAlphaComparer()
		{
			this.m_compareInfo = CultureInfo.InvariantCulture.CompareInfo;
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x000A52E9 File Offset: 0x000A34E9
		public int Compare(object a, object b)
		{
			return this.m_compareInfo.Compare(a.ToString(), b.ToString());
		}

		// Token: 0x04000EB0 RID: 3760
		private CompareInfo m_compareInfo;

		// Token: 0x04000EB1 RID: 3761
		internal static readonly EnumValAlphaComparer Default = new EnumValAlphaComparer();
	}
}
