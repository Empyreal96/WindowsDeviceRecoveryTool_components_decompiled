using System;

namespace System.Windows.Controls
{
	/// <summary>Enables you to select an <see cref="T:System.Windows.Controls.ItemContainerTemplate" /> for each item within an <see cref="T:System.Windows.Controls.ItemsControl" />. </summary>
	// Token: 0x020004F3 RID: 1267
	public abstract class ItemContainerTemplateSelector
	{
		/// <summary>When overridden in a derived class, returns an <see cref="T:System.Windows.Controls.ItemContainerTemplate" /> based on custom logic.</summary>
		/// <param name="item">The object for which to select the template.</param>
		/// <param name="parentItemsControl">The container for the items.</param>
		/// <returns>The template. The default implementation returns <see langword="null" />.</returns>
		// Token: 0x0600503C RID: 20540 RVA: 0x0000C238 File Offset: 0x0000A438
		public virtual DataTemplate SelectTemplate(object item, ItemsControl parentItemsControl)
		{
			return null;
		}
	}
}
