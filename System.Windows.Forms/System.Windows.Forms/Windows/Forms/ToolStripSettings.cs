using System;
using System.Configuration;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020003F1 RID: 1009
	internal partial class ToolStripSettings : ApplicationSettingsBase
	{
		// Token: 0x060043E6 RID: 17382 RVA: 0x001221F2 File Offset: 0x001203F2
		internal ToolStripSettings(string settingsKey) : base(settingsKey)
		{
		}

		// Token: 0x17001106 RID: 4358
		// (get) Token: 0x060043E9 RID: 17385 RVA: 0x00122220 File Offset: 0x00120420
		// (set) Token: 0x060043EA RID: 17386 RVA: 0x00122232 File Offset: 0x00120432
		[UserScopedSetting]
		public string ItemOrder
		{
			get
			{
				return this["ItemOrder"] as string;
			}
			set
			{
				this["ItemOrder"] = value;
			}
		}

		// Token: 0x17001107 RID: 4359
		// (get) Token: 0x060043EB RID: 17387 RVA: 0x00122240 File Offset: 0x00120440
		// (set) Token: 0x060043EC RID: 17388 RVA: 0x00122252 File Offset: 0x00120452
		[UserScopedSetting]
		public string Name
		{
			get
			{
				return this["Name"] as string;
			}
			set
			{
				this["Name"] = value;
			}
		}

		// Token: 0x1700110A RID: 4362
		// (get) Token: 0x060043F1 RID: 17393 RVA: 0x001222AA File Offset: 0x001204AA
		// (set) Token: 0x060043F2 RID: 17394 RVA: 0x001222BC File Offset: 0x001204BC
		[UserScopedSetting]
		public string ToolStripPanelName
		{
			get
			{
				return this["ToolStripPanelName"] as string;
			}
			set
			{
				this["ToolStripPanelName"] = value;
			}
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x001222EF File Offset: 0x001204EF
		public override void Save()
		{
			this.IsDefault = false;
			base.Save();
		}
	}
}
