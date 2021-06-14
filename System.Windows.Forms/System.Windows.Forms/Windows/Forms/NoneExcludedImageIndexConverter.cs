using System;

namespace System.Windows.Forms
{
	// Token: 0x020002FB RID: 763
	internal sealed class NoneExcludedImageIndexConverter : ImageIndexConverter
	{
		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x06002E13 RID: 11795 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected override bool IncludeNoneAsStandardValue
		{
			get
			{
				return false;
			}
		}
	}
}
