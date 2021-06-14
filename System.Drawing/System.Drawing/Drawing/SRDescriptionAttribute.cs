using System;
using System.ComponentModel;

namespace System.Drawing
{
	// Token: 0x0200004F RID: 79
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class SRDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x060006F4 RID: 1780 RVA: 0x0001C3F8 File Offset: 0x0001A5F8
		public SRDescriptionAttribute(string description) : base(description)
		{
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0001C401 File Offset: 0x0001A601
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

		// Token: 0x040005A7 RID: 1447
		private bool replaced;
	}
}
