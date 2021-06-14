using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MS.Internal.Controls
{
	// Token: 0x02000765 RID: 1893
	internal class ValidationRuleCollection : Collection<ValidationRule>
	{
		// Token: 0x06007853 RID: 30803 RVA: 0x0022457F File Offset: 0x0022277F
		protected override void InsertItem(int index, ValidationRule item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.InsertItem(index, item);
		}

		// Token: 0x06007854 RID: 30804 RVA: 0x00224597 File Offset: 0x00222797
		protected override void SetItem(int index, ValidationRule item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.SetItem(index, item);
		}
	}
}
