using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200012E RID: 302
	internal class CsdlDirectValueAnnotation : CsdlElement
	{
		// Token: 0x060005CB RID: 1483 RVA: 0x0000F2D2 File Offset: 0x0000D4D2
		public CsdlDirectValueAnnotation(string namespaceName, string name, string value, bool isAttribute, CsdlLocation location) : base(location)
		{
			this.namespaceName = namespaceName;
			this.name = name;
			this.value = value;
			this.isAttribute = isAttribute;
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x0000F2F9 File Offset: 0x0000D4F9
		public string NamespaceName
		{
			get
			{
				return this.namespaceName;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x0000F301 File Offset: 0x0000D501
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x0000F309 File Offset: 0x0000D509
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x0000F311 File Offset: 0x0000D511
		public bool IsAttribute
		{
			get
			{
				return this.isAttribute;
			}
		}

		// Token: 0x0400030B RID: 779
		private readonly string namespaceName;

		// Token: 0x0400030C RID: 780
		private readonly string name;

		// Token: 0x0400030D RID: 781
		private readonly string value;

		// Token: 0x0400030E RID: 782
		private readonly bool isAttribute;
	}
}
