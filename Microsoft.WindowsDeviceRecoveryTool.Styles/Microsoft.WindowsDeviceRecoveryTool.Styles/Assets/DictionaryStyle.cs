using System;
using System.Drawing;
using System.Windows.Media;
using Microsoft.WindowsDeviceRecoveryTool.Localization;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Assets
{
	// Token: 0x02000003 RID: 3
	public class DictionaryStyle
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000020BC File Offset: 0x000002BC
		public DictionaryStyle(string name, string fileName, System.Drawing.Color color)
		{
			this.Name = name;
			this.FileName = fileName;
			this.MainColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002108 File Offset: 0x00000308
		public string LocalizedName
		{
			get
			{
				return LocalizationManager.GetTranslation(this.Name);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002128 File Offset: 0x00000328
		// (set) Token: 0x06000009 RID: 9 RVA: 0x0000213F File Offset: 0x0000033F
		public string Name { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002148 File Offset: 0x00000348
		// (set) Token: 0x0600000B RID: 11 RVA: 0x0000215F File Offset: 0x0000035F
		public System.Windows.Media.Brush MainColor { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002168 File Offset: 0x00000368
		// (set) Token: 0x0600000D RID: 13 RVA: 0x0000217F File Offset: 0x0000037F
		internal string FileName { get; set; }

		// Token: 0x0600000E RID: 14 RVA: 0x00002188 File Offset: 0x00000388
		public override string ToString()
		{
			return this.LocalizedName;
		}
	}
}
