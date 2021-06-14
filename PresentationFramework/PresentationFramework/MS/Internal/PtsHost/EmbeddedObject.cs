using System;
using System.Windows;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000618 RID: 1560
	internal abstract class EmbeddedObject
	{
		// Token: 0x060067BF RID: 26559 RVA: 0x001D175A File Offset: 0x001CF95A
		protected EmbeddedObject(int dcp)
		{
			this.Dcp = dcp;
		}

		// Token: 0x060067C0 RID: 26560 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void Dispose()
		{
		}

		// Token: 0x060067C1 RID: 26561
		internal abstract void Update(EmbeddedObject newObject);

		// Token: 0x17001918 RID: 6424
		// (get) Token: 0x060067C2 RID: 26562
		internal abstract DependencyObject Element { get; }

		// Token: 0x04003387 RID: 13191
		internal int Dcp;
	}
}
