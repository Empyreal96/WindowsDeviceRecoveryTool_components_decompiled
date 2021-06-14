using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace System.Windows
{
	// Token: 0x020000C0 RID: 192
	internal class FrameworkContextData
	{
		// Token: 0x060004F8 RID: 1272 RVA: 0x0000E004 File Offset: 0x0000C204
		public static FrameworkContextData From(Dispatcher context)
		{
			FrameworkContextData frameworkContextData = (FrameworkContextData)context.Reserved2;
			if (frameworkContextData == null)
			{
				frameworkContextData = new FrameworkContextData();
				context.Reserved2 = frameworkContextData;
			}
			return frameworkContextData;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0000E02E File Offset: 0x0000C22E
		private FrameworkContextData()
		{
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0000E044 File Offset: 0x0000C244
		public void AddWalker(object data, DescendentsWalkerBase walker)
		{
			FrameworkContextData.WalkerEntry item = default(FrameworkContextData.WalkerEntry);
			item.Data = data;
			item.Walker = walker;
			this._currentWalkers.Add(item);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0000E078 File Offset: 0x0000C278
		public void RemoveWalker(object data, DescendentsWalkerBase walker)
		{
			int index = this._currentWalkers.Count - 1;
			this._currentWalkers.RemoveAt(index);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0000E0A0 File Offset: 0x0000C2A0
		public bool WasNodeVisited(DependencyObject d, object data)
		{
			if (this._currentWalkers.Count > 0)
			{
				int index = this._currentWalkers.Count - 1;
				FrameworkContextData.WalkerEntry walkerEntry = this._currentWalkers[index];
				if (walkerEntry.Data == data)
				{
					return walkerEntry.Walker.WasVisited(d);
				}
			}
			return false;
		}

		// Token: 0x0400064B RID: 1611
		private List<FrameworkContextData.WalkerEntry> _currentWalkers = new List<FrameworkContextData.WalkerEntry>(4);

		// Token: 0x02000815 RID: 2069
		private struct WalkerEntry
		{
			// Token: 0x04003BAF RID: 15279
			public object Data;

			// Token: 0x04003BB0 RID: 15280
			public DescendentsWalkerBase Walker;
		}
	}
}
