using System;

namespace System.Windows.Controls
{
	// Token: 0x020004F4 RID: 1268
	internal class DefaultItemContainerTemplateSelector : ItemContainerTemplateSelector
	{
		// Token: 0x0600503E RID: 20542 RVA: 0x001686F2 File Offset: 0x001668F2
		public override DataTemplate SelectTemplate(object item, ItemsControl parentItemsControl)
		{
			return FrameworkElement.FindTemplateResourceInternal(parentItemsControl, item, typeof(ItemContainerTemplate)) as DataTemplate;
		}
	}
}
