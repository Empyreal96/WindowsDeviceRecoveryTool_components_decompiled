using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000434 RID: 1076
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class WinCategoryAttribute : CategoryAttribute
	{
		// Token: 0x06004AFA RID: 19194 RVA: 0x00135CC4 File Offset: 0x00133EC4
		public WinCategoryAttribute(string category) : base(category)
		{
		}

		// Token: 0x06004AFB RID: 19195 RVA: 0x00135CD0 File Offset: 0x00133ED0
		protected override string GetLocalizedString(string value)
		{
			string text = base.GetLocalizedString(value);
			if (text == null)
			{
				text = (string)SR.GetObject("WinFormsCategory" + value);
			}
			return text;
		}
	}
}
