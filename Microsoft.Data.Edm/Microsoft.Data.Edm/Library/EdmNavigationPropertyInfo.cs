using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020000D5 RID: 213
	public sealed class EdmNavigationPropertyInfo
	{
		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x0000BD30 File Offset: 0x00009F30
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x0000BD38 File Offset: 0x00009F38
		public string Name { get; set; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x0000BD41 File Offset: 0x00009F41
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x0000BD49 File Offset: 0x00009F49
		public IEdmEntityType Target { get; set; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x0000BD52 File Offset: 0x00009F52
		// (set) Token: 0x06000447 RID: 1095 RVA: 0x0000BD5A File Offset: 0x00009F5A
		public EdmMultiplicity TargetMultiplicity { get; set; }

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x0000BD63 File Offset: 0x00009F63
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x0000BD6B File Offset: 0x00009F6B
		public IEnumerable<IEdmStructuralProperty> DependentProperties { get; set; }

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x0000BD74 File Offset: 0x00009F74
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x0000BD7C File Offset: 0x00009F7C
		public bool ContainsTarget { get; set; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x0000BD85 File Offset: 0x00009F85
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x0000BD8D File Offset: 0x00009F8D
		public EdmOnDeleteAction OnDelete { get; set; }

		// Token: 0x0600044E RID: 1102 RVA: 0x0000BD98 File Offset: 0x00009F98
		public EdmNavigationPropertyInfo Clone()
		{
			return new EdmNavigationPropertyInfo
			{
				Name = this.Name,
				Target = this.Target,
				TargetMultiplicity = this.TargetMultiplicity,
				DependentProperties = this.DependentProperties,
				ContainsTarget = this.ContainsTarget,
				OnDelete = this.OnDelete
			};
		}
	}
}
