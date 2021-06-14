using System;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000037 RID: 55
	internal class DataServiceClientMessageReaderSettingsShim : ODataMessageReaderSettingsBase
	{
		// Token: 0x06000198 RID: 408 RVA: 0x0000906E File Offset: 0x0000726E
		internal DataServiceClientMessageReaderSettingsShim(ODataMessageReaderSettingsBase settings)
		{
			this.settings = settings;
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000907D File Offset: 0x0000727D
		// (set) Token: 0x0600019A RID: 410 RVA: 0x0000908A File Offset: 0x0000728A
		public override bool EnableAtomMetadataReading
		{
			get
			{
				return this.settings.EnableAtomMetadataReading;
			}
			set
			{
				this.settings.EnableAtomMetadataReading = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00009098 File Offset: 0x00007298
		// (set) Token: 0x0600019C RID: 412 RVA: 0x000090A5 File Offset: 0x000072A5
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

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600019D RID: 413 RVA: 0x000090B3 File Offset: 0x000072B3
		// (set) Token: 0x0600019E RID: 414 RVA: 0x000090C0 File Offset: 0x000072C0
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

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600019F RID: 415 RVA: 0x000090CE File Offset: 0x000072CE
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x000090DB File Offset: 0x000072DB
		public override Func<string, bool> ShouldIncludeAnnotation
		{
			get
			{
				return this.settings.ShouldIncludeAnnotation;
			}
			set
			{
				this.settings.ShouldIncludeAnnotation = value;
			}
		}

		// Token: 0x040001FF RID: 511
		private readonly ODataMessageReaderSettingsBase settings;
	}
}
