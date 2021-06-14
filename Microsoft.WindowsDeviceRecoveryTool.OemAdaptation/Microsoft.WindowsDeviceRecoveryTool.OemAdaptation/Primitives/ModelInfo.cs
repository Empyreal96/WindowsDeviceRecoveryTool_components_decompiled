using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives
{
	// Token: 0x02000006 RID: 6
	public sealed class ModelInfo
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002194 File Offset: 0x00000394
		public ModelInfo(string name, Bitmap bitmap, DetectionInfo detectionInfo, IEnumerable<VariantInfo> variants)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (bitmap == null)
			{
				throw new ArgumentNullException("bitmap");
			}
			if (detectionInfo == null)
			{
				throw new ArgumentNullException("detectionInfo");
			}
			if (variants == null)
			{
				throw new ArgumentNullException("variants");
			}
			VariantInfo[] array = variants.ToArray<VariantInfo>();
			if (array.Length == 0)
			{
				throw new ArgumentException("variants should have at least one element");
			}
			this.Name = name;
			this.Bitmap = bitmap;
			this.DetectionInfo = detectionInfo;
			this.Variants = array;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002214 File Offset: 0x00000414
		// (set) Token: 0x0600000F RID: 15 RVA: 0x0000221C File Offset: 0x0000041C
		public string Name { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002225 File Offset: 0x00000425
		// (set) Token: 0x06000011 RID: 17 RVA: 0x0000222D File Offset: 0x0000042D
		public Bitmap Bitmap { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002236 File Offset: 0x00000436
		// (set) Token: 0x06000013 RID: 19 RVA: 0x0000223E File Offset: 0x0000043E
		public DetectionInfo DetectionInfo { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002247 File Offset: 0x00000447
		// (set) Token: 0x06000015 RID: 21 RVA: 0x0000224F File Offset: 0x0000044F
		public VariantInfo[] Variants { get; private set; }
	}
}
