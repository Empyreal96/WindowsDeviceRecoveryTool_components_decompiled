using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x0200010D RID: 269
	internal class AlphaSortedEnumConverter : EnumConverter
	{
		// Token: 0x0600059A RID: 1434 RVA: 0x0001011F File Offset: 0x0000E31F
		public AlphaSortedEnumConverter(Type type) : base(type)
		{
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x00010128 File Offset: 0x0000E328
		protected override IComparer Comparer
		{
			get
			{
				return EnumValAlphaComparer.Default;
			}
		}
	}
}
