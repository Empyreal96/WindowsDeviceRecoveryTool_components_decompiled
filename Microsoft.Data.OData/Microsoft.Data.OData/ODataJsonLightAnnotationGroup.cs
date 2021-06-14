using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Data.OData
{
	// Token: 0x02000160 RID: 352
	[DebuggerDisplay("{Name}")]
	internal sealed class ODataJsonLightAnnotationGroup
	{
		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x0001EA70 File Offset: 0x0001CC70
		// (set) Token: 0x060009AE RID: 2478 RVA: 0x0001EA78 File Offset: 0x0001CC78
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060009AF RID: 2479 RVA: 0x0001EA81 File Offset: 0x0001CC81
		// (set) Token: 0x060009B0 RID: 2480 RVA: 0x0001EA89 File Offset: 0x0001CC89
		public IDictionary<string, object> Annotations
		{
			get
			{
				return this.annotations;
			}
			set
			{
				this.annotations = value;
			}
		}

		// Token: 0x04000395 RID: 917
		private string name;

		// Token: 0x04000396 RID: 918
		private IDictionary<string, object> annotations;
	}
}
