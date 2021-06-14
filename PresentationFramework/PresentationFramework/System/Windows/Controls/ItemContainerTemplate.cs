using System;
using System.Windows.Markup;

namespace System.Windows.Controls
{
	/// <summary>Provides the template for producing a container for an <see cref="T:System.Windows.Controls.ItemsControl" /> object. </summary>
	// Token: 0x020004F1 RID: 1265
	[DictionaryKeyProperty("ItemContainerTemplateKey")]
	public class ItemContainerTemplate : DataTemplate
	{
		/// <summary>Gets the default key of the <see cref="T:System.Windows.Controls.ItemContainerTemplate" />. </summary>
		/// <returns>The default key of the <see cref="T:System.Windows.Controls.ItemContainerTemplate" />.</returns>
		// Token: 0x1700138B RID: 5003
		// (get) Token: 0x06005038 RID: 20536 RVA: 0x001686C8 File Offset: 0x001668C8
		public object ItemContainerTemplateKey
		{
			get
			{
				if (base.DataType == null)
				{
					return null;
				}
				return new ItemContainerTemplateKey(base.DataType);
			}
		}
	}
}
