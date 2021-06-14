using System;
using System.ComponentModel;

namespace System.Drawing
{
	// Token: 0x02000050 RID: 80
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class SRCategoryAttribute : CategoryAttribute
	{
		// Token: 0x060006F6 RID: 1782 RVA: 0x0001C429 File Offset: 0x0001A629
		public SRCategoryAttribute(string category) : base(category)
		{
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0001C432 File Offset: 0x0001A632
		protected override string GetLocalizedString(string value)
		{
			return SR.GetString(value);
		}
	}
}
