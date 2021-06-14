using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.Metadata
{
	// Token: 0x0200021E RID: 542
	internal sealed class CachedPrimitiveKeepInContentAnnotation
	{
		// Token: 0x060010DE RID: 4318 RVA: 0x0003F217 File Offset: 0x0003D417
		internal CachedPrimitiveKeepInContentAnnotation(IEnumerable<string> keptInContentPropertyNames)
		{
			this.keptInContentPropertyNames = ((keptInContentPropertyNames == null) ? null : new HashSet<string>(keptInContentPropertyNames, StringComparer.Ordinal));
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0003F236 File Offset: 0x0003D436
		internal bool IsKeptInContent(string propertyName)
		{
			return this.keptInContentPropertyNames != null && this.keptInContentPropertyNames.Contains(propertyName);
		}

		// Token: 0x04000630 RID: 1584
		private readonly HashSet<string> keptInContentPropertyNames;
	}
}
