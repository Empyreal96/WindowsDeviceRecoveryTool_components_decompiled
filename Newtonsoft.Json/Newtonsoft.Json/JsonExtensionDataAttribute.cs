using System;

namespace Newtonsoft.Json
{
	// Token: 0x0200004C RID: 76
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class JsonExtensionDataAttribute : Attribute
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000AD17 File Offset: 0x00008F17
		// (set) Token: 0x060002CE RID: 718 RVA: 0x0000AD1F File Offset: 0x00008F1F
		public bool WriteData { get; set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000AD28 File Offset: 0x00008F28
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x0000AD30 File Offset: 0x00008F30
		public bool ReadData { get; set; }

		// Token: 0x060002D1 RID: 721 RVA: 0x0000AD39 File Offset: 0x00008F39
		public JsonExtensionDataAttribute()
		{
			this.WriteData = true;
			this.ReadData = true;
		}
	}
}
