using System;
using System.Collections.Generic;
using System.Security;
using System.Xaml;

namespace System.Windows
{
	// Token: 0x02000120 RID: 288
	internal class TemplateLoadData
	{
		// Token: 0x06000BFD RID: 3069 RVA: 0x0000326D File Offset: 0x0000146D
		internal TemplateLoadData()
		{
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x0002CE77 File Offset: 0x0002B077
		// (set) Token: 0x06000BFF RID: 3071 RVA: 0x0002CE7F File Offset: 0x0002B07F
		internal TemplateContent.StackOfFrames Stack { get; set; }

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000C00 RID: 3072 RVA: 0x0002CE88 File Offset: 0x0002B088
		internal Dictionary<string, XamlType> NamedTypes
		{
			get
			{
				if (this._namedTypes == null)
				{
					this._namedTypes = new Dictionary<string, XamlType>();
				}
				return this._namedTypes;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000C01 RID: 3073 RVA: 0x0002CEA3 File Offset: 0x0002B0A3
		// (set) Token: 0x06000C02 RID: 3074 RVA: 0x0002CEAB File Offset: 0x0002B0AB
		internal XamlReader Reader { get; [SecurityCritical] set; }

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000C03 RID: 3075 RVA: 0x0002CEB4 File Offset: 0x0002B0B4
		// (set) Token: 0x06000C04 RID: 3076 RVA: 0x0002CEBC File Offset: 0x0002B0BC
		internal string RootName { get; set; }

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000C05 RID: 3077 RVA: 0x0002CEC5 File Offset: 0x0002B0C5
		// (set) Token: 0x06000C06 RID: 3078 RVA: 0x0002CECD File Offset: 0x0002B0CD
		internal object RootObject { get; set; }

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000C07 RID: 3079 RVA: 0x0002CED6 File Offset: 0x0002B0D6
		// (set) Token: 0x06000C08 RID: 3080 RVA: 0x0002CEDE File Offset: 0x0002B0DE
		internal TemplateContent.ServiceProviderWrapper ServiceProviderWrapper { get; set; }

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000C09 RID: 3081 RVA: 0x0002CEE7 File Offset: 0x0002B0E7
		// (set) Token: 0x06000C0A RID: 3082 RVA: 0x0002CEEF File Offset: 0x0002B0EF
		internal XamlObjectWriter ObjectWriter { get; set; }

		// Token: 0x04000AC6 RID: 2758
		internal Dictionary<string, XamlType> _namedTypes;
	}
}
