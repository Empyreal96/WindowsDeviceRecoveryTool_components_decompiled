using System;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x0200003F RID: 63
	public class MessageWriterSettingsArgs
	{
		// Token: 0x060001FF RID: 511 RVA: 0x0000AC6A File Offset: 0x00008E6A
		public MessageWriterSettingsArgs(ODataMessageWriterSettingsBase settings)
		{
			WebUtil.CheckArgumentNull<ODataMessageWriterSettingsBase>(settings, "settings");
			this.Settings = settings;
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000AC85 File Offset: 0x00008E85
		// (set) Token: 0x06000201 RID: 513 RVA: 0x0000AC8D File Offset: 0x00008E8D
		public ODataMessageWriterSettingsBase Settings { get; private set; }
	}
}
