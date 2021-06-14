using System;
using System.Drawing;

namespace Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives
{
	// Token: 0x02000005 RID: 5
	public sealed class ManufacturerInfo
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002140 File Offset: 0x00000340
		public ManufacturerInfo(string name, Bitmap bitmap)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (bitmap == null)
			{
				throw new ArgumentNullException("bitmap");
			}
			this.Name = name;
			this.Bitmap = bitmap;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002172 File Offset: 0x00000372
		// (set) Token: 0x0600000A RID: 10 RVA: 0x0000217A File Offset: 0x0000037A
		public string Name { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002183 File Offset: 0x00000383
		// (set) Token: 0x0600000C RID: 12 RVA: 0x0000218B File Offset: 0x0000038B
		public Bitmap Bitmap { get; private set; }
	}
}
