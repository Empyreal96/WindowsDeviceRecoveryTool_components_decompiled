using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000444 RID: 1092
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class SRCategoryAttribute : CategoryAttribute
	{
		// Token: 0x06004CA5 RID: 19621 RVA: 0x00135CC4 File Offset: 0x00133EC4
		public SRCategoryAttribute(string category) : base(category)
		{
		}

		// Token: 0x06004CA6 RID: 19622 RVA: 0x0013AA08 File Offset: 0x00138C08
		protected override string GetLocalizedString(string value)
		{
			return SR.GetString(value);
		}
	}
}
