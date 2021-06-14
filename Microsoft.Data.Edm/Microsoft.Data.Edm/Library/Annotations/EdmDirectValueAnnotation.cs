using System;
using Microsoft.Data.Edm.Annotations;

namespace Microsoft.Data.Edm.Library.Annotations
{
	// Token: 0x020001DE RID: 478
	public class EdmDirectValueAnnotation : EdmNamedElement, IEdmDirectValueAnnotation, IEdmNamedElement, IEdmElement
	{
		// Token: 0x06000B5B RID: 2907 RVA: 0x00020EDC File Offset: 0x0001F0DC
		public EdmDirectValueAnnotation(string namespaceUri, string name, object value) : this(namespaceUri, name)
		{
			EdmUtil.CheckArgumentNull<object>(value, "value");
			this.value = value;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00020EF9 File Offset: 0x0001F0F9
		internal EdmDirectValueAnnotation(string namespaceUri, string name) : base(name)
		{
			EdmUtil.CheckArgumentNull<string>(namespaceUri, "namespaceUri");
			this.namespaceUri = namespaceUri;
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x00020F15 File Offset: 0x0001F115
		public string NamespaceUri
		{
			get
			{
				return this.namespaceUri;
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06000B5E RID: 2910 RVA: 0x00020F1D File Offset: 0x0001F11D
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x04000551 RID: 1361
		private readonly object value;

		// Token: 0x04000552 RID: 1362
		private readonly string namespaceUri;
	}
}
