using System;
using System.Collections;

namespace System.Windows.Forms
{
	// Token: 0x020003D2 RID: 978
	internal class ToolStripCustomIComparer : IComparer
	{
		// Token: 0x060040E8 RID: 16616 RVA: 0x001181F0 File Offset: 0x001163F0
		int IComparer.Compare(object x, object y)
		{
			if (x.GetType() == y.GetType())
			{
				return 0;
			}
			if (x.GetType().IsAssignableFrom(y.GetType()))
			{
				return 1;
			}
			if (y.GetType().IsAssignableFrom(x.GetType()))
			{
				return -1;
			}
			return 0;
		}
	}
}
