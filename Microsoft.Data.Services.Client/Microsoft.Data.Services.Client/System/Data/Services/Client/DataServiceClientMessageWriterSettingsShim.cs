using System;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000036 RID: 54
	internal class DataServiceClientMessageWriterSettingsShim : ODataMessageWriterSettingsBase
	{
		// Token: 0x06000191 RID: 401 RVA: 0x0000900E File Offset: 0x0000720E
		internal DataServiceClientMessageWriterSettingsShim(ODataMessageWriterSettingsBase settings)
		{
			this.settings = settings;
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000192 RID: 402 RVA: 0x0000901D File Offset: 0x0000721D
		// (set) Token: 0x06000193 RID: 403 RVA: 0x0000902A File Offset: 0x0000722A
		public override bool Indent
		{
			get
			{
				return this.settings.Indent;
			}
			set
			{
				this.settings.Indent = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00009038 File Offset: 0x00007238
		// (set) Token: 0x06000195 RID: 405 RVA: 0x00009045 File Offset: 0x00007245
		public override bool CheckCharacters
		{
			get
			{
				return this.settings.CheckCharacters;
			}
			set
			{
				this.settings.CheckCharacters = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00009053 File Offset: 0x00007253
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00009060 File Offset: 0x00007260
		public override ODataMessageQuotas MessageQuotas
		{
			get
			{
				return this.settings.MessageQuotas;
			}
			set
			{
				this.settings.MessageQuotas = value;
			}
		}

		// Token: 0x040001FE RID: 510
		private readonly ODataMessageWriterSettingsBase settings;
	}
}
