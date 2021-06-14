using System;
using Microsoft.Data.Edm.Annotations;

namespace Microsoft.Data.Edm.Library.Annotations
{
	// Token: 0x02000120 RID: 288
	public class EdmDirectValueAnnotationBinding : IEdmDirectValueAnnotationBinding
	{
		// Token: 0x06000589 RID: 1417 RVA: 0x0000DA78 File Offset: 0x0000BC78
		public EdmDirectValueAnnotationBinding(IEdmElement element, string namespaceUri, string name, object value)
		{
			EdmUtil.CheckArgumentNull<IEdmElement>(element, "element");
			EdmUtil.CheckArgumentNull<string>(namespaceUri, "namespaceUri");
			EdmUtil.CheckArgumentNull<string>(name, "name");
			this.element = element;
			this.namespaceUri = namespaceUri;
			this.name = name;
			this.value = value;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0000DACC File Offset: 0x0000BCCC
		public EdmDirectValueAnnotationBinding(IEdmElement element, string namespaceUri, string name)
		{
			EdmUtil.CheckArgumentNull<IEdmElement>(element, "element");
			EdmUtil.CheckArgumentNull<string>(namespaceUri, "namespaceUri");
			EdmUtil.CheckArgumentNull<string>(name, "name");
			this.element = element;
			this.namespaceUri = namespaceUri;
			this.name = name;
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x0000DB18 File Offset: 0x0000BD18
		public IEdmElement Element
		{
			get
			{
				return this.element;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x0000DB20 File Offset: 0x0000BD20
		public string NamespaceUri
		{
			get
			{
				return this.namespaceUri;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x0000DB28 File Offset: 0x0000BD28
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x0000DB30 File Offset: 0x0000BD30
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x04000200 RID: 512
		private readonly IEdmElement element;

		// Token: 0x04000201 RID: 513
		private readonly string namespaceUri;

		// Token: 0x04000202 RID: 514
		private readonly string name;

		// Token: 0x04000203 RID: 515
		private readonly object value;
	}
}
