using System;
using System.Collections.Generic;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200004D RID: 77
	public class AclCollection : HashSet<ResourceAcl>
	{
		// Token: 0x060001A5 RID: 421 RVA: 0x000093FF File Offset: 0x000075FF
		public AclCollection() : base(ResourceAcl.Comparer)
		{
		}
	}
}
