using System;
using Microsoft.Data.Edm.Annotations;

namespace Microsoft.Data.Edm.Library.Annotations
{
	// Token: 0x02000122 RID: 290
	public class EdmTypedDirectValueAnnotationBinding<T> : EdmNamedElement, IEdmDirectValueAnnotationBinding
	{
		// Token: 0x06000591 RID: 1425 RVA: 0x0000DB5B File Offset: 0x0000BD5B
		public EdmTypedDirectValueAnnotationBinding(IEdmElement element, T value) : base(ExtensionMethods.TypeName<T>.LocalName)
		{
			this.element = element;
			this.value = value;
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x0000DB76 File Offset: 0x0000BD76
		public IEdmElement Element
		{
			get
			{
				return this.element;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x0000DB7E File Offset: 0x0000BD7E
		public string NamespaceUri
		{
			get
			{
				return "http://schemas.microsoft.com/ado/2011/04/edm/internal";
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x0000DB85 File Offset: 0x0000BD85
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x04000205 RID: 517
		private readonly IEdmElement element;

		// Token: 0x04000206 RID: 518
		private readonly T value;
	}
}
