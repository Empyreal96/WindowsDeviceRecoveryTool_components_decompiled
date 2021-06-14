using System;
using Microsoft.WindowsDeviceRecoveryTool.Localization;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Assets
{
	// Token: 0x02000002 RID: 2
	public class ThemeStyle
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public ThemeStyle(string name)
		{
			this.Name = name;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x00002064 File Offset: 0x00000264
		public string LocalizedName
		{
			get
			{
				return LocalizationManager.GetTranslation(this.Name);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002084 File Offset: 0x00000284
		// (set) Token: 0x06000004 RID: 4 RVA: 0x0000209B File Offset: 0x0000029B
		public string Name { get; private set; }

		// Token: 0x06000005 RID: 5 RVA: 0x000020A4 File Offset: 0x000002A4
		public override string ToString()
		{
			return this.LocalizedName;
		}
	}
}
