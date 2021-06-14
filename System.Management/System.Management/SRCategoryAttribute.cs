using System;
using System.ComponentModel;

namespace System.Management
{
	// Token: 0x020000A9 RID: 169
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class SRCategoryAttribute : CategoryAttribute
	{
		// Token: 0x0600047B RID: 1147 RVA: 0x00021E4D File Offset: 0x0002004D
		public SRCategoryAttribute(string category) : base(category)
		{
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00021E56 File Offset: 0x00020056
		protected override string GetLocalizedString(string value)
		{
			return SR.GetString(value);
		}
	}
}
