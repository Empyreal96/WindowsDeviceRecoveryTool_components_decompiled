using System;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x0200003E RID: 62
	public class MessageReaderSettingsArgs
	{
		// Token: 0x060001FC RID: 508 RVA: 0x0000AC3E File Offset: 0x00008E3E
		public MessageReaderSettingsArgs(ODataMessageReaderSettingsBase settings)
		{
			WebUtil.CheckArgumentNull<ODataMessageReaderSettingsBase>(settings, "settings");
			this.Settings = settings;
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0000AC59 File Offset: 0x00008E59
		// (set) Token: 0x060001FE RID: 510 RVA: 0x0000AC61 File Offset: 0x00008E61
		public ODataMessageReaderSettingsBase Settings { get; private set; }
	}
}
