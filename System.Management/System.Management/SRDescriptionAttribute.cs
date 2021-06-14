using System;
using System.ComponentModel;

namespace System.Management
{
	// Token: 0x020000A8 RID: 168
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class SRDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x06000479 RID: 1145 RVA: 0x00021E1C File Offset: 0x0002001C
		public SRDescriptionAttribute(string description) : base(description)
		{
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x00021E25 File Offset: 0x00020025
		public override string Description
		{
			get
			{
				if (!this.replaced)
				{
					this.replaced = true;
					base.DescriptionValue = SR.GetString(base.Description);
				}
				return base.Description;
			}
		}

		// Token: 0x04000498 RID: 1176
		private bool replaced;
	}
}
