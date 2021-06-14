using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Common
{
	// Token: 0x02000155 RID: 341
	internal class XmlAnnotationInfo
	{
		// Token: 0x060006A4 RID: 1700 RVA: 0x000110FC File Offset: 0x0000F2FC
		internal XmlAnnotationInfo(CsdlLocation location, string namespaceName, string name, string value, bool isAttribute)
		{
			this.Location = location;
			this.NamespaceName = namespaceName;
			this.Name = name;
			this.Value = value;
			this.IsAttribute = isAttribute;
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x00011129 File Offset: 0x0000F329
		// (set) Token: 0x060006A6 RID: 1702 RVA: 0x00011131 File Offset: 0x0000F331
		internal string NamespaceName { get; private set; }

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x0001113A File Offset: 0x0000F33A
		// (set) Token: 0x060006A8 RID: 1704 RVA: 0x00011142 File Offset: 0x0000F342
		internal string Name { get; private set; }

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x0001114B File Offset: 0x0000F34B
		// (set) Token: 0x060006AA RID: 1706 RVA: 0x00011153 File Offset: 0x0000F353
		internal CsdlLocation Location { get; private set; }

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x0001115C File Offset: 0x0000F35C
		// (set) Token: 0x060006AC RID: 1708 RVA: 0x00011164 File Offset: 0x0000F364
		internal string Value { get; private set; }

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x0001116D File Offset: 0x0000F36D
		// (set) Token: 0x060006AE RID: 1710 RVA: 0x00011175 File Offset: 0x0000F375
		internal bool IsAttribute { get; private set; }
	}
}
