using System;

namespace System.Windows.Controls
{
	/// <summary>Provides a resource key for an <see cref="T:System.Windows.Controls.ItemContainerTemplate" /> object.</summary>
	// Token: 0x020004F2 RID: 1266
	public class ItemContainerTemplateKey : TemplateKey
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ItemContainerTemplateKey" /> class.</summary>
		// Token: 0x0600503A RID: 20538 RVA: 0x001686DF File Offset: 0x001668DF
		public ItemContainerTemplateKey() : base(TemplateKey.TemplateType.TableTemplate)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ItemContainerTemplateKey" /> class with the specified data type.</summary>
		/// <param name="dataType">The type for which this template is designed.</param>
		// Token: 0x0600503B RID: 20539 RVA: 0x001686E8 File Offset: 0x001668E8
		public ItemContainerTemplateKey(object dataType) : base(TemplateKey.TemplateType.TableTemplate, dataType)
		{
		}
	}
}
