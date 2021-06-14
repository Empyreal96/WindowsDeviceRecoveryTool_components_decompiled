using System;
using System.ComponentModel;

namespace System.Windows
{
	// Token: 0x02000097 RID: 151
	internal sealed class CustomCategoryAttribute : CategoryAttribute
	{
		// Token: 0x06000266 RID: 614 RVA: 0x0000631B File Offset: 0x0000451B
		internal CustomCategoryAttribute(string name) : base(name)
		{
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00006324 File Offset: 0x00004524
		protected override string GetLocalizedString(string value)
		{
			if (string.Compare(value, "Content", StringComparison.Ordinal) == 0)
			{
				return SR.Get("DesignerMetadata_CustomCategory_Content");
			}
			if (string.Compare(value, "Accessibility", StringComparison.Ordinal) == 0)
			{
				return SR.Get("DesignerMetadata_CustomCategory_Accessibility");
			}
			return SR.Get("DesignerMetadata_CustomCategory_Navigation");
		}
	}
}
