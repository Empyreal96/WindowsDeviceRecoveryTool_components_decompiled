using System;
using System.Drawing;
using System.Linq;
using Microsoft.WindowsDeviceRecoveryTool.Core;

namespace Microsoft.WindowsDeviceRecoveryTool.HtcAdaptation
{
	// Token: 0x02000003 RID: 3
	internal sealed class ModelInfo
	{
		// Token: 0x06000002 RID: 2 RVA: 0x000020A8 File Offset: 0x000002A8
		public ModelInfo(string friendlyName, Bitmap bitmap, params VidPidPair[] vidPidPairs)
		{
			if (friendlyName == null)
			{
				throw new ArgumentNullException("friendlyName");
			}
			if (bitmap == null)
			{
				throw new ArgumentNullException("bitmap");
			}
			if (vidPidPairs == null)
			{
				throw new ArgumentNullException("vidPidPairs");
			}
			VidPidPair[] array = vidPidPairs.ToArray<VidPidPair>();
			if (array.Length == 0)
			{
				throw new ArgumentException("vidPidPairs should have at least one element");
			}
			this.FriendlyName = friendlyName;
			this.Bitmap = bitmap;
			this.VidPidPairs = array;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x0000213C File Offset: 0x0000033C
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002153 File Offset: 0x00000353
		public string FriendlyName { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x0000215C File Offset: 0x0000035C
		// (set) Token: 0x06000006 RID: 6 RVA: 0x00002173 File Offset: 0x00000373
		public Bitmap Bitmap { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000217C File Offset: 0x0000037C
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002193 File Offset: 0x00000393
		public VidPidPair[] VidPidPairs { get; private set; }
	}
}
