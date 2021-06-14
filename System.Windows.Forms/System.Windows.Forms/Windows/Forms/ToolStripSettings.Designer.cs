using System;
using System.Configuration;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020003F1 RID: 1009
	internal partial class ToolStripSettings : ApplicationSettingsBase
	{
		// Token: 0x17001105 RID: 4357
		// (get) Token: 0x060043E7 RID: 17383 RVA: 0x001221FB File Offset: 0x001203FB
		// (set) Token: 0x060043E8 RID: 17384 RVA: 0x0012220D File Offset: 0x0012040D
		[UserScopedSetting]
		[DefaultSettingValue("true")]
		public bool IsDefault
		{
			get
			{
				return (bool)this["IsDefault"];
			}
			set
			{
				this["IsDefault"] = value;
			}
		}

		// Token: 0x17001108 RID: 4360
		// (get) Token: 0x060043ED RID: 17389 RVA: 0x00122260 File Offset: 0x00120460
		// (set) Token: 0x060043EE RID: 17390 RVA: 0x00122272 File Offset: 0x00120472
		[UserScopedSetting]
		[DefaultSettingValue("0,0")]
		public Point Location
		{
			get
			{
				return (Point)this["Location"];
			}
			set
			{
				this["Location"] = value;
			}
		}

		// Token: 0x17001109 RID: 4361
		// (get) Token: 0x060043EF RID: 17391 RVA: 0x00122285 File Offset: 0x00120485
		// (set) Token: 0x060043F0 RID: 17392 RVA: 0x00122297 File Offset: 0x00120497
		[UserScopedSetting]
		[DefaultSettingValue("0,0")]
		public Size Size
		{
			get
			{
				return (Size)this["Size"];
			}
			set
			{
				this["Size"] = value;
			}
		}

		// Token: 0x1700110B RID: 4363
		// (get) Token: 0x060043F3 RID: 17395 RVA: 0x001222CA File Offset: 0x001204CA
		// (set) Token: 0x060043F4 RID: 17396 RVA: 0x001222DC File Offset: 0x001204DC
		[UserScopedSetting]
		[DefaultSettingValue("true")]
		public bool Visible
		{
			get
			{
				return (bool)this["Visible"];
			}
			set
			{
				this["Visible"] = value;
			}
		}
	}
}
