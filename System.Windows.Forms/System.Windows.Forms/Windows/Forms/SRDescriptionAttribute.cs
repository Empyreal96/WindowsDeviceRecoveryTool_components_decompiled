using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000443 RID: 1091
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class SRDescriptionAttribute : DescriptionAttribute
	{
		// Token: 0x06004CA3 RID: 19619 RVA: 0x0013A9D7 File Offset: 0x00138BD7
		public SRDescriptionAttribute(string description) : base(description)
		{
		}

		// Token: 0x170012D4 RID: 4820
		// (get) Token: 0x06004CA4 RID: 19620 RVA: 0x0013A9E0 File Offset: 0x00138BE0
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

		// Token: 0x040027ED RID: 10221
		private bool replaced;
	}
}
