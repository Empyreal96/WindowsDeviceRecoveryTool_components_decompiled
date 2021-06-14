using System;

namespace System.Windows.Controls
{
	/// <summary>Represents an item in a <see cref="T:System.Windows.Controls.ListView" /> control.</summary>
	// Token: 0x020004FE RID: 1278
	public class ListViewItem : ListBoxItem
	{
		// Token: 0x06005177 RID: 20855 RVA: 0x0016D6C8 File Offset: 0x0016B8C8
		internal void SetDefaultStyleKey(object key)
		{
			base.DefaultStyleKey = key;
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x0016D6D1 File Offset: 0x0016B8D1
		internal void ClearDefaultStyleKey()
		{
			base.ClearValue(FrameworkElement.DefaultStyleKeyProperty);
		}
	}
}
