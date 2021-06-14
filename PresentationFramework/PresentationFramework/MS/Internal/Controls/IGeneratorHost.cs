using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MS.Internal.Controls
{
	// Token: 0x02000756 RID: 1878
	internal interface IGeneratorHost
	{
		// Token: 0x17001C5F RID: 7263
		// (get) Token: 0x060077A0 RID: 30624
		ItemCollection View { get; }

		// Token: 0x060077A1 RID: 30625
		bool IsItemItsOwnContainer(object item);

		// Token: 0x060077A2 RID: 30626
		DependencyObject GetContainerForItem(object item);

		// Token: 0x060077A3 RID: 30627
		void PrepareItemContainer(DependencyObject container, object item);

		// Token: 0x060077A4 RID: 30628
		void ClearContainerForItem(DependencyObject container, object item);

		// Token: 0x060077A5 RID: 30629
		bool IsHostForItemContainer(DependencyObject container);

		// Token: 0x060077A6 RID: 30630
		GroupStyle GetGroupStyle(CollectionViewGroup group, int level);

		// Token: 0x060077A7 RID: 30631
		void SetIsGrouping(bool isGrouping);

		// Token: 0x17001C60 RID: 7264
		// (get) Token: 0x060077A8 RID: 30632
		int AlternationCount { get; }
	}
}
